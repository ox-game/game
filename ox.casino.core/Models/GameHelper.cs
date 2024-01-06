using OX.Casino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX
{
    public static class GameHelper
    {
        public static IEnumerable<GameKind> Valid(this IEnumerable<GameKind> kinds)
        {
            foreach (var kind in kinds)
            {
                if (kind == GameKind.MarkSix) continue;
                yield return kind;
            }
        }
        public static IEnumerable<GameKind> RiddlesValid(this IEnumerable<GameKind> kinds)
        {
            foreach (var kind in kinds)
            {
                if (kind == GameKind.TeamKill) continue;
                yield return kind;
            }
        }
        
        public static uint ReviseIndex(this MixRoom room)
        {
            return ReviseIndex(room.Request.Flag, room.Request.Kind, room.Request.GameSpeed);
        }
        public static uint ReviseIndex(byte flag, GameKind kind, GameSpeed periodMutiple)
        {
            if (kind == GameKind.EatSmall)
            {
                if (Game.PeroidBlocks(kind, periodMutiple) == 100)
                {
                    var rem = flag % 10;
                    return (uint)rem * 10;
                }
            }
            return 0;
        }
    }
}
