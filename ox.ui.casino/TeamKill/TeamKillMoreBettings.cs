using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.Wallets.UI.Forms;
using OX.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OX.SmartContract;

namespace OX.UI.Casino
{
    public partial class TeamKillMoreBettings : DarkDialog
    {
        public TeamKillMoreBettings(RoomView roomView, string address, Betting[] bettings)
        {
            InitializeComponent();
            this.Text = UIHelper.LocalString($"{address}  投注明细", $"{address}  bet details");
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");
            foreach (var bet in bettings)
            {
                var pb = new TeamKillBettingButton(roomView, bet);
                this.container.Controls.Add(pb);
            }
        }
    }
}
