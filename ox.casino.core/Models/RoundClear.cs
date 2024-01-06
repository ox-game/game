using OX.IO;
using System.IO;

namespace OX
{
    public class RoundClear : ISerializable
    {
        public UInt160 BetAddress;
        public byte Version = 0x00;
        public uint Index;
        public uint SNO;
        public byte DataType;
        public byte[] Data;
        public virtual int Size => BetAddress.Size + sizeof(byte) + sizeof(uint) + sizeof(uint) + sizeof(byte) + Data.GetVarSize();
        public RoundClear()
        {
            Data = new byte[] { 0x00 };
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Version);
            writer.Write(Index);
            writer.Write(SNO);
            writer.Write(DataType);
            writer.WriteVarBytes(Data);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Version = reader.ReadByte();
            Index = reader.ReadUInt32();
            SNO = reader.ReadUInt32();
            DataType = reader.ReadByte();
            Data = reader.ReadVarBytes();
        }
    }
    public class RoundClearTip : ISerializable
    {
        public Fixed8 NetFee;
        public Fixed8 SystemFee;
        public Fixed8 RoomFee;
        public Fixed8 PoolFee;
        public Fixed8 BetTotal;
        public Fixed8 GlobalPoolFee;
        public byte[] Data;
        public string Remark;
        public virtual int Size => NetFee.Size + SystemFee.Size + RoomFee.Size + PoolFee.Size + BetTotal.Size + GlobalPoolFee.Size + Data.GetVarSize() + Remark.GetVarSize();
        public RoundClearTip()
        {
            Data = new byte[] { 0x00 };
            Remark = "0";
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(NetFee);
            writer.Write(SystemFee);
            writer.Write(RoomFee);
            writer.Write(PoolFee);
            writer.Write(BetTotal);
            writer.Write(GlobalPoolFee);
            writer.WriteVarBytes(Data);
            writer.WriteVarString(Remark);
        }
        public void Deserialize(BinaryReader reader)
        {
            NetFee = reader.ReadSerializable<Fixed8>();
            SystemFee = reader.ReadSerializable<Fixed8>();
            RoomFee = reader.ReadSerializable<Fixed8>();
            PoolFee = reader.ReadSerializable<Fixed8>();
            BetTotal = reader.ReadSerializable<Fixed8>();
            GlobalPoolFee = reader.ReadSerializable<Fixed8>();
            Data = reader.ReadVarBytes();
            Remark = reader.ReadVarString();
        }
    }
}
