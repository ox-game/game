using OX.IO;
using System.IO;

namespace OX
{
    public class RoundClearResult : ISerializable
    {
        public UInt256 TxHash;
        public RoundClear RoundClear;
        public virtual int Size => TxHash.Size + RoundClear.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TxHash);
            writer.Write(RoundClear);
        }
        public void Deserialize(BinaryReader reader)
        {
            TxHash = reader.ReadSerializable<UInt256>();
            RoundClear = reader.ReadSerializable<RoundClear>();
        }
    }
}
