using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Bapps;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace OX.UI.Bury
{
    public class BuryButton : DarkButton
    {
        BuryRecord BuryRecord;
        BuryMergeTx BuryMergeTx;
        BuryView BuryView;
        public BuryButton(BuryView buryView, BuryRecord br, BuryMergeTx buryMergeTx, int index)
        {
            this.Width = 80;
            this.Height = 25;
            this.BuryRecord = br;
            this.BuryMergeTx = buryMergeTx;
            this.BuryView = buryView;
            this.Text = $"{br.Request.PlainBuryPoint}";
            string ms = string.Empty;
            if (buryView.Operater.Wallet.ContainsAndHeld(br.Request.From))
            {
                this.SpecialTextColor = Color.Red;
                var act = buryView.Operater.Wallet.GetAccount(br.Request.From);
                if (act.IsNotNull() && act.Label.IsNotNullAndEmpty())
                {
                    ms = $"{act.Label.Trim()} :  ";
                }
            }
            else if (BuryView.Operater.Wallet is OpenWallet openWallet)
            {
                var cs = openWallet.GetAssetTrustContacts();
                if (cs.IsNotNullAndEmpty())
                {
                    if (cs.ContainsKey(br.Request.From))
                    {
                        this.SpecialTextColor = Color.Green;
                    }
                }
            }
            if (index <= 100)
                this.SpecialBorderColor = Color.Yellow;

            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this, $"{ms}{br.BuryAmount.ToString()}  :  {br.Request.From.ToAddress()}  :  {br.Request.VerifyHash.ToString()}");
            this.Click += BettingButton_Click;

            this.Margin = new Padding() { All = 5 };
        }

        private void BettingButton_Click(object sender, EventArgs e)
        {
            if (this.BuryMergeTx.IsNotNull())
            {
                new ReplyBuryDetail(this.BuryRecord, this.BuryMergeTx).ShowDialog();
            }
            else
            {
                var hash = this.BuryRecord.Request.VerifyHash.ToString();
                Clipboard.SetText(hash);
                string msg = hash + UIHelper.LocalString("  已复制", "  copied");
                Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.BuryView.Module.Bapp });

                DarkMessageBox.ShowInformation(msg, "");
            }
        }
    }
}
