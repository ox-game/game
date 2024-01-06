using OX.Cryptography.ECC;
using OX.IO;
using OX.Network.P2P;
using System.IO;


namespace OX
{
    public class SeasonPassport : ISignatureTarget
    {
        public UInt160 BetAddress;
        public uint SplitIndex;
        public uint MaxIndex;
        public UInt160 Gambler;
        public ECPoint PublicKey { get; set; }
        public virtual int Size => BetAddress.Size + sizeof(uint) + sizeof(uint) + Gambler.Size + PublicKey.Size;
        public SeasonPassport()
        {
            SplitIndex = 0;
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(SplitIndex);
            writer.Write(MaxIndex);
            writer.Write(Gambler);
            writer.Write(PublicKey);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            SplitIndex = reader.ReadUInt32();
            MaxIndex = reader.ReadUInt32();
            Gambler = reader.ReadSerializable<UInt160>();
            PublicKey = reader.ReadSerializable<ECPoint>();
        }
    }
}
