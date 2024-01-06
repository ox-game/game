using OX.Wallets;
using OX.Wallets.UI.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;
using OX.Cryptography.ECC;
using OX.SmartContract;

namespace OX.UI.Casino
{
    public class TeamKillMergeButton : DarkButton
    {
        RoomView RoomView;
        Betting[] Bettings;
        ECPoint PubKey;
        public TeamKillMergeButton(RoomView roomView, ECPoint pubkey, Betting[] bettings)
        {
            this.RoomView = roomView;
            this.Width = 200;
            this.PubKey = pubkey;
            this.Bettings = bettings;
            this.Text = bettings.Sum(m => m.Amount).ToString();
            var sh = pubkey.IsNotNull() ? Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash() : null;
            string ms = string.Empty;
            if (roomView.Operater.Wallet.ContainsAndHeld(sh))
            {
                this.SpecialBorderColor = Color.Red;
                var act = roomView.Operater.Wallet.GetAccount(sh);
                if (act.IsNotNull() && act.Label.IsNotNullAndEmpty())
                {
                    ms = $"{act.Label.Trim()}-";
                }
            }
            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this, $"{ms}{sh.ToAddress()}");
            this.Click += BettingButton_Click;
            this.Margin = new Padding() { All = 5 };
        }

        private void BettingButton_Click(object sender, EventArgs e)
        {
            if (this.PubKey.IsNotNull())
            {
                var address = Contract.CreateSignatureRedeemScript(this.PubKey).ToScriptHash().ToAddress();
                new TeamKillMoreBettings(this.RoomView, address, this.Bettings).ShowDialog();
            }
        }
    }
}
