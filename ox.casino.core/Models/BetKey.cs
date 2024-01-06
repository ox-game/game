using OX.IO;
using System.IO;

namespace OX
{
    public class BetKey : ISerializable
    {
        public UInt160 BetAddress;
        public uint Index;
        public UInt256 TxHash;
        //public ushort N;
        public virtual int Size => BetAddress.Size + sizeof(uint) + TxHash.Size /*+ sizeof(ushort)*/;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Index);
            writer.Write(TxHash);
            //writer.Write(N);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Index = reader.ReadUInt32();
            TxHash = reader.ReadSerializable<UInt256>();
            //N = reader.ReadUInt16();
        }
    }
    public class BetSummaryKey : ISerializable
    {
        public UInt160 Gambler;
        public UInt160 BetAddress;

        public virtual int Size => Gambler.Size + BetAddress.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Gambler);
            writer.Write(BetAddress);
        }
        public void Deserialize(BinaryReader reader)
        {
            Gambler = reader.ReadSerializable<UInt160>();
            BetAddress = reader.ReadSerializable<UInt160>();
        }
    }
    public class BetLandlordSummaryKey : ISerializable
    {
        public UInt160 BetAddress;
        public UInt160 Gambler;

        public virtual int Size => BetAddress.Size + Gambler.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Gambler);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Gambler = reader.ReadSerializable<UInt160>();
        }
    }
    public class PrizeSummaryKey : ISerializable
    {
        public UInt160 Gambler;
        public UInt160 BetAddress;

        public virtual int Size => Gambler.Size + BetAddress.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Gambler);
            writer.Write(BetAddress);
        }
        public void Deserialize(BinaryReader reader)
        {
            Gambler = reader.ReadSerializable<UInt160>();
            BetAddress = reader.ReadSerializable<UInt160>();
        }
    }
    public class PrizeLandlordSummaryKey : ISerializable
    {
        public UInt160 BetAddress;
        public UInt160 Gambler;


        public virtual int Size => BetAddress.Size + Gambler.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Gambler);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Gambler = reader.ReadSerializable<UInt160>();
        }
    }
}
