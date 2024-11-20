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
using Org.BouncyCastle.Asn1.Ocsp;

namespace OX.BMS
{
    public static class BitSixBetHelper
    {
        public static DateTime ToBeijingTime(this uint timestamp)
        {
            return timestamp.ToDateTime().ToBeijingTime();
        }
        public static DateTime ToBeijingTime(this DateTime DateTime)
        {
            return DateTime.ToUniversalTime().AddHours(8);
        }
        public static DateTime BeijingNow()
        {
            return DateTime.Now.ToBeijingTime();
        }
        public static MarkTerm GetBetTermForPlayer(this DateTime DateTime, uint criticalSeconds)
        {
            var blockBJTime = DateTime.ToBeijingTime();
            return blockBJTime.GetBetTermForPlayerFromBeijing(criticalSeconds);
        }
        public static MarkTerm GetBetTermForPlayerFromBeijing(this DateTime BeijingDateTime, uint criticalSeconds)
        {
            MarkTerm term = new MarkTerm
            {
                Year = (ushort)BeijingDateTime.Year,
                Month = (byte)BeijingDateTime.Month,
                Day = (byte)BeijingDateTime.Day,
            };
            var dt = new DateTime(BeijingDateTime.Year, BeijingDateTime.Month, BeijingDateTime.Day);
            dt = dt.AddSeconds(criticalSeconds);
            if (BeijingDateTime > dt)
                term.Day += 1;
            return term;
        }

        public static MarkTerm GetBetTerm(this uint timestamp)
        {
            var blockBJTime = timestamp.ToBeijingTime();
            MarkTerm term = new MarkTerm
            {
                Year = (ushort)blockBJTime.Year,
                Month = (byte)blockBJTime.Month,
                Day = (byte)blockBJTime.Day,
            };
            return term;
        }
        public static MarkTerm GetBetTerm(this DateTime UTCDateTime)
        {
            var blockBJTime = UTCDateTime.ToBeijingTime();
            MarkTerm term = new MarkTerm
            {
                Year = (ushort)blockBJTime.Year,
                Month = (byte)blockBJTime.Month,
                Day = (byte)blockBJTime.Day,
            };
            return term;
        }
        //public static bool TryGetMarkSixOrder(this DeadlineTransaction dt, out MarkPlainBetOrder order)
        //{
        //    order = default;
        //    var attr = dt.Attributes.Where(m => m.Usage == TransactionAttributeUsage.RelatedData).FirstOrDefault();
        //    if (attr.IsNotNull())
        //    {
        //        try
        //        {
        //            order = attr.Data.AsSerializable<MarkPlainBetOrder>();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}
        public static bool VerifyMarkSixOrder(this MarkCipherBetOrder order, TransactionOutput output)
        {
            if (output.AssetId == Blockchain.OXC)
            {
                return output.Value >= Fixed8.One * order.Amount;
            }
            return false;
        }
        public static bool VerifyMarkSixBankerBet(this TransactionOutput output)
        {
            return output.AssetId == Blockchain.OXC;
            //if (output.AssetId == Blockchain.OXC)
            //{
            //    return output.Value >= Fixed8.One * 1000;
            //}
            //return false;
        }
        public static bool VerifyMarkSixInternalTransfer(this TransactionOutput output)
        {
            return output.AssetId == Blockchain.OXC;
        }
    }
}
