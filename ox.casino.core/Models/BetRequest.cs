using OX.IO;
using OX.Network.P2P;
using System.IO;

namespace OX
{
    public class BetRequest : ISerializable
    {
        public UInt160 BetAddress;
        public byte Version = 0x00;
        public uint Index;
        public string BetPoint;
        public UInt160 From;
        public byte Flag;
        public SignatureValidator<SeasonPassport> Passport;
        public SignatureValidator<CommonAuthorizeMark> Mark;
        public byte[] CryptData;
        public byte TipKind;
        public byte[] Tips;
        public virtual int Size => BetAddress.Size + sizeof(byte) + sizeof(uint) + BetPoint.GetVarSize() + From.Size + sizeof(byte) + sizeof(uint) + (Passport.IsNotNull() ? Passport.Size : 0) + CryptData.GetVarSize() + sizeof(uint) + (Mark.IsNotNull() ? Mark.Size : 0) + CryptData.GetVarSize() + sizeof(byte) + Tips.GetVarSize();
        public BetRequest()
        {
            CryptData = new byte[0];
            Tips = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Version);
            writer.Write(Index);
            writer.WriteVarString(BetPoint);
            writer.Write(From);
            writer.Write(Flag);
            if (Passport.IsNotNull())
            {
                writer.Write((uint)Passport.Size);
                writer.Write(Passport);
            }
            else
                writer.Write((uint)0);
            if (Mark.IsNotNull())
            {
                writer.Write((uint)Mark.Size);
                writer.Write(Mark);
            }
            else
                writer.Write((uint)0);
            writer.WriteVarBytes(CryptData);
            writer.Write(TipKind);
            writer.WriteVarBytes(Tips);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Version = reader.ReadByte();
            Index = reader.ReadUInt32();
            BetPoint = reader.ReadVarString();
            From = reader.ReadSerializable<UInt160>();
            Flag = reader.ReadByte();
            uint c = reader.ReadUInt32();
            if (c > 0)
            {
                Passport = reader.ReadSerializable<SignatureValidator<SeasonPassport>>();
            }
            uint d = reader.ReadUInt32();
            if (d > 0)
            {
                Mark = reader.ReadSerializable<SignatureValidator<CommonAuthorizeMark>>();
            }
            CryptData = reader.ReadVarBytes();
            TipKind = reader.ReadByte();
            Tips = reader.ReadVarBytes();
        }
    }
    public class BetReturn : ISerializable
    {
        public uint RoomId;
        public byte Version = 0x00;
        public uint Index;
        public UInt256 BetTxId;
        public virtual int Size => sizeof(uint) + sizeof(byte) + sizeof(uint) + BetTxId.Size;
        public BetReturn()
        {
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RoomId);
            writer.Write(Version);
            writer.Write(Index);
            writer.Write(BetTxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
            Version = reader.ReadByte();
            Index = reader.ReadUInt32();
            BetTxId = reader.ReadSerializable<UInt256>();
        }
    }
}

