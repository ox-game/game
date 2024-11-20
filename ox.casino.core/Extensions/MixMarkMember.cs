using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using OX.Cryptography.ECC;
using System.Security.Policy;
using System.IO;
using OX.Network.P2P;
using OX.Wallets;

namespace OX.BMS
{
    public class MixMarkMember : ISerializable
    {
        public uint MarkMemberId;
        public UInt160 BetAddress;
        public UInt160 PoolAddress;
        public UInt160 FeeAddress;
        public UInt160 PledgeAddress;
        public UInt160 DepositAddress;
        public UInt160 Holder;
        public ECPoint HolderPubkey;
        public RegMarkMemberRequest Request;
        public MarkSetting MarkSetting;
        public uint ExpireTimeStamp;
        public ulong TotalBetAmount;
        public ulong TotalPrizeAmount;

        public virtual int Size => sizeof(uint) + BetAddress.Size + PoolAddress.Size + FeeAddress.Size
            + PledgeAddress.Size+DepositAddress.Size + Holder.Size + HolderPubkey.Size + Request.Size + (MarkSetting.IsNotNull() ? MarkSetting.Size : 0) + sizeof(uint) + sizeof(ulong) + sizeof(ulong);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MarkMemberId);
            writer.Write(BetAddress);
            writer.Write(PoolAddress);
            writer.Write(FeeAddress);
            writer.Write(PledgeAddress);
            writer.Write(DepositAddress);
            writer.Write(Holder);
            writer.Write(HolderPubkey);
            writer.Write(Request);
            if (MarkSetting.IsNotNull())
            {
                writer.Write((uint)MarkSetting.Size);
                writer.Write(MarkSetting);
            }
            else
                writer.Write((uint)0);
            writer.Write(ExpireTimeStamp);
            writer.Write(TotalBetAmount);
            writer.Write(TotalPrizeAmount);
        }
        public void Deserialize(BinaryReader reader)
        {
            MarkMemberId = reader.ReadUInt32();
            BetAddress = reader.ReadSerializable<UInt160>();
            PoolAddress = reader.ReadSerializable<UInt160>();
            FeeAddress = reader.ReadSerializable<UInt160>();
            PledgeAddress = reader.ReadSerializable<UInt160>();
            DepositAddress = reader.ReadSerializable<UInt160>();
            Holder = reader.ReadSerializable<UInt160>();
            HolderPubkey = reader.ReadSerializable<ECPoint>();
            Request = reader.ReadSerializable<RegMarkMemberRequest>();
            uint d = reader.ReadUInt32();
            if (d > 0)
            {
                MarkSetting = reader.ReadSerializable<MarkSetting>();
            }
            ExpireTimeStamp = reader.ReadUInt32();
            TotalBetAmount = reader.ReadUInt64();
            TotalPrizeAmount = reader.ReadUInt64();
        }
        public override bool Equals(object obj)
        {
            if (obj is MixMarkMember mr)
            {
                return mr.BetAddress == this.BetAddress;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.BetAddress.GetHashCode();
        }
        public override string ToString()
        {
            return this.BetAddress.ToAddress();
        }
    }
}
