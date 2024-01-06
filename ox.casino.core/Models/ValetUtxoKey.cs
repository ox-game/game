using OX.IO;
using System.IO;

namespace OX
{
    public class OutputKey : ISerializable
    {
        public UInt256 TxId;
        public ushort N;
        public virtual int Size => TxId.Size + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TxId);
            writer.Write(N);
        }
        public void Deserialize(BinaryReader reader)
        {
            TxId = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
        }
    }
    public class ValetUtxoKey : ISerializable
    {
        public UInt160 Valet;
        public UInt256 AssetId;
        public UInt256 TxId;
        public ushort N;
        public virtual int Size => Valet.Size + AssetId.Size + TxId.Size + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Valet);
            writer.Write(AssetId);
            writer.Write(TxId);
            writer.Write(N);
        }
        public void Deserialize(BinaryReader reader)
        {
            Valet = reader.ReadSerializable<UInt160>();
            AssetId = reader.ReadSerializable<UInt256>();
            TxId = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
        }
    }
    public class ValetOutputState : ISerializable
    {
        public Fixed8 Amount;
        public bool Valid;
        public uint ExpireIndex;
        public virtual int Size => Amount.Size + sizeof(bool) + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Amount);
            writer.Write(Valid);
            writer.Write(ExpireIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            Amount = reader.ReadSerializable<Fixed8>();
            Valid = reader.ReadBoolean();
            ExpireIndex = reader.ReadUInt32();
        }
    }
    public class ValetUtxoMerge
    {
        public ValetUtxoKey ValetUtxoKey;
        public ValetOutputState ValetOutputState;
    }
    public class CustodyOutGoldKey : ISerializable
    {
        public UInt160 Valet;
        public UInt256 TxId;
        public uint Timestamp;
        public virtual int Size => Valet.Size + TxId.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Valet);
            writer.Write(TxId);
            writer.Write(Timestamp);
        }
        public void Deserialize(BinaryReader reader)
        {
            Valet = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
            Timestamp = reader.ReadUInt32();
        }
    }
    public class CustodyOutGoldKeyAndAmount : ISerializable
    {
        public CustodyOutGoldKey CustodyOutGoldKey;
        public Fixed8 Amount;
        public virtual int Size => CustodyOutGoldKey.Size + Amount.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(CustodyOutGoldKey);
            writer.Write(Amount);
        }
        public void Deserialize(BinaryReader reader)
        {
            CustodyOutGoldKey = reader.ReadSerializable<CustodyOutGoldKey>();
            Amount = reader.ReadSerializable<Fixed8>();
        }
    }
    public class CustodyInGoldKey : ISerializable
    {
        public UInt160 Valet;
        public UInt256 TxId;
        public uint Timestamp;
        public virtual int Size => Valet.Size + TxId.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Valet);
            writer.Write(TxId);
            writer.Write(Timestamp);
        }
        public void Deserialize(BinaryReader reader)
        {
            Valet = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
            Timestamp = reader.ReadUInt32();
        }
    }
    public class CustodyInGoldKeyAndAmount : ISerializable
    {
        public CustodyInGoldKey CustodyInGoldKey;
        public Fixed8 Amount;
        public virtual int Size => CustodyInGoldKey.Size + Amount.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(CustodyInGoldKey);
            writer.Write(Amount);
        }
        public void Deserialize(BinaryReader reader)
        {
            CustodyInGoldKey = reader.ReadSerializable<CustodyInGoldKey>();
            Amount = reader.ReadSerializable<Fixed8>();
        }
    }
    public class ValetOXSClaimKey : ISerializable
    {
        public UInt160 Valet;
        public UInt256 TxId;
        public ushort N;
        public Fixed8 Value;
        public virtual int Size => Valet.Size + TxId.Size + sizeof(ushort) + Value.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Valet);
            writer.Write(TxId);
            writer.Write(N);
            writer.Write(Value);
        }
        public void Deserialize(BinaryReader reader)
        {
            Valet = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
            Value = reader.ReadSerializable<Fixed8>();
        }
    }
    public class ValetOXSClaimRecord : ISerializable
    {
        public byte Flag;
        public uint Index;
        public uint SpendIndex;
        public virtual int Size => sizeof(byte) + sizeof(uint) + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Flag);
            writer.Write(Index);
            writer.Write(SpendIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            Flag = reader.ReadByte();
            Index = reader.ReadUInt32();
            SpendIndex = reader.ReadUInt32();
        }
    }
}
