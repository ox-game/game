using Org.BouncyCastle.Utilities;
using System.Linq;
using OX.IO;
using OX.Ledger;
using System.Collections.ObjectModel;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OX.Cryptography;
using OX.Network.P2P;
using System;
using System.Drawing;
using OX.Network.P2P.Payloads;
using System.Numerics;
using System.Security.Policy;

namespace OX.BMS
{
    public class MarkEncodedBetSet : ISerializable
    {
        public BitMarkSixBet[] BetItems;
        public virtual int Size => BetItems.GetVarSize();
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetItems);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetItems = reader.ReadSerializableArray<BitMarkSixBet>();
        }
    }
    public class MarkCipherBetOrder : ISerializable
    {
        public UInt160 Player;
        public MarkTerm Term;
        public MarkChannelRound ChannelRound;
        public BitMarkSixBet[] BetItems;
        public UInt160 PortHolder;
        public virtual int Size => Player.Size + Term.Size + ChannelRound.Size + BetItems.GetVarSize() + PortHolder.Size;
        public uint Amount
        {
            get
            {
                uint amt = 0;
                if (BetItems.IsNotNullAndEmpty())
                {
                    amt = (uint)BetItems.Sum(m => (int)m.Amount);
                }
                return amt;
            }
        }
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

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Player);
            writer.Write(Term);
            writer.Write(ChannelRound);
            writer.Write(BetItems);
            writer.Write(PortHolder);
        }
        public void Deserialize(BinaryReader reader)
        {
            Player = reader.ReadSerializable<UInt160>();
            Term = reader.ReadSerializable<MarkTerm>();
            ChannelRound = reader.ReadSerializable<MarkChannelRound>();
            BetItems = reader.ReadSerializableArray<BitMarkSixBet>();
            PortHolder = reader.ReadSerializable<UInt160>();
        }
    }
    public class MarkEncodedBetOrder : ISerializable
    {
        public UInt160 Player;
        public MarkTerm Term;
        public MarkChannelRound ChannelRound;
        public UInt160 PortHolder;
        public byte[] EncodedData;
        public virtual int Size => Player.Size + Term.Size + ChannelRound.Size + PortHolder.Size + EncodedData.GetVarSize();

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Player);
            writer.Write(Term);
            writer.Write(ChannelRound);
            writer.Write(PortHolder);
            writer.WriteVarBytes(EncodedData);
        }
        public void Deserialize(BinaryReader reader)
        {
            Player = reader.ReadSerializable<UInt160>();
            Term = reader.ReadSerializable<MarkTerm>();
            ChannelRound = reader.ReadSerializable<MarkChannelRound>();
            PortHolder = reader.ReadSerializable<UInt160>();
            EncodedData = reader.ReadVarBytes();
        }
    }

    public class MarkCipherBetOrderKey : IEquatable<MarkCipherBetOrderKey>, ISerializable
    {
        public UInt160 Player;
        public MarkTerm Term;
        public MarkChannelRound ChannelRound;
        public UInt256 OrderHash;
        public ulong Nonce;
        public virtual int Size => Player.Size + Term.Size + ChannelRound.Size + OrderHash.Size + sizeof(ulong);
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
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Player);
            writer.Write(Term);
            writer.Write(ChannelRound);
            writer.Write(OrderHash);
            writer.Write(Nonce);
        }
        public void Deserialize(BinaryReader reader)
        {
            Player = reader.ReadSerializable<UInt160>();
            Term = reader.ReadSerializable<MarkTerm>();
            ChannelRound = reader.ReadSerializable<MarkChannelRound>();
            OrderHash = reader.ReadSerializable<UInt256>();
            Nonce = reader.ReadUInt64();
        }
        public override string ToString()
        {
            return this.Hash.ToString();
        }
        public bool Equals(MarkCipherBetOrderKey other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return this.Player == other.Player
                && this.Term == other.Term
                && this.ChannelRound == other.ChannelRound
                && this.OrderHash == other.OrderHash
                && this.Nonce == other.Nonce;
        }

        public override int GetHashCode()
        {
            return this.Hash.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is MarkCipherBetOrderKey))
                return false;
            return this.Equals((MarkCipherBetOrderKey)obj);
        }
        public static bool operator ==(MarkCipherBetOrderKey left, MarkCipherBetOrderKey right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }
        public static bool operator !=(MarkCipherBetOrderKey left, MarkCipherBetOrderKey right)
        {
            return !(left == right);
        }
    }
    public class MarkCipherBetOrderValue : ISerializable
    {
        public MarkCipherBetOrder Order;
        public MarkCipherOrderStatus State;
        public UInt256 TxHash;
        public AskTransaction BetTx;
        public virtual int Size => Order.Size + sizeof(MarkCipherOrderStatus) + TxHash.Size + sizeof(uint) + (BetTx.IsNotNull() ? BetTx.Size : 0);
        public MarkCipherBetOrderValue()
        {
            TxHash = UInt256.Zero;
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Order);
            writer.Write((byte)State);
            writer.Write(TxHash);
            if (BetTx.IsNotNull())
            {
                writer.Write((uint)BetTx.Size);
                writer.Write(BetTx);
            }
            else
            {
                writer.Write((uint)0);
            }
        }
        public void Deserialize(BinaryReader reader)
        {
            Order = reader.ReadSerializable<MarkCipherBetOrder>();
            State = (MarkCipherOrderStatus)reader.ReadByte();
            TxHash = reader.ReadSerializable<UInt256>();
            var size = reader.ReadUInt32();
            if (size > 0)
            {
                BetTx = reader.ReadSerializable<AskTransaction>();
            }
        }
    }
    public class MarkCipherTermBetOrder : ISerializable
    {
        public MarkCipherBetOrderKey Key;
        public MarkCipherBetOrderValue Value;
        public virtual int Size => Key.Size + Value.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Key);
            writer.Write(Value);
        }
        public void Deserialize(BinaryReader reader)
        {
            Key = reader.ReadSerializable<MarkCipherBetOrderKey>();
            Value = reader.ReadSerializable<MarkCipherBetOrderValue>();
        }
    }
    public class MarkCipherTermClear : ISerializable
    {
        public MarkTerm Term;
        public MarkChannelRound ChannelRound;
        public uint SNO;

        public virtual int Size => Term.Size + ChannelRound.Size + sizeof(uint);

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
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Term);
            writer.Write(ChannelRound);
            writer.Write(SNO);
        }
        public void Deserialize(BinaryReader reader)
        {
            Term = reader.ReadSerializable<MarkTerm>();
            ChannelRound = reader.ReadSerializable<MarkChannelRound>();
            SNO = reader.ReadUInt32();
        }
    }

    public class MarkPortPlayerTermKey : ISerializable
    {
        public MarkTerm Term;
        public UInt160 PortHolder;
        public UInt160 Player;

        public virtual int Size => Term.Size + PortHolder.Size + Player.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Term);
            writer.Write(PortHolder);
            writer.Write(Player);
        }
        public void Deserialize(BinaryReader reader)
        {
            Term = reader.ReadSerializable<MarkTerm>();
            PortHolder = reader.ReadSerializable<UInt160>();
            Player = reader.ReadSerializable<UInt160>();
        }
        public class MarkPortPlayerTermValue : ISerializable
        {
            public Fixed8 TotalBetAmount;
            public Fixed8 TotalPrizeAmount;
            public UInt256 PayFeeTxHash;
            public Fixed8 FeeAmount;
            public virtual int Size => TotalBetAmount.Size + TotalPrizeAmount.Size + PayFeeTxHash.Size + FeeAmount.Size;
            public MarkPortPlayerTermValue()
            {
                TotalBetAmount = Fixed8.Zero;
                TotalPrizeAmount = Fixed8.Zero;
                PayFeeTxHash = UInt256.Zero;
                FeeAmount = Fixed8.Zero;
            }
            public void Serialize(BinaryWriter writer)
            {
                writer.Write(TotalBetAmount);
                writer.Write(TotalPrizeAmount);
                writer.Write(PayFeeTxHash);
                writer.Write(FeeAmount);
            }
            public void Deserialize(BinaryReader reader)
            {
                TotalBetAmount = reader.ReadSerializable<Fixed8>();
                TotalPrizeAmount = reader.ReadSerializable<Fixed8>();
                PayFeeTxHash = reader.ReadSerializable<UInt256>();
                FeeAmount = reader.ReadSerializable<Fixed8>();
            }
        }
        public class MarkPortPlayerTermRecord : ISerializable
        {
            public MarkPortPlayerTermKey MarkPortPlayerTermKey;
            public MarkPortPlayerTermValue MarkPortPlayerTermValue;
            public virtual int Size => MarkPortPlayerTermKey.Size + MarkPortPlayerTermValue.Size;
            public void Serialize(BinaryWriter writer)
            {
                writer.Write(MarkPortPlayerTermKey);
                writer.Write(MarkPortPlayerTermValue);
            }
            public void Deserialize(BinaryReader reader)
            {
                MarkPortPlayerTermKey = reader.ReadSerializable<MarkPortPlayerTermKey>();
                MarkPortPlayerTermValue = reader.ReadSerializable<MarkPortPlayerTermValue>();
            }
        }
    }
}
