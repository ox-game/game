using OX.Cryptography.ECC;
using OX.IO;
using OX.Network.P2P.Payloads;
using System.IO;
//using System.Runtime.InteropServices.WindowsRuntime;

namespace OX
{
    public class GameMiningAirdropResult : ISerializable
    {
        public UInt160 Winner;
        public UInt256 AssetId;
        public Fixed8 Amount;
        public uint LockExpiration;
        public virtual int Size => Winner.Size + AssetId.Size + Amount.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Winner);
            writer.Write(AssetId);
            writer.Write(Amount);
            writer.Write(LockExpiration);
        }
        public void Deserialize(BinaryReader reader)
        {
            Winner = reader.ReadSerializable<UInt160>();
            AssetId = reader.ReadSerializable<UInt256>();
            Amount = reader.ReadSerializable<Fixed8>();
            LockExpiration = reader.ReadUInt32();
        }
    }
    public class GameMiningAirdrop : ISerializable
    {
        public GameMiningKind Kind;
        public uint BetIndex;
        public byte Flag;
        public virtual int Size => sizeof(byte) + sizeof(uint) + sizeof(byte);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Kind);
            writer.Write(BetIndex);
            writer.Write(Flag);
        }
        public void Deserialize(BinaryReader reader)
        {
            Kind = (GameMiningKind)reader.ReadByte();
            BetIndex = reader.ReadUInt32();
            Flag = reader.ReadByte();
        }
        public override bool Equals(object obj)
        {
            if (obj is GameMiningSeed gmt)
            {
                return gmt.Kind == this.Kind && gmt.Position == this.Flag && gmt.BetIndex == this.BetIndex;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.Kind.GetHashCode() + this.Flag.GetHashCode() + this.BetIndex.GetHashCode();
        }
    }
    public class GameMiningSeedKey : ISerializable
    {
        public GameMiningSeed Seed;
        public UInt256 TxId;
        public ushort N;
        public virtual int Size => Seed.Size + TxId.Size + sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Seed);
            writer.Write(TxId);
            writer.Write(N);
        }
        public void Deserialize(BinaryReader reader)
        {
            Seed = reader.ReadSerializable<GameMiningSeed>();
            TxId = reader.ReadSerializable<UInt256>();
            N = reader.ReadUInt16();
        }
    }
    public class GameMiningSeedValue : ISerializable
    {
        public UInt160 Player;
        public UInt256 AssetId;
        public Fixed8 Amount;
        public uint LockExpiration;
        public virtual int Size => Player.Size + AssetId.Size + Amount.Size + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Player);
            writer.Write(AssetId);
            writer.Write(Amount);
            writer.Write(LockExpiration);
        }
        public void Deserialize(BinaryReader reader)
        {
            Player = reader.ReadSerializable<UInt160>();
            AssetId = reader.ReadSerializable<UInt256>();
            Amount = reader.ReadSerializable<Fixed8>();
            LockExpiration = reader.ReadUInt32();
        }
    }
    public class GameMiningSeed : ISerializable
    {
        public GameMiningKind Kind;
        public uint BetIndex;
        public byte Position;
        public virtual int Size => sizeof(byte) + sizeof(uint) + sizeof(byte);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Kind);
            writer.Write(BetIndex);
            writer.Write(Position);
        }
        public void Deserialize(BinaryReader reader)
        {
            Kind = (GameMiningKind)reader.ReadByte();
            BetIndex = reader.ReadUInt32();
            Position = reader.ReadByte();
        }
        public override bool Equals(object obj)
        {
            if (obj is GameMiningSeed gmt)
            {
                return gmt.Kind == this.Kind && gmt.Position == this.Position && gmt.BetIndex == this.BetIndex;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.Kind.GetHashCode() + this.Position.GetHashCode() + this.BetIndex.GetHashCode();
        }
    }
    public class GameMiningTaskKey : ISerializable
    {
        public GameMiningKind Kind;
        public uint BetIndex;
        public virtual int Size => sizeof(GameMiningKind) + sizeof(uint);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Kind);
            writer.Write(BetIndex);
        }
        public void Deserialize(BinaryReader reader)
        {
            Kind = (GameMiningKind)reader.ReadByte();
            BetIndex = reader.ReadUInt32();
        }
        public override bool Equals(object obj)
        {
            if (obj is GameMiningTaskKey gmt)
            {
                return gmt.Kind == this.Kind && gmt.BetIndex == this.BetIndex;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.Kind.GetHashCode() + this.BetIndex.GetHashCode();
        }
    }
    public class GameMiningTask : BizModel
    {
        public const uint MAXBETRANGE = 100;
        public static Fixed8 MINSEED = Fixed8.One * 10;
        public byte Version = 0x00;
        public UInt256 AssetId;
        public uint BetLockExpiration;
        public Fixed8 AirDropAmount;
        public uint AirdropLockRange;

        public override int Size => base.Size + sizeof(byte) + AssetId.Size + sizeof(uint) + AirDropAmount.Size + sizeof(uint);
        public GameMiningTask() : base((byte)CasinoBizModelType.GameMiningTask)
        { }
        public override void SerializeBizModel(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(AssetId);
            writer.Write(BetLockExpiration);
            writer.Write(AirDropAmount);
            writer.Write(AirdropLockRange);
        }
        public override void DeserializeBizModel(BinaryReader reader)
        {
            Version = reader.ReadByte();
            AssetId = reader.ReadSerializable<UInt256>();
            BetLockExpiration = reader.ReadUInt32();
            AirDropAmount = reader.ReadSerializable<Fixed8>();
            AirdropLockRange = reader.ReadUInt32();
        }
    }

}
