using OX.Cryptography;
using OX.IO;
using OX.Network.P2P;
using System.IO;
using System.Linq;

namespace OX
{
    public class RiddlesAndHash : ISerializable
    {
        public Riddles Riddles;
        public RiddlesHash RiddlesHash;
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
        public int Size => sizeof(uint) + (Riddles.IsNotNull() ? Riddles.Size : 0) + sizeof(uint) + (RiddlesHash.IsNotNull() ? RiddlesHash.Size : 0);
        public void Serialize(BinaryWriter writer)
        {
            if (Riddles.IsNotNull())
            {
                writer.Write((uint)Riddles.Size);
                writer.Write(Riddles);
            }
            else
                writer.Write((uint)0);

            if (RiddlesHash.IsNotNull())
            {
                writer.Write((uint)RiddlesHash.Size);
                writer.Write(RiddlesHash);
            }
            else
                writer.Write((uint)0);
        }
        public void Deserialize(BinaryReader reader)
        {
            uint c = reader.ReadUInt32();
            if (c > 0)
            {
                Riddles = reader.ReadSerializable<Riddles>();
            }
            uint c2 = reader.ReadUInt32();
            if (c2 > 0)
            {
                RiddlesHash = reader.ReadSerializable<RiddlesHash>();
            }
        }
    }
    public class Riddles : ISerializable
    {
        public uint Index;
        public byte Version = 0x00;
        public GuessKey[] GuessKeys;
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
        public int Size => sizeof(uint) + sizeof(byte) + GuessKeys.GetVarSize();
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Version);
            writer.Write(GuessKeys);
        }
        public void Deserialize(BinaryReader reader)
        {
            Index = reader.ReadUInt32();
            Version = reader.ReadByte();
            GuessKeys = reader.ReadSerializableArray<GuessKey>();
        }
        public GuessKey GetGuessKey(GameKind kind)
        {
            RiddlesKind rk = default;
            if (kind == GameKind.EatSmall)
                rk = RiddlesKind.EatSmall;
            else if (kind == GameKind.Lotto  || kind == GameKind.TeamKill)
                rk = RiddlesKind.Lotto;
            else if (kind == GameKind.MarkSix)
                rk = RiddlesKind.MarkSix;
            return this.GuessKeys.FirstOrDefault(m => m.RiddlesKind == rk);
        }
    }
    public class RiddlesHash : ISerializable
    {
        public uint Index;
        public UInt256 VerifyHash;
        public int Size => sizeof(uint) + VerifyHash.Size;
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
            writer.Write(Index);
            writer.Write(VerifyHash);
        }
        public void Deserialize(BinaryReader reader)
        {
            Index = reader.ReadUInt32();
            VerifyHash = reader.ReadSerializable<UInt256>();
        }
    }
}
