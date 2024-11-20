using System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1;
using OX;
using OX.Casino;
using OX.Cryptography;
using OX.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OX.BMS
{
    public class GuessAnswerKey : ISerializable
    {
        public MarkTerm Term;
        public MarkChannelRound ChannelRound;

        public virtual int Size => Term.Size + ChannelRound.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Term);
            writer.Write(ChannelRound);
        }
        public void Deserialize(BinaryReader reader)
        {
            Term = reader.ReadSerializable<MarkTerm>();
            ChannelRound = reader.ReadSerializable<MarkChannelRound>();
        }
        public override string ToString()
        {
            return $"{Term.ToString()}-{ChannelRound.ToString()}";
        }
    }
    public class GuessAnswerValue : ISerializable
    {
        public byte P1;
        public byte P2;
        public byte P3;
        public byte P4;
        public byte P5;
        public byte P6;
        public byte T;
        public byte G;
        public Zodiac Zodiac;
        public virtual int Size => sizeof(byte) * 8 + sizeof(Zodiac);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(P1);
            writer.Write(P2);
            writer.Write(P3);
            writer.Write(P4);
            writer.Write(P5);
            writer.Write(P6);
            writer.Write(T);
            writer.Write(G);
            writer.Write((byte)Zodiac);
        }
        public void Deserialize(BinaryReader reader)
        {
            P1 = reader.ReadByte();
            P2 = reader.ReadByte();
            P3 = reader.ReadByte();
            P4 = reader.ReadByte();
            P5 = reader.ReadByte();
            P6 = reader.ReadByte();
            T = reader.ReadByte();
            G = reader.ReadByte();
            Zodiac = (Zodiac)reader.ReadByte();
        }
        public byte[] GetAllCodes()
        {
            List<byte> list = new List<byte>();
            list.Add(T);
            list.Add(P1);
            list.Add(P2);
            list.Add(P3);
            list.Add(P4);
            list.Add(P5);
            list.Add(P6);
            return list.ToArray();
        }
        public byte[] GetAllNormalCodes()
        {
            List<byte> list = new List<byte>();
            list.Add(P1);
            list.Add(P2);
            list.Add(P3);
            list.Add(P4);
            list.Add(P5);
            list.Add(P6);
            return list.ToArray();
        }
        public override string ToString()
        {
            return $"{this.P1},{this.P2},{this.P3},{this.P4},{this.P5},{this.P6},{this.T}#{this.G}";
        }
    }
    public class GuessAnswer : ISerializable
    {
        public GuessAnswerKey Key;
        public GuessAnswerValue Value;

        public virtual int Size => Key.Size + Value.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Key);
            writer.Write(Value);
        }
        public void Deserialize(BinaryReader reader)
        {
            Key = reader.ReadSerializable<GuessAnswerKey>();
            Value = reader.ReadSerializable<GuessAnswerValue>();
        }

        public Zodiac[] GetAllZodiacs()
        {
            List<Zodiac> list = new List<Zodiac>();
            bool ok = true;
            if (this.Value.T.TryGetZodiac(this.Key.Term.Year, out Zodiac z))
            {
                if (!list.Contains(z)) list.Add(z);
            }
            else
                ok = false;

            if (this.Value.P1.TryGetZodiac(this.Key.Term.Year, out Zodiac z1))
            {
                if (!list.Contains(z1)) list.Add(z1);
            }
            else ok = false;
            if (this.Value.P2.TryGetZodiac(this.Key.Term.Year, out Zodiac z2))
            {
                if (!list.Contains(z2)) list.Add(z2);
            }
            else ok = false;
            if (this.Value.P3.TryGetZodiac(this.Key.Term.Year, out Zodiac z3))
            {
                if (!list.Contains(z3)) list.Add(z3);
            }
            else ok = false;
            if (this.Value.P4.TryGetZodiac(this.Key.Term.Year, out Zodiac z4))
            {
                if (!list.Contains(z4)) list.Add(z4);
            }
            else ok = false;
            if (this.Value.P5.TryGetZodiac(this.Key.Term.Year, out Zodiac z5))
            {
                if (!list.Contains(z5)) list.Add(z5);
            }
            else ok = false;
            if (this.Value.P6.TryGetZodiac(this.Key.Term.Year, out Zodiac z6))
            {
                if (!list.Contains(z6)) list.Add(z6);
            }
            else ok = false;
            if (!ok)
                throw new Exception("zodiac transfer error");
            return list.ToArray();
        }
        public Zodiac GetSpecialZodiac()
        {
            if (this.Value.T.TryGetZodiac(this.Key.Term.Year, out Zodiac z))
            {
                return z;
            }
            throw new Exception("special code error");
        }
        public Zodiac GetMasterZodiac()
        {
            byte code = 1;
            if (code.TryGetZodiac(this.Key.Term.Year, out Zodiac z))
            {
                return z;
            }
            throw new Exception("special code error");
        }
    }
    public class GuessAnswerReply : ISerializable
    {
        public MarkTerm Term;
        public GuessAnswer[] Answers;
        public virtual int Size => Term.Size + Answers.GetVarSize();

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Term);
            writer.Write(Answers);
        }
        public void Deserialize(BinaryReader reader)
        {
            Term = reader.ReadSerializable<MarkTerm>();
            Answers = reader.ReadSerializableArray<GuessAnswer>();
        }

    }

    public class BetPoint : IEquatable<BetPoint>, ISerializable
    {

        private byte[] data_bytes;
        public int Size => data_bytes.GetVarSize();
        private UInt256 _hash = null;
        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    _hash = new UInt256(Crypto.Default.Hash256(data_bytes));
                }
                return _hash;
            }
        }
        public BetPoint()
        {
            this.data_bytes = new byte[0];
        }
        public BetPoint(byte[] value) : this()
        {
            if (value.IsNotNullAndEmpty())
                this.data_bytes = value.Order().ToArray();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarBytes(this.data_bytes);
        }
        public void Deserialize(BinaryReader reader)
        {
            data_bytes = reader.ReadVarBytes();
        }



        public bool Equals(BetPoint other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (data_bytes.Length != other.data_bytes.Length)
                return false;
            return data_bytes.Order().SequenceEqual(other.data_bytes.Order());
        }


        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is BetPoint))
                return false;
            return this.Equals((BetPoint)obj);
        }

        public override int GetHashCode()
        {
            return ToInt32(data_bytes.Order().ToArray(), 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        unsafe internal int ToInt32(byte[] value, int startIndex)
        {
            fixed (byte* pbyte = &value[startIndex])
            {
                return *((int*)pbyte);
            }
        }





        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ToArray()
        {
            return data_bytes;
        }


        public override string ToString()
        {
            return "0x" + data_bytes.Reverse().ToHexString();
        }
        public static bool TryFromChinaDisplayString(BetChannel channel, byte method, string displayString, out BetPoint betpoint)
        {
            betpoint = default;
            try
            {
                if (channel == BetChannel.MarkSix)
                {
                    MarkSixBetMethod m = (MarkSixBetMethod)method;
                    switch (m)
                    {
                        case MarkSixBetMethod.TM:
                            if (!byte.TryParse(displayString, out byte code)) return false;
                            betpoint = new BetPoint(new byte[] { code });
                            break;
                        case MarkSixBetMethod.ZM2:
                            var ss_zm2 = displayString.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            if (ss_zm2.Length != 2) return false;
                            List<byte> list_zm2 = new List<byte>();
                            foreach (var s in ss_zm2)
                            {
                                if (!byte.TryParse(s, out byte code_zm2)) return false;
                                list_zm2.Add(code_zm2);
                            }
                            betpoint = new BetPoint(list_zm2.ToArray());
                            break;
                        case MarkSixBetMethod.TP:
                            var ss_tp = displayString.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            if (ss_tp.Length != 2) return false;
                            List<byte> list_tp = new List<byte>();
                            foreach (var s in ss_tp)
                            {
                                if (!byte.TryParse(s, out byte code_tp)) return false;
                                list_tp.Add(code_tp);
                            }
                            betpoint = new BetPoint(list_tp.ToArray());
                            break;
                        case MarkSixBetMethod.PT1X:
                            bool find_pt1x = false;
                            byte z_pt1x = 0;
                            foreach (var zodiac in NoneFlagEnumHelper.All<Zodiac>())
                            {
                                if (zodiac.GetName().Name == displayString)
                                {
                                    find_pt1x = true;
                                    z_pt1x = (byte)zodiac;
                                    break;
                                }
                            }
                            if (!find_pt1x) return false;
                            betpoint = new BetPoint(new byte[] { z_pt1x });
                            break;
                        case MarkSixBetMethod.PT2X:
                            var ss_PT2X = displayString.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            if (ss_PT2X.Length != 2) return false;
                            List<byte> list_PT2X = new List<byte>();
                            foreach (var s in ss_PT2X)
                            {
                                foreach (var zodiac in NoneFlagEnumHelper.All<Zodiac>())
                                {
                                    if (zodiac.GetName().Name == s)
                                    {
                                        var b = (byte)zodiac;
                                        if (list_PT2X.Contains(b)) return false;
                                        list_PT2X.Add(b);
                                    }
                                }
                            }
                            if (list_PT2X.Count != 2) return false;
                            betpoint = new BetPoint(list_PT2X.ToArray());
                            break;
                        case MarkSixBetMethod.PT3X:
                            var ss_PT3X = displayString.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            if (ss_PT3X.Length != 3) return false;
                            List<byte> list_PT3X = new List<byte>();
                            foreach (var s in ss_PT3X)
                            {
                                foreach (var zodiac in NoneFlagEnumHelper.All<Zodiac>())
                                {
                                    if (zodiac.GetName().Name == s)
                                    {
                                        var b = (byte)zodiac;
                                        if (list_PT3X.Contains(b)) return false;
                                        list_PT3X.Add(b);
                                    }
                                }
                            }
                            if (list_PT3X.Count != 3) return false;
                            betpoint = new BetPoint(list_PT3X.ToArray());
                            break;
                        case MarkSixBetMethod.PT4X:
                            var ss_PT4X = displayString.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            if (ss_PT4X.Length != 4) return false;
                            List<byte> list_PT4X = new List<byte>();
                            foreach (var s in ss_PT4X)
                            {
                                foreach (var zodiac in NoneFlagEnumHelper.All<Zodiac>())
                                {
                                    if (zodiac.GetName().Name == s)
                                    {
                                        var b = (byte)zodiac;
                                        if (list_PT4X.Contains(b)) return false;
                                        list_PT4X.Add(b);
                                    }
                                }
                            }
                            if (list_PT4X.Count != 4) return false;
                            betpoint = new BetPoint(list_PT4X.ToArray());
                            break;
                        case MarkSixBetMethod.PT5X:
                            var ss_PT5X = displayString.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            if (ss_PT5X.Length != 5) return false;
                            List<byte> list_PT5X = new List<byte>();
                            foreach (var s in ss_PT5X)
                            {
                                foreach (var zodiac in NoneFlagEnumHelper.All<Zodiac>())
                                {
                                    if (zodiac.GetName().Name == s)
                                    {
                                        var b = (byte)zodiac;
                                        if (list_PT5X.Contains(b)) return false;
                                        list_PT5X.Add(b);
                                    }
                                }
                            }
                            if (list_PT5X.Count != 5) return false;
                            betpoint = new BetPoint(list_PT5X.ToArray());
                            break;
                        case MarkSixBetMethod.TM_Color:
                            bool find_TM_Color = false;
                            byte z_TM_Color = 0;
                            foreach (var zodiac in NoneFlagEnumHelper.All<MarkSixColor>())
                            {
                                if (zodiac.GetName().Name == displayString)
                                {
                                    find_TM_Color = true;
                                    z_TM_Color = (byte)zodiac;
                                    break;
                                }
                            }
                            if (!find_TM_Color) return false;
                            betpoint = new BetPoint(new byte[] { z_TM_Color });
                            break;
                        case MarkSixBetMethod.DXDSJY:
                            bool find_DXDSJY = false;
                            byte z_DXDSJY = 0;
                            foreach (var dsf in NoneFlagEnumHelper.All<DualisticFull>())
                            {
                                if (dsf.GetName().Name == displayString)
                                {
                                    find_DXDSJY = true;
                                    z_DXDSJY = (byte)dsf;
                                    break;
                                }
                            }
                            if (!find_DXDSJY) return false;
                            betpoint = new BetPoint(new byte[] { z_DXDSJY });
                            break;
                        case MarkSixBetMethod.Color_DS:
                            bool find_ColorDS = false;
                            byte z_ColorDS = 0;
                            foreach (var cds in NoneFlagEnumHelper.All<ColorDS>())
                            {
                                if (cds.GetName().Name == displayString)
                                {
                                    find_ColorDS = true;
                                    z_ColorDS = (byte)cds;
                                    break;
                                }
                            }
                            if (!find_ColorDS) return false;
                            betpoint = new BetPoint(new byte[] { z_ColorDS });
                            break;
                        case MarkSixBetMethod.PTTail:
                            var ds = displayString.Substring(0, 1);
                            if (!byte.TryParse(ds, out var tail)) return false;
                            betpoint = new BetPoint(new byte[] { tail });
                            break;
                    }
                }
                else if (channel == BetChannel.MarkOne)
                {
                    MarkOneBetMethod m = (MarkOneBetMethod)method;
                    switch (m)
                    {
                        case MarkOneBetMethod.Zodiac:
                            bool find_pt1x = false;
                            byte z_pt1x = 0;
                            foreach (var zodiac in NoneFlagEnumHelper.All<Zodiac>())
                            {
                                if (zodiac.GetName().Name == displayString)
                                {
                                    find_pt1x = true;
                                    z_pt1x = (byte)zodiac;
                                    break;
                                }
                            }
                            if (!find_pt1x) return false;
                            betpoint = new BetPoint(new byte[] { z_pt1x });
                            break;
                        case MarkOneBetMethod.ZodiacColor:
                            bool find_TM_Color = false;
                            byte z_TM_Color = 0;
                            foreach (var zodiac in NoneFlagEnumHelper.All<MarkSixColor>())
                            {
                                if (zodiac.GetName().Name == displayString)
                                {
                                    find_TM_Color = true;
                                    z_TM_Color = (byte)zodiac;
                                    break;
                                }
                            }
                            if (!find_TM_Color) return false;
                            betpoint = new BetPoint(new byte[] { z_TM_Color });
                            break;
                        case MarkOneBetMethod.JYYYTD:
                            bool find_DXDSJY = false;
                            byte z_DXDSJY = 0;
                            foreach (var dsf in NoneFlagEnumHelper.All<DualisticSimple>())
                            {
                                if (dsf.GetName().Name == displayString)
                                {
                                    find_DXDSJY = true;
                                    z_DXDSJY = (byte)dsf;
                                    break;
                                }
                            }
                            if (!find_DXDSJY) return false;
                            betpoint = new BetPoint(new byte[] { z_DXDSJY });
                            break;
                    }
                }
            }
            catch
            {
                return false;
            }
            return betpoint.IsNotNull();
        }
        public string ToDisplayString(BetChannel channel, byte method, bool isChina)
        {
            string str = string.Empty;
            if (channel == BetChannel.MarkSix)
            {
                MarkSixBetMethod m = (MarkSixBetMethod)method;
                switch (m)
                {
                    case MarkSixBetMethod.TM:
                    case MarkSixBetMethod.ZM2:
                    case MarkSixBetMethod.TP:
                        //case FullMarkSixBetMethod.ZM3:
                        str = string.Join(",", this.data_bytes.ToArray());
                        break;
                    case MarkSixBetMethod.PT1X:
                    case MarkSixBetMethod.PT2X:
                    case MarkSixBetMethod.PT3X:
                    case MarkSixBetMethod.PT4X:
                    case MarkSixBetMethod.PT5X:
                        List<string> ss = new List<string>();
                        foreach (var b in this.data_bytes.ToArray())
                        {
                            Zodiac zd = (Zodiac)b;
                            var name = zd.GetName();
                            var s = isChina ? name.Name : name.EngName;
                            ss.Add(s);
                        }
                        str = string.Join(",", ss);
                        break;
                    case MarkSixBetMethod.TM_Color:
                        List<string> ssc = new List<string>();
                        foreach (var b in this.data_bytes.ToArray())
                        {
                            MarkSixColor zd = (MarkSixColor)b;
                            var name = zd.GetName();
                            var s = isChina ? name.Name : name.EngName;
                            ssc.Add(s);
                        }
                        str = string.Join(",", ssc);
                        break;
                    case MarkSixBetMethod.DXDSJY:
                        List<string> dxdsjys = new List<string>();
                        foreach (var b in this.data_bytes.ToArray())
                        {
                            DualisticFull zd = (DualisticFull)b;
                            var name = zd.GetName();
                            var ssDXDSJY = isChina ? name.Name : name.EngName;
                            dxdsjys.Add(ssDXDSJY);
                        }
                        str = string.Join(",", dxdsjys);
                        break;
                    case MarkSixBetMethod.Color_DS:
                        List<string> cdss = new List<string>();
                        foreach (var b in this.data_bytes.ToArray())
                        {
                            ColorDS zd = (ColorDS)b;
                            var name = zd.GetName();
                            var sss = isChina ? name.Name : name.EngName;
                            cdss.Add(sss);
                        }
                        str = string.Join(",", cdss);
                        break;
                    case MarkSixBetMethod.PTTail:
                        var tailStr = this.data_bytes[0];
                        var sPTTail = isChina ? "尾" : "tail";
                        str = tailStr.ToString() + sPTTail;
                        break;
                }
            }
            else if (channel == BetChannel.MarkOne)
            {
                MarkOneBetMethod m = (MarkOneBetMethod)method;
                switch (m)
                {
                    case MarkOneBetMethod.Zodiac:
                        List<string> ss = new List<string>();
                        foreach (var b in this.data_bytes.ToArray())
                        {
                            Zodiac zd = (Zodiac)b;
                            var name = zd.GetName();
                            var s = isChina ? name.Name : name.EngName;
                            ss.Add(s);
                        }
                        str = string.Join(",", ss);
                        break;
                    case MarkOneBetMethod.ZodiacColor:
                        List<string> ssc = new List<string>();
                        foreach (var b in this.data_bytes.ToArray())
                        {
                            MarkSixColor zd = (MarkSixColor)b;
                            var name = zd.GetName();
                            var s = isChina ? name.Name : name.EngName;
                            ssc.Add(s);
                        }
                        str = string.Join(",", ssc);
                        break;
                    case MarkOneBetMethod.JYYYTD:
                        List<string> dxdsjys = new List<string>();
                        foreach (var b in this.data_bytes.ToArray())
                        {
                            DualisticSimple zd = (DualisticSimple)b;
                            var name = zd.GetName();
                            var s = isChina ? name.Name : name.EngName;
                            dxdsjys.Add(s);
                        }
                        str = string.Join(",", dxdsjys);
                        break;
                }
            }
            return str;
        }

        public static bool operator ==(BetPoint left, BetPoint right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(BetPoint left, BetPoint right)
        {
            return !(left == right);
        }
    }
    public class BitMarkSixBetTarget : IEquatable<BitMarkSixBetTarget>, ISerializable
    {
        public byte Method;
        public BetPoint BetPoint;


        public virtual int Size => sizeof(byte) + BetPoint.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Method);
            writer.Write(BetPoint);
        }
        public void Deserialize(BinaryReader reader)
        {
            Method = reader.ReadByte();
            BetPoint = reader.ReadSerializable<BetPoint>();
        }


        public bool Equals(BitMarkSixBetTarget other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return this.Method == other.Method && this.BetPoint == other.BetPoint;
        }


        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is BitMarkSixBetTarget))
                return false;
            return this.Equals((BitMarkSixBetTarget)obj);
        }

        public override int GetHashCode()
        {
            return this.Method.GetHashCode() + this.BetPoint.GetHashCode();
        }



        public override string ToString()
        {
            return $"{this.Method}--{this.BetPoint.ToArray().ToHexString()}";
        }


        public static bool operator ==(BitMarkSixBetTarget left, BitMarkSixBetTarget right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(BitMarkSixBetTarget left, BitMarkSixBetTarget right)
        {
            return !(left == right);
        }
    }
    public class BitMarkSixBet : ISerializable
    {
        public BitMarkSixBetTarget BetTarget;
        public uint Amount;


        public virtual int Size => BetTarget.Size + sizeof(uint);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(BetTarget);
            writer.Write(Amount);
        }
        public void Deserialize(BinaryReader reader)
        {
            BetTarget = reader.ReadSerializable<BitMarkSixBetTarget>();
            Amount = reader.ReadUInt32();
        }

    }
    public class BitMarkSixBetPrize : ISerializable
    {
        public uint Amount = 0;
        public virtual int Size => sizeof(uint);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Amount);
        }
        public void Deserialize(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
        }
    }
    public interface IBetUI
    {

    }
    public abstract class BaseBitMarkSixMethodHandler
    {
        public BaseBitMarkSixMethodHandler()
        {

        }
        public abstract BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet);
        public abstract bool VerifyBet(GuessAnswer result, BitMarkSixBet bet);
        public abstract uint Combine(BitMarkSixBet[] betitems);
    }
}
