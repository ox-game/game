using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using OX.Cryptography.ECC;
using OX.SmartContract;
using OX.VM;
using OX.BMS;

namespace OX.Casino
{
    public static class SideTransactionHelper
    {
        public static readonly Fixed8 MinSidePoolOXC = Fixed8.One * 1000;
        public static bool VerifyRegRoom(this SlotSideTransaction tx, out ECPoint pubkey)
        {
            pubkey = default;
            if (!tx.Slot.Equals(casino.CasinoMasterAccountPubKey) || tx.Channel != 0x00 || tx.SideType != SideType.PublicKey || !tx.AuthContract.Equals(Blockchain.SideAssetContractScriptHash)) return false;
            try
            {
                pubkey = tx.Data.AsSerializable<ECPoint>();
                var sh = tx.GetContract().ScriptHash;
                var outputs = tx.Outputs.Where(m => m.AssetId.Equals(Blockchain.OXC) && m.ScriptHash.Equals(sh));
                if (outputs.IsNullOrEmpty()) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool VerifyRegRoomFee(this SlotSideTransaction tx, Fixed8 regRoomFeeSetting)
        {
            var outputs = tx.Outputs.Where(m => tx.Channel == 0x00 && m.AssetId.Equals(Blockchain.OXC) && m.ScriptHash.Equals(casino.CasinoMasterAccountAddress) && tx.AuthContract.Equals(Blockchain.SideAssetContractScriptHash));
            if (outputs.IsNullOrEmpty()) return false;
            if (outputs.Sum(m => m.Value) < regRoomFeeSetting) return false;
            return true;
        }
        public static bool VerifyRegRoomRequest(this SlotSideTransaction st, out RegRoomRequest request)
        {
            request = default;

            if (st.Attach.TryAsSerializable<RegRoomRequest>(out request) && ((request.Permission == RoomPermission.Public && request.AssetId == Blockchain.OXC) || request.Permission == RoomPermission.Private))
            {
                if (request.DividendRatio > 0 && request.DividendRatio <= 100)
                {
                    if (request.Flag < 10 && request.BonusMultiple >= 2 && request.BonusMultiple < 10)
                        return true;
                }
            }
            return false;
        }
        public static Contract GetContractForOtherFlag(this SlotSideTransaction st, byte channel, byte flag)
        {
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitPush(st.Slot);
                sb.EmitPush(flag);
                sb.EmitPush(st.Data);
                sb.EmitPush((byte)st.SideType);
                sb.EmitPush((byte)0x00);
                sb.EmitAppCall(st.AuthContract);
                return Contract.Create(new[] { ContractParameterType.Signature }, sb.ToArray());
            }
        }
        public static bool VerifyRegMarkMember(this SlotSideTransaction tx, out ECPoint pubkey)
        {
            pubkey = default;
            if (!tx.Slot.Equals(casino.CasinoMasterAccountPubKey) || tx.Channel != 0x02 || tx.SideType != SideType.PublicKey || !tx.AuthContract.Equals(Blockchain.SideAssetContractScriptHash)) return false;
            try
            {
                pubkey = tx.Data.AsSerializable<ECPoint>();
                var sh = tx.GetContract().ScriptHash;
                var outputs = tx.Outputs.Where(m => m.AssetId.Equals(Blockchain.OXC) && m.ScriptHash.Equals(sh));
                if (outputs.IsNullOrEmpty()) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool VerifyRegBitSixBankerRequest(this SlotSideTransaction st, out RegMarkMemberRequest request)
        {
            return st.Attach.TryAsSerializable<RegMarkMemberRequest>(out request);
        }
        public static bool VerifyRegMarkMemberFee(this SlotSideTransaction tx, RegMarkMemberRequest request, uint dayfee, out uint days)
        {
            days = 0;
            if (tx.Channel != 0x02) return false;
            if (tx.AuthContract != Blockchain.SideAssetContractScriptHash) return false;
            if (request.Flag > 10) return false;
            var outputs = tx.Outputs.Where(m => m.AssetId.Equals(Blockchain.OXC) && m.ScriptHash.Equals(casino.CasinoMasterAccountAddress));
            if (outputs.IsNullOrEmpty()) return false;
            var total = outputs.Sum(m => m.Value);
            if (total < Fixed8.One * dayfee) return false;
            days = (uint)(total.GetInternalValue() / (Fixed8.D * dayfee));
            return true;
        }
    }
}
