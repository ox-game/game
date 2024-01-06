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

namespace OX.Casino
{
    public class MixRoom : ISerializable
    {
        public uint RoomId;
        public UInt160 BetAddress;
        public UInt160 PoolAddress;
        public UInt160 FeeAddress;
        public UInt160 BankerAddress;
        public UInt160 Holder;
        public ECPoint HolderPubkey;
        public RegRoomRequest Request;
        public RoomMemberSetting RoomMemberSetting;

        public virtual int Size => sizeof(uint) + BetAddress.Size + PoolAddress.Size + FeeAddress.Size
            + BankerAddress.Size + Holder.Size + HolderPubkey.Size + Request.Size + (RoomMemberSetting.IsNotNull() ? RoomMemberSetting.Size : 0);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RoomId);
            writer.Write(BetAddress);
            writer.Write(PoolAddress);
            writer.Write(FeeAddress);
            writer.Write(BankerAddress);
            writer.Write(Holder);
            writer.Write(HolderPubkey);
            writer.Write(Request);
            if (RoomMemberSetting.IsNotNull())
            {
                writer.Write((uint)RoomMemberSetting.Size);
                writer.Write(RoomMemberSetting);
            }
            else
                writer.Write((uint)0);
        }
        public void Deserialize(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
            BetAddress = reader.ReadSerializable<UInt160>();
            PoolAddress = reader.ReadSerializable<UInt160>();
            FeeAddress = reader.ReadSerializable<UInt160>();
            BankerAddress = reader.ReadSerializable<UInt160>();
            Holder = reader.ReadSerializable<UInt160>();
            HolderPubkey = reader.ReadSerializable<ECPoint>();
            Request = reader.ReadSerializable<RegRoomRequest>();
            uint d = reader.ReadUInt32();
            if (d > 0)
            {
                RoomMemberSetting = reader.ReadSerializable<RoomMemberSetting>();
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is MixRoom mr)
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
