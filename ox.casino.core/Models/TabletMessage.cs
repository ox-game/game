using OX.IO;
using OX.Network.P2P;
using System.IO;

namespace OX
{

    public class TabletMessage : ISerializable
    {
        public byte MessageType = 0x00;
        public string CnContent;
        public string EnContent;
        public virtual int Size => sizeof(byte) + CnContent.GetVarSize() + EnContent.GetVarSize();

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MessageType);
            writer.WriteVarString(CnContent);
            writer.WriteVarString(EnContent);
        }
        public void Deserialize(BinaryReader reader)
        {
            MessageType = reader.ReadByte();
            CnContent = reader.ReadVarString();
            EnContent = reader.ReadVarString();
        }
    }
    public class PortMessage : ISerializable
    {
        public byte MessageType = 0x00;
        public byte Flag;
        public string Content;
        public virtual int Size => sizeof(byte) + sizeof(byte) + Content.GetVarSize();

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MessageType);
            writer.Write(Flag);
            writer.WriteVarString(Content);
        }
        public void Deserialize(BinaryReader reader)
        {
            MessageType = reader.ReadByte();
            Flag = reader.ReadByte();
            Content = reader.ReadVarString();
        }
    }
    public class PortMessageMix : ISerializable
    {
        public PortMessage Message;
        public uint BlockIndex;
        public uint TimeStamp;
        public virtual int Size => Message.Size+sizeof(uint)+sizeof(uint);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Message);
            writer.Write(BlockIndex);
            writer.Write(TimeStamp);
        }
        public void Deserialize(BinaryReader reader)
        {
            Message = reader.ReadSerializable<PortMessage>();
            BlockIndex = reader.ReadUInt32();
            TimeStamp = reader.ReadUInt32();
        }
    }
}

