using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets;
using OX.Wallets.UI.Forms;
using OX.Bapps;
using OX.IO.Data.LevelDB;
using Akka.Pattern;

namespace OX.UI.Casino
{
    public partial class PlayerQuery : DarkForm
    {
        public PlayerQuery()
        {
            InitializeComponent();
            this.Text = UIHelper.LocalString("玩家查询", "Gambler Query");
            this.bt_query.Text = UIHelper.LocalString("查询", "Query");
            this.bt_close.Text = UIHelper.LocalString("关闭", "Close");
            this.lb_address.Text = UIHelper.LocalString("玩家地址:", "Gambler Address:");
            this.lb_roomid.Text = UIHelper.LocalString("房间号:", "Room Id:");
        }

        private void bt_query_Click(object sender, EventArgs e)
        {
            var address = this.tb_address.Text;
            if (address.IsNullOrEmpty())
            {
                DarkMessageBox.ShowError(UIHelper.LocalString("玩家地址不能为空", "Gambler address cannot be empty"), "");
                return;
            }
            this.flowLayoutPanel1.Controls.Clear();
            try
            {
                var sh = address.ToScriptHash();
                var strRoomId = this.tb_roomId.Text;
                uint roomid = 0;
                if (strRoomId.IsNotNullAndEmpty())
                {
                    if (!uint.TryParse(strRoomId, out roomid))
                    {
                        DarkMessageBox.ShowError(UIHelper.LocalString("房间号无效", "Room Id invalid"), "");
                        this.tb_roomId.Text = string.Empty;
                        return;
                    }
                }
                var casinoProvider = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                if (casinoProvider.IsNotNull())
                {
                    Fixed8 betSum = Fixed8.Zero;
                    Fixed8 PrizSum = Fixed8.Zero;
                    if (roomid == 0)
                    {
                        var betamounts = casinoProvider.GetAll<BetSummaryKey, Fixed8>(CasinoBizPersistencePrefixes.Casino_Bet_Summary, SliceBuilder.Begin().Add(sh).ToArray());
                        foreach (var bet in betamounts)
                        {
                            PrizeSummaryKey PrizeSummaryKey = new PrizeSummaryKey { Gambler = sh, BetAddress = bet.Key.BetAddress };
                            var prizeamounts = casinoProvider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Prize_Summary, SliceBuilder.Begin().Add(PrizeSummaryKey).ToArray());
                            var prizeTotal = prizeamounts == default ? Fixed8.Zero : prizeamounts;
                            betSum += bet.Value;
                            PrizSum += prizeTotal;
                            var room = casinoProvider.GetRoom(bet.Key.BetAddress);
                            if (room.IsNotNull())
                                this.flowLayoutPanel1.Controls.Add(new PlayerQueryButton(room.RoomId, bet.Value, prizeTotal));

                        }
                    }
                    else
                    {
                        var room = casinoProvider.GetRoom(roomid);
                        if (room.IsNull())
                        {
                            DarkMessageBox.ShowError(UIHelper.LocalString("房间不存在", "Room not found"), "");
                            this.tb_roomId.Text = string.Empty;
                            return;
                        }
                        BetSummaryKey BetSummaryKey = new BetSummaryKey { Gambler = sh, BetAddress = room.BetAddress };
                        var betamounts = casinoProvider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Bet_Summary, SliceBuilder.Begin().Add(BetSummaryKey).ToArray());
                        PrizeSummaryKey PrizeSummaryKey = new PrizeSummaryKey { Gambler = sh, BetAddress = room.BetAddress };
                        var prizeamounts = casinoProvider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Prize_Summary, SliceBuilder.Begin().Add(PrizeSummaryKey).ToArray());
                        var betTotal = betamounts == default ? Fixed8.Zero : betamounts;
                        var prizeTotal = prizeamounts == default ? Fixed8.Zero : prizeamounts;
                        betSum += betTotal;
                        PrizSum += prizeTotal;
                        this.flowLayoutPanel1.Controls.Add(new PlayerQueryButton(roomid, betTotal, prizeTotal));
                    }
                    this.Text = $"{PrizSum}-{betSum}={PrizSum - betSum}";
                }
            }
            catch
            {
                DarkMessageBox.ShowError(UIHelper.LocalString("玩家地址无效", "Gambler address invalid"), "");
                this.tb_address.Text = string.Empty;
                return;
            }

        }

        private void bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
