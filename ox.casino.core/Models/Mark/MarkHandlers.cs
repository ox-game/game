using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka.Util;
using OX.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static OX.GuessKey;
using OX.Casino;

namespace OX.BMS
{
    public static class CodeHelper
    {
        //public static byte RebuildCode(this GuessAnswerValue answer, byte v)
        //{
        //    return RebuildCode(answer.G, v);            
        //}
        public static byte RebuildCode(byte G, byte v)
        {
            var r = (G + v) % 49;
            if (r == 0) r = 49;
            return (byte)r;
        }
    }
    //full mark six
    public class Handler_SpecialCode : BaseBitMarkSixMethodHandler
    {
        public Handler_SpecialCode()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                if (bs[0] == result.Value.T)
                {
                    BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                if (bs[0] == result.Value.T)
                {
                    return true;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            if (betitems.IsNullOrEmpty()) return 0;
            for (byte i = 1; i < 50; i++)
            {
                if (!betitems.Any(m => m.BetTarget.BetPoint.ToArray()[0] == i))
                {
                    return 0;
                }
            }
            var amt = betitems.OrderBy(m => m.Amount).First().Amount;
            foreach (var item in betitems)
            {
                item.Amount -= amt;
            }
            return (uint)(amt * betitems.Length);
        }

    }
    public class Handler_PT1X : BaseBitMarkSixMethodHandler
    {
        public Handler_PT1X()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                var zodiac = (Zodiac)bs[0];
                if (result.GetAllZodiacs().Contains(zodiac))
                {
                    var odds = methodSetting.Odds;
                    if (zodiac == result.GetMasterZodiac())
                        odds -= 100;
                    BetResult.Amount = (bet.Amount * odds) / 100;
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                var zodiac = (Zodiac)bs[0];
                if (result.GetAllZodiacs().Contains(zodiac))
                {
                    return true;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            if (betitems.IsNullOrEmpty()) return 0;
            var zs = NoneFlagEnumHelper.All<Zodiac>();
            foreach (var z in zs)
            {
                if (!betitems.Any(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)z))
                {
                    return 0;
                }
            }
            var amt = betitems.OrderBy(m => m.Amount).First().Amount;
            foreach (var item in betitems)
            {
                item.Amount -= amt;
            }
            return (uint)(amt * betitems.Length);
        }

    }
    public class Handler_PT2X : BaseBitMarkSixMethodHandler
    {
        public Handler_PT2X()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 2)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var zodiacs = result.GetAllZodiacs();
                    foreach (var bp in bs)
                    {
                        var zodiac = (Zodiac)bp;
                        if (!zodiacs.Contains(zodiac))
                            ok = false;
                    }
                    if (ok)
                    {
                        var odds = methodSetting.Odds;
                        foreach (var bp in bs)
                        {
                            var zodiac = (Zodiac)bp;
                            if (zodiac == result.GetMasterZodiac())
                            {
                                odds -= 100;
                                break;
                            }
                        }
                        BetResult.Amount = (bet.Amount * odds) / 100;
                    }
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 2)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var zodiacs = result.GetAllZodiacs();
                    foreach (var bp in bs)
                    {
                        var zodiac = (Zodiac)bp;
                        if (!zodiacs.Contains(zodiac))
                            ok = false;
                    }
                    return ok;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            return 0;
        }

    }
    public class Handler_PT3X : BaseBitMarkSixMethodHandler
    {
        public Handler_PT3X()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 3)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var zodiacs = result.GetAllZodiacs();
                    foreach (var bp in bs)
                    {
                        var zodiac = (Zodiac)bp;
                        if (!zodiacs.Contains(zodiac))
                            ok = false;
                    }
                    if (ok)
                    {
                        var odds = methodSetting.Odds;
                        foreach (var bp in bs)
                        {
                            var zodiac = (Zodiac)bp;
                            if (zodiac == result.GetMasterZodiac())
                            {
                                odds -= 100;
                                break;
                            }
                        }
                        BetResult.Amount = (bet.Amount * odds) / 100;
                    }
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 3)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var zodiacs = result.GetAllZodiacs();
                    foreach (var bp in bs)
                    {
                        var zodiac = (Zodiac)bp;
                        if (!zodiacs.Contains(zodiac))
                            ok = false;
                    }
                    return ok;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            return 0;
        }

    }
    public class Handler_PT4X : BaseBitMarkSixMethodHandler
    {
        public Handler_PT4X()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 4)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var zodiacs = result.GetAllZodiacs();
                    foreach (var bp in bs)
                    {
                        var zodiac = (Zodiac)bp;
                        if (!zodiacs.Contains(zodiac))
                            ok = false;
                    }
                    if (ok)
                    {
                        var odds = methodSetting.Odds;
                        foreach (var bp in bs)
                        {
                            var zodiac = (Zodiac)bp;
                            if (zodiac == result.GetMasterZodiac())
                            {
                                odds -= 100;
                                break;
                            }
                        }
                        BetResult.Amount = (bet.Amount * odds) / 100;
                    }
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 4)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var zodiacs = result.GetAllZodiacs();
                    foreach (var bp in bs)
                    {
                        var zodiac = (Zodiac)bp;
                        if (!zodiacs.Contains(zodiac))
                            ok = false;
                    }
                    return ok;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            return 0;
        }

    }
    public class Handler_PT5X : BaseBitMarkSixMethodHandler
    {
        public Handler_PT5X()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 5)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var zodiacs = result.GetAllZodiacs();
                    foreach (var bp in bs)
                    {
                        var zodiac = (Zodiac)bp;
                        if (!zodiacs.Contains(zodiac))
                            ok = false;
                    }
                    if (ok)
                    {
                        var odds = methodSetting.Odds;
                        foreach (var bp in bs)
                        {
                            var zodiac = (Zodiac)bp;
                            if (zodiac == result.GetMasterZodiac())
                            {
                                odds -= 100;
                                break;
                            }
                        }
                        BetResult.Amount = (bet.Amount * odds) / 100;
                    }
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 5)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var zodiacs = result.GetAllZodiacs();
                    foreach (var bp in bs)
                    {
                        var zodiac = (Zodiac)bp;
                        if (!zodiacs.Contains(zodiac))
                            ok = false;
                    }
                    return ok;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            return 0;
        }

    }
    public class Handler_ZM2 : BaseBitMarkSixMethodHandler
    {
        public Handler_ZM2()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 2)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var codes = result.Value.GetAllNormalCodes();
                    foreach (var bp in bs)
                    {
                        if (!codes.Contains(bp))
                            ok = false;
                    }
                    if (ok)
                    {
                        BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
                    }
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 2)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var codes = result.Value.GetAllNormalCodes();
                    foreach (var bp in bs)
                    {
                        if (!codes.Contains(bp))
                            ok = false;
                    }
                    return ok;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            return 0;
        }

    }
    public class Handler_ZM3 : BaseBitMarkSixMethodHandler
    {
        public Handler_ZM3()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 3)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var codes = result.Value.GetAllNormalCodes();
                    foreach (var bp in bs)
                    {
                        if (!codes.Contains(bp))
                            ok = false;
                    }
                    if (ok)
                    {
                        BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
                    }
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 3)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var codes = result.Value.GetAllNormalCodes();
                    foreach (var bp in bs)
                    {
                        if (!codes.Contains(bp))
                            ok = false;
                    }
                    return ok;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            return 0;
        }

    }
    public class Handler_TP : BaseBitMarkSixMethodHandler
    {
        public Handler_TP()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 2)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var codes = result.Value.GetAllCodes();
                    foreach (var bp in bs)
                    {
                        if (!codes.Contains(bp))
                            ok = false;
                    }
                    if (ok)
                    {
                        bool findT = false;
                        foreach (var bp in bs)
                        {
                            if (bp == result.Value.T)
                            {
                                findT = true;
                                break;
                            }
                        }
                        if (!findT)
                            ok = false;
                    }
                    if (ok)
                    {
                        BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
                    }
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 2)
            {
                var isRepeat = bs.GroupBy(item => item).Where(group => group.Count() > 1).IsNotNullAndEmpty();
                if (!isRepeat)
                {
                    bool ok = true;
                    var codes = result.Value.GetAllCodes();
                    foreach (var bp in bs)
                    {
                        if (!codes.Contains(bp))
                            ok = false;
                    }
                    if (ok)
                    {
                        bool findT = false;
                        foreach (var bp in bs)
                        {
                            if (bp == result.Value.T)
                            {
                                findT = true;
                                break;
                            }
                        }
                        if (!findT)
                            ok = false;
                    }
                    return ok;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            return 0;
        }

    }
    public class Handler_SpecialCode_Color : BaseBitMarkSixMethodHandler
    {
        public Handler_SpecialCode_Color()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                if (result.Value.T.TryGetColor(out MarkSixColor color))
                {
                    var c = (MarkSixColor)bs[0];
                    if (c == color)
                    {
                        BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
                    }
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                if (result.Value.T.TryGetColor(out MarkSixColor color))
                {
                    var c = (MarkSixColor)bs[0];
                    return c == color;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            if (betitems.IsNullOrEmpty()) return 0;
            var bs = NoneFlagEnumHelper.All<MarkSixColor>();
            foreach (var s in bs)
            {
                if (!betitems.Any(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)s))
                {
                    return 0;
                }
            }
            var amt = betitems.OrderBy(m => m.Amount).First().Amount;
            foreach (var item in betitems)
            {
                item.Amount -= amt;
            }
            return (uint)(amt * betitems.Length);
        }

    }
    public class Handler_DXDSJY : BaseBitMarkSixMethodHandler
    {
        public Handler_DXDSJY()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                var dl = (DualisticFull)bs[0];
                bool ok = false;
                switch (dl)
                {
                    case DualisticFull.Dashu:
                        if (result.Value.T > 24) ok = true;
                        break;
                    case DualisticFull.Xiaoshu:
                        if (result.Value.T < 25) ok = true;
                        break;
                    case DualisticFull.Danshu:
                        if (result.Value.T % 2 == 1) ok = true;
                        break;
                    case DualisticFull.Shuangshu:
                        if (result.Value.T % 2 == 0) ok = true;
                        break;
                    case DualisticFull.JiaQin:
                        if (BitSixHelper.GetPoultries().Contains(result.GetSpecialZodiac()))
                            ok = true;
                        break;
                    case DualisticFull.YeShou:
                        if (BitSixHelper.GetBeasts().Contains(result.GetSpecialZodiac()))
                            ok = true;
                        break;
                }
                if (ok)
                    BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                var dl = (DualisticFull)bs[0];
                bool ok = false;
                switch (dl)
                {
                    case DualisticFull.Dashu:
                        if (result.Value.T > 24) ok = true;
                        break;
                    case DualisticFull.Xiaoshu:
                        if (result.Value.T < 25) ok = true;
                        break;
                    case DualisticFull.Danshu:
                        if (result.Value.T % 2 == 1) ok = true;
                        break;
                    case DualisticFull.Shuangshu:
                        if (result.Value.T % 2 == 0) ok = true;
                        break;
                    case DualisticFull.JiaQin:
                        if (BitSixHelper.GetPoultries().Contains(result.GetSpecialZodiac()))
                            ok = true;
                        break;
                    case DualisticFull.YeShou:
                        if (BitSixHelper.GetBeasts().Contains(result.GetSpecialZodiac()))
                            ok = true;
                        break;
                }
                return ok;
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            if (betitems.IsNullOrEmpty()) return 0;
            uint TotalCut = 0;
            var Dashu = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticFull.Dashu);
            var Xiaoshu = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticFull.Xiaoshu);
            var Danshu = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticFull.Danshu);
            var Shuangshu = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticFull.Shuangshu);
            var Jiaqin = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticFull.JiaQin);
            var Yeshou = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticFull.YeShou);
            if (Dashu.IsNotNullAndEmpty() && Xiaoshu.IsNotNullAndEmpty())
            {
                var dxAmt = Math.Min(Dashu.OrderBy(m => m.Amount).First().Amount, Xiaoshu.OrderBy(m => m.Amount).First().Amount);
                foreach (var item in Dashu)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
                foreach (var item in Xiaoshu)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
            }

            if (Danshu.IsNotNullAndEmpty() && Shuangshu.IsNotNullAndEmpty())
            {
                var dxAmt = Math.Min(Danshu.OrderBy(m => m.Amount).First().Amount, Shuangshu.OrderBy(m => m.Amount).First().Amount);
                foreach (var item in Danshu)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
                foreach (var item in Shuangshu)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
            }

            if (Jiaqin.IsNotNullAndEmpty() && Yeshou.IsNotNullAndEmpty())
            {
                var dxAmt = Math.Min(Jiaqin.OrderBy(m => m.Amount).First().Amount, Yeshou.OrderBy(m => m.Amount).First().Amount);
                foreach (var item in Jiaqin)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
                foreach (var item in Yeshou)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
            }


            return TotalCut;
        }

    }
    public class Handler_Color_DS : BaseBitMarkSixMethodHandler
    {
        public Handler_Color_DS()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                var newT = result.Value.T;
                if (newT.TryGetColor(out MarkSixColor color))
                {
                    var dl = (ColorDS)bs[0];
                    bool ok = false;
                    switch (dl)
                    {
                        case ColorDS.HongDan:
                            if (color == MarkSixColor.Red && newT % 2 == 1) ok = true;
                            break;
                        case ColorDS.HongShuang:
                            if (color == MarkSixColor.Red && newT % 2 == 0) ok = true;
                            break;
                        case ColorDS.LanDan:
                            if (color == MarkSixColor.Blue && newT % 2 == 1) ok = true;
                            break;
                        case ColorDS.LanShuang:
                            if (color == MarkSixColor.Blue && newT % 2 == 0) ok = true;
                            break;
                        case ColorDS.LvDan:
                            if (color == MarkSixColor.Green && newT % 2 == 1) ok = true;
                            break;
                        case ColorDS.LvShuang:
                            if (color == MarkSixColor.Green && newT % 2 == 0) ok = true;
                            break;
                    }
                    if (ok)
                        BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                var newT = result.Value.T;
                if (newT.TryGetColor(out MarkSixColor color))
                {
                    var dl = (ColorDS)bs[0];
                    bool ok = false;
                    switch (dl)
                    {
                        case ColorDS.HongDan:
                            if (color == MarkSixColor.Red && newT % 2 == 1) ok = true;
                            break;
                        case ColorDS.HongShuang:
                            if (color == MarkSixColor.Red && newT % 2 == 0) ok = true;
                            break;
                        case ColorDS.LanDan:
                            if (color == MarkSixColor.Blue && newT % 2 == 1) ok = true;
                            break;
                        case ColorDS.LanShuang:
                            if (color == MarkSixColor.Blue && newT % 2 == 0) ok = true;
                            break;
                        case ColorDS.LvDan:
                            if (color == MarkSixColor.Green && newT % 2 == 1) ok = true;
                            break;
                        case ColorDS.LvShuang:
                            if (color == MarkSixColor.Green && newT % 2 == 0) ok = true;
                            break;
                    }
                    return ok;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            if (betitems.IsNullOrEmpty()) return 0;
            var bs = NoneFlagEnumHelper.All<ColorDS>();
            foreach (var s in bs)
            {
                if (!betitems.Any(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)s))
                {
                    return 0;
                }
            }
            var amt = betitems.OrderBy(m => m.Amount).First().Amount;
            foreach (var item in betitems)
            {
                item.Amount -= amt;
            }
            return (uint)(amt * betitems.Length);
        }

    }
    public class Handler_PTTail : BaseBitMarkSixMethodHandler
    {
        public Handler_PTTail()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                foreach (var c in result.Value.GetAllCodes())
                {
                    var ct = c % 10;
                    if (ct == bs[0])
                    {
                        BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
                        break;
                    }
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                foreach (var c in result.Value.GetAllCodes())
                {
                    var ct = c % 10;
                    if (ct == bs[0])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            if (betitems.IsNullOrEmpty()) return 0;
            for (byte i = 0; i < 10; i++)
            {
                if (!betitems.Any(m => m.BetTarget.BetPoint.ToArray()[0] == i))
                {
                    return 0;
                }
            }
            var amt = betitems.OrderBy(m => m.Amount).First().Amount;
            foreach (var item in betitems)
            {
                item.Amount -= amt;
            }
            return (uint)(amt * betitems.Length);
        }

    }

    //simple mark six
    public class Handler_SpecialZodiac : BaseBitMarkSixMethodHandler
    {
        public Handler_SpecialZodiac()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                if (bs[0] == (byte)result.Value.Zodiac)
                {
                    BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                if (bs[0] == (byte)result.Value.Zodiac)
                {
                    return true;
                }
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            if (betitems.IsNullOrEmpty()) return 0;
            var bs = NoneFlagEnumHelper.All<Zodiac>();
            foreach (var s in bs)
            {
                if (!betitems.Any(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)s))
                {
                    return 0;
                }
            }
            var amt = betitems.OrderBy(m => m.Amount).First().Amount;
            foreach (var item in betitems)
            {
                item.Amount -= amt;
            }
            return (uint)(amt * betitems.Length);
        }

    }
    public class Handler_Zodiac_Color : BaseBitMarkSixMethodHandler
    {
        public Handler_Zodiac_Color()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                bool ok = false;
                var c = (MarkSixColor)bs[0];
                var z = (Zodiac)result.Value.Zodiac;
                switch (c)
                {
                    case MarkSixColor.Red:
                        if (BitSixHelper.GetRedZodiacs().Contains(z))
                            ok = true;
                        break;
                    case MarkSixColor.Blue:
                        if (BitSixHelper.GetBlueZodiacs().Contains(z))
                            ok = true;
                        break;
                    case MarkSixColor.Green:
                        if (BitSixHelper.GetGreenZodiacs().Contains(z))
                            ok = true;
                        break;
                }

                if (ok)
                {
                    BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
                }
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                bool ok = false;
                var c = (MarkSixColor)bs[0];
                var z = (Zodiac)result.Value.Zodiac;
                switch (c)
                {
                    case MarkSixColor.Red:
                        if (BitSixHelper.GetRedZodiacs().Contains(z))
                            ok = true;
                        break;
                    case MarkSixColor.Blue:
                        if (BitSixHelper.GetBlueZodiacs().Contains(z))
                            ok = true;
                        break;
                    case MarkSixColor.Green:
                        if (BitSixHelper.GetGreenZodiacs().Contains(z))
                            ok = true;
                        break;
                }
                return ok;
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            if (betitems.IsNullOrEmpty()) return 0;
            var bs = NoneFlagEnumHelper.All<MarkSixColor>();
            foreach (var s in bs)
            {
                if (!betitems.Any(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)s))
                {
                    return 0;
                }
            }
            var amt = betitems.OrderBy(m => m.Amount).First().Amount;
            foreach (var item in betitems)
            {
                item.Amount -= amt;
            }
            return (uint)(amt * betitems.Length);
        }

    }
    public class Handler_JYYYTD : BaseBitMarkSixMethodHandler
    {
        public Handler_JYYYTD()
        { }
        public override BitMarkSixBetPrize Handle(BMSPlayMethod methodSetting, GuessAnswer result, BitMarkSixBet bet)
        {
            BitMarkSixBetPrize BetResult = new BitMarkSixBetPrize();
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                var dl = (DualisticSimple)bs[0];
                var z = (Zodiac)result.Value.Zodiac;
                bool ok = false;
                switch (dl)
                {
                    case DualisticSimple.Negative:
                        if (BitSixHelper.GetNegatives().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Positive:
                        if (BitSixHelper.GetPositives().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Sky:
                        if (BitSixHelper.GetSkys().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Land:
                        if (BitSixHelper.GetLands().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Poultry:
                        if (BitSixHelper.GetPoultries().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Beast:
                        if (BitSixHelper.GetBeasts().Contains(z))
                            ok = true;
                        break;
                }
                if (ok)
                    BetResult.Amount = (bet.Amount * methodSetting.Odds) / 100;
            }
            return BetResult;
        }
        public override bool VerifyBet(GuessAnswer result, BitMarkSixBet bet)
        {
            var bs = bet.BetTarget.BetPoint.ToArray();
            if (bs.IsNotNullAndEmpty() && bs.Length == 1)
            {
                var dl = (DualisticSimple)bs[0];
                var z = (Zodiac)result.Value.Zodiac;
                bool ok = false;
                switch (dl)
                {
                    case DualisticSimple.Negative:
                        if (BitSixHelper.GetNegatives().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Positive:
                        if (BitSixHelper.GetPositives().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Sky:
                        if (BitSixHelper.GetSkys().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Land:
                        if (BitSixHelper.GetLands().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Poultry:
                        if (BitSixHelper.GetPoultries().Contains(z))
                            ok = true;
                        break;
                    case DualisticSimple.Beast:
                        if (BitSixHelper.GetBeasts().Contains(z))
                            ok = true;
                        break;
                }
                return ok;
            }
            return false;
        }
        public override uint Combine(BitMarkSixBet[] betitems)
        {
            if (betitems.IsNullOrEmpty()) return 0;
            uint TotalCut = 0;
            var Negative = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticSimple.Negative);
            var Positive = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticSimple.Positive);
            var Sky = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticSimple.Sky);
            var Land = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticSimple.Land);
            var Poultry = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticSimple.Poultry);
            var Beast = betitems.Where(m => m.BetTarget.BetPoint.ToArray()[0] == (byte)DualisticSimple.Beast);
            if (Negative.IsNotNullAndEmpty() && Positive.IsNotNullAndEmpty())
            {
                var dxAmt = Math.Min(Negative.OrderBy(m => m.Amount).First().Amount, Positive.OrderBy(m => m.Amount).First().Amount);
                foreach (var item in Negative)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
                foreach (var item in Positive)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
            }

            if (Sky.IsNotNullAndEmpty() && Land.IsNotNullAndEmpty())
            {
                var dxAmt = Math.Min(Sky.OrderBy(m => m.Amount).First().Amount, Land.OrderBy(m => m.Amount).First().Amount);
                foreach (var item in Sky)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
                foreach (var item in Land)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
            }

            if (Poultry.IsNotNullAndEmpty() && Beast.IsNotNullAndEmpty())
            {
                var dxAmt = Math.Min(Poultry.OrderBy(m => m.Amount).First().Amount, Beast.OrderBy(m => m.Amount).First().Amount);
                foreach (var item in Poultry)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
                foreach (var item in Beast)
                {
                    item.Amount -= dxAmt;
                    TotalCut += dxAmt;
                }
            }


            return TotalCut;
        }

    }
}
