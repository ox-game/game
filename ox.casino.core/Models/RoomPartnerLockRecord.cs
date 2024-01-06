using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;

namespace OX
{
    public class RoomPartnerLockRecord : ISerializable
    {
        public UInt160 BetAddress;
        public UInt160 Partner;
        public uint StartIndex;
        public uint EndIndex;
        public UInt160 LockAddress;
        public Fixed8 Amount;
        public uint Timestamp;
        public ushort TxIndex;
        public virtual int Size => BetAddress.Size + Partner.Size + sizeof(uint) + sizeof(uint) + LockAddress.Size + Amount.Size + sizeof(uint) + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(Partner);
            writer.Write(StartIndex);
            writer.Write(EndIndex);
            writer.Write(LockAddress);
            writer.Write(Amount);
            writer.Write(Timestamp);
            writer.Write(TxIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            Partner = reader.ReadSerializable<UInt160>();
            StartIndex = reader.ReadUInt32();
            EndIndex = reader.ReadUInt32();
            LockAddress = reader.ReadSerializable<UInt160>();
            Amount = reader.ReadSerializable<Fixed8>();
            Timestamp = reader.ReadUInt32();
            TxIndex = reader.ReadUInt16();
        }
    }
}
