using OX.Cryptography;
using OX.Cryptography.ECC;
using OX.IO;
using OX.Network.P2P;
using System;
using System.IO;
using System.Linq;

namespace OX
{
    public class RoomSplitRequest : ISignatureTarget, ISerializable
    {
        public byte Version = 0x00;
        public ECPoint PublicKey { get; set; }
        public uint RoomId;
        public virtual int Size => sizeof(byte) + PublicKey.Size + sizeof(uint);
        private UInt256 _hash = null;
        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    _hash = new UInt256(Crypto.Default.Hash256(this.GetHashData()));
                }
                return _hash;
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(PublicKey);
            writer.Write(RoomId);
        }
        public void Deserialize(BinaryReader reader)
        {
            Version = reader.ReadByte();
            PublicKey = reader.ReadSerializable<ECPoint>();
            RoomId = reader.ReadUInt32();
        }
    }
}
