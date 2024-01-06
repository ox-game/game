using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using OX.Cryptography.ECC;
using System.Security.Policy;
using System.IO;

namespace OX.Casino
{
    public class LastRoomId : ISerializable
    {
        public uint RoomId;
        public virtual int Size => sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(RoomId);

        }
        public void Deserialize(BinaryReader reader)
        {
            RoomId = reader.ReadUInt32();
        }
    }
}
