using OX.IO;
using System;
using System.IO;

namespace OX.BMS
{
    [EnumStrings(typeof(Zodiac), "龙", "兔", "虎", "牛", "鼠", "猪", "狗", "鸡", "猴", "羊", "马", "蛇", EnumName = "生肖")]
    [EnumEngStrings(typeof(Zodiac), "dragon", "rabbit", "tiger", "ox", "rat", "pig", "dog", "rooster", "monkey", "goat", "horse", "snake", EnumName = "Zodiac")]
    public enum Zodiac : byte
    {
        [Name("龙", "dragon")]
        Long = 1,
        [Name("兔", "rabbit")]
        Tu = 2,
        [Name("虎", "tiger")]
        Hu = 3,
        [Name("牛", "ox")]
        Niu = 4,
        [Name("鼠", "rat")]
        Shu = 5,
        [Name("猪", "pig")]
        Zhu = 6,
        [Name("狗", "dog")]
        Gou = 7,
        [Name("鸡", "rooster")]
        Ji = 8,
        [Name("猴", "monkey")]
        Hou = 9,
        [Name("羊", "goat")]
        Yan = 10,
        [Name("马", "horse")]
        Ma = 11,
        [Name("蛇", "snake")]
        She = 12,
    }

    [EnumStrings(typeof(BetChannel), "一合彩", "六合彩", EnumName = "开奖规则")]
    [EnumEngStrings(typeof(BetChannel), "Mark One", "Mark Six", EnumName = "Winning Rules")]
    [Flags]
    public enum BetChannel : byte
    {
        [Name("一合彩", "Mark One")]
        MarkOne = 1 << 0,
        [Name("六合彩", "Mark Six")]
        MarkSix = 1 << 1
    }
    public enum MarkSixBetMethod : byte
    {
        [MethodSetting(Name = "特码", EngName = "Special Code", MaxOdds = 4800, MinOdds = 4000, DefaultOdds = 4500, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_SpecialCode))]
        TM = 0,
        [MethodSetting(Name = "平特一肖", EngName = "One Zodiac", MaxOdds = 250, MinOdds = 150, DefaultOdds = 200, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_PT1X))]
        PT1X = 1,
        [MethodSetting(Name = "平特二肖", EngName = "Two Zodiac", MaxOdds = 500, MinOdds = 300, DefaultOdds = 400, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_PT2X))]
        PT2X = 2,
        [MethodSetting(Name = "平特三肖", EngName = "Three Zodiac", MaxOdds = 1200, MinOdds = 800, DefaultOdds = 1000, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_PT3X))]
        PT3X = 3,
        [MethodSetting(Name = "平特四肖", EngName = "Four Zodiac", MaxOdds = 3500, MinOdds = 2500, DefaultOdds = 3000, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_PT4X))]
        PT4X = 4,
        [MethodSetting(Name = "平特五肖", EngName = "Five Zodiac", MaxOdds = 13000, MinOdds = 8000, DefaultOdds = 10000, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_PT5X))]
        PT5X = 5,
        [MethodSetting(Name = "平码二中二", EngName = "Hit 2 Code", MaxOdds = 7000, MinOdds = 5000, DefaultOdds = 6000, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_ZM2))]
        ZM2 = 6,
        //[MethodSetting(Name = "平码三中三", EngName = "Hit 3 Code", MaxOdds = 65000, MinOdds = 50000, DefaultOdds = 60000, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_ZM3))]
        //ZM3 = 7,
        [MethodSetting(Name = "特码波色", EngName = "Ball Color", MaxOdds = 300, MinOdds = 200, DefaultOdds = 250, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_SpecialCode_Color))]
        TM_Color = 8,
        [MethodSetting(Name = "大小单双家野", EngName = "Hit Dualistic", MaxOdds = 200, MinOdds = 150, DefaultOdds = 180, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_DXDSJY))]
        DXDSJY = 9,
        [MethodSetting(Name = "波色单双", EngName = "Color Odd&Even", MaxOdds = 600, MinOdds = 400, DefaultOdds = 500, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_Color_DS))]
        Color_DS = 10,
        [MethodSetting(Name = "平特尾数", EngName = "Tail Zodiac", MaxOdds = 200, MinOdds = 150, DefaultOdds = 180, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_PTTail))]
        PTTail = 11,
        [MethodSetting(Name = "特碰", EngName = "Special Hit", MaxOdds = 13000, MinOdds = 8000, DefaultOdds = 10000, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_TP))]
        TP = 12
    }
    //[EnumStrings(typeof(MarkOneBetMethod), "特肖", "肖色", "家野阴阳天地", EnumName = "玩法")]
    //[EnumEngStrings(typeof(MarkOneBetMethod), "Special Zodiac", "Zodiac Color", "Zodiac Dualistic", EnumName = "Bet Method")]
    public enum MarkOneBetMethod : byte
    {
        [MethodSetting(Name = "特肖", EngName = "Special Zodiac", MaxOdds = 1200, MinOdds = 800, DefaultOdds = 1000, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_SpecialZodiac))]
        Zodiac = 0,
        [MethodSetting(Name = "肖色", EngName = "Zodiac Color", MaxOdds = 300, MinOdds = 200, DefaultOdds = 250, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_Zodiac_Color))]
        ZodiacColor = 1,
        [MethodSetting(Name = "家野阴阳天地", EngName = "Zodiac Dualistic", MaxOdds = 200, MinOdds = 150, DefaultOdds = 180, MaxCommission = 10, MinCommission = 0, DefaultCommission = 5, MethodHandlerType = typeof(Handler_JYYYTD))]
        JYYYTD = 2
    }
    [EnumStrings(typeof(MarkSixColor), "红", "蓝", "绿", EnumName = "彩色")]
    [EnumEngStrings(typeof(MarkSixColor), "Red", "Blue", "Green", EnumName = "Color")]
    public enum MarkSixColor : byte
    {
        [Name("红", "Red")]
        Red = 1,
        [Name("蓝", "Blue")]
        Blue = 2,
        [Name("绿", "Green")]
        Green = 3
    }
    [EnumStrings(typeof(DualisticFull), "大数", "小数", "单数", "双数", "家畜", "野兽", EnumName = "大小单双家野")]
    [EnumEngStrings(typeof(DualisticFull), "large", "small ", "odd", "even", "poultry", "beast", EnumName = "Dualistic")]
    public enum DualisticFull : byte
    {
        [Name("大数", "large")]
        Dashu = 0,
        [Name("小数", "small")]
        Xiaoshu = 1,
        [Name("单数", "odd")]
        Danshu = 2,
        [Name("双数", "even")]
        Shuangshu = 3,
        [Name("家畜", "poultry")]
        JiaQin = 4,
        [Name("野兽", "beast")]
        YeShou = 5
    }
    [EnumStrings(typeof(DualisticSimple), "阴肖", "阳肖", "天肖", "地肖", "家肖", "野肖", EnumName = "大小单双家野")]
    [EnumEngStrings(typeof(DualisticSimple), "negative", "positive ", "sky", "land", "poultry", "beast", EnumName = "Dualistic")]
    public enum DualisticSimple : byte
    {
        [Name("阴肖", "negative")]
        Negative = 0,
        [Name("阳肖", "positive")]
        Positive = 1,
        [Name("天肖", "sky")]
        Sky = 2,
        [Name("地肖", "land")]
        Land = 3,
        [Name("家肖", "poultry")]
        Poultry = 4,
        [Name("野肖", "beast")]
        Beast = 5
    }
    [EnumStrings(typeof(ColorDS), "红单", "红双", "蓝单", "蓝双", "绿单", "绿双", EnumName = "波色单双")]
    [EnumEngStrings(typeof(ColorDS), "red-odd", "red-even", "blue-odd", "blue-even", "green-odd", "green-even", EnumName = "color-odd-even")]
    public enum ColorDS : byte
    {
        [Name("红单", "red-odd")]
        HongDan = 0,
        [Name("红双", "red-even")]
        HongShuang = 1,
        [Name("蓝单", "blue-odd")]
        LanDan = 2,
        [Name("蓝双", "blue-even")]
        LanShuang = 3,
        [Name("绿单", "green-odd")]
        LvDan = 4,
        [Name("绿双", "green-even")]
        LvShuang = 5
    }
    public enum MarkOneRound : byte
    {
        [Name("下午1点", "1 PM")]
        PM_1 = 1,
        [Name("下午3点", "3 PM")]
        PM_3 = 2,
        [Name("下午5点", "5 PM")]
        PM_5 = 3,
        [Name("晚上7点", "7 PM")]
        PM_7 = 4
    }
    [Flags]
    public enum MarkSixRound : byte
    {
        [Name("港澳联合彩", "Mark Six Union")]
        MarkUnion = 1 << 0,
        [Name("香港六合彩", "Mark Six HK")]
        MarkHK = 1 << 1,
        [Name("新澳门六合彩", "Mark Six Macau")]
        MarkMacau = 1 << 2,
    }
    [Flags]
    public enum MarkCipherOrderStatus : byte
    {
        [Name("已提交", "Submitted")]
        Inited = 1 << 0,
        [Name("已广播", "Broadcasted")]
        Broadcast = 1 << 1,
        [Name("已确认", "Confirmed")]
        Confirmed = 1 << 2,
    }
    [Flags]
    public enum MarkPlainOrderStatus : byte
    {
        [Name("已提交", "Submitted")]
        Inited = 1 << 0,
        [Name("已收单", "Signed")]
        Signed = 1 << 1,
        [Name("已结算", "Settled")]
        Settled = 1 << 2,
    }
    public enum InboundOrigin : byte
    {
        [Name("分销", "Level Sale")]
        LevelSale = 1,
        [Name("直销", "Direct Sale")]
        DirectSale = 2,
        [Name("自投", "Self Buy")]
        SelfBuy = 3
    }
    public enum MarkMemberType : byte
    {
        [Name("盘口", "Port")]
        Port = 1,
        [Name("代理", "Agent")]
        Agent = 2,

    }
}
