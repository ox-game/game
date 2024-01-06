using OX.IO;
using OX.Network.P2P;
using System.IO;

namespace OX
{
    public class PrivateRoomMemberSettingRequest : ISerializable
    {
        public UInt160 BetAddress;
        public RoomMemberSetting RoomMemberSetting;

        public virtual int Size => BetAddress.Size + RoomMemberSetting.Size;
        public PrivateRoomMemberSettingRequest()
        {
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetAddress);
            writer.Write(RoomMemberSetting);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetAddress = reader.ReadSerializable<UInt160>();
            RoomMemberSetting = reader.ReadSerializable<RoomMemberSetting>();
        }
    }

}

