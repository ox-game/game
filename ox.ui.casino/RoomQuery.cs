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
    public partial class RoomQuery : DarkForm
    {
        public UInt160 BetAddress;
        public RoomQuery(UInt160 betAddress)
        {
            BetAddress = betAddress;
            InitializeComponent();
            this.Text = UIHelper.LocalString("房间查询", "Room Query");
            this.bt_query.Text = UIHelper.LocalString("查询", "Query");
            this.bt_close.Text = UIHelper.LocalString("关闭", "Close");
            this.lb_address.Text = UIHelper.LocalString("玩家地址:", "Gambler Address:");
        }

        private void bt_query_Click(object sender, EventArgs e)
        {
            var address = this.tb_address.Text;
            this.flowLayoutPanel1.Controls.Clear();
            try
            {

                var casinoProvider = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                if (casinoProvider.IsNotNull())
                {

                    if (address.IsNullOrEmpty())
                    {
                        var betamounts = casinoProvider.GetAll<BetLandlordSummaryKey, Fixed8>(CasinoBizPersistencePrefixes.Casino_Landlord_Bet_Summary, SliceBuilder.Begin().Add(this.BetAddress).ToArray());
                        int c = 0;
                        foreach (var bet in betamounts.OrderByDescending(m => m.Value))
                        {
                            c++;
                            PrizeLandlordSummaryKey PrizeSummaryKey = new PrizeLandlordSummaryKey { Gambler = bet.Key.Gambler, BetAddress = bet.Key.BetAddress };
                            var prizeamounts = casinoProvider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Landlord_Prize_Summary, SliceBuilder.Begin().Add(PrizeSummaryKey).ToArray());
                            var prizeTotal = prizeamounts == default ? Fixed8.Zero : prizeamounts;
                            this.flowLayoutPanel1.Controls.Add(new RoomQueryButton(bet.Key.Gambler.ToAddress(), bet.Value, prizeTotal));
                            if (c >= 200) break;
                        }
                    }
                    else
                    {
                        var sh = address.ToScriptHash();
                        BetLandlordSummaryKey BetSummaryKey = new BetLandlordSummaryKey { Gambler = sh, BetAddress = this.BetAddress };
                        var betamounts = casinoProvider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Landlord_Bet_Summary, SliceBuilder.Begin().Add(BetSummaryKey).ToArray());
                        PrizeLandlordSummaryKey PrizeSummaryKey = new PrizeLandlordSummaryKey { Gambler = sh, BetAddress = this.BetAddress };
                        var prizeamounts = casinoProvider.Get<Fixed8>(CasinoBizPersistencePrefixes.Casino_Landlord_Prize_Summary, SliceBuilder.Begin().Add(PrizeSummaryKey).ToArray());
                        var betTotal = betamounts == default ? Fixed8.Zero : betamounts;
                        var prizeTotal = prizeamounts == default ? Fixed8.Zero : prizeamounts;
                        this.flowLayoutPanel1.Controls.Add(new RoomQueryButton(address, betTotal, prizeTotal));
                    }
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
