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

namespace OX.Casino
{
    public static class SideTransactionHelper
    {
        public static readonly Fixed8 MinSidePoolOXC = Fixed8.One * 1000;
        public static bool VerifyRegRoom(this SideTransaction tx, out ECPoint pubkey)
        {
            pubkey = default;
            if (!tx.Recipient.Equals(casino.CasinoSettleAccountPubKey) || tx.SideType != SideType.PublicKey || !tx.AuthContract.Equals(Blockchain.SideAssetContractScriptHash)) return false;
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
        public static bool VerifyRegRoomFee(this SideTransaction tx, Fixed8 regRoomFeeSetting)
        {
            var outputs = tx.Outputs.Where(m => m.AssetId.Equals(Blockchain.OXC) && m.ScriptHash.Equals(casino.CasinoSettleAccountAddress));
            if (outputs.IsNullOrEmpty()) return false;
            if (outputs.Sum(m => m.Value) < regRoomFeeSetting) return false;
            return true;
        }
        public static bool VerifyRegRoomRequest(this SideTransaction st, out RegRoomRequest request)
        {
            request = default;
            try
            {
                request = st.Attach.AsSerializable<RegRoomRequest>();
                if ((request.Permission == RoomPermission.Public && request.AssetId == Blockchain.OXC) || request.Permission == RoomPermission.Private)
                {
                    if (request.DividendRatio > 0 && request.DividendRatio <= 100)
                    {
                        if (request.Flag < 10 && request.BonusMultiple >= 2 && request.BonusMultiple < 10)
                            return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        public static Contract GetContractForOtherFlag(this SideTransaction st, byte flag)
        {
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitPush(st.Recipient);
                sb.EmitPush(flag);
                sb.EmitPush(st.Data);
                sb.EmitPush((byte)st.SideType);
                sb.EmitAppCall(st.AuthContract);
                return Contract.Create(new[] { ContractParameterType.Signature }, sb.ToArray());
            }
        }
    }
}
