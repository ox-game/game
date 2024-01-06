using OX.IO;
using OX.Network.P2P;
using System.IO;

namespace OX
{
    public class ValetRegister : ISerializable
    {
        public UInt160 Valet;
        public UInt160 Beneficiary;
        public byte[] Tips;
        public virtual int Size => Valet.Size + Beneficiary.Size + Tips.GetVarSize();
        public ValetRegister()
        {
            Tips = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Valet);
            writer.Write(Beneficiary);
            writer.WriteVarBytes(Tips);
        }
        public void Deserialize(BinaryReader reader)
        {
            Valet = reader.ReadSerializable<UInt160>();
            Beneficiary = reader.ReadSerializable<UInt160>();
            Tips = reader.ReadVarBytes();
        }
    }
}
