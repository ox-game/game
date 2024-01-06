using OX.Casino;
using OX.SmartContract;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace OX.UI.Casino
{
    public class PrizeButton : DarkButton
    {
        public Wallet Wallet;
        public string Address { get; private set; }
        public Fixed8 Amount { get; private set; }
        public string ms = string.Empty;
        public PrizeButton(string address, Fixed8 amount, Wallet wallet, MixRoom room, string DibsAccount)
        {
            this.Width = 80;
            this.Height = 25;
            this.Wallet = wallet;
            this.Address = address;

            this.Click += BettingButton_Click;
            var sh = Address.ToScriptHash();
            if (wallet.ContainsAndHeld(sh))
            {
                this.SpecialBorderColor = Color.Red;
                var act = Wallet.GetAccount(sh);
                if (act.IsNotNull() && act.Label.IsNotNullAndEmpty())
                {
                    ms = $"{act.Label.Trim()}-";
                }
            }
            else if (room.Holder.Equals(sh))
                this.SpecialBorderColor = Color.Blue;
            else if (room.PoolAddress.Equals(sh))
                this.SpecialBorderColor = Color.Yellow;
            else if (DibsAccount == Address)
                this.SpecialBorderColor = Color.Orange;
            this.Margin = new Padding() { All = 5 };
            ResetAmount(amount);
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
            toolTip1.SetToolTip(this, $"{ms}{Address} : {Amount.ToString()}");
        }
        private void BettingButton_Click(object sender, EventArgs e)
        {

        }
    }
}
