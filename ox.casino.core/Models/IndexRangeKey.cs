using OX.IO;
using System.IO;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX
{
    public class IndexRangeKey : ISerializable
    {
        public uint IndexRange;
        public uint Index;
        public virtual int Size => sizeof(uint) + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(IndexRange);
            writer.Write(Index);
        }
        public void Deserialize(BinaryReader reader)
        {
            IndexRange = reader.ReadUInt32();
            Index = reader.ReadUInt32();
        }
    }
}
