using OX.Wallets.UI.Controls;
using System;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public class PrizePoolButton : DarkButton
    {
        public string Address { get; private set; }
        public Fixed8 Amount { get; private set; }
        public PrizePoolButton(string address, Fixed8 amount)
        {
            this.Address = address;
            ResetAmount(amount);
            this.Click += BettingButton_Click;
            this.Margin = new Padding() { All = 5 };
        }
        public void ResetAmount(Fixed8 amount)
        {
            this.Amount = amount;
            this.Amount = amount;
            this.Text = amount.ToString();
            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this, $"{Address} : {Amount.ToString()}");
        }
        private void BettingButton_Click(object sender, EventArgs e)
        {

        }
    }
}
