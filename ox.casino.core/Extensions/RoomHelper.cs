using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using OX.Wallets;
using OX.Cryptography.ECC;
using System.Security.Policy;
using System.IO;
using OX.Network.P2P;
using System.Runtime;

namespace OX.Casino
{
    public static class RoomHelper
    {
        public static bool ValidPrivateRoom(this MixRoom room, Wallet wallet)
        {
            if (room.Request.Permission == RoomPermission.Private)
            {
                bool ok = false;
                if (room.RoomMemberSetting.IsNotNull())
                {
                    foreach (var sh in room.RoomMemberSetting.Members)
                    {
                        if (wallet.ContainsAndHeld(sh))
                        {
                            ok = true;
                            break;
                        }
                    }
                }
                return ok;
            }
            else
            {
                return true;
            }
        }
        public static bool ValidPrivateRoom(this MixRoom room, UInt160 member)
        {
            if (room.Request.Permission == RoomPermission.Private)
            {
                bool ok = false;
                if (room.RoomMemberSetting.IsNotNull())
                {
                    ok = room.RoomMemberSetting.Members.Contains(member);
                }
                return ok;
            }
            else
            {
                return true;
            }
        }
        public static bool VerifyPrivateRoomBetFee(this MixRoom room, Transaction tx)
        {
            if (room.RoomMemberSetting.IsNull()) return true;
            if (room.RoomMemberSetting.Flag == 0) return true;
            var outputs = tx.Outputs.Where(m => m.AssetId == Blockchain.OXC && m.ScriptHash == room.Holder);
            if (outputs.IsNotNullAndEmpty())
            {
                if (outputs.Sum(m => m.Value) >= Fixed8.One * room.RoomMemberSetting.Flag) return true;
            }
            return false;
        }
        public static Fixed8 GetPrivateRoomBetFee(this MixRoom room)
        {
            if (room.RoomMemberSetting.IsNull()) return Fixed8.Zero;
            if (room.RoomMemberSetting.Flag == 0) return Fixed8.Zero;
            return Fixed8.One * room.RoomMemberSetting.Flag;
        }
    }
}
