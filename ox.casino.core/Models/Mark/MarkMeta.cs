using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Akka.Util;
using OX.Cryptography;
using OX.IO;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;

namespace OX.BMS
{
    [global::System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    [ComVisible(true)]
    public class NameAttribute : Attribute
    {
        public NameAttribute(string name, string engName)
        {
            this.Name = name;
            this.EngName = engName;
        }
        public string Name { get; set; }
        public string EngName { get; set; }
    }
    [global::System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    [ComVisible(true)]
    public class MethodSettingAttribute : Attribute
    {
        public string Name { get; set; }
        public string EngName { get; set; }
        public ushort MaxOdds { get; set; }
        public ushort MinOdds { get; set; }
        public ushort DefaultOdds { get; set; }
        public ushort MaxCommission { get; set; }
        public ushort MinCommission { get; set; }
        public ushort DefaultCommission { get; set; }
        public Type MethodHandlerType { get; set; }
        public BitMarkSixBetPrize CheckBet(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            if (MethodHandlerType.IsNotNull())
            {
                var instance = Activator.CreateInstance(MethodHandlerType);
                if (instance.IsNotNull())
                {
                    BaseBitMarkSixMethodHandler handler = instance as BaseBitMarkSixMethodHandler;
                    return handler.Handle(methodSetting, result, bet);
                }
            }
            return default;
        }
        public bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            if (MethodHandlerType.IsNotNull())
            {
                var instance = Activator.CreateInstance(MethodHandlerType);
                if (instance.IsNotNull())
                {
                    BaseBitMarkSixMethodHandler handler = instance as BaseBitMarkSixMethodHandler;
                    return handler.VerifyBet(result, bet);
                }
            }
            return false;
        }
        public uint Combine(BitMarkSixBet[] betitems)
        {
            if (MethodHandlerType.IsNotNull())
            {
                var instance = Activator.CreateInstance(MethodHandlerType);
                if (instance.IsNotNull())
                {
                    BaseBitMarkSixMethodHandler handler = instance as BaseBitMarkSixMethodHandler;
                    return handler.Combine(betitems);
                }
            }
            return 0;
        }

    }
    public class BMSPlayMethod : ISerializable
    {
        public byte Method;
        public ushort Odds;
        public ushort Fee;
        public virtual int Size => sizeof(byte) + sizeof(ushort) + sizeof(ushort);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Method);
            writer.Write(Odds);
            writer.Write(Fee);
        }
        public void Deserialize(BinaryReader reader)
        {
            Method = reader.ReadByte();
            Odds = reader.ReadUInt16();
            Fee = reader.ReadUInt16();
        }
    }
    public class MarkTerm : IEquatable<MarkTerm>, IComparable<MarkTerm>, ISerializable
    {
        public ushort Year;
        public byte Month;
        public byte Day;
        public virtual int Size => sizeof(ushort) + sizeof(byte) + sizeof(byte);
        public MarkTerm()
        { }
        public MarkTerm(ushort year, byte month, byte day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Year);
            writer.Write(Month);
            writer.Write(Day);
        }
        public void Deserialize(BinaryReader reader)
        {
            Year = reader.ReadUInt16();
            Month = reader.ReadByte();
            Day = reader.ReadByte();
        }
        public override string ToString()
        {
            return $"{Year}-{Month}-{Day}";
        }
        public DateTime ToDateTime(int hour = 0, int minute = 0, int second = 0)
        {
            return new DateTime(Year, Month, Day, hour, minute, second);
        }
        public int ToValue()
        {
            return Year * 10000 + Month * 100 + Day;
        }
        public bool Equals(MarkTerm other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return this.Year == other.Year && this.Month == other.Month && this.Day == other.Day;
        }

        public override int GetHashCode()
        {
            return Year.GetHashCode() + Month.GetHashCode() + Day.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is MarkTerm))
                return false;
            return this.Equals((MarkTerm)obj);
        }
        public static bool operator ==(MarkTerm left, MarkTerm right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }
        public static bool operator !=(MarkTerm left, MarkTerm right)
        {
            return !(left == right);
        }
        public int CompareTo(MarkTerm obj)
        {
            int result;
            int v1 = this.ToValue();
            int v2 = obj.ToValue();
            if (v1 > v2)
                result = -1;
            else if (v1 == v2)
                result = 0;
            else
                result = 1;

            return result;
        }
    }
    public class MarkChannelRound : IEquatable<MarkChannelRound>, ISerializable
    {
        public BetChannel Channel;
        public byte Round;
        public int Size => sizeof(BetChannel) + sizeof(byte);
        private UInt256 _hash = null;
        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    _hash = new UInt256(Crypto.Default.Hash256(this.ToArray()));
                }
                return _hash;
            }
        }
        public MarkChannelRound()
        {

        }
        public MarkChannelRound(BetChannel channel, byte round)
        {
            Channel = channel;
            Round = round;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Channel);
            writer.Write(Round);
        }
        public void Deserialize(BinaryReader reader)
        {
            Channel = (BetChannel)reader.ReadByte();
            Round = reader.ReadByte();
        }



        public bool Equals(MarkChannelRound other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return this.Channel == other.Channel && this.Round == other.Round;
        }


        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is MarkChannelRound))
                return false;
            return this.Equals((MarkChannelRound)obj);
        }

        public override int GetHashCode()
        {
            return (byte)this.Channel * 10 + this.Round;
        }


        public override string ToString()
        {
            return $"{this.Channel}-{this.Round}";
        }


        public static bool operator ==(MarkChannelRound left, MarkChannelRound right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(MarkChannelRound left, MarkChannelRound right)
        {
            return !(left == right);
        }
    }

}
