using Org.BouncyCastle.Utilities;
using System.Linq;
using System.Collections.Generic;
using OX.IO;
using OX.Ledger;
using System.Collections.ObjectModel;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Channels;
using OX.Cryptography;
using OX.Network.P2P;


namespace OX.BMS
{
    public class MarkInboundOrderKey : ISerializable
    {
        public UInt160 Reception;
        public MarkTerm Term;
        public MarkChannelRound ChannelRound;
        public ulong CNO;
        public InboundOrigin InboundOrigin;
        public virtual int Size => Reception.Size+ Term.Size + ChannelRound.Size + sizeof(ulong) + sizeof(InboundOrigin);

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
            writer.Write(Reception);
            writer.Write(Term);
            writer.Write(ChannelRound);
            writer.Write(CNO);
            writer.Write((byte)InboundOrigin);
        }
        public void Deserialize(BinaryReader reader)
        {
            Reception = reader.ReadSerializable<UInt160>();
            Term = reader.ReadSerializable<MarkTerm>();
            ChannelRound = reader.ReadSerializable<MarkChannelRound>();
            CNO = reader.ReadUInt64();
            InboundOrigin = (InboundOrigin)reader.ReadByte();
        }
    }
    public class MarknboundOrderValue : ISerializable
    {
        public BitMarkSixBet[] BetItems;
        /// <summary>
        /// 0:wait post
        /// 1:merge post
        /// </summary>
        public byte State;
        public string FromName;
        public virtual int Size => BetItems.GetVarSize() + sizeof(byte)+FromName.GetVarSize();
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
            writer.Write(BetItems);
            writer.Write(State);
            writer.WriteVarString(FromName);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetItems = reader.ReadSerializableArray<BitMarkSixBet>();
            State = reader.ReadByte();
            FromName = reader.ReadVarString();
        }
    }
    public class MarkInboundOrder : IInBoundOrder, ISerializable
    {
        public MarkInboundOrderKey OrderHead;
        public MarknboundOrderValue OrderBody;
        public virtual int Size => OrderHead.Size + OrderBody.Size;
        public uint Amount
        {
            get
            {
                uint amt = 0;
                if (OrderBody.BetItems.IsNotNullAndEmpty())
                {
                    amt = (uint)OrderBody.BetItems.Sum(m => (int)m.Amount);
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
            writer.Write(OrderHead);
            writer.Write(OrderBody);
        }
        public void Deserialize(BinaryReader reader)
        {
            OrderHead = reader.ReadSerializable<MarkInboundOrderKey>();
            OrderBody = reader.ReadSerializable<MarknboundOrderValue>();
        }
    }
    public class MarkMemoryBetSet : ISerializable
    {
        public Dictionary<BitMarkSixBetTarget, uint> BetItems;
        public byte Method;
        public MarkMemoryBetSet()
        {
            BetItems = new Dictionary<BitMarkSixBetTarget, uint>();
        }
        public virtual int Size => sizeof(uint) + BetItems.Sum(m => m.Key.Size) + BetItems.Count * sizeof(uint) + sizeof(byte);
        public uint Amount
        {
            get
            {
                uint amt = 0;
                if (BetItems.IsNotNullAndEmpty())
                {
                    amt = (uint)BetItems.Sum(m => (int)m.Value);
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
            var items = BetItems.Where(p => p.Value > 0).ToArray();
            writer.Write((uint)items.Length);
            foreach (var pair in items)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
            writer.Write(Method);
        }
        public void Deserialize(BinaryReader reader)
        {
            uint count = reader.ReadUInt32();
            BetItems = new Dictionary<BitMarkSixBetTarget, uint>();
            for (uint i = 0; i < count; i++)
            {
                var key = reader.ReadSerializable<BitMarkSixBetTarget>();
                var value = reader.ReadUInt32();
                BetItems[key] = value;
            }
            Method = reader.ReadByte();
        }
        public uint GetBetTargetAmount(BitMarkSixBetTarget betTarget)
        {
            if (BetItems.TryGetValue(betTarget, out uint v))
                return v;
            return 0;
        }
        public void ResetBetTargetAmount(BitMarkSixBetTarget betTarget, uint amount)
        {
            BetItems[betTarget] = amount;
        }
        public void PlusBetTargetAmount(BitMarkSixBetTarget betTarget, int amount)
        {
            if (BetItems.TryGetValue(betTarget, out uint v))
            {
                int newAmount = (int)v + amount;
                if (newAmount < 0) newAmount = 0;
                BetItems[betTarget] = (uint)newAmount;
            }
            else
            {
                if (amount < 0) amount = 0;
                BetItems[betTarget] = (uint)amount;
            }
        }
        public bool RemoveBetTarget(BitMarkSixBetTarget betTarget)
        {
            return BetItems.Remove(betTarget);
        }
        public void ClearAll()
        {
            this.BetItems.Clear();
        }
    }
    public class MarkMemoryOrderSet : ISerializable
    {
        public Dictionary<byte, MarkMemoryBetSet> BetSets;
        public MarkMemoryOrderSet()
        {
            BetSets = new Dictionary<byte, MarkMemoryBetSet>();
        }
        public virtual int Size => sizeof(uint) + BetSets.Count * sizeof(byte) + BetSets.Sum(m => m.Value.Size);
        public uint Amount
        {
            get
            {
                uint amt = 0;
                if (BetSets.IsNotNullAndEmpty())
                {
                    amt = (uint)BetSets.Sum(m => (int)m.Value.Amount);
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
            var items = BetSets.Where(p => p.Value.BetItems.Any()).ToArray();
            writer.Write((uint)items.Length);
            foreach (var pair in items)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
        }
        public void Deserialize(BinaryReader reader)
        {
            uint count = reader.ReadUInt32();
            BetSets = new Dictionary<byte, MarkMemoryBetSet>();
            for (uint i = 0; i < count; i++)
            {
                var b = reader.ReadByte();
                var value = reader.ReadSerializable<MarkMemoryBetSet>();
                BetSets[b] = value;
            }
        }
        public MarkMemoryBetSet GetMethodBetSet(byte method)
        {
            if (!BetSets.TryGetValue(method, out var betSet))
            {
                betSet = new MarkMemoryBetSet() { Method = method };
                BetSets[method] = betSet;
            }
            return betSet;
        }
        public void ClearAll()
        {
            this.BetSets.Clear();
        }
    }
}
