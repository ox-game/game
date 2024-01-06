using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.IO.Data.LevelDB;
using OX.Wallets;

namespace OX
{
    public static class DividentSlopeHelper
    {
        public static decimal ComputeBonusRatio(this DividentSlope slope, Fixed8 PoolTotal, Fixed8 frontAmount, Fixed8 selfAmount)
        {
            return slope.ComputeBonusRatio(PoolTotal.GetInternalValue(), frontAmount.GetInternalValue(), selfAmount.GetInternalValue());
        }
        public static decimal ComputeBonusRatio(this DividentSlope slope, long PoolTotal, long frontAmount, long selfAmount)
        {
            decimal r = 0;
            var x1 = (decimal)frontAmount / (decimal)PoolTotal;
            var x2 = (decimal)selfAmount / (decimal)PoolTotal;
            switch (slope)
            {
                case DividentSlope.Big_5:
                    r = (x2 * 4 - x2 * x2 - 2 * x2 * x1) / 3;
                    break;
                case DividentSlope.Medium_6:
                    r = (x2 * 5 - x2 * x2 - 2 * x2 * x1) / 4;
                    break;
                case DividentSlope.Small_8:
                    r = (x2 * 10 - x2 * x2 - 2 * x2 * x1) / 9;
                    break;
            }
            return r;
        }
    }
}
