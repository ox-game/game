using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using OX.Cryptography.ECC;
using System.Security.Policy;
using System.IO;
using OX.Network.P2P;
using System.Runtime;
using System.Reflection;

namespace OX.BMS
{
    public static class BitSixHelper
    {
        public static NameAttribute GetName<EnumType>(this EnumType enm) where EnumType : struct
        {
            Type type = enm.GetType();
            string name = Enum.GetName(type, enm);
            FieldInfo field = type.GetField(name);
            return Attribute.GetCustomAttribute(field, typeof(NameAttribute)) as NameAttribute;
        }
        public static IEnumerable<MethodSettingAttribute> AllMethodSettings<EnumType>() where EnumType : struct
        {
            foreach (var enm in NoneFlagEnumHelper.All<EnumType>())
            {
                var setting = GetMethodSetting(enm);
                if (setting.IsNotNull())
                    yield return setting;
            }
        }
        public static MethodSettingAttribute GetMethodSetting<EnumType>(this EnumType enm) where EnumType : struct
        {
            Type type = enm.GetType();
            string name = Enum.GetName(type, enm);
            FieldInfo field = type.GetField(name);
            return Attribute.GetCustomAttribute(field, typeof(MethodSettingAttribute)) as MethodSettingAttribute;
        }
        public static bool VerifyMethodSetting(this MarkSixBetMethod method, BMSPlayMethod methodSetting)
        {
            if (methodSetting.Method != (byte)method) return false;
            Type type = method.GetType();
            string name = Enum.GetName(type, method);
            FieldInfo field = type.GetField(name);
            MethodSettingAttribute attribute = Attribute.GetCustomAttribute(field, typeof(MethodSettingAttribute)) as MethodSettingAttribute;
            if (attribute.IsNull()) return false;
            if (methodSetting.Odds > attribute.MaxOdds) return false;
            if (methodSetting.Odds < attribute.MinOdds) return false;
            if (methodSetting.Fee > attribute.MaxCommission) return false;
            if (methodSetting.Fee < attribute.MinCommission) return false;
            return true;
        }
        public static bool VerifyMethodSetting(this MarkOneBetMethod method, BMSPlayMethod methodSetting)
        {
            if (methodSetting.Method != (byte)method) return false;
            Type type = method.GetType();
            string name = Enum.GetName(type, method);
            FieldInfo field = type.GetField(name);
            MethodSettingAttribute attribute = Attribute.GetCustomAttribute(field, typeof(MethodSettingAttribute)) as MethodSettingAttribute;
            if (attribute.IsNull()) return false;
            if (methodSetting.Odds > attribute.MaxOdds) return false;
            if (methodSetting.Odds < attribute.MinOdds) return false;
            if (methodSetting.Fee > attribute.MaxCommission) return false;
            if (methodSetting.Fee < attribute.MinCommission) return false;
            return true;
        }
        public static bool TryGetZodiac(this byte Code, ushort Year, out Zodiac zodiac)
        {
            zodiac = default;
            if (Code > 49 || Code == 0) return false;
            var n = Year - 2024;
            var m = Code - n;
            while (m < 1)
            {
                m += 12;
            }
            while (m > 12)
            {
                m -= 12;
            }
            zodiac = (Zodiac)m;
            return true;
        }
        public static bool TryGetColor(this byte Code, out MarkSixColor codeColor)
        {
            codeColor = default;
            if (Code > 49 || Code == 0) return false;
            if (Code == 1 || Code == 2 || Code == 7 || Code == 8 || Code == 12 || Code == 13 || Code == 18 || Code == 19 || Code == 23 || Code == 24 || Code == 29 || Code == 30 || Code == 34 || Code == 35 || Code == 40 || Code == 45 || Code == 46)
                codeColor = MarkSixColor.Red;
            else if (Code == 3 || Code == 4 || Code == 9 || Code == 10 || Code == 14 || Code == 15 || Code == 20 || Code == 25 || Code == 26 || Code == 31 || Code == 36 || Code == 37 || Code == 41 || Code == 42 || Code == 47 || Code == 48)
                codeColor = MarkSixColor.Blue;
            else
                codeColor = MarkSixColor.Green;
            return true;
        }
        /// <summary>
        /// jiaqing
        /// </summary>
        /// <returns></returns>
        public static Zodiac[] GetPoultries()
        {
            List<Zodiac> list = new List<Zodiac>();
            list.Add(Zodiac.Niu);
            list.Add(Zodiac.Ma);
            list.Add(Zodiac.Yan);
            list.Add(Zodiac.Ji);
            list.Add(Zodiac.Gou);
            list.Add(Zodiac.Zhu);
            return list.ToArray();
        }
        /// <summary>
        /// yeshou
        /// </summary>
        /// <returns></returns>
        public static Zodiac[] GetBeasts()
        {
            List<Zodiac> list = new List<Zodiac>();
            list.Add(Zodiac.Shu);
            list.Add(Zodiac.Hu);
            list.Add(Zodiac.Tu);
            list.Add(Zodiac.Long);
            list.Add(Zodiac.She);
            list.Add(Zodiac.Hou);
            return list.ToArray();
        }
        /// <summary>
        /// yin
        /// </summary>
        /// <returns></returns>
        public static Zodiac[] GetNegatives()
        {
            List<Zodiac> list = new List<Zodiac>();
            list.Add(Zodiac.Shu);
            list.Add(Zodiac.Long);
            list.Add(Zodiac.She);
            list.Add(Zodiac.Ma);
            list.Add(Zodiac.Gou);
            list.Add(Zodiac.Zhu);
            return list.ToArray();
        }
        /// <summary>
        /// yang
        /// </summary>
        /// <returns></returns>
        public static Zodiac[] GetPositives()
        {
            List<Zodiac> list = new List<Zodiac>();
            list.Add(Zodiac.Niu);
            list.Add(Zodiac.Hu);
            list.Add(Zodiac.Tu);
            list.Add(Zodiac.Yan);
            list.Add(Zodiac.Hou);
            list.Add(Zodiac.Ji);
            return list.ToArray();
        }
        /// <summary>
        /// tian
        /// </summary>
        /// <returns></returns>
        public static Zodiac[] GetSkys()
        {
            List<Zodiac> list = new List<Zodiac>();
            list.Add(Zodiac.Tu);
            list.Add(Zodiac.Ma);
            list.Add(Zodiac.Hou);
            list.Add(Zodiac.Zhu);
            list.Add(Zodiac.Niu);
            list.Add(Zodiac.Long);
            return list.ToArray();
        }
        /// <summary>
        /// di
        /// </summary>
        /// <returns></returns>
        public static Zodiac[] GetLands()
        {
            List<Zodiac> list = new List<Zodiac>();
            list.Add(Zodiac.She);
            list.Add(Zodiac.Yan);
            list.Add(Zodiac.Ji);
            list.Add(Zodiac.Gou);
            list.Add(Zodiac.Shu);
            list.Add(Zodiac.Hu);
            return list.ToArray();
        }
        /// <summary>
        /// red zodiac
        /// </summary>
        /// <returns></returns>
        public static Zodiac[] GetRedZodiacs()
        {
            List<Zodiac> list = new List<Zodiac>();
            list.Add(Zodiac.Ma);
            list.Add(Zodiac.Tu);
            list.Add(Zodiac.Shu);
            list.Add(Zodiac.Ji);
            return list.ToArray();
        }
        /// <summary>
        /// blue zodiac
        /// </summary>
        /// <returns></returns>
        public static Zodiac[] GetBlueZodiacs()
        {
            List<Zodiac> list = new List<Zodiac>();
            list.Add(Zodiac.She);
            list.Add(Zodiac.Hu);
            list.Add(Zodiac.Zhu);
            list.Add(Zodiac.Hou);
            return list.ToArray();
        }
        /// <summary>
        /// green zodiac
        /// </summary>
        /// <returns></returns>
        public static Zodiac[] GetGreenZodiacs()
        {
            List<Zodiac> list = new List<Zodiac>();
            list.Add(Zodiac.Yan);
            list.Add(Zodiac.Long);
            list.Add(Zodiac.Niu);
            list.Add(Zodiac.Gou);
            return list.ToArray();
        }
    }
}
