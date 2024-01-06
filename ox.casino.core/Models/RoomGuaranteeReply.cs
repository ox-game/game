using OX.IO;
using System.IO;
using OX.Cryptography.ECC;
namespace OX
{
    public class RoomGuaranteeReply : ISerializable
    {
        public uint RoomId;
        public UInt160 Guarantor;
        public UInt256 TxId;
        public byte[] Data;
        public virtual int Size => sizeof(uint) + Guarantor.Size + TxId.Size + Data.GetVarSize();
        public RoomGuaranteeReply()
        {
            Data = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RoomId);
            writer.Write(Guarantor);
            writer.Write(TxId);
            writer.WriteVarBytes(Data);
        }
        public void Deserialize(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
            Guarantor = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
            Data = reader.ReadVarBytes();
        }
    }
}
