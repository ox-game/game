using OX.IO;
using System.IO;

namespace OX
{
    public class Betting : ISerializable
    {
        public Fixed8 Amount;
        public BetRequest BetRequest;
        public UInt256 TxId;
        public virtual int Size => Amount.Size + BetRequest.Size + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Amount);
            writer.Write(BetRequest);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            Amount = reader.ReadSerializable<Fixed8>();
            BetRequest = reader.ReadSerializable<BetRequest>();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
}
