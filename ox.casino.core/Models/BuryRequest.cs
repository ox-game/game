using OX.IO;
using OX.Network.P2P;
using System.IO;
using OX.Cryptography;
using OX.Network.P2P.Payloads;

namespace OX
{
    public class BuryRequest : ISerializable
    {
        public UInt160 BetAddress;
        public byte Version = 0x00;
        public byte PlainBuryPoint;
        public UInt160 From;
        public UInt256 VerifyHash;
        public byte[] CryptoData;
        public virtual int Size => BetAddress.Size + sizeof(byte) + sizeof(byte) + From.Size + VerifyHash.Size + CryptoData.GetVarSize();
        public BuryRequest()
        {
            CryptoData = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Version);
            writer.Write(PlainBuryPoint);
            writer.Write(From);
            writer.Write(VerifyHash);
            writer.WriteVarBytes(CryptoData);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Version = reader.ReadByte();
            PlainBuryPoint = reader.ReadByte();
            From = reader.ReadSerializable<UInt160>();
            VerifyHash = reader.ReadSerializable<UInt256>();
            CryptoData = reader.ReadVarBytes();
        }
    }
    public class PrivateBuryRequest : ISerializable
    {
        public byte CipherBuryPoint;
        public uint Rand;
        public virtual int Size => sizeof(byte) + sizeof(uint);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(CipherBuryPoint);
            writer.Write(Rand);
        }
        public void Deserialize(BinaryReader reader)
        {
            CipherBuryPoint = reader.ReadByte();
            Rand = reader.ReadUInt32();
        }
    }
    public class VerifyPrivateBuryRequest : ISerializable
    {
        public UInt160 From;
        public PrivateBuryRequest PrivateBuryRequest;
        public virtual int Size => From.Size + PrivateBuryRequest.Size;
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
            writer.Write(From);
            writer.Write(PrivateBuryRequest);
        }
        public void Deserialize(BinaryReader reader)
        {
            From = reader.ReadSerializable<UInt160>();
            PrivateBuryRequest = reader.ReadSerializable<PrivateBuryRequest>();
        }
    }
    public class ReplyPrivateBuryRequest : ISerializable
    {
        public UInt256 LuckBuryTxId;
        public UInt256 VerifyHash;
        public PrivateBuryRequest PrivateBuryRequest;
        public virtual int Size => LuckBuryTxId.Size + VerifyHash.Size + PrivateBuryRequest.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(LuckBuryTxId);
            writer.Write(VerifyHash);
            writer.Write(PrivateBuryRequest);
        }
        public void Deserialize(BinaryReader reader)
        {
            LuckBuryTxId = reader.ReadSerializable<UInt256>();
            VerifyHash = reader.ReadSerializable<UInt256>();
            PrivateBuryRequest = reader.ReadSerializable<PrivateBuryRequest>();
        }
    }
    public class ReplyBury : ISerializable
    {
        public UInt160 BetAddress;
        public PrivateBuryRequest PrivateBuryRequest;
        public virtual int Size => BetAddress.Size + PrivateBuryRequest.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(PrivateBuryRequest);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            PrivateBuryRequest = reader.ReadSerializable<PrivateBuryRequest>();
        }
    }
    public class BuryMergeTx : ISerializable
    {
        public ReplyBury ReplyBury;
        public CoinReference Input;
        public TransactionOutput[] Outputs;
        public uint BlockIndex;
        public virtual int Size => ReplyBury.Size + Input.Size + Outputs.GetVarSize() + sizeof(uint);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(ReplyBury);
            writer.Write(Input);
            writer.Write(Outputs);
            writer.Write(BlockIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            ReplyBury = reader.ReadSerializable<ReplyBury>();
            Input = reader.ReadSerializable<CoinReference>();
            Outputs = reader.ReadSerializableArray<TransactionOutput>();
            BlockIndex = reader.ReadUInt32();
        }
    }
}

