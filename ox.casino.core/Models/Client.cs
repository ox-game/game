using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.Cryptography.ECC;
using OX.Wallets;
using OX.IO;
using OX.Network.P2P;

namespace OX
{
    public class ClientName
    {
        public string Name;
    }
    public class ClientInfo : ClientName
    {
        public string Address;
    }
    public class ClientAccount
    {
        public string Name { get; set; }
        public string HostAddress { get; set; }
        public KeyPair HostKey { get; set; }
        public ECPoint PubKey { get; set; }
        public OXContract Contract { get; set; }
        public string AccessCode { get; set; }
        public Fixed8 AgentFee { get; set; }
        public Fixed8 BettingCap { get; set; }
        public ClientAccountRoomAuthentication[] Auths { get; set; }
    }
    public class SimplexClientAccount : ClientAccount
    {
        public string HolderAddress { get; set; }

        public string AllowSelfReturn { get; set; }
    }
    public class DuplexClientAccount : ClientAccount
    {
        public int DuplexId { get; set; }
        public Fixed8 ProfitThreshold { get; set; }
    }
    public class ClientAccountRoomAuthentication
    {
        public uint RoomId { get; set; }
        public string Auth { get; set; }
        public SignatureValidator<SeasonPassport> Validator { get; private set; }
        public SignatureValidator<SeasonPassport> GetValidator()
        {
            if (Validator.IsNull())
            {
                var bs = Auth.HexToBytes();
                Validator = bs.AsSerializable<SignatureValidator<SeasonPassport>>();
            }
            return Validator;
        }
    }
}
