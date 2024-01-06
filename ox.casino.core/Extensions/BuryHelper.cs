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
    public static class BuryHelper
    {
        public static UInt160 GetBuryAddress()
        {
            var tx = new SideTransaction()
            {
                Recipient = casino.CasinoSettleAccountPubKey,
                SideType = SideType.PublicKey,
                Data = casino.CasinoMasterAccountPubKey.ToArray(),
                Flag = 0,
                 AuthContract = Blockchain.SideAssetContractScriptHash,
                Attributes = new TransactionAttribute[0],
                Outputs = new TransactionOutput[0],
                Inputs = new CoinReference[0]
            };
            return tx.GetContract().ScriptHash;
        }
    }
}
