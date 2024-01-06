using OX.IO;
using System.IO;

namespace OX
{
    public class CreateRoomGuaranteeRequest : ISerializable
    {
        public uint RoomId;
        public UInt160 Guarantor;
        public byte[] Data;
        public virtual int Size => sizeof(uint) + Guarantor.Size + Data.GetVarSize();
        public CreateRoomGuaranteeRequest()
        {
            Data = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RoomId);
            //if (DividendRatio == 0 || DividendRatio >= 100) throw new System.Exception("devidend ration error");
            writer.Write(Guarantor);
            writer.WriteVarBytes(Data);
        }
        public void Deserialize(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
            Guarantor = reader.ReadSerializable<UInt160>();
            Data = reader.ReadVarBytes();
        }
    }
}
