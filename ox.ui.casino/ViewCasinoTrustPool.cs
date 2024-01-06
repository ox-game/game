using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets;
using OX.Wallets.UI.Forms;
using OX.Bapps;
using OX.IO.Data.LevelDB;
using Akka.Pattern;
using OX.Network.P2P.Payloads;
using OX.Cryptography.ECC;
using OX.SmartContract;
using OX.Ledger;
using OX.Casino;

namespace OX.UI.Casino
{
    public partial class ViewCasinoTrustPool : DarkDialog
    {
     

        UInt160 TrustAddress;
        public ViewCasinoTrustPool()
        {
            InitializeComponent();
            this.Text = UIHelper.LocalString($"娱乐信托池", $"View Casino Trust Pool");
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");
        }



        private void GuaranteeQuery_Load(object sender, EventArgs e)
        {
            var AssetState = Blockchain.Singleton.CurrentSnapshot.Assets.TryGet(casino.GamblerLuckBonusAsset);
            this.lb_truster.Text = UIHelper.LocalString("委托人:", "Truster:");
            this.lb_trustee.Text = UIHelper.LocalString("受托人:", "Trustee:");
            this.lb_balance_OXC.Text = UIHelper.LocalString("OXC 余额:", "OXC Balance:");
            this.lb_balance_Bit.Text = UIHelper.LocalString($"{AssetState.GetName()} 余额:", $"{AssetState.GetName()} Balance:");
            this.lb_scope.Text = UIHelper.LocalString("信托范围:", "Trust Scope:");
            this.bt_query.Text = UIHelper.LocalString("查询余额", "Query Balance");
            this.lb_trustaddr.Text = UIHelper.LocalString("信托地址:", "Trust Address:");
            this.tb_truster_pub.Text = CasinoTrustPoolHelper.Truster.ToString();
            this.lb_truster_addr.Text = Contract.CreateSignatureRedeemScript(CasinoTrustPoolHelper.Truster).ToScriptHash().ToAddress();
            this.tb_trustee_pub.Text = CasinoTrustPoolHelper.Trustee.ToString();
            this.lb_trustee_addr.Text = Contract.CreateSignatureRedeemScript(CasinoTrustPoolHelper.Trustee).ToScriptHash().ToAddress();
            this.tb_scope.Text = casino.GamblerLuckBonusAssetDexPool;
            //Query();
        }
        void Query()
        {

            TrustAddress = CasinoTrustPoolHelper.TrustPoolAddress;
            this.tb_trustaddr.Text = TrustAddress.ToAddress();
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(TrustAddress, () => null);
            if (acts.IsNotNull())
            {
                this.tb_balance_oxc.Text = acts.GetBalance(Blockchain.OXC).ToString();
                this.tb_balance_bit.Text = acts.GetBalance(casino.GamblerLuckBonusAsset).ToString();
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       

        private void tb_truster_pub_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_trustee_pub_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_scope_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt_query_Click(object sender, EventArgs e)
        {
            Query();
        }
    }
}
