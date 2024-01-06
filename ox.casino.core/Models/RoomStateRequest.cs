using OX.Cryptography;
using OX.Cryptography.ECC;
using OX.IO;
using OX.Network.P2P;
using System;
using System.IO;
using System.Linq;

namespace OX
{
    public class RoomStateRequest : ISignatureTarget, ISerializable
    {
        public const int MAXADMINCOUNT = 10;
        public const int MAXREMARKLENGTH = 1024;
        public byte Version = 0x00;
        public ECPoint PublicKey { get; set; }
        public uint RoomId;
        public RoomState State;
        public ECPoint[] Admins;
        public string Remark;

        public virtual int Size => sizeof(byte) + PublicKey.Size + sizeof(uint) + sizeof(RoomState) + sizeof(uint) + (Admins.IsNotNullAndEmpty() ? Admins.GetVarSize() : 0) + Remark.GetVarSize();
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
        public RoomStateRequest()
        {
            this.Remark = "0";
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(PublicKey);
            writer.Write(RoomId);
            writer.Write((byte)State);
            if (Admins.IsNotNullAndEmpty())
            {
                if (Admins.Length > MAXADMINCOUNT)
                    Admins = Admins.Take(MAXADMINCOUNT).ToArray();
                writer.Write((uint)Admins.GetVarSize());
                writer.Write(Admins);
            }
            else
                writer.Write((uint)0);
            if (Remark.GetVarSize() > MAXREMARKLENGTH)
                throw new Exception($"remark max length {MAXREMARKLENGTH}");
            writer.WriteVarString(Remark);
        }
        public void Deserialize(BinaryReader reader)
        {
            Version = reader.ReadByte();
            PublicKey = reader.ReadSerializable<ECPoint>();
            RoomId = reader.ReadUInt32();
            State = (RoomState)reader.ReadByte();
            uint c = reader.ReadUInt32();
            if (c > 0)
            {
                Admins = reader.ReadSerializableArray<ECPoint>();
            }
            Remark = reader.ReadVarString();
        }
    }
}
