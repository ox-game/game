using OX.Cryptography.ECC;
using OX.IO;
using OX.Network.P2P.Payloads;
using System.IO;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX
{
    public class RoomRecord : BizModel
    {
        public uint RoomId;
        public byte Version = 0x00;
        public ECPoint Address;
        public ECPoint PoolAddress;
        public ECPoint MasterAddress;
        public GameKind Kind;
        public GameSpeed PeriodMultiple;
        public byte Level;
        public byte CommissionValue;
        public RoomPermission State;
        public override int Size => base.Size + sizeof(uint) + sizeof(byte) + Address.Size + PoolAddress.Size + MasterAddress.Size + sizeof(GameKind) + sizeof(GameSpeed) + sizeof(byte) + sizeof(byte) + sizeof(RoomPermission);
        public RoomRecord() : base((byte)CasinoBizModelType.Room)
        { }
        public override void SerializeBizModel(BinaryWriter writer)
        {
            writer.Write(RoomId);
            writer.Write(Version);
            writer.Write(Address);
            writer.Write(PoolAddress);
            writer.Write(MasterAddress);
            writer.Write((byte)Kind);
            writer.Write((byte)PeriodMultiple);
            writer.Write(Level);
            writer.Write(CommissionValue);
            writer.Write((byte)State);
        }
        public override void DeserializeBizModel(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
            Version = reader.ReadByte();
            Address = reader.ReadSerializable<ECPoint>();
            PoolAddress = reader.ReadSerializable<ECPoint>();
            MasterAddress = reader.ReadSerializable<ECPoint>();
            Kind = (GameKind)reader.ReadByte();
            PeriodMultiple = (GameSpeed)reader.ReadByte();
            Level =reader.ReadByte();
            CommissionValue = reader.ReadByte();
            State = (RoomPermission)reader.ReadByte();
        }
    }
    
}
