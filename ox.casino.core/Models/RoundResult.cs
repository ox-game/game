using System.Collections.Generic;

namespace OX
{
    public abstract class RoundResult
    {
        public uint Index;
        //public Fixed8 NetFee;
        //public Fixed8 SystemFee;
        //public Fixed8 RoomFee;
        //public Fixed8 PoolFee;
        //public Fixed8 BetTotal;
        public Dictionary<string, Fixed8> Bonus = new Dictionary<string, Fixed8>();
    }
}
