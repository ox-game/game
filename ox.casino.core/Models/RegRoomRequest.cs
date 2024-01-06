using OX.IO;
using OX.Ledger;
using System.Collections.ObjectModel;
using System.IO;

namespace OX
{
    public class RegRoomRequest : ISerializable
    {
        public UInt256 AssetId;
        public GameKind Kind;
        public GameSpeed GameSpeed;
        public byte Level;
        public byte CommissionValue;
        public RoomPermission Permission;
        public byte Flag;
        public byte BonusMultiple;
        public byte DividendRatio;
        public DividentSlope DividentSlope;
        /// <summary>
        /// 0:none,1:RoomMemberSetting
        /// </summary>
        public byte DataKind;
        public byte[] Data;
        public virtual int Size => AssetId.Size + sizeof(GameKind) + sizeof(GameSpeed) 
            + sizeof(byte) + sizeof(byte) + sizeof(RoomPermission) 
            + sizeof(byte) + sizeof(byte) + sizeof(byte)
            +sizeof(DividentSlope) + sizeof(byte) + Data.GetVarSize();
        public RegRoomRequest()
        {
            AssetId = Blockchain.OXC;
            CommissionValue = 1;
            Flag = 0;
            BonusMultiple = 0;
            DataKind = 0;
            Data = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write((byte)Kind);
            writer.Write((byte)GameSpeed);
            writer.Write(Level);
            writer.Write(CommissionValue);
            writer.Write((byte)Permission);
            writer.Write(Flag);
            writer.Write(BonusMultiple);
            writer.Write(DividendRatio);
            writer.Write((byte)DividentSlope);
            writer.Write(DataKind);
            writer.WriteVarBytes(Data);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            Kind = (GameKind)reader.ReadByte();
            GameSpeed = (GameSpeed)reader.ReadByte();
            Level =reader.ReadByte();
            CommissionValue = reader.ReadByte();
            Permission = (RoomPermission)reader.ReadByte();
            Flag = reader.ReadByte();
            BonusMultiple = reader.ReadByte();
            DividendRatio = reader.ReadByte();
            DividentSlope = (DividentSlope)reader.ReadByte();
            DataKind = reader.ReadByte();
            Data = reader.ReadVarBytes();
        }
    }
    public class RoomMemberSetting : ISerializable
    {
        public byte Flag;
        public UInt160[] Members;
        public byte[] Data;
        public virtual int Size => sizeof(byte) + Members.GetVarSize() + Data.GetVarSize();
        public RoomMemberSetting()
        {
            Flag = 0;
            Data = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {

            writer.Write(Flag);
            writer.Write(Members);
            writer.WriteVarBytes(Data);
        }
        public void Deserialize(BinaryReader reader)
        {
            Flag = reader.ReadByte();
            Members = reader.ReadSerializableArray<UInt160>();
            Data = reader.ReadVarBytes();
        }
    }
}
