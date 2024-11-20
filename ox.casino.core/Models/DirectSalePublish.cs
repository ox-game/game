using Newtonsoft.Json.Linq;
using OX.BMS;
using OX.IO;
using OX.IO.Wrappers;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using System;
using System.IO;

namespace OX
{
    public sealed class UInt16Wrapper : SerializableWrapper<ushort>, IEquatable<UInt16Wrapper>
    {
        public override int Size => sizeof(ushort);

        public ushort Value => this.value;
        public UInt16Wrapper()
        {
        }

        private UInt16Wrapper(ushort value)
        {
            this.value = value;
        }

        public override void Deserialize(BinaryReader reader)
        {
            value = reader.ReadUInt16();
        }

        public bool Equals(UInt16Wrapper other)
        {
            return value == other.value;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(value);
        }

        public static implicit operator UInt16Wrapper(ushort value)
        {
            return new UInt16Wrapper(value);
        }

        public static implicit operator ushort(UInt16Wrapper wrapper)
        {
            return wrapper.value;
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is UInt16Wrapper))
                return false;
            return this.Equals((UInt16Wrapper)obj);
        }
        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }
    public sealed class UInt64Wrapper : SerializableWrapper<ulong>, IEquatable<UInt64Wrapper>
    {
        public override int Size => sizeof(ulong);

        public ulong Value => this.value;
        public UInt64Wrapper()
        {
        }

        private UInt64Wrapper(ulong value)
        {
            this.value = value;
        }

        public override void Deserialize(BinaryReader reader)
        {
            value = reader.ReadUInt32();
        }

        public bool Equals(UInt64Wrapper other)
        {
            return value == other.value;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(value);
        }

        public static implicit operator UInt64Wrapper(ulong value)
        {
            return new UInt64Wrapper(value);
        }

        public static implicit operator ulong(UInt64Wrapper wrapper)
        {
            return wrapper.value;
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is UInt64Wrapper))
                return false;
            return this.Equals((UInt64Wrapper)obj);
        }
        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }
    public class DirectSalePublish : ISerializable
    {
        public UInt256 AssetId;
        public string Contact;
        public string Remarks;
        public virtual int Size => AssetId.Size+ Contact.GetVarSize()+Remarks.GetVarSize();
        public DirectSalePublish()
        {
            Contact = string.Empty;
            Remarks=string.Empty;
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.WriteVarString(Contact);
            writer.WriteVarString(Remarks);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId=reader.ReadSerializable<UInt256>();
            Contact=reader.ReadVarString();
            Remarks=reader.ReadVarString();
        }
    }
    public class DirectSalePublishMerge : ISerializable
    {
        public ulong N;
        public uint TimeStamp;
        public DirectSalePublish Publish;
        public AskTransaction Tx;
        public virtual int Size =>sizeof(ulong)+ sizeof(uint)+Publish.Size + Tx.Size;
       
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(N);
            writer.Write(TimeStamp);
            writer.Write(Publish);
            writer.Write(Tx);
        }
        public void Deserialize(BinaryReader reader)
        {
            N = reader.ReadUInt64();
            TimeStamp = reader.ReadUInt32();
            Publish = reader.ReadSerializable<DirectSalePublish>();
            Tx = reader.ReadSerializable<AskTransaction>();
        }
    }
}

