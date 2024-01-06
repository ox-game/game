using OX.Wallets;
using OX.Wallets.UI.Controls;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public class LuckEatSmallBettingButton : DarkButton
    {
        Betting Betting;
        public LuckEatSmallBettingButton(RoomView roomView, Betting betting)
        {
            this.Width = 80;
            this.Height = 25;
            this.Betting = betting;
            this.Text = betting.Amount.ToString();
            if (roomView.ClearResult.IsNotNullAndEmpty() && !roomView.RoundClearTxIds.Contains(betting.TxId))
                this.SpecialTextColor = Color.Red;
            string ms = string.Empty;
            if (roomView.Operater.Wallet.ContainsAndHeld(betting.BetRequest.From))
            {
                this.SpecialBorderColor = Color.Yellow;
                var act = roomView.Operater.Wallet.GetAccount(betting.BetRequest.From);
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
            var cc = string.Empty;
            var spccs = betting.BetRequest.BetPoint.Split('|');
            if (spccs.IsNotNullAndEmpty() && spccs.Length > 1)
            {
                cc = spccs[1];
            }
            toolTip1.SetToolTip(this, $"{ms}{betting.BetRequest.From.ToAddress()} : {betting.Amount.ToString()} : {cc}");
            this.Click += BettingButton_Click;

            this.Margin = new Padding() { All = 5 };
        }

        private void BettingButton_Click(object sender, EventArgs e)
        {

        }
    }
}
