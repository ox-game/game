using OX.Cryptography.ECC;
using OX.IO;
using System.IO;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX
{
    public class RoomKey : ISerializable
    {
        public ECPoint Holder;
        public uint RoomId;
        public virtual int Size => Holder.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Holder);
            writer.Write(RoomId);
        }
        public void Deserialize(BinaryReader reader)
        {
            Holder = reader.ReadSerializable<ECPoint>();
            RoomId = reader.ReadUInt32();
        }
    }
    public class RoomKeyRecord : ISerializable
    {
        public RoomKey RoomKey;
        public RoomRecord RoomRecord;
        public virtual int Size => RoomKey.Size + RoomRecord.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RoomKey);
            writer.Write(RoomRecord);
        }
        public void Deserialize(BinaryReader reader)
        {
            RoomKey = reader.ReadSerializable<RoomKey>();
            RoomRecord = reader.ReadSerializable<RoomRecord>();
        }
    }
    public class RoomSplitKey : ISerializable
    {
        public uint RoomId;
        public uint SplitIndex;
        public virtual int Size => sizeof(uint) + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RoomId);
            writer.Write(SplitIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
            SplitIndex = reader.ReadUInt32();
        }
    }
    public class RoomAdminKey : ISerializable
    {
        public UInt160 Administrator;
        public uint RoomId;
        public virtual int Size => Administrator.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Administrator);
            writer.Write(RoomId);
        }
        public void Deserialize(BinaryReader reader)
        {
            Administrator = reader.ReadSerializable<UInt160>();
            RoomId = reader.ReadUInt32();
        }
    }
}
