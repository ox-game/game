using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX.BMS
{
    public static class MarkOrderHelper
    {
        public static uint CheckBet(this MarkSetting setting, GuessAnswer answer, BitMarkSixBet[] BetItems)
        {
            uint Prize = 0;
            foreach (var orderItem in BetItems)
            {
                var playMethod = setting.MarkSixMethods.FirstOrDefault(m => m.Method == orderItem.BetTarget.Method);
                if (playMethod.IsNotNull())
                {
                    var method = (MarkSixBetMethod)orderItem.BetTarget.Method;
                    var methodSetting = method.GetMethodSetting();
                    var ret = methodSetting.CheckBet(playMethod, answer, orderItem);
                    if (ret.IsNotNull())
                    {
                        Prize += ret.Amount;
                    }
                }
            }
            return Prize;
        }

    }
}
