using OX.IO;
using OX.Wallets;
using System.IO;

namespace OX
{
    public class BuryCodeKeyMergeCount
    {
        public BuryCodeKey BuryCodeKey;
        public uint Count;
    }
    public class BuryCodeKey : ISerializable
    {
        public UInt160 BetAddress;
        public byte CodeKind;
        public byte Code;

        public virtual int Size => BetAddress.Size + sizeof(byte) + sizeof(byte);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(CodeKind);
            writer.Write(Code);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            CodeKind = reader.ReadByte();
            Code = reader.ReadByte();
        }
        public override bool Equals(object obj)
        {
            if (obj is BuryCodeKey bck)
            {
                return this.BetAddress == bck.BetAddress && this.CodeKind == bck.CodeKind && this.Code == bck.Code;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return (BetAddress.GetHashCode() + Code * 10 + CodeKind).GetHashCode();
        }
        public override string ToString()
        {
            return $"{this.BetAddress.ToAddress()}-{this.CodeKind}-{this.Code}";
        }
    }
    public class MyBuryKey : ISerializable
    {
        public UInt160 BetAddress;
        public UInt160 Player;
        public UInt256 TxId;

        public virtual int Size => BetAddress.Size + Player.Size + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Player);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Player = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
        }

    }
    public class BuryKey : ISerializable
    {
        public UInt160 BetAddress;
        public uint Number;

        public virtual int Size => BetAddress.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Number);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Number = reader.ReadUInt32();
        }
    }

    public class BuryRecord : ISerializable
    {
        public uint BlockIndex;
        public ushort N;
        public Fixed8 BuryAmount;
        public UInt256 TxId;
        public BuryRequest Request;

        public virtual int Size => sizeof(uint) + sizeof(ushort) + BuryAmount.Size + TxId.Size + Request.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BlockIndex);
            writer.Write(N);
            writer.Write(BuryAmount);
            writer.Write(TxId);
            writer.Write(Request);
        }
        public void Deserialize(BinaryReader reader)
        {
            BlockIndex = reader.ReadUInt32();
            N = reader.ReadUInt16();
            BuryAmount = reader.ReadSerializable<Fixed8>();
            TxId = reader.ReadSerializable<UInt256>();
            Request = reader.ReadSerializable<BuryRequest>();
        }
    }
}
