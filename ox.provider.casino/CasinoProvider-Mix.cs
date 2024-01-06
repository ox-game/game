using Akka.Actor;
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
    public partial class CasinoProvider
    {
        public LastRoomId LastRoomId { get; private set; } = new LastRoomId { RoomId = 1000 };
        /// <summary>
        /// key is room's bet address
        /// </summary>
        public Dictionary<UInt160, MixRoom> MixRooms { get; private set; } = new Dictionary<UInt160, MixRoom>();
        public MixRoom[] AllRooms
        {
            get
            {
                return MixRooms.Values.ToArray();
            }
        }
        public void OnSideTransaction(WriteBatch batch, Block block, SideTransaction st)
        {
            if (st.VerifyRegRoom(out ECPoint holderPubKey))
            {
                this.LastRoomId.RoomId++;
                batch.Save_LastRoomId(this.LastRoomId);
                var settings = this.GetAllCasinoSettings();
                var publicfeesetting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PublicRoomFee }));
                if (publicfeesetting.Equals(new KeyValuePair<byte[], CasinoSettingRecord>())) return;
                var publicfee = Fixed8.FromDecimal(decimal.Parse(publicfeesetting.Value.Value));
                Fixed8 privatefee = publicfee;
                var privatefeesetting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PrivateRoomFee }));
                if (!privatefeesetting.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                {
                    privatefee = Fixed8.FromDecimal(decimal.Parse(privatefeesetting.Value.Value));
                }

                if (st.VerifyRegRoomRequest(out RegRoomRequest request))
                {
                    var fee = request.Permission == RoomPermission.Public ? publicfee : privatefee;
                    if (st.VerifyRegRoomFee(fee))
                    {
                        var betSH = st.GetContract().ScriptHash;
                        if (!this.MixRooms.TryGetValue(betSH, out MixRoom room))
                        {
                            var addr = Contract.CreateSignatureRedeemScript(holderPubKey).ToScriptHash();
                            room = new MixRoom
                            {
                                RoomId = this.LastRoomId.RoomId,
                                BetAddress = betSH,
                                PoolAddress = st.GetContractForOtherFlag(1).ScriptHash,
                                FeeAddress = st.GetContractForOtherFlag(2).ScriptHash,
                                BankerAddress = st.GetContractForOtherFlag(3).ScriptHash,
                                Holder = addr,
                                HolderPubkey = holderPubKey,
                                Request = request
                            };
                            RoomMemberSetting rms = default;
                            if (request.DataKind == 1)
                            {
                                try
                                {
                                    rms = request.Data.AsSerializable<RoomMemberSetting>();
                                }
                                catch
                                {

                                }
                            }
                            room.RoomMemberSetting = rms;
                            batch.Save_MixRoom(this, room);
                        }
                    }
                }
            }
        }
        public void OnLockAssetTransactionForRoomPartner(WriteBatch batch, Block block, ushort txIndex, LockAssetTransaction lat)
        {
            if (!lat.IsTimeLock && !lat.IsIssue)
            {
                var shs = lat.RelatedScriptHashes;
                if (shs.IsNotNullAndEmpty())
                {
                    var sh = shs.FirstOrDefault();
                    if (this.MixRooms.TryGetValue(sh, out MixRoom room))
                    {
                        var recepientSH = Contract.CreateSignatureRedeemScript(lat.Recipient).ToScriptHash();
                        var contractSH = lat.GetContract().ScriptHash;
                        for (ushort k = 0; k < lat.Outputs.Length; k++)
                        {
                            TransactionOutput output = lat.Outputs[k];
                            if (output.ScriptHash.Equals(contractSH) && output.AssetId.Equals(Blockchain.OXS))
                            {
                                var settings = this.GetAllCasinoSettings();
                                var RoomOXSMinLock = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.RoomOXSMinLock }));
                                if (RoomOXSMinLock.Equals(new KeyValuePair<byte[], CasinoSettingRecord>())) return;
                                var RoomOXSMinLockAmount = Fixed8.FromDecimal(decimal.Parse(RoomOXSMinLock.Value.Value));
                                var PublicRoomPledgePeriod = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PublicRoomPledgePeriod }));
                                if (PublicRoomPledgePeriod.Equals(new KeyValuePair<byte[], CasinoSettingRecord>())) return;
                                var PublicRoomPledgePeriodBlocks = uint.Parse(PublicRoomPledgePeriod.Value.Value);
                                var PrivateRoomPledgePeriodBlocks = PublicRoomPledgePeriodBlocks;
                                var PrivateRoomPledgePeriod = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PrivateRoomPledgePeriod }));
                                if (!PrivateRoomPledgePeriod.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                                {
                                    PrivateRoomPledgePeriodBlocks = uint.Parse(PrivateRoomPledgePeriod.Value.Value);
                                }

                                var blockPeriod = room.Request.Permission == RoomPermission.Public ? PublicRoomPledgePeriodBlocks : PrivateRoomPledgePeriodBlocks;

                                if (lat.LockExpiration - block.Index >= blockPeriod && output.Value >= RoomOXSMinLockAmount)
                                {
                                    batch.Save_RoomPartnerLockRecord(this, block, txIndex, room, lat, output, recepientSH);
                                }
                            }
                        }
                    }
                }
            }
        }


        public IEnumerable<KeyValuePair<UInt160, MixRoom>> GetRooms()
        {
            var builder = SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room);
            return this.Db.Find(ReadOptions.Default, builder, (k, v) =>
            {
                var ks = k.ToArray();
                var length = ks.Length - sizeof(byte);
                ks = ks.TakeLast(length).ToArray();
                byte[] data = v.ToArray();
                return new KeyValuePair<UInt160, MixRoom>(ks.AsSerializable<UInt160>(), data.AsSerializable<MixRoom>());
            });
        }
        public IEnumerable<KeyValuePair<RoomPartnerLockRecord, LockAssetTransaction>> GetRoomPartnerLockRecords(UInt160 betAddress)
        {
            return this.GetAll<RoomPartnerLockRecord, LockAssetTransaction>(CasinoBizPersistencePrefixes.Casino_RoomPartnerLock_Record, betAddress);
        }
    }
}
