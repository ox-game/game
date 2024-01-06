using OX.Wallets;
using OX.Wallets.UI.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public class LottoBettingButton : DarkButton
    {
        Betting Betting;
        public LottoBettingButton(RoomView roomView, Betting betting)
        {
            this.Betting = betting;
            this.Text = betting.Amount.ToString();
            if (roomView.ClearResult.IsNotNullAndEmpty() && !roomView.RoundClearTxIds.Contains(betting.TxId))
                this.SpecialTextColor = Color.Red;
            string ms = string.Empty;
            if (roomView.Operater.Wallet.ContainsAndHeld(betting.BetRequest.From))
            {
                this.SpecialBorderColor = Color.Red;
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
            toolTip1.SetToolTip(this, $"{ms}{betting.BetRequest.From.ToAddress()} : {BetPack.ShowBetPoint(betting.BetRequest.BetPoint)}");
            this.Click += BettingButton_Click;
            this.Margin = new Padding() { All = 5 };
        }

        private void BettingButton_Click(object sender, EventArgs e)
        {

        }
    }
}
