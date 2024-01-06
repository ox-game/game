using OX.Bapps;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using System;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public class PlayerQueryButton : DarkButton
    {
        uint RoomID;
        Fixed8 BetTotal;
        Fixed8 PrizeTotal;
        public PlayerQueryButton(uint roomid, Fixed8 betTotal, Fixed8 prizeTotal)
        {
            this.RoomID = roomid;
            this.BetTotal = betTotal;
            this.PrizeTotal = prizeTotal;
            var rstr = roomid == 0 ? UIHelper.LocalString("所有房间::", "All Rooms::") : $"{roomid}:";
            //var strBet = UIHelper.LocalString($"投注:{betTotal}", $"Bet:{betTotal}");
            //var strPrize = UIHelper.LocalString($"获奖:{prizeTotal}", $"Prize:{prizeTotal}");
            this.Text = $"{rstr}   {prizeTotal}-{betTotal}={prizeTotal - betTotal}";
            this.Width = 400;
            this.Height = 50;
            this.Margin = new Padding() { All = 5 };
        }

    }
}
