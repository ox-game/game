using OX.Cryptography.ECC;
using OX.IO;
using System.IO;
using OX.Network.P2P;

namespace OX
{
    public class BankerWithdraw : ISignatureTarget
    {
        public byte Version = 0x00;
        public UInt160 BetAddress;
        public ECPoint PublicKey { get; set; }
        public virtual int Size => sizeof(byte) + BetAddress.Size + PublicKey.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(BetAddress);
            writer.Write(PublicKey);
        }
        public void Deserialize(BinaryReader reader)
        {
            Version = reader.ReadByte();
            BetAddress = reader.ReadSerializable<UInt160>();
            PublicKey = reader.ReadSerializable<ECPoint>();
        }
    }

}
