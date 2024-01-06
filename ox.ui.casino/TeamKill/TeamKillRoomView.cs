using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using OX.SmartContract;
using OX.Wallets.UI.Config;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json.Linq;
using OX.Casino;

namespace OX.UI.Casino
{
    public class TeamKillRoomView : RoomView
    {
        //Fixed8 poolBalance = Fixed8.Zero;
        TeamKillTable TeamKillTable = default;
        public override GameKind GameKind => GameKind.TeamKill;
        public override byte MintBetSetting => CasinoSettingTypes.TeamKillMinBet;
        public TeamKillRoomView(INotecase notecase, MixRoom room, bool ok) : base(notecase, room, ok)
        {
        }
        public override void InitRoomView(MixRoom roomkey)
        {
        }
        public override void ReloadGameData()
        {
            //using var snapshot = Blockchain.Singleton.GetSnapshot();
            //var poolAddress = Contract.CreateSignatureRedeemScript(Room.RoomRecord.PoolAddress).ToScriptHash();
            //var acts2 = snapshot.Accounts.GetAndChange(poolAddress, () => null);
            //poolBalance = acts2.IsNotNull() ? acts2.GetBalance(Blockchain.OXC) : Fixed8.Zero;

        }
        public override void SubExport(System.Text.StringBuilder sb)
        {
            //if (this.poolBalance != default)
            //{
            //    var m1 = UIHelper.LocalString("奖池余额:", "Pool Balance:");
            //    sb.AppendLine($"{m1} : {this.poolBalance}");
            //    sb.AppendLine("-------------------------------------------------------------");
            //}
        }
        public override void ReloadGameUI()
        {
            if (this.Bettings != default)
            {
                this.TeamKillTable.FillBettings(this.Bettings.Select(m => m.Value).ToArray());
            }
            if (!this.ViewHead.Controls.ContainsKey("_bt_teamkillbet"))
            {
                var lootingbet = new DarkButton();
                lootingbet.Name = "_bt_teamkillbet";
                lootingbet.Text = UIHelper.LocalString("下注", "Bet");
                lootingbet.Width = 100;
                lootingbet.Height = 30;
                lootingbet.Top = 210;
                lootingbet.Left = 50;
                lootingbet.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                this.ViewHead.Controls.Add(lootingbet);
                lootingbet.Click += Lootingbet_Click;
            }
            if (this.Riddles.IsNotNull())
            {
                if (!this.ViewHead.Controls.ContainsKey("verifyRiddles"))
                {
                    var verifyRiddles = new DarkButton();
                    verifyRiddles.Name = "verifyRiddles";
                    verifyRiddles.Text = UIHelper.LocalString("验证谜底", "Verify Riddles");
                    verifyRiddles.Width = 100;
                    verifyRiddles.Height = 30;
                    verifyRiddles.Top = 6;
                    verifyRiddles.Left = this.ClientSize.Width - 110;
                    verifyRiddles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    this.ViewHead.Controls.Add(verifyRiddles);
                    verifyRiddles.Click += VerifyRiddles_Click;
                }
            }
            else
            {
                if (this.ViewHead.Controls.ContainsKey("verifyRiddles"))
                    this.ViewHead.Controls.RemoveByKey("verifyRiddles");
            }
        }

        private void Lootingbet_Click(object sender, System.EventArgs e)
        {
            if (AllowBet())
            {
                this.BetWatch = new TeamKillBet(this.Module, this.Operater, this.Room, this.MinBet, this.CurrentIndex);
                if (this.BetWatch is Form form)
                {
                    form.ShowDialog();
                    form.Activate();
                }
            }
            else
            {
                string msg = UIHelper.LocalString($"{this.Room.RoomId}处于关闭状态,无法下注", $"Room:{Room.RoomId} closed, cannot bet");
                Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                DarkMessageBox.ShowInformation(msg, "");
            }
        }

        private void VerifyRiddles_Click(object sender, System.EventArgs e)
        {
            if (this.Riddles.IsNotNull())
            {
                VerifyRiddles form = new VerifyRiddles(this.Room.BetAddress, this.Riddles, this.MineNonce);
                form.ShowDialog();
            }
        }

        public void OnViewBodyPaint(PaintEventArgs e)
        {
            //写方位投注合计及开奖结果
            var g = e.Graphics;
            //using (var b = new SolidBrush(Colors.LightText))
            //{
            //    using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
            //    {
            //        g.DrawString(total.ToString(), bigFont, b, new PointF(100, 5));
            //    }
            //}
            //if (this.Riddles.IsNotNull())
            //{
            //    var guessKey = this.Riddles.GetGuessKey(this.Room.RoomRecord.Kind);
            //    if (guessKey.IsNotNull())
            //    {
            //        char[] keys = guessKey.Keys.ToCharArray();
            //        var c = keys[cell.Data.Position].ToString();
            //        using (var b = new SolidBrush(Color.Yellow))
            //        {
            //            using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
            //            {
            //                g.DrawString(c, bigFont, b, new PointF(33, 5));
            //            }
            //        }
            //    }
            //}
        }
        public override void OnViewHeadPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            if (this.Bettings != default)
            {
                var allAmount = this.Bettings.Sum(m => m.Value.Amount);
                using (var b = new SolidBrush(Color.Yellow))
                {
                    using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                    {

                        g.DrawString(allAmount.ToString(), bigFont, b, new PointF(5, 160));
                    }
                }
            }


            using (var b = new SolidBrush(Color.Yellow))
            {
                using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                {
                    g.DrawString($"{this.Room.RoomId.ToString()}#", bigFont, b, new PointF(5, 5));
                    //g.DrawString(poolBalance.ToString(), bigFont, b, new PointF(5, 105));
                    //if (this.Riddles.IsNotNull())
                    //{
                    //    var guessKey = this.Riddles.GetGuessKey(this.Room.RoomRecord.Kind);
                    //    if (guessKey.IsNotNull())
                    //    {
                    //        var k = guessKey.Keys;
                    //        g.DrawString(k, bigFont, b, new PointF(5, 55));
                    //    }
                    //}
                    g.DrawString(this.CurrentIndex.ToString(), bigFont, b, new PointF(510, 5));
                }
            }
            using (var b = new SolidBrush(Colors.LightText))
            {
                //g.DrawString(UIHelper.LocalString("开奖结果:", "Lottery Result:"), this.Font, b, new PointF(5, 35));
                //g.DrawString(UIHelper.LocalString("房间池余额:", "Pool Balance:"), this.Font, b, new PointF(5, 80));
                g.DrawString(UIHelper.LocalString("总投注额:", "Current Bankroll:"), this.Font, b, new PointF(5, 140));
            }
            using (var p = new Pen(Colors.GreyHighlight))
            {
                var p1 = new PointF(10, 135);
                var p2 = new PointF(234, 135);
                g.DrawLine(p, p1, p2);
            }
        }
        public override Control CreateViewBody()
        {
            TeamKillTable = new TeamKillTable(this, OnViewBodyPaint, moreBet);
            return TeamKillTable;
        }
        //void bet(byte position)
        //{
        //    if (AllowBet())
        //    {
        //        this.BetWatch = new LottoBet(this.Module, this.Operater, this.Room, this.MinBet, this.CurrentIndex, position);
        //        if (this.BetWatch is Form form)
        //        {
        //            form.ShowDialog();
        //            form.Activate();
        //        }
        //    }
        //    else
        //    {
        //        string msg = UIHelper.LocalString($"{this.Room.RoomKey.RoomId}处于关闭状态,无法下注", $"Room:{Room.RoomKey.RoomId} closed, cannot bet");
        //        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
        //        DarkMessageBox.ShowInformation(msg, "");
        //    }
        //}
        void moreBet(byte position)
        {
        }
    }
}
