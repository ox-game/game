using System.Collections.Generic;

namespace OX
{


    /// <summary>
    /// 游戏基类
    /// </summary>
    public abstract class Game
    {
        public static uint PeroidBlocks(RegRoomRequest request)
        {
            return PeroidBlocks(request.Kind, request.GameSpeed);
        }
        public static uint PeroidBlocks(GameKind kind, GameSpeed periodMutiple)
        {
            return MinBlocks(kind) * MutlpleValue(periodMutiple);
        }
        public static uint MinBlocks(GameKind kind)
        {
            if (kind == GameKind.EatSmall)
                return 10;
            return 1000;
        }
        public static uint MutlpleValue(GameSpeed mutiple)
        {
            switch (mutiple)
            {
                case GameSpeed.Fast:
                    return 1;
                case GameSpeed.Medium:
                    return 10;
                case GameSpeed.Slow:
                    return 100;
            }
            return 1;
        }
        public abstract GameKind Kind { get; }
        public abstract IEnumerable<RoundResult> Run(object room, uint index, GuessKey guessKey, ulong minNonce, IEnumerable<BettingItem> utxos, IEnumerable<BettingItem> poolItems, Fixed8 minBet, Fixed8 unitFee, Fixed8 RoomFeeMinBet, Fixed8 PoolBonusMinBet, string DibsAccount);
        public abstract bool Verify(object roomobj, BetRequest request, Fixed8 amount);
    }
}
