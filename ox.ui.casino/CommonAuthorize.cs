using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using OX.Wallets.Models;
using OX.Wallets.NEP6;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OX.Cryptography;
using OX.Cryptography;

namespace OX.UI.Casino
{
    public partial class CommonAuthorize : DarkForm
    {
        INotecase Operater;
        Module Module;
        public CommonAuthorize(Module module, INotecase operater)
        {
            this.Module = module;
            this.Operater = operater;

            InitializeComponent();

            foreach (var act in operater.Wallet.GetHeldAccounts())
            {
                this.cbAccounts.Items.Add(new AccountListItem(act));
            }
        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.Text = UIHelper.LocalString("通用授权", "Common Authorize");
            this.lb_balance.Text = UIHelper.LocalString("被授权账户:", "Target Address:");
            this.lb_from.Text = UIHelper.LocalString("授权账户:", "Authorize Account:");
            this.bt_bet.Text = UIHelper.LocalString("签署授权", "Build Authorize");
            this.lb_markproof.Text = UIHelper.LocalString("授权证:", "Authorize Proof:");
        }



        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void darkComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void bt_NewRoom_Click(object sender, EventArgs e)
        {
            var from = this.cbAccounts.SelectedItem as AccountListItem;
            var address = from?.Account.Address;
            if (address.IsNullOrEmpty()) return;
            var TargetAddress = this.tb_address.Text;
            if (TargetAddress.IsNullOrEmpty()) return;
            try
            {
                var sh = TargetAddress.ToScriptHash();
                var fromkey = from.Account.GetKey();
                CommonAuthorizeMark mark = new CommonAuthorizeMark()
                {
                    Gambler = sh,
                    PublicKey = fromkey.PublicKey
                };
                SignatureValidator<CommonAuthorizeMark> validator = new SignatureValidator<CommonAuthorizeMark>() { Target = mark, Signature = mark.Sign(fromkey) };
                var str = validator.ToArray().ToHexString();
                this.tb_markproof.Text = str;
                Clipboard.SetText(str);
                string msg = str + UIHelper.LocalString("  已复制", "  copied");
                Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                DarkMessageBox.ShowInformation(msg, "");
            }
            catch
            {
                return;
            }
        }


        private void tb_amount_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
