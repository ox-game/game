using OX.IO;
using System.IO;

namespace OX
{
    public class RoundClearKey : ISerializable
    {
        public UInt160 BetAddress;
        public uint Index;
        public uint SNO;
        public virtual int Size => BetAddress.Size + sizeof(uint) + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Index);
            writer.Write(SNO);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Index = reader.ReadUInt32();
            SNO = reader.ReadUInt32();
        }
    }
}
