using System;
using System.Collections.Generic;
using System.Linq;
using OX.IO;
using OX.Network.P2P;
using System.IO;
using OX.Cryptography.ECC;

namespace OX
{
    public class CommonAuthorizeMark : ISignatureTarget
    {

        public UInt160 Gambler;
        public ECPoint PublicKey { get; set; }
        public virtual int Size => Gambler.Size + PublicKey.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Gambler);
            writer.Write(PublicKey);
        }
        public void Deserialize(BinaryReader reader)
        {
            Gambler = reader.ReadSerializable<UInt160>();
            PublicKey = reader.ReadSerializable<ECPoint>();
        }
    }
}
