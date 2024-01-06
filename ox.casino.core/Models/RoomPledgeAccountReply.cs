using OX.IO;
using System.IO;
using OX.Cryptography.ECC;
namespace OX
{
    public class RoomPledgeAccountReply : ISerializable
    {
        public uint RoomId;
        public byte Version = 0x00;
        public ECPoint Address;
        public ECPoint BackupAddress;
        public uint SettleIndex;
        public uint DividendRatio;
        public DividentSlope DividentSlope;
        public byte[] Data;
        public virtual int Size => sizeof(uint) + sizeof(byte) + Address.Size + BackupAddress.Size + sizeof(uint) + sizeof(uint) + sizeof(DividentSlope) + Data.GetVarSize();
        public RoomPledgeAccountReply()
        {
            Data = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RoomId);
            writer.Write(Version);
            writer.Write(Address);
            writer.Write(BackupAddress);
            writer.Write(SettleIndex);
            writer.Write(DividendRatio);
            writer.Write((byte)DividentSlope);
            writer.WriteVarBytes(Data);
        }
        public void Deserialize(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
            Version = reader.ReadByte();
            Address = reader.ReadSerializable<ECPoint>();
            BackupAddress = reader.ReadSerializable<ECPoint>();
            SettleIndex = reader.ReadUInt32();
            DividendRatio = reader.ReadUInt32();
            DividentSlope = (DividentSlope)reader.ReadByte();
            Data = reader.ReadVarBytes();
        }
    }
}
