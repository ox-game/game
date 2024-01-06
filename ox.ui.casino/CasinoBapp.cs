using OX.Bapps;
using OX.Casino;
using OX.IO.Json;
using OX.Network.P2P.Payloads;
using OX.Cryptography.ECC;
using Akka.IO;
using OX.Wallets;
//using OX.UI.Casino.API;

namespace OX.UI.Casino
{
    public class CasinoBapp : Bapp
    {
        public override string MatchKernelVersion => "1.0.2";
        public override ECPoint[] BizPublicKeys => casino.BizPublicKeys;

        public override IBappProvider BuildBappProvider()
        {
            return new CasinoProvider(this);
        }
        public override IBappApi BuildBappApi()
        {
            return default;
            //return new CasinoApi(this);
        }
        public override IBappUi BuildBappUi()
        {
            return new CasinoUI(this);
        }
        public override SideScope[] GetSideScopes()
        {
            return new SideScope[] {
                new SideScope {
              MasterAddress=casino.CasinoSettleAccountAddress,
               Description=UIHelper.LocalString("娱乐边际信托","Casino Side Trust")
             },
                 new SideScope {
              MasterAddress=casino.CasinoWitnessAccountAddress,
               Description=UIHelper.LocalString("娱乐见证信托","Casino Witness Trust")
             }
            };
        }
        protected override void InitBapp() { }
    }

}
