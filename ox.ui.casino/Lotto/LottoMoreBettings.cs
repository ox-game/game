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
using OX.Casino;

namespace OX.UI.Casino
{
    public partial class LottoMoreBettings : DarkDialog
    {
        public LottoMoreBettings(RoomView roomView, Wallet wallet, byte position, MixRoom roominfo, uint index, string dibsaccount)
        {
            InitializeComponent();
            this.Text = UIHelper.LocalString($"{roominfo.RoomId}房间{index}局{position}号位", $"RoomId:{roominfo.RoomId},Round:{index},Position:{position}");
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");
            var plugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (plugin.IsNotNull())
            {
                var Bettings = plugin.GetBettings(roominfo.BetAddress, index);
                if (Bettings != default)
                {
                    foreach (var bs in Bettings.Where(m =>
                       {
                           var cs = m.Value.BetRequest.BetPoint.ToCharArray();
                           return cs.IsNotNullAndEmpty() && cs[0].ToString() == position.ToString();
                       }).Select(m => m.Value).OrderByDescending(m => m.Amount))
                    {
                        var pb = new LottoBettingButton(roomView, bs);
                        this.container.Controls.Add(pb);
                    }
                }
            }
        }
    }
}
