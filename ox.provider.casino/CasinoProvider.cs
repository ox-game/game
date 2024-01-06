using Akka.IO;
using OX.Bapps;
using OX.Cryptography.ECC;
using OX.IO;
using OX.IO.Data.LevelDB;
using OX.Ledger;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace OX.Casino
{
    public partial class CasinoProvider : BaseBappProvider, ICasinoProvider
    {
        Wallet _wallet;
        public override Wallet Wallet
        {
            get { return _wallet; }
            set
            {
                _wallet = value;
            }
        }

        Dictionary<Type, IGameProvider> gameProviders = new Dictionary<Type, IGameProvider>();
        public uint BuryNumber { get; set; }

        
        public CasinoProvider(Bapp bapp) : base(bapp)
        {
            Db = DB.Open(Path.GetFullPath($"{WalletIndexDirectory}\\csn_{Message.Magic.ToString("X8")}"), new Options { CreateIfMissing = true });
            var LR = this.Get<LastRoomId>(CasinoBizPersistencePrefixes.Casino_Last_RoomId, casino.CasinoSettleAccountAddress);
            if (LR.IsNull())
            {
                LR = new LastRoomId { RoomId = 1000 };
            }
            this.LastRoomId = LR;
            this.MixRooms = new Dictionary<UInt160, MixRoom>(this.GetAll<UInt160, MixRoom>(CasinoBizPersistencePrefixes.Casino_Room));
            this.BuryNumber = this.GetBuryNumber(casino.BuryBetAddress);
        }

        #region IBappProvider
        
        public T GetGameProvider<T>() where T : class, IGameProvider
        {
            if (gameProviders.TryGetValue(typeof(T), out IGameProvider provider))
                return provider as T;
            return default;
        }
        public override void OnBappEvent(BappEvent bappEvent)
        {
            foreach (var gameProvider in gameProviders.Values)
            {
                gameProvider.OnBappEvent(bappEvent);
            }
        }
        public override void OnCrossBappMessage(CrossBappMessage message)
        {
            foreach (var gameProvider in gameProviders.Values)
            {
                gameProvider.OnCrossBappMessage(message);
            }
        }
        public override void OnRebuild(Wallet wallet)
        {
            WriteBatch batch = new WriteBatch();
            ReadOptions options = new ReadOptions { FillCache = false };
            using (Iterator it = Db.NewIterator(options))
            {
                for (it.SeekToFirst(); it.Valid(); it.Next())
                {
                    batch.Delete(it.Key());
                }
            }
            Db.Write(WriteOptions.Default, batch);
            Bapp.PushEvent(new BappEvent { EventItems = new BappEventItem[] { new BappEventItem() { EventType = CasinoBappEventType.ReBuildIndex.Value() } } });
            foreach (var gameProvider in gameProviders.Values)
            {
                gameProvider.OnRebuild(wallet);
            }
            //ValetRegisters.Clear();
            //RoomPledges.Clear();
        }
        public override void BeforeOnBlock(Block block)
        { }
        public override void AfterOnBlock(Block block)
        {

        }
        public override void OnBlock(Block block)
        {
            WriteBatch batch = new WriteBatch();
            BlockContext context = new BlockContext();
            for (ushort i = 0; i < block.Transactions.Length; i++)
            {
                var tx = block.Transactions[i];
                bool isBiz = false;
                bool isBill = false;
                bool isReplay = false;
                bool isAsk = false;
                if (this.Bapp.IsBizTransaction(tx, out BizTransaction biztx))
                {
                    isBiz = true;
                    ushort? n = null;
                    if (biztx is BillTransaction bt)
                    {
                        isBill = true;
                        foreach (var record in bt.Records)
                        {
                            var bizModel = CasinoBizRecordHelper.BuildModel(record);
                            if (bizModel.Model is CasinoSettingRecord CasinoSettingRecord)
                                batch.Save_CasinoSettingRecord(bizModel, CasinoSettingRecord);
                            else if (bizModel.Model is GameMiningTask gameMiningTask)
                            {
                                batch.Save_GameMiningTask(block, tx, bizModel, gameMiningTask);
                            }
                        }
                    }
                    else if (biztx is ReplyTransaction rt)
                    {
                        isReplay = true;
                        this.OnCasinoReplayTransaction(batch, context, block, rt, i);
                    }
                    else if (biztx is AskTransaction at)
                    {
                        isAsk = true;
                        this.OnCasinoAskTransaction(batch, context, block, at, i, out n);
                    }
                }
                else if (tx is LockAssetTransaction lat)
                {
                    OnLockAssetTransaction(batch, block, i, lat);
                }
                else if (tx is IssueTransaction ist)
                {
                    OnIssueTransaction(batch, block, ist);
                }
                else if (tx is SideTransaction st)
                {
                    OnSideTransaction(batch, block, st);
                }
                else if (tx is RangeTransaction rgt)
                {
                    ushort? n = null;
                    this.OnRangeTransaction(batch, context, block, rgt, i, out n);
                }
                //2.txo
                foreach (KeyValuePair<CoinReference, TransactionOutput> kp in tx.References)
                {
                    //if (kp.Value.AssetId == Blockchain.OXC)
                    //{
                    //    var address = kp.Value.ScriptHash;
                    //    var amt = kp.Value.Value;
                    //    if (this.RoomIds.ContainsKey(address) || this.RoomPoolKeys.ContainsKey(address))
                    //    {
                    //        if (GetWatchBalance(address, out Fixed8 balance))
                    //        {
                    //            batch.BalanceTXO(address, amt, balance);
                    //        }
                    //        else
                    //        {
                    //            batch.BalanceTXO(address, amt);
                    //        }
                    //    }
                    //}
                }
                //2.utxo
                if (tx.Outputs.IsNotNullAndEmpty())
                {
                    for (ushort k = 0; k < tx.Outputs.Length; k++)
                    {
                        TransactionOutput output = tx.Outputs[k];
                        //if (output.AssetId == Blockchain.OXC)
                        //{
                        //    var address = output.ScriptHash;
                        //    var amt = output.Value;
                        //    if (this.RoomIds.ContainsKey(address) || this.RoomPoolKeys.ContainsKey(address))
                        //    {
                        //        if (GetWatchBalance(address, out Fixed8 balance))
                        //        {
                        //            batch.BalanceUTXO(address, amt, balance);
                        //        }
                        //        else
                        //        {
                        //            batch.BalanceUTXO(address, amt);
                        //        }
                        //    }
                        //}
                    }
                }
                //3.从主链筛选出代理托管账户txo
                Dictionary<string, CustodyOutGoldKeyAndAmount> cgaa = new Dictionary<string, CustodyOutGoldKeyAndAmount>();
                foreach (var input in tx.References)
                {
                    var txid = input.Key.PrevHash;
                    var n = input.Key.PrevIndex;
                    OutputKey outputkey = new OutputKey { TxId = txid, N = n };
                    var valetUtxoKey = this.Get<ValetUtxoKey>(CasinoBizPersistencePrefixes.Casino_ValetOutput, outputkey);
                    if (valetUtxoKey.IsNotNull())
                    {
                        batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_ValetOutput).Add(outputkey));
                        batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_ValetUtxo).Add(valetUtxoKey));
                        if (!isAsk && input.Value.AssetId.Equals(Blockchain.OXC))//如果不是投注，那么就是出金
                        {
                            var adr = input.Value.ScriptHash.ToAddress();
                            if (cgaa.TryGetValue(adr, out CustodyOutGoldKeyAndAmount cga))
                            {
                                cga.Amount += input.Value.Value;
                            }
                            else
                            {
                                cga = new CustodyOutGoldKeyAndAmount();
                                cga.CustodyOutGoldKey = new CustodyOutGoldKey { Valet = input.Value.ScriptHash, TxId = tx.Hash, Timestamp = block.Timestamp };
                                cga.Amount = input.Value.Value;
                                cgaa[adr] = cga;
                            }
                        }
                    }
                    if (input.Value.AssetId.Equals(Blockchain.OXS))
                    {
                        ValetOXSClaimKey valetoxsclaimkey = new ValetOXSClaimKey { Valet = input.Value.ScriptHash, TxId = txid, N = n, Value = input.Value.Value };
                        var ValetOXSClaimRecord = this.Get<ValetOXSClaimRecord>(CasinoBizPersistencePrefixes.Casino_Custody_OXS_Claim, valetoxsclaimkey);
                        if (ValetOXSClaimRecord.IsNotNull())
                        {
                            ValetOXSClaimRecord.Flag = 1;
                            ValetOXSClaimRecord.SpendIndex = block.Index;
                            batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Custody_OXS_Claim).Add(valetoxsclaimkey), SliceBuilder.Begin().Add(ValetOXSClaimRecord));
                        }
                    }
                }
                if (cgaa.Count() > 0)
                    batch.Save_CustodyOutGoldRecord(block.Index, tx, cgaa.Values.ToArray());
                //4.从主链筛选出代理托管账户utxo
                Dictionary<string, CustodyInGoldKeyAndAmount> cgba = new Dictionary<string, CustodyInGoldKeyAndAmount>();
                if (tx.Outputs.IsNotNullAndEmpty())
                {
                    for (ushort k = 0; k < tx.Outputs.Length; k++)
                    {
                        TransactionOutput output = tx.Outputs[k];
                        //if (this.ValetRegisters.ContainsKey(output.ScriptHash))
                        //{
                        //    OutputKey outputkey = new OutputKey { TxId = tx.Hash, N = k };
                        //    ValetUtxoKey utxokey = new ValetUtxoKey { Valet = output.ScriptHash, AssetId = output.AssetId, TxId = tx.Hash, N = k };
                        //    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_ValetOutput).Add(outputkey), SliceBuilder.Begin().Add(utxokey));
                        //    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_ValetUtxo).Add(utxokey), SliceBuilder.Begin().Add(new ValetOutputState { Amount = output.Value, Valid = true, ExpireIndex = 0 }));
                        //    if (!isReplay && !isAsk && output.AssetId.Equals(Blockchain.OXC))//如果不是奖励，那么就是入金
                        //    {
                        //        var adr = output.ScriptHash.ToAddress();
                        //        if (cgba.TryGetValue(adr, out CustodyInGoldKeyAndAmount cgb))
                        //        {
                        //            cgb.Amount += output.Value;
                        //        }
                        //        else
                        //        {
                        //            cgb = new CustodyInGoldKeyAndAmount();
                        //            cgb.CustodyInGoldKey = new CustodyInGoldKey { Valet = output.ScriptHash, TxId = tx.Hash, Timestamp = block.Timestamp };
                        //            cgb.Amount = output.Value;
                        //            cgba[adr] = cgb;
                        //        }
                        //    }
                        //    if (output.AssetId.Equals(Blockchain.OXS))
                        //    {
                        //        ValetOXSClaimKey valetoxsclaimkey = new ValetOXSClaimKey { Valet = output.ScriptHash, TxId = tx.Hash, N = k, Value = output.Value };
                        //        batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Custody_OXS_Output).Add(outputkey), SliceBuilder.Begin().Add(valetoxsclaimkey));
                        //        batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Custody_OXS_Claim).Add(valetoxsclaimkey), SliceBuilder.Begin().Add(new ValetOXSClaimRecord { Flag = 0, Index = block.Index, SpendIndex = 0 }));
                        //    }
                        //}

                    }
                }
                if (cgba.Count() > 0)
                    batch.Save_CustodyInGoldRecord(block.Index, tx, cgba.Values.ToArray());
                if (tx is ClaimTransaction claimTx)
                {
                    foreach (var claim in claimTx.Claims)
                    {
                        var outputkey = new OutputKey { TxId = claim.PrevHash, N = claim.PrevIndex };
                        var valetoxsclaimkey = this.Get<ValetOXSClaimKey>(CasinoBizPersistencePrefixes.Casino_Custody_OXS_Output, outputkey);
                        if (valetoxsclaimkey.IsNotNull())
                        {
                            batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Custody_OXS_Claim).Add(valetoxsclaimkey));
                            batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Custody_OXS_Output).Add(outputkey));
                        }
                    }
                }
                else if (tx is EventTransaction eventTx)
                {
                    switch (eventTx.EventType)
                    {
                        case EventType.Board:
                            break;
                        case EventType.Engrave:
                            batch.On_Event_Engrave(block, eventTx, i);
                            break;
                        case EventType.Digg:
                            break;
                    }
                }
            }
            foreach (var gameProvider in gameProviders.Values)
            {
                gameProvider.OnBlock(block, batch);
            }
            this.Db.Write(WriteOptions.Default, batch);
        }

        public void OnCasinoReplayTransaction(WriteBatch batch, BlockContext context, Block block, ReplyTransaction rt, ushort txindex)
        {
            IReadOnlyDictionary<CoinReference, TransactionOutput> rfs = rt.References;
            var shs = rfs.Values.GroupBy(m => m.ScriptHash).Select(n => n.Key.ToAddress());
            var bizshs = this.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();

            //1.验证Tip标签的真实性,不真实则返回不做后续处理
            switch (rt.DataType)
            {
                case (byte)CasinoType.CreateRoomRequest:
                    break;
                case (byte)CasinoType.RoundClear:
                    if (rt.GetDataModel<RoundClear>(bizshs, (byte)CasinoType.RoundClear, out RoundClear roundClear))
                    {
                        batch.Save_RoundClear(this, roundClear, rt);
                    }
                    break;
                case (byte)CasinoType.Bet:

                    break;
                case (byte)CasinoType.RoomStateRequest:

                    break;
                case (byte)CasinoType.RiddlesAndHash:
                    if (rt.GetDataModel<RiddlesAndHash>(bizshs, (byte)CasinoType.RiddlesAndHash, out RiddlesAndHash riddlesandhash))
                    {
                        batch.Save_RiddlesRecord(riddlesandhash.Riddles);
                        batch.Save_RiddlesHashRecord(riddlesandhash.RiddlesHash);
                    }
                    break;
                
                case (byte)CasinoType.ReplyBury:
                    if (rt.GetDataModel<ReplyBury>(bizshs, (byte)CasinoType.ReplyBury, out ReplyBury replyBury))
                    {
                        batch.Save_ReplyBury(this, context, replyBury, block, rt);
                    }
                    break;
            }

        }
        public void OnCasinoAskTransaction(WriteBatch batch, BlockContext context, Block block, AskTransaction at, ushort txindex, out ushort? n)
        {
            if (at.Hash.ToString() == "0x1b9f3a85f4aa2647d07c11fa416df31605c48d4ead12b8d6a7871a2969523564")
            {

            }
            else if (at.Hash.ToString() == "0x3ea43273dc87459c6fde596478c5c6dd993624985f7d3429eff687f8e8a86e47")
            {

            }
            n = null;
            IReadOnlyDictionary<CoinReference, TransactionOutput> rfs = at.References;
            var shs = rfs.Values.GroupBy(m => m.ScriptHash).Select(n => n.Key.ToAddress());
            var bizshs = this.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();

            //1.验证Tip标签的真实性,不真实则返回不做后续处理
            switch (at.DataType)
            {
                case (byte)CasinoType.CreateRoomRequest:
                    break;
                case (byte)CasinoType.RoundClear:

                    break;
                case (byte)CasinoType.Bet:
                    if (at.GetDataModel<BetRequest>(bizshs, (byte)CasinoType.Bet, out BetRequest request))
                    {
                        //this.RoomStates.TryGetValue(request.BetAddress, out RoomStateRequest rsr);
                        if (request.VerifyBetRequest(at, this.MixRooms, out n) && n.HasValue)
                        {
                            batch.Save_Bet(this, block, request, at, n.Value);
                        }
                    }
                    break;
                case (byte)CasinoType.PrivateRoomMemberSetting:
                    if (at.GetDataModel<PrivateRoomMemberSettingRequest>(bizshs, (byte)CasinoType.PrivateRoomMemberSetting, out PrivateRoomMemberSettingRequest memberRequest))
                    {
                        if (this.MixRooms.TryGetValue(memberRequest.BetAddress, out MixRoom room))
                        {
                            if (at.From.Equals(room.HolderPubkey))
                            {
                                room.RoomMemberSetting = memberRequest.RoomMemberSetting;
                                batch.Save_MixRoom(this, room);
                            }
                        }
                    }
                    break;
                 
                case (byte)CasinoType.Bury:
                    if (at.GetDataModel<BuryRequest>(bizshs, (byte)CasinoType.Bury, out BuryRequest buryrequest))
                    {
                        if (buryrequest.VerifyBuryRequest(at, out n) && n.HasValue)
                        {
                            batch.Save_Bury(this, context, buryrequest, at, block.Index, n.Value);
                        }
                    }
                    break;
            }

        }
        public void OnLockAssetTransaction(WriteBatch batch, Block block, ushort txIndex, LockAssetTransaction lat)
        {
            if (lat.LockContract.Equals(Blockchain.LockAssetContractScriptHash))
            {
                OnLockAssetTransactionForSeed(batch, block, lat);
                OnLockAssetTransactionForGameMiningAirdrop(batch, block, lat);
                OnLockAssetTransactionForRoomPartner(batch, block, txIndex, lat);
            }
        }
        public void OnLockAssetTransactionForGameMiningAirdrop(WriteBatch batch, Block block, LockAssetTransaction lat)
        {
            if (!lat.IsTimeLock && lat.IsIssue)
            {
                var sh = Contract.CreateSignatureRedeemScript(lat.Recipient).ToScriptHash();
                var contractSH = lat.GetContract().ScriptHash;
                for (ushort k = 0; k < lat.Outputs.Length; k++)
                {
                    TransactionOutput output = lat.Outputs[k];
                    if (output.ScriptHash.Equals(contractSH) && output.AssetId.Equals(casino.GamblerLuckBonusAsset) && lat.Attributes.IsNotNullAndEmpty())
                    {
                        var attr = lat.Attributes.FirstOrDefault(m => m.Usage == TransactionAttributeUsage.Tip6);
                        if (attr.IsNotNull())
                        {
                            try
                            {
                                var gma = attr.Data.AsSerializable<GameMiningAirdrop>();
                                if (gma.IsNotNull())
                                {
                                    batch.Save_GameMiningAirdrop(this, block, lat, output, k, gma, sh);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }
        public void OnLockAssetTransactionForSeed(WriteBatch batch, Block block, LockAssetTransaction lat)
        {
            if (!lat.IsTimeLock)
            {
                var sh = Contract.CreateSignatureRedeemScript(lat.Recipient).ToScriptHash();
                var contractSH = lat.GetContract().ScriptHash;
                if (lat.Witnesses.Select(m => m.ScriptHash).Contains(sh))//self lock
                {
                    for (ushort k = 0; k < lat.Outputs.Length; k++)
                    {
                        TransactionOutput output = lat.Outputs[k];
                        if (output.ScriptHash.Equals(contractSH))
                        {
                            var attr = lat.Attributes.FirstOrDefault(m => m.Usage == TransactionAttributeUsage.Tip5);
                            if (attr.IsNotNull())
                            {
                                try
                                {
                                    var gms = attr.Data.AsSerializable<GameMiningSeed>();
                                    if (gms.IsNotNull())
                                    {
                                        if (gms.BetIndex > block.Index && gms.BetIndex - block.Index < GameMiningTask.MAXBETRANGE && gms.Position >= 0 && gms.Position <= 9 && output.Value >= GameMiningTask.MINSEED)
                                        {
                                            batch.Save_GameMiningSeed(this, block, lat, output, k, gms, sh);
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
        }
        public void OnIssueTransaction(WriteBatch batch, Block block, IssueTransaction ist)
        {

        }
        public void OnRangeTransaction(WriteBatch batch, BlockContext context, Block block, RangeTransaction rgt, ushort txindex, out ushort? n)
        {
            if (rgt.TryGetBetRequest(out BetRequest request, out TransactionOutput output, out n, out UInt160 fromSH, out ECPoint fromPubKey))
            {
                //this.RoomStates.TryGetValue(request.BetAddress, out RoomStateRequest rsr);
                if (request.VerifyBetRequest(rgt, output, this.MixRooms) && n.HasValue)
                {
                    batch.Save_Bet(this, block, request, rgt, n.Value);
                }
            }
            if (rgt.TryGetBuryRequest(out BuryRequest buryrequest, out TransactionOutput buryoutput, out n, out UInt160 fromBurySH, out ECPoint fromBuryPubKey))
            {
                if (buryrequest.VerifyBuryRequest(buryoutput) && n.HasValue)
                {
                    batch.Save_Bury(this, context, buryrequest, rgt, block.Index, n.Value);
                }
            }
        }

        #endregion

        #region ICasinoBizPersistence

        public IEnumerable<KeyValuePair<byte[], CasinoSettingRecord>> GetAllCasinoSettings()
        {
            return this.Db.Find(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Setting), (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<byte[], CasinoSettingRecord>(ks, data.AsSerializable<CasinoSettingRecord>());
            });
        }
        public IEnumerable<KeyValuePair<byte[], RoomDestroyRecord>> GetAllRoomDestroies()
        {
            return this.Db.Find(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_Destroy), (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<byte[], RoomDestroyRecord>(ks, data.AsSerializable<RoomDestroyRecord>());
            });
        }
        public RoomDestroyRecord GetRoomDestory(uint roomId)
        {
            Slice value;
            if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_Destroy).Add(roomId), out value))
            {
                byte[] data = value.ToArray();
                return data.AsSerializable<RoomDestroyRecord>();
            }
            else
            {
                return default;
            }
        }
        public MixRoom[] GetWalletRooms()
        {
            List<MixRoom> list = new List<MixRoom>();
            foreach (var p in this.MixRooms.Where(m => this.Wallet.ContainsAndHeld(m.Value.Holder)))
            {
                list.Add(p.Value);
            }
            return list.ToArray();
        }
        public MixRoom GetRoom(UInt160 betAddress)
        {
            this.MixRooms.TryGetValue(betAddress, out MixRoom room);
            return room;
        }
        public MixRoom GetRoom(uint roomId)
        {
            if (this.MixRooms.IsNullOrEmpty()) return default;
            var r = this.MixRooms.FirstOrDefault(m => m.Value.RoomId == roomId);
            if (r.Equals(new KeyValuePair<UInt160, MixRoom>())) return default;
            return r.Value;
             
        }
        
        public Riddles GetRiddles(uint index)
        {
            Slice value;
            var rem = index % 1000;
            var indexrange = index - rem;
            IndexRangeKey key = new IndexRangeKey() { IndexRange = indexrange, Index = index };
            if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Riddles).Add(key), out value))
            {
                byte[] data = value.ToArray();
                return data.AsSerializable<Riddles>();
            }
            else
            {
                return default;
            }
        }
        public RiddlesHash GetRiddlesHash(uint index)
        {
            Slice value;
            var rem = index % 1000;
            var indexrange = index - rem;
            IndexRangeKey key = new IndexRangeKey() { IndexRange = indexrange, Index = index };
            if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Riddles_Hash).Add(key), out value))
            {
                byte[] data = value.ToArray();
                return data.AsSerializable<RiddlesHash>();
            }
            else
            {
                return default;
            }
        }
        public IEnumerable<KeyValuePair<IndexRangeKey, RiddlesHash>> GetRangeRiddlesHash(uint indexrange)
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Riddles_Hash).Add(indexrange);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<IndexRangeKey, RiddlesHash>(ks.AsSerializable<IndexRangeKey>(), data.AsSerializable<RiddlesHash>());
            });
        }
        public IEnumerable<KeyValuePair<IndexRangeKey, Riddles>> GetRangeRiddles(uint indexrange)
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Riddles).Add(indexrange);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<IndexRangeKey, Riddles>(ks.AsSerializable<IndexRangeKey>(), data.AsSerializable<Riddles>());
            });
        }
        public IEnumerable<KeyValuePair<UInt160, uint>> GetAllRoomIds()
        {
            return this.Db.Find(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_Address), (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<UInt160, uint>(ks.AsSerializable<UInt160>(), BitConverter.ToUInt32(data));
            });
        }
        public IEnumerable<KeyValuePair<UInt160, RoomKey>> GetAllRoomPools()
        {
            return this.Db.Find(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_PoolAddress), (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<UInt160, RoomKey>(ks.AsSerializable<UInt160>(), data.AsSerializable<RoomKey>());
            });
        }

        public IEnumerable<KeyValuePair<BetKey, Betting>> GetBettings(UInt160 betAddress, uint? index = null)
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bet).Add(betAddress);
            if (index.HasValue)
                builder = builder.Add(index.Value);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<BetKey, Betting>(ks.AsSerializable<BetKey>(), data.AsSerializable<Betting>());
            });
        }
        public Betting GetBetting(BetKey key)
        {
            Slice value;
            if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bet).Add(key), out value))
            {
                byte[] data = value.ToArray();
                return data.AsSerializable<Betting>();
            }
            else
            {
                return default;
            }
        }
        public IEnumerable<KeyValuePair<RoundClearKey, RoundClearResult>> GetRoundClearResults(UInt160 betAddress, uint? index = null, uint? sno = null)
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoundClear).Add(betAddress);
            if (index.HasValue)
            {
                builder = builder.Add(index.Value);
                if (sno.HasValue)
                    builder = builder.Add(sno.Value);
            }
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<RoundClearKey, RoundClearResult>(ks.AsSerializable<RoundClearKey>(), data.AsSerializable<RoundClearResult>());
            });
        }
        public bool GetWatchBalance(UInt160 scriptHash, out Fixed8 amount)
        {
            Slice value;
            if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_WatchBalance).Add(scriptHash), out value))
            {
                byte[] data = value.ToArray();
                amount = data.AsSerializable<Fixed8>();
                return true;
            }
            else
            {
                amount = Fixed8.Zero;
                return false;
            }
        }
        public bool ExistRoomAdmin(RoomAdminKey rak)
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_State_Admin).Add(rak);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                return ks.AsSerializable<RoomAdminKey>();
            }).IsNotNullAndEmpty();
        }
        public IEnumerable<RoomAdminKey> GetRoomAdminsInWallet()
        {
            List<RoomAdminKey> list = new List<RoomAdminKey>();
            foreach (var act in this.Wallet.GetHeldAccounts())
            {
                var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_State_Admin).Add(act.ScriptHash);
                list.AddRange(this.Db.Find(ReadOptions.Default, builder, (k, v) =>
                 {
                     var ks = k.ToArray();
                     var length = ks.Length - sizeof(byte);
                     ks = ks.TakeLast(length).ToArray();
                     return ks.AsSerializable<RoomAdminKey>();
                 }));
            }
            return list.Distinct();
        }
        
        #endregion
        #region Bury
        public uint GetBuryNumber(UInt160 betAddress)
        {
            Slice value;
            if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_Number).Add(betAddress), out value))
            {
                byte[] data = value.ToArray();
                return BitConverter.ToUInt32(data);
            }
            else
            {
                return 0;
            }
        }
        public BuryRecord GetBury(UInt160 BetAddress, uint number)
        {
            Slice value;
            if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury).Add(new BuryKey
            {
                BetAddress = BetAddress,
                Number = number
            }), out value))
            {
                byte[] data = value.ToArray();
                return data.AsSerializable<BuryRecord>();
            }
            else
            {
                return default;
            }
        }
        public BuryMergeTx GetRoomReplyBury(UInt256 txid)
        {
            Slice value;
            if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_ReplyBury).Add(txid), out value))
            {
                byte[] data = value.ToArray();
                return data.AsSerializable<BuryMergeTx>();
            }
            else
            {
                return default;
            }
        }
        public uint GetRoomCodeCount(BuryCodeKey codekey)
        {
            Slice value;
            if (this.Db.TryGet(ReadOptions.Default, SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_CodeCount).Add(codekey), out value))
            {
                byte[] data = value.ToArray();
                return BitConverter.ToUInt32(data);
            }
            else
            {
                return 0;
            }
        }
        public IEnumerable<KeyValuePair<BuryCodeKey, uint>> GetRoomCodeCount(UInt160 betAddress)
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_CodeCount);
            builder = builder.Add(betAddress);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<BuryCodeKey, uint>(ks.AsSerializable<BuryCodeKey>(), BitConverter.ToUInt32(data));
            });
        }
        public IEnumerable<BuryRecord> GetMyBuryRecords(UInt160 betAddress, UInt160 player)
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_MyBury).Add(betAddress).Add(player);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                byte[] data = v.ToArray();
                return data.AsSerializable<BuryRecord>();
            });
        }
        public IEnumerable<BuryMergeTx> GetMyHitBuryRecords(UInt160 betAddress, UInt160 player)
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_MyHit).Add(betAddress).Add(player);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                byte[] data = v.ToArray();
                return data.AsSerializable<BuryMergeTx>();
            });
        }
        public IEnumerable<BuryMergeTx> GetEthMapHitBuryRecords(UInt160 betAddress, UInt160 player)
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_Hit_EthMap).Add(betAddress).Add(player);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                byte[] data = v.ToArray();
                return data.AsSerializable<BuryMergeTx>();
            });
        }
        #endregion
    }
}
