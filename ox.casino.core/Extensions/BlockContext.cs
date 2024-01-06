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
using OX.Wallets;

namespace OX.Casino
{
    public class BlockContext
    {
        public Dictionary<string, object> Items = new Dictionary<string, object>();
    }
}
