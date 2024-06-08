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
            return GetBuryMetaSideTransaction().GetContract().ScriptHash;
        }
        public static SlotSideTransaction GetBuryMetaSideTransaction()
        {
            return new SlotSideTransaction()
            {
                Slot = casino.CasinoMasterAccountPubKey,
                Channel = 0x01,
                SideType = SideType.PublicKey,
                Data = casino.CasinoMasterAccountPubKey.ToArray(),
                Flag = 0,
                AuthContract = Blockchain.SideAssetContractScriptHash,
                Attributes = new TransactionAttribute[0],
                Outputs = new TransactionOutput[0],
                Inputs = new CoinReference[0]
            };

        }
    }
}
