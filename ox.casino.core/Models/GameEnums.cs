using System;

namespace OX
{
    [EnumStrings(typeof(GameState), "投注中", "封注中", "已开奖", EnumName = "赛局状态")]
    [EnumEngStrings(typeof(GameState), "Betting", "Capping", "Awarded", EnumName = "Round State")]
    [Flags]
    public enum GameState : byte
    {
        Betting = 1 << 0,
        Capping = 1 << 1,
        Awarded = 1 << 2
    }
    [EnumStrings(typeof(RoomPermission), "公开投注", "私有投注", EnumName = "许可")]
    [EnumEngStrings(typeof(RoomPermission), "Public Bet", "Private Bet", EnumName = "Permission")]
    [Flags]
    public enum RoomPermission : byte
    {
        Public = 1 << 0,
        Private = 1 << 1
    }
    [EnumStrings(typeof(RoomState), "已开放", "已关闭", EnumName = "房间状态")]
    [EnumEngStrings(typeof(RoomState), "Opened", "Closed", EnumName = "Room State")]
    [Flags]
    public enum RoomState : byte
    {
        Open = 1 << 0,
        Close = 1 << 1
    }
    //[EnumStrings(typeof(CommissionKind), "定额", "比例", EnumName = "抽佣类型")]
    //[EnumEngStrings(typeof(CommissionKind), "Quota", "Percent", EnumName = "Commission Type")]
    //[Flags]
    //public enum CommissionKind : byte
    //{
    //    Quota = 1 << 0,
    //    Percent = 1 << 1
    //}
    [EnumStrings(typeof(GameSpeed), "快速", "中速", "慢速", EnumName = "游戏速度")]
    [EnumEngStrings(typeof(GameSpeed), "Fast", "Medium ", "Slow", EnumName = "Game Speed")]
    [Flags]
    public enum GameSpeed : byte
    {
        Fast = 1 << 0,
        Medium = 1 << 1,
        Slow = 1 << 2
    }
    [EnumStrings(typeof(GameKind), "大吃小", "乐透", "团杀", "六合", EnumName = "游戏类型")]
    [EnumEngStrings(typeof(GameKind), "EatSmall", "Lotto", "TeamKill", "MarkSix", EnumName = "Game Type")]
    [Flags]
    public enum GameKind : byte
    {
        /// <summary>
        /// 大吃小
        /// </summary>
        EatSmall = 1 << 0,
        /// <summary>
        /// 乐透
        /// </summary>
        Lotto = 1 << 1,
        /// <summary>
        /// 团杀
        /// </summary>
        TeamKill = 1 << 2,
        /// <summary>
        /// 六合
        /// </summary>
        MarkSix = 1 << 3

    }
    [EnumStrings(typeof(RiddlesKind), "大吃小", "乐透", "骰宝", "六合", "扑克", "字母", EnumName = "谜底类型")]
    [EnumEngStrings(typeof(RiddlesKind), "EatSmall", "Lotto", "SicBo", "MarkSix", "Poker", "Letter", EnumName = "Riddles Type")]
    [Flags]
    public enum RiddlesKind : byte
    {
        /// <summary>
        /// 大吃小
        /// </summary>
        EatSmall = 1 << 0,
        /// <summary>
        /// 乐透
        /// </summary>
        Lotto = 1 << 1,
        /// <summary>
        /// 骰宝
        /// </summary>
        SicBo = 1 << 2,
        /// <summary>
        /// 六合
        /// </summary>
        MarkSix = 1 << 3,
        /// <summary>
        /// 扑克
        /// </summary>
        Poker = 1 << 4,
        /// <summary>
        /// 字母
        /// </summary>
        Letter = 1 << 5
    }
    [EnumStrings(typeof(DividentSlope), "大", "中", "小", EnumName = "分红递减坡度")]
    [EnumEngStrings(typeof(DividentSlope), "Big", "Medium ", "Small", EnumName = "Divident Diminish Slope")]
    [Flags]
    public enum DividentSlope : byte
    {
        Big_5 = 1 << 0,
        Medium_6 = 1 << 1,
        Small_8 = 1 << 2
    }
    [EnumStrings(typeof(GameMiningKind), "原位竞技挖矿", "本位竞技挖矿", EnumName = "竞技挖矿类型")]
    [EnumEngStrings(typeof(GameMiningKind), "Fixed Game Mining", "Floating Game Mining", EnumName = "Game Mining Type")]
    [Flags]
    public enum GameMiningKind : byte
    {
        /// <summary>
        /// 原位竞技挖矿
        /// </summary>
        Fixed = 1 << 0,
        /// <summary>
        /// 本位竞技挖矿
        /// </summary>
        Floating = 1 << 1
    }
   
}
