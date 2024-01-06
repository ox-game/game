using OX.Cryptography.ECC;
using OX.IO;
using System.IO;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
namespace OX
{
    public class RoomDestroyRecord : BizModel
    {
        public uint RoomId;
        public override int Size => base.Size + sizeof(uint);
        public RoomDestroyRecord() : base((byte)CasinoBizModelType.RoomDestroy)
        { }
        public override void SerializeBizModel(BinaryWriter writer)
        {
            writer.Write(RoomId);
        }
        public override void DeserializeBizModel(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
        }
    }

}
