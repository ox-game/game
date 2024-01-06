using OX.IO;
using System.IO;
using OX.Network.P2P.Payloads;

namespace OX
{

    public class RoomPledgeGuarantee : ISerializable
    {
        public UInt160 PledgeGuarantor;
        public UInt160 PledgeAccount;
        public uint RoomId;
        public UInt256 TxId;
        public ushort N;
        public Fixed8 Value;
        public virtual int Size => PledgeGuarantor.Size + PledgeAccount.Size + sizeof(uint) + TxId.Size + sizeof(ushort) + Value.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PledgeGuarantor);
            writer.Write(PledgeAccount);
            writer.Write(RoomId);
            writer.Write(TxId);
            writer.Write(N);
            writer.Write(Value);
        }
        public void Deserialize(BinaryReader reader)
        {
            PledgeGuarantor = reader.ReadSerializable<UInt160>();
            PledgeAccount = reader.ReadSerializable<UInt160>();
            RoomId = reader.ReadUInt32();
            TxId = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
            Value = reader.ReadSerializable<Fixed8>();
        }
    }
    public class RoomPledgeGuaranteeRequestAndOutput : ISerializable
    {
        public CreateRoomGuaranteeRequest CreateRoomGuaranteeRequest;
        public TransactionOutput Output;
        public UInt256 TxId;
        public uint BlockIndex;
        public ushort TxIndex;
        public virtual int Size => CreateRoomGuaranteeRequest.Size + Output.Size + TxId.Size + sizeof(uint) + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(CreateRoomGuaranteeRequest);
            writer.Write(Output);
            writer.Write(TxId);
            writer.Write(BlockIndex);
            writer.Write(TxIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            CreateRoomGuaranteeRequest = reader.ReadSerializable<CreateRoomGuaranteeRequest>();
            Output = reader.ReadSerializable<TransactionOutput>();
            TxId = reader.ReadSerializable<UInt256>();
            BlockIndex = reader.ReadUInt32();
            TxIndex = reader.ReadUInt16();
        }
    }
    public class RoomPledgeOXSClaimKey : ISerializable
    {
        public UInt160 PledgeAccount;
        public UInt256 TxId;
        public ushort N;
        public Fixed8 Value;
        public virtual int Size => PledgeAccount.Size + TxId.Size + sizeof(ushort) + Value.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PledgeAccount);
            writer.Write(TxId);
            writer.Write(N);
            writer.Write(Value);
        }
        public void Deserialize(BinaryReader reader)
        {
            PledgeAccount = reader.ReadSerializable<UInt160>();
            TxId = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
            Value = reader.ReadSerializable<Fixed8>();
        }
    }
    public class RoomPledgeOXSClaimRecord : ISerializable
    {
        public byte Flag;
        public uint Index;
        public uint SpendIndex;
        public virtual int Size => sizeof(byte) + sizeof(uint) + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Flag);
            writer.Write(Index);
            writer.Write(SpendIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            Flag = reader.ReadByte();
            Index = reader.ReadUInt32();
            SpendIndex = reader.ReadUInt32();
        }
    }
    public class UintHashKey : ISerializable
    {
        public uint Value;
        public UInt256 TxId;
        public virtual int Size => sizeof(uint) + TxId.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Value);
            writer.Write(TxId);
        }
        public void Deserialize(BinaryReader reader)
        {
            Value = reader.ReadUInt32();
            TxId = reader.ReadSerializable<UInt256>();
        }
    }
}
