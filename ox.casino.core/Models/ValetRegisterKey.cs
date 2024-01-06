using OX.IO;
using System.IO;

namespace OX
{
    public class ValetRegisterKey : ISerializable
    {
        public UInt160 Beneficiary;
        public UInt160 RegisterFrom;
        public virtual int Size => Beneficiary.Size + RegisterFrom.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Beneficiary);
            writer.Write(RegisterFrom);
        }
        public void Deserialize(BinaryReader reader)
        {
            Beneficiary = reader.ReadSerializable<UInt160>();
            RegisterFrom = reader.ReadSerializable<UInt160>();
        }
    }
}
