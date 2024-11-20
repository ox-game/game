using Org.BouncyCastle.Utilities;
using System.Linq;
using OX.IO;
using OX.Ledger;
using System.Collections.ObjectModel;
using System.IO;
using OX.Casino;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace OX.BMS
{
    public class RegMarkMemberRequest : ISerializable
    {
        public MarkMemberType MemberType;
        public UInt160 PortHolder;
        public Fixed8 MinBondBalance;
        public byte Flag;
        public byte[] Data;
        public virtual int Size => sizeof(MarkMemberType) + PortHolder.Size + MinBondBalance.Size + sizeof(byte) + Data.GetVarSize();
        public RegMarkMemberRequest()
        {
            Data = new byte[0];
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)MemberType);
            writer.Write(PortHolder);
            writer.Write(MinBondBalance);
            writer.Write(Flag);
            writer.WriteVarBytes(Data);
        }
        public void Deserialize(BinaryReader reader)
        {
            MemberType = (MarkMemberType)reader.ReadByte();
            PortHolder = reader.ReadSerializable<UInt160>();
            MinBondBalance = reader.ReadSerializable<Fixed8>();
            Flag = reader.ReadByte();
            Data = reader.ReadVarBytes();
        }
    }
    //public class SetMarkMemberRequest : ISerializable
    //{
    //    public UInt160 BetAddress;
    //    public MarkSetting Setting;
    //    public virtual int Size => BetAddress.Size + Setting.Size;
    //    public void Serialize(BinaryWriter writer)
    //    {
    //        writer.Write(BetAddress);
    //        writer.Write(Setting);
    //    }
    //    public void Deserialize(BinaryReader reader)
    //    {
    //        BetAddress = reader.ReadSerializable<UInt160>();
    //        Setting = reader.ReadSerializable<MarkSetting>();
    //    }
    //    public bool VerifySetTime()
    //    {
    //        var dt = BitSixBetHelper.BeijingNow();
    //        return dt.Day == 1 && dt.Hour >= 0 && dt.Hour < 12;
    //    }
    //}
    public class MarkSetting : ISerializable
    {
        public Fixed8 MaxAmount;
        public byte Flag;
        public BMSPlayMethod[] MarkSixMethods;
        public BMSPlayMethod[] MarkOneMethods;
        public byte[] Data;
        public virtual int Size => MaxAmount.Size + sizeof(byte) + MarkSixMethods.GetVarSize() + MarkOneMethods.GetVarSize() + Data.GetVarSize();
        public MarkSetting()
        {
            MaxAmount = Fixed8.Zero;
            Flag = 0;
            Data = new byte[0];
            this.InitDefault();
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MaxAmount);
            writer.Write(Flag);
            writer.Write(MarkSixMethods);
            writer.Write(MarkOneMethods);
            writer.WriteVarBytes(Data);
        }
        public void Deserialize(BinaryReader reader)
        {
            MaxAmount = reader.ReadSerializable<Fixed8>();
            Flag = reader.ReadByte();
            MarkSixMethods = reader.ReadSerializableArray<BMSPlayMethod>();
            MarkOneMethods = reader.ReadSerializableArray<BMSPlayMethod>();
            Data = reader.ReadVarBytes();
        }
        public bool Verify()
        {
            try
            {
                if (MarkSixMethods.GroupBy(item => item.Method).Where(group => group.Count() > 1).IsNotNullAndEmpty()) return false;
                if (MarkOneMethods.GroupBy(item => item.Method).Where(group => group.Count() > 1).IsNotNullAndEmpty()) return false;
                if (MarkSixMethods.Length != NoneFlagEnumHelper.All<MarkSixBetMethod>().Length) return false;
                //if (MarkOneMethods.Length != NoneFlagEnumHelper.All<MarkOneBetMethod>().Length) return false;
                foreach (var m in MarkSixMethods)
                {
                    var method = (MarkSixBetMethod)m.Method;
                    if (!method.VerifyMethodSetting(m)) return false;
                }
                //foreach (var m in MarkOneMethods)
                //{
                //    var method = (MarkOneBetMethod)m.Method;
                //    if (!method.VerifyMethodSetting(m)) return false;
                //}
            }
            catch
            {
                return false;
            }
            return true;
        }
        public void InitDefault()
        {
            List<BMSPlayMethod> fullsettings = new List<BMSPlayMethod>();


            foreach (var f in NoneFlagEnumHelper.All<MarkSixBetMethod>())
            {
                var setting = f.GetMethodSetting();
                if (setting.IsNotNull())
                {
                    fullsettings.Add(new BMSPlayMethod { Method = (byte)f, Odds = setting.DefaultOdds, Fee = setting.DefaultCommission });
                }
            }
            this.MarkSixMethods = fullsettings.ToArray();

            List<BMSPlayMethod> simplesettings = new List<BMSPlayMethod>();
            foreach (var f in NoneFlagEnumHelper.All<MarkOneBetMethod>())
            {
                var setting = f.GetMethodSetting();
                if (setting.IsNotNull())
                {
                    simplesettings.Add(new BMSPlayMethod { Method = (byte)f, Odds = setting.DefaultOdds, Fee = setting.DefaultCommission });
                }
            }
            this.MarkOneMethods = simplesettings.ToArray();
        }
    }
}
