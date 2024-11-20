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

namespace OX.BMS
{
    //signedData = Crypto.Default.Sign(raw, keys.PrivateKey, keys.PublicKey.EncodePoint(false).Skip(1).ToArray());
    public class MarkPlainBetOrder : ISerializable
    {
        public UInt160 Player;
        public MarkTerm Term;
        public MarkChannelRound ChannelRound;
        public BitMarkSixBet[] BetItems;
        public virtual int Size => Player.Size + Term.Size + ChannelRound.Size + BetItems.GetVarSize();
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
        }
        public void Deserialize(BinaryReader reader)
        {
            Player = reader.ReadSerializable<UInt160>();
            Term = reader.ReadSerializable<MarkTerm>();
            ChannelRound = reader.ReadSerializable<MarkChannelRound>();
            BetItems = reader.ReadSerializableArray<BitMarkSixBet>();
        }
    }
    public class MarkPlainCopyOrder : ISerializable
    {
        public MarkPlainBetOrder Order;
        public ulong Nonce;
        public virtual int Size => Order.Size + sizeof(ulong);
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
            writer.Write(Order);
            writer.Write(Nonce);
        }
        public void Deserialize(BinaryReader reader)
        {
            Order = reader.ReadSerializable<MarkPlainBetOrder>();
            Nonce = reader.ReadUInt64();
        }
    }



    public class MarkPlainBetOrderKey : IEquatable<MarkPlainBetOrderKey>, ISerializable
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
        public bool Equals(MarkPlainBetOrderKey other)
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
            if (!(obj is MarkPlainBetOrderKey))
                return false;
            return this.Equals((MarkPlainBetOrderKey)obj);
        }
        public static bool operator ==(MarkPlainBetOrderKey left, MarkPlainBetOrderKey right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }
        public static bool operator !=(MarkPlainBetOrderKey left, MarkPlainBetOrderKey right)
        {
            return !(left == right);
        }
    }
    public class MarkPlainBetOrderValue : ISerializable
    {
        public MarkPlainBetOrder Order;

        public MarkPlainOrderStatus State;
        public virtual int Size => Order.Size + sizeof(MarkPlainOrderStatus);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Order);
            writer.Write((byte)State);
        }
        public void Deserialize(BinaryReader reader)
        {
            Order = reader.ReadSerializable<MarkPlainBetOrder>();
            State = (MarkPlainOrderStatus)reader.ReadByte();
        }
    }
    public class MarkPlainTermBetOrder : ISerializable
    {
        public MarkPlainBetOrderKey Key;
        public MarkPlainBetOrderValue Value;
        public virtual int Size => Key.Size + Value.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Key);
            writer.Write(Value);
        }
        public void Deserialize(BinaryReader reader)
        {
            Key = reader.ReadSerializable<MarkPlainBetOrderKey>();
            Value = reader.ReadSerializable<MarkPlainBetOrderValue>();
        }
    }
}
