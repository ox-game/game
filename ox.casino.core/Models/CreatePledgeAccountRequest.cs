using OX.IO;
using System.IO;

namespace OX
{
    public class CreatePledgeAccountRequest : ISerializable
    {
        public uint RoomId;
        public uint DividendRatio;
        public DividentSlope DividentSlope;
        public byte[] Data;
        public virtual int Size => sizeof(uint) + sizeof(uint) + sizeof(DividentSlope) + Data.GetVarSize();
        public CreatePledgeAccountRequest()
        {
            Data = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RoomId);
            writer.Write(DividendRatio);
            writer.Write((byte)DividentSlope);
            writer.WriteVarBytes(Data);
        }
        public void Deserialize(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
            DividendRatio = reader.ReadUInt32();
            DividentSlope = (DividentSlope)reader.ReadByte();
            Data = reader.ReadVarBytes();
        }
    }
}
