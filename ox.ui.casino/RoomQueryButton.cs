using OX.Bapps;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using System;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public class RoomQueryButton : DarkButton
    {
        string Address;
        Fixed8 BetTotal;
        Fixed8 PrizeTotal;
        public RoomQueryButton(string address, Fixed8 betTotal, Fixed8 prizeTotal)
        {
            this.Address = address;
            this.BetTotal = betTotal;
            this.PrizeTotal = prizeTotal;
            //var strBet = UIHelper.LocalString($"投注:{betTotal}", $"Bet:{betTotal}");
            //var strPrize = UIHelper.LocalString($"获奖:{prizeTotal}", $"Prize:{prizeTotal}");
            this.Text = $"{this.Address}   {prizeTotal}-{betTotal}={prizeTotal - betTotal}";
            this.Width = 700;
            this.Height = 50;
            this.Margin = new Padding() { All = 5 };
        }

    }
}
