using OX.IO;
using OX.IO.Data.LevelDB;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX;
using OX.Ledger;
using System.Linq;
using System.Runtime;
using OX.Wallets;

namespace OX.Casino
{
    public static partial class CasinoPersistenceHelper
    {
        public static void Save_CasinoSettingRecord(this WriteBatch batch, BizRecordModel model, CasinoSettingRecord record)
        {
            if (record != default && record.Value != default)
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Setting).Add(model.Key), SliceBuilder.Begin().Add(record));
        }
        //public static void Save_RoomDestroyRecord(this WriteBatch batch, BizRecordModel model, RoomDestroyRecord record)
        //{
        //    if (record != default)
        //        batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_Destroy).Add(model.Key), SliceBuilder.Begin().Add(record));
        //}

        public static void Save_RiddlesHashRecord(this WriteBatch batch, RiddlesHash record)
        {
            if (record != default)
            {
                var rem = record.Index % 1000;
                var indexrange = record.Index - rem;
                IndexRangeKey key = new IndexRangeKey() { IndexRange = indexrange, Index = record.Index };
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Riddles_Hash).Add(key), SliceBuilder.Begin().Add(record));
            }
        }
        public static void Save_RiddlesRecord(this WriteBatch batch, Riddles record)
        {
            if (record != default)
            {
                var rem = record.Index % 1000;
                var indexrange = record.Index - rem;
                IndexRangeKey key = new IndexRangeKey() { IndexRange = indexrange, Index = record.Index };
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Riddles).Add(key), SliceBuilder.Begin().Add(record));
            }
        }
        public static void Save_Bet(this WriteBatch batch, CasinoProvider provider, Block block, BetRequest request, Transaction at, ushort n)
        {
            if (request.IsNotNull())
            {
                var output = at.Outputs[n];
                BetKey key = new BetKey
                {
                    BetAddress = request.BetAddress,
                    Index = request.Index,
                    TxHash = at.Hash,
                    //N = n
                };
                Betting bet = new Betting()
                {
                    Amount = output.Value,
                    BetRequest = request,
                    TxId = at.Hash
                };
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bet).Add(key), SliceBuilder.Begin().Add(bet));
                BetSummaryKey betsummarykey = new BetSummaryKey
                {
                    Gambler = request.From,
                    BetAddress = request.BetAddress
                };
                BetLandlordSummaryKey betlandlordsummarykey = new BetLandlordSummaryKey
                {
                    Gambler = request.From,
                    BetAddress = request.BetAddress
                };
                Fixed8 betAmount = output.Value;
                var amt = provider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Bet_Summary, betsummarykey);
                if (amt != default)
                {
                    betAmount += amt;
                }
                Fixed8 landlordbetAmount = output.Value;
                var landlordamt = provider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Landlord_Bet_Summary, betlandlordsummarykey);
                if (landlordamt != default)
                {
                    landlordbetAmount += landlordamt;
                }
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bet_Summary).Add(betsummarykey), SliceBuilder.Begin().Add(betAmount));
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Landlord_Bet_Summary).Add(betlandlordsummarykey), SliceBuilder.Begin().Add(landlordbetAmount));
            }
        }
        //public static void Save_RoomState(this WriteBatch batch, CasinoProvider provider, SignatureValidator<RoomStateRequest> request, AskTransaction at)
        //{
        //    if (request.IsNotNull())
        //    {
        //        var state = provider.GetRoomState(request.Target.RoomId);
        //        if (state.IsNotNull())
        //        {
        //            if (state.Admins.IsNotNullAndEmpty())
        //            {
        //                foreach (var ad in state.Admins)
        //                {
        //                    RoomAdminKey key = new RoomAdminKey() { Administrator = Contract.CreateSignatureRedeemScript(ad).ToScriptHash(), RoomId = request.Target.RoomId };
        //                    batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_State_Admin).Add(key));
        //                }
        //            }
        //        }
        //        batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_State).Add(request.Target.RoomId), SliceBuilder.Begin().Add(request.Target));
        //        if (request.Target.Admins.IsNotNullAndEmpty())
        //        {
        //            foreach (var adm in request.Target.Admins)
        //            {
        //                RoomAdminKey key = new RoomAdminKey() { Administrator = Contract.CreateSignatureRedeemScript(adm).ToScriptHash(), RoomId = request.Target.RoomId };
        //                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_State_Admin).Add(key), SliceBuilder.Begin(0));
        //            }
        //        }
        //    }
        //}
        //public static void Save_RoomSplit(this WriteBatch batch, SignatureValidator<RoomSplitRequest> request, AskTransaction at, Block block)
        //{
        //    if (request.IsNotNull())
        //    {
        //        batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room_Split).Add(request.Target.RoomId), SliceBuilder.Begin().Add(block.Index));
        //    }
        //}
        public static void Save_RoundClear(this WriteBatch batch, CasinoProvider provider, RoundClear roundClear, ReplyTransaction rt)
        {
            if (roundClear.IsNotNull())
            {
                RoundClearKey key = new RoundClearKey()
                {
                    BetAddress = roundClear.BetAddress,
                    Index = roundClear.Index,
                    SNO = roundClear.SNO
                };
                RoundClearResult rcr = new RoundClearResult() { RoundClear = roundClear, TxHash = rt.Hash };
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoundClear).Add(key), SliceBuilder.Begin().Add(rcr));
                foreach (var output in rt.Outputs)
                {
                    PrizeSummaryKey prizesummarykey = new PrizeSummaryKey
                    {
                        Gambler = output.ScriptHash,
                        BetAddress = roundClear.BetAddress
                    };
                    PrizeLandlordSummaryKey prizelandlordsummarykey = new PrizeLandlordSummaryKey
                    {
                        Gambler = output.ScriptHash,
                        BetAddress = roundClear.BetAddress
                    };
                    Fixed8 prizeAmount = output.Value;
                    var amt = provider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Prize_Summary, prizesummarykey);
                    if (amt != default)
                    {
                        prizeAmount += amt;
                    }
                    Fixed8 prizeLandlordAmount = output.Value;
                    var landlordamt = provider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Landlord_Prize_Summary, prizelandlordsummarykey);
                    if (landlordamt != default)
                    {
                        prizeLandlordAmount += landlordamt;
                    }
                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Prize_Summary).Add(prizesummarykey), SliceBuilder.Begin().Add(prizeAmount));
                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Landlord_Prize_Summary).Add(prizelandlordsummarykey), SliceBuilder.Begin().Add(prizeLandlordAmount));
                }
            }
        }
        public static void BalanceTXO(this WriteBatch batch, UInt160 sh, Fixed8 amount, Fixed8? balance = null)
        {
            if (balance.HasValue && balance.Value > amount)
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_WatchBalance).Add(sh), SliceBuilder.Begin().Add(balance.Value - amount));
        }
        public static void BalanceUTXO(this WriteBatch batch, UInt160 sh, Fixed8 amount, Fixed8? balance = null)
        {
            batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_WatchBalance).Add(sh), SliceBuilder.Begin().Add(balance.HasValue ? balance.Value + amount : amount));
        }
        //public static void Save_ValetRegister(this WriteBatch batch, CasinoProvider provider, ValetRegister valetregister, AskTransaction at)
        //{
        //    if (valetregister.IsNotNull())
        //    {
        //        var fromAddress = Contract.CreateSignatureRedeemScript(at.From).ToScriptHash();
        //        if (provider.Wallet.IsNotNull())
        //        {
        //            if (!provider.Wallet.GetHeldAccounts().Select(m => m.ScriptHash).Contains(fromAddress)) return;
        //        }
        //        if (provider.ValetRegisters.ContainsKey(valetregister.Valet)) return;
        //        ValetRegisterKey valetregisterkey = new ValetRegisterKey { Beneficiary = valetregister.Beneficiary, RegisterFrom = fromAddress };
        //        batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_ValetRegister).Add(valetregister.Valet), SliceBuilder.Begin().Add(valetregisterkey));
        //        provider.ValetRegisters[valetregister.Valet] = valetregisterkey;
        //    }
        //}
        public static void Save_CustodyOutGoldRecord(this WriteBatch batch, uint index, Transaction tx, CustodyOutGoldKeyAndAmount[] cgaa)
        {
            foreach (var cga in cgaa)
                if (cga.Amount > Fixed8.Zero)
                {
                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Custody_OutGod_Record).Add(cga), SliceBuilder.Begin().Add(cga.Amount));
                }
        }
        public static void Save_CustodyInGoldRecord(this WriteBatch batch, uint index, Transaction tx, CustodyInGoldKeyAndAmount[] cgba)
        {
            foreach (var cgb in cgba)
                if (cgb.Amount > Fixed8.Zero)
                {
                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Custody_InGod_Record).Add(cgb), SliceBuilder.Begin().Add(cgb.Amount));
                }
        }
        //public static void Save_RoomPledgeAccountReply(this WriteBatch batch, CasinoProvider provider, RoomPledgeAccountReply reply)
        //{
        //    if (reply != default)
        //    {
        //        batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoomPledgeAccountReply).Add(reply.RoomId), SliceBuilder.Begin().Add(reply));
        //        var sh = Contract.CreateSignatureRedeemScript(reply.Address).ToScriptHash();
        //        provider.RoomPledges[sh] = reply;
        //    }
        //}
        //public static void Save_CreateRoomGuaranteeRequest(this WriteBatch batch, CasinoProvider provider, CreateRoomGuaranteeRequest createRoomGuaranteeRequest, AskTransaction at, uint blockindex, ushort txindex)
        //{
        //    if (createRoomGuaranteeRequest.IsNotNull() && at.Outputs.IsNotNullAndEmpty())
        //    {
        //        var pledge = provider.RoomPledges.Values.FirstOrDefault(m => m.RoomId == createRoomGuaranteeRequest.RoomId);
        //        if (pledge.IsNotNull())
        //        {
        //            var pledgeAdr = Contract.CreateSignatureRedeemScript(pledge.Address).ToScriptHash();
        //            for (ushort k = 0; k < at.Outputs.Length; k++)
        //            {
        //                var output = at.Outputs[k];
        //                if (pledgeAdr.Equals(output.ScriptHash) && output.AssetId.Equals(Blockchain.OXS))
        //                {
        //                    //var fromAddress = Contract.CreateSignatureRedeemScript(at.From).ToScriptHash();
        //                    RoomPledgeGuarantee guarantee = new RoomPledgeGuarantee { PledgeGuarantor = createRoomGuaranteeRequest.Guarantor, PledgeAccount = pledgeAdr, RoomId = pledge.RoomId, TxId = at.Hash, N = k, Value = output.Value };
        //                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoomPledge_Guarantee_Record).Add(at.Hash), SliceBuilder.Begin().Add(guarantee));
        //                    var r = new RoomPledgeGuaranteeRequestAndOutput { CreateRoomGuaranteeRequest = createRoomGuaranteeRequest, Output = output, TxId = at.Hash, BlockIndex = blockindex, TxIndex = txindex };
        //                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoomPledge_Guarantor).Add(guarantee), SliceBuilder.Begin().Add(r));
        //                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoomPledge_Guarantee_RoomId).Add(new UintHashKey { Value = createRoomGuaranteeRequest.RoomId, TxId = at.Hash }), SliceBuilder.Begin().Add(r));
        //                }
        //            }
        //        }
        //    }
        //}

        //public static void Save_RoomGuaranteeReply(this WriteBatch batch, CasinoProvider provider, RoomGuaranteeReply reply)
        //{
        //    if (reply != default)
        //    {
        //        var guarantee = provider.Get<RoomPledgeGuarantee>(CasinoBizPersistencePrefixes.Casino_RoomPledge_Guarantee_Record, reply.TxId);
        //        if (guarantee.IsNotNull())
        //        {
        //            batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoomPledge_Guarantee_Record).Add(reply.TxId));
        //            batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoomPledge_Guarantor).Add(guarantee));
        //            batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoomPledge_Guarantee_RoomId).Add(new UintHashKey { Value = guarantee.RoomId, TxId = reply.TxId }));
        //        }
        //    }
        //}
        public static void On_Event_Engrave(this WriteBatch batch, Block block, EventTransaction model, ushort n)
        {
            if (model.IsNotNull())
            {
                try
                {
                    var engrave = model.Data.AsSerializable<Engrave>();
                    if (engrave.IsNotNull())
                    {
                        var k = $"{engrave.BoardTxIndex}-{engrave.BoardTxPosition}";
                        if (k == casino.OfficalEventBoardId)
                            OX.Bapps.Bapp.PushCrossBappMessage(new OX.Bapps.CrossBappMessage() { MessageType = 1, Attachment = casino.OfficalEventBoardId });
                    }
                }
                catch
                {

                }
            }
        }
        #region bury
        public static void Save_Bury(this WriteBatch batch, CasinoProvider provider, BlockContext context, BuryRequest request, Transaction at, uint blockIndex, ushort n)
        {
            if (request.IsNotNull())
            {
                var output = at.Outputs[n];
                Fixed8 buryAmount = output.Value;
                provider.BuryNumber++;
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_Number).Add(request.BetAddress), SliceBuilder.Begin().Add(provider.BuryNumber));
                if (provider.BuryNumber > 200)
                {
                    var bk = new BuryKey { BetAddress = request.BetAddress, Number = provider.BuryNumber - 200 };
                    var record = provider.GetBury(bk.BetAddress, bk.Number);
                    batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury).Add(bk));
                    if (record.IsNotNull())
                        batch.Delete(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_ReplyBury).Add(record.TxId));
                }
                BuryKey key = new BuryKey
                {
                    BetAddress = request.BetAddress,
                    Number = provider.BuryNumber
                };
                BuryRecord br = new BuryRecord
                {
                    BlockIndex = blockIndex,
                    N = n,
                    BuryAmount = buryAmount,
                    TxId = at.Hash,
                    Request = request
                };
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury).Add(key), SliceBuilder.Begin().Add(br));
                BuryCodeKey codekey = new BuryCodeKey { BetAddress = request.BetAddress, CodeKind = 0, Code = request.PlainBuryPoint };
                if (!context.Items.TryGetValue(codekey.ToString(), out object obj))
                {
                    var c = provider.GetRoomCodeCount(codekey);
                    obj = c;
                    context.Items[codekey.ToString()] = obj;
                }
                var count = (uint)obj;
                count++;
                context.Items[codekey.ToString()] = count;
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_CodeCount).Add(codekey), SliceBuilder.Begin().Add(count));
                if (provider.Wallet.IsNotNull() && provider.Wallet.ContainsAndHeld(request.From))
                {
                    MyBuryKey mbk = new MyBuryKey { BetAddress = request.BetAddress, Player = request.From, TxId = at.Hash };
                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_MyBury).Add(mbk), SliceBuilder.Begin().Add(br));
                }
            }
        }
        public static void Save_ReplyBury(this WriteBatch batch, CasinoProvider provider, BlockContext context, ReplyBury replyBury, Block block, ReplyTransaction rt)
        {
            if (replyBury.IsNotNull())
            {
                var input = rt.Inputs.FirstOrDefault();
                BuryMergeTx BuryMergeTx = new BuryMergeTx { ReplyBury = replyBury, Input = input, Outputs = rt.Outputs, BlockIndex = block.Index };
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_ReplyBury).Add(input.PrevHash), SliceBuilder.Begin().Add(BuryMergeTx));
                BuryCodeKey codekey = new BuryCodeKey { BetAddress = replyBury.BetAddress, CodeKind = 1, Code = replyBury.PrivateBuryRequest.CipherBuryPoint };
                if (!context.Items.TryGetValue(codekey.ToString(), out object obj))
                {
                    var c = provider.GetRoomCodeCount(codekey);
                    obj = c;
                    context.Items[codekey.ToString()] = obj;
                }
                var count = (uint)obj;
                count++;
                context.Items[codekey.ToString()] = count;
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_CodeCount).Add(codekey), SliceBuilder.Begin().Add(count));
                if (provider.Wallet.IsNotNull())
                {
                    if (!provider.Wallet.ContainsAndHeld(rt.To))
                    {
                        var output = rt.Outputs.FirstOrDefault(m => !m.ScriptHash.Equals(rt.To) && provider.Wallet.ContainsAndHeld(m.ScriptHash));
                        if (output.IsNotNull())
                        {
                            MyBuryKey mbk = new MyBuryKey { BetAddress = replyBury.BetAddress, Player = output.ScriptHash, TxId = rt.Hash };
                            batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_MyHit).Add(mbk), SliceBuilder.Begin().Add(BuryMergeTx));
                        }
                    }
                    if (OXRunTime.RunMode == RunMode.Server)
                    {
                        var output = rt.Outputs.FirstOrDefault(m => !m.ScriptHash.Equals(rt.To));
                        if (output.IsNotNull())
                        {
                            MyBuryKey mbk = new MyBuryKey { BetAddress = replyBury.BetAddress, Player = output.ScriptHash, TxId = rt.Hash };
                            batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Bury_Hit_EthMap).Add(mbk), SliceBuilder.Begin().Add(BuryMergeTx));
                        }
                    }
                }
            }
        }
        public static void Save_GameMiningTask(this WriteBatch batch, Block block, Transaction tx, BizRecordModel model, GameMiningTask record)
        {
            if (record != default)
            {
                var key = model.Key.AsSerializable<GameMiningTaskKey>();
                if (key.IsNotNull())
                {
                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_GameMining_Task).Add(model.Key), SliceBuilder.Begin().Add(record));
                }
            }
        }
        public static void Save_GameMiningSeed(this WriteBatch batch, CasinoProvider provider, Block block, LockAssetTransaction lat, TransactionOutput output, ushort N, GameMiningSeed seed, UInt160 sh)
        {
            if (seed != default)
            {
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_GameMining_Seed).Add(new GameMiningSeedKey { Seed = seed, TxId = lat.Hash, N = N }), SliceBuilder.Begin().Add(new GameMiningSeedValue { LockExpiration = lat.LockExpiration, AssetId = output.AssetId, Amount = output.Value, Player = sh }));
            }
        }
        public static void Save_GameMiningAirdrop(this WriteBatch batch, CasinoProvider provider, Block block, LockAssetTransaction lat, TransactionOutput output, ushort N, GameMiningAirdrop airdrop, UInt160 sh)
        {
            if (airdrop != default)
            {
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_GameMining_Airdrop).Add(airdrop), SliceBuilder.Begin().Add(new GameMiningAirdropResult { LockExpiration = lat.LockExpiration, AssetId = output.AssetId, Amount = output.Value, Winner = sh }));
            }
        }

        #endregion
    }
}
