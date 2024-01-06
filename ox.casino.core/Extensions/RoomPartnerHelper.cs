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

namespace OX.Casino
{
    public static class RoomPartnerHelper
    {
        public static IEnumerable<RoomPartnerLockRecord> Valid(this IEnumerable<RoomPartnerLockRecord> records, uint blockIndex)
        {
            return records.Where(m => m.EndIndex >= blockIndex);
        }
        public static Fixed8 Calculate(this IEnumerable<RoomPartnerLockRecord> records, uint blockIndex, out IEnumerable<RoomPartnerLockRecord> validRecords)
        {
            validRecords = records.Valid(blockIndex);
            if (validRecords.IsNullOrEmpty()) return Fixed8.Zero;
            return validRecords.Sum(m => m.Amount);
        }
        public static bool VerifyPartnerLock(this ICasinoProvider provider, MixRoom room, out IEnumerable<RoomPartnerLockRecord> validRecords, out Fixed8 haveLockTotal, out Fixed8 needLockTotal, out uint EarliestExpiration)
        {
            validRecords = default;
            haveLockTotal = Fixed8.Zero;
            needLockTotal = Fixed8.Zero;
            EarliestExpiration = 0;
            var settings = provider.GetAllCasinoSettings();
            var PrivateRoomOXSLockVolume = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PrivateRoomOXSLockVolume }));
            if (PrivateRoomOXSLockVolume.Equals(new KeyValuePair<byte[], CasinoSettingRecord>())) return false;
            var PrivateRoomOXSLockAmount = Fixed8.FromDecimal(decimal.Parse(PrivateRoomOXSLockVolume.Value.Value));

            var PublicRoomOXSLockVolume = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PublicRoomOXSLockVolume }));
            if (PublicRoomOXSLockVolume.Equals(new KeyValuePair<byte[], CasinoSettingRecord>())) return false;
            var PublicRoomOXSLockAmount = Fixed8.FromDecimal(decimal.Parse(PublicRoomOXSLockVolume.Value.Value));
            Fixed8 TotalLockVolume = Fixed8.Zero;
            if (room.Request.Permission == RoomPermission.Public)
            {
                TotalLockVolume = PublicRoomOXSLockAmount;
            }
            else
            {
                TotalLockVolume = PrivateRoomOXSLockAmount;
            }
            needLockTotal = TotalLockVolume;
            Fixed8 trs = Fixed8.Zero;
            var lrs = provider.GetRoomPartnerLockRecords(room.BetAddress);
            if (lrs.IsNotNullAndEmpty())
            {
                trs = lrs.Select(m => m.Key).Calculate(Blockchain.Singleton.HeaderHeight, out validRecords);
                if (validRecords.IsNotNullAndEmpty()) EarliestExpiration = validRecords.OrderBy(m => m.EndIndex).FirstOrDefault().EndIndex;
            }
            haveLockTotal = trs;
            return trs >= TotalLockVolume;
        }
    }
}
