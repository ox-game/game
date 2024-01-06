using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using System.Linq;

namespace OX.UI.Casino
{
    public static class BlockHelper
    {
        public static ulong GetMineNonce(uint blockIndex)
        {
            var hash = Blockchain.Singleton.GetBlockHash(blockIndex);
            if (hash.IsNotNull())
            {
                var block = Blockchain.Singleton.GetBlock(hash);
                if (block.IsNotNull())
                {
                    return block.ConsensusData;
                }
            }
            return 0;
        }
    }
}
