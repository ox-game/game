using OX.Wallets;
using OX.Wallets.UI.Controls;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace OX.UI.GameMining
{
    public class GMSangongBettingButton : DarkButton
    {
        GMBetting Betting;
        public GMSangongBettingButton(GM roomView, GMBetting betting)
        {
            this.Width = 80;
            this.Height = 25;
            this.Betting = betting;
            this.Text = betting.Amount.ToString();
        
            string ms = string.Empty;
            if (roomView.Operater.Wallet.ContainsAndHeld(betting.SH))
            {
                this.SpecialBorderColor = Color.Yellow;
                var act = roomView.Operater.Wallet.GetAccount(betting.SH);
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
            toolTip1.SetToolTip(this, $"{ms}{betting.SH.ToAddress()} : {betting.Amount.ToString()}");
            this.Click += BettingButton_Click;

            this.Margin = new Padding() { All = 5 };
        }

        private void BettingButton_Click(object sender, EventArgs e)
        {

        }
    }
}
