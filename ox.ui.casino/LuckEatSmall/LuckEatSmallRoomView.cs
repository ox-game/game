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
using OX.Casino;
using Akka.Actor;

namespace OX.UI.Casino
{
    public class LuckEatSmallRoomView : RoomView
    {
        byte BankerPosition = 0;
        Fixed8 masterBalance = Fixed8.Zero;
        Fixed8 poolBalance = Fixed8.Zero;
        LuckEatSmallTable LuckEatSmallTable = default;
        public override GameKind GameKind => GameKind.EatSmall;
        public override byte MintBetSetting => CasinoSettingTypes.EatSmallMinBet;
        public LuckEatSmallRoomView(INotecase notecase, MixRoom room, bool ok) : base(notecase, room, ok)
        {
        }
        public override void InitRoomView(MixRoom room)
        {
            BankerPosition = room.Request.Flag;
        }
        public override void ReloadGameData()
        {
            using var snapshot = Blockchain.Singleton.GetSnapshot();
            var acts = snapshot.Accounts.GetAndChange(Room.BankerAddress, () => null);
            masterBalance = acts.IsNotNull() ? acts.GetBalance(Room.Request.AssetId) : Fixed8.Zero;
            var acts2 = snapshot.Accounts.GetAndChange(Room.PoolAddress, () => null);
            poolBalance = acts2.IsNotNull() ? acts2.GetBalance(Room.Request.AssetId) : Fixed8.Zero;

        }
        public override void SubExport(System.Text.StringBuilder sb)
        {
            if (this.poolBalance != default && this.Room.Request.Permission == RoomPermission.Public)
            {
                var m1 = UIHelper.LocalString("奖池余额:", "Pool Balance:");
                sb.AppendLine($"{m1} : {this.poolBalance}");
                sb.AppendLine("-------------------------------------------------------------");
            }
            if (this.masterBalance != default)
            {
                var m1 = UIHelper.LocalString("庄家余额:", "Banker Balance:");
                sb.AppendLine($"{m1} : {this.masterBalance}");
                sb.AppendLine("-------------------------------------------------------------");
            }
        }
        public override void ReloadGameUI()
        {
            for (byte i = 0; i < 10; i++)
            {
                if (i != this.BankerPosition)
                {
                    if (this.LuckEatSmallTable.Cells.TryGetValue(i, out LuckEatSmallCell cell))
                    {
                        List<Betting> bettings = new List<Betting>();
                        if (this.Bettings != default)
                        {
                            var bs = this.Bettings.Where(m =>
                            {
                                var cs = m.Value.BetRequest.BetPoint.ToCharArray();
                                return cs.IsNotNullAndEmpty() && cs[0].ToString() == i.ToString();
                            }).Select(m => m.Value);
                            if (bs.IsNotNullAndEmpty())
                            {
                                bettings.AddRange(bs);
                            }
                        }
                        cell.Bettings = bettings.ToArray();
                        cell.FillBettingPanel();
                    }
                }
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

        private void VerifyRiddles_Click(object sender, System.EventArgs e)
        {
            if (this.Riddles.IsNotNull())
            {
                VerifyRiddles form = new VerifyRiddles(this.Room.BetAddress, this.Riddles, this.MineNonce);
                form.ShowDialog();
            }
        }

        public void OnViewBodyPaint(LuckEatSmallCell cell, PaintEventArgs e)
        {
            //写方位投注合计及开奖结果
            var g = e.Graphics;
            var total = cell.Bettings.Sum(m => m.Amount);
            using (var b = new SolidBrush(Colors.LightText))
            {
                using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                {
                    g.DrawString(total.ToString(), bigFont, b, new PointF(100, 5));
                }
            }
            if (this.Riddles.IsNotNull() && this.MineNonce > 0)
            {
                var guessKey = this.Riddles.GetGuessKey(this.Room.Request.Kind);
                if (guessKey.IsNotNull())
                {
                    char[] keys = guessKey.ReRandomSanGongOrLotto(this.MineNonce, this.CurrentIndex);
                    var c = keys[cell.Data.Position].ToString();
                    using (var b = new SolidBrush(Color.Yellow))
                    {
                        using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                        {
                            g.DrawString(c, bigFont, b, new PointF(33, 5));
                        }
                    }
                }
            }
        }
        public override void OnViewHeadPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            if (this.Bettings != default)
            {
                var allAmount = this.Bettings.Sum(m => m.Value.Amount);
                var bs = this.Bettings.Where(m =>
                {
                    var cs = m.Value.BetRequest.BetPoint.ToCharArray();
                    return cs.IsNotNullAndEmpty() && cs[0].ToString() == this.BankerPosition.ToString();
                }).Select(m => m.Value);


                using (var b = new SolidBrush(Color.Yellow))
                {
                    using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                    {
                        if (bs.IsNotNullAndEmpty())
                        {
                            var bankertotal = bs.Sum(m => m.Amount);
                            g.DrawString(bankertotal.ToString(), bigFont, b, new PointF(5, 160));
                        }
                        g.DrawString(allAmount.ToString(), bigFont, b, new PointF(5, 210));
                    }
                }
            }


            using (var b = new SolidBrush(Color.Yellow))
            {
                using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                {
                    g.DrawString($"{this.Room.RoomId.ToString()}#{this.Room.Request.BonusMultiple}", bigFont, b, new PointF(5, 5));
                    if (this.Room.Request.Permission == RoomPermission.Public)
                        g.DrawString(poolBalance.ToString(), bigFont, b, new PointF(5, 55));
                    g.DrawString(masterBalance.ToString(), bigFont, b, new PointF(5, 105));
                    g.DrawString(this.CurrentIndex.ToString(), bigFont, b, new PointF(510, 45));
                }
            }
            using (var b = new SolidBrush(Colors.LightText))
            {
                if (this.Room.Request.Permission == RoomPermission.Public)
                {
                    g.DrawString(UIHelper.LocalString("奖池余额:", "Pool Balance:"), this.Font, b, new PointF(5, 35));
                }
                g.DrawString(UIHelper.LocalString("庄家余额:", "Banker Balance:"), this.Font, b, new PointF(5, 80));
                var c = string.Empty;
                if (this.Room.Request.Permission == RoomPermission.Public && this.Riddles.IsNotNull() && this.MineNonce > 0)
                {
                    var guessKey = this.Riddles.GetGuessKey(this.Room.Request.Kind);
                    if (guessKey.IsNotNull())
                    {
                        char[] keys = guessKey.ReRandomSanGongOrLotto(this.MineNonce, this.CurrentIndex);
                        c = keys[this.BankerPosition].ToString();
                        using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                        {
                            g.DrawString($"{guessKey.SpecialChar} - {guessKey.SpecialPosition}", bigFont, new SolidBrush(Color.Yellow), new PointF(170, 5));
                        }
                    }
                }
                g.DrawString(UIHelper.LocalString($"庄家投注{this.BankerPosition}号位:  {c}", $"Banker Bet Position {this.BankerPosition}:  {c}"), this.Font, b, new PointF(5, 140));
                g.DrawString(UIHelper.LocalString("总投注额:", "Current Bankroll:"), this.Font, b, new PointF(5, 185));
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
            List<LuckEatSmallData> l = new List<LuckEatSmallData>();
            for (byte i = 0; i < 10; i++)
            {
                if (i != this.BankerPosition)
                {
                    l.Add(new LuckEatSmallData() { Position = i });
                }
            }
            LuckEatSmallTable = new LuckEatSmallTable(this, l.ToArray(), OnViewBodyPaint, bet, moreBet);
            return LuckEatSmallTable;
        }
        void bet(byte position, bool isTrust)
        {
            if (AllowBet())
            {
                if (isTrust)
                {
                    this.BetWatch = new LuckEatSmallPositionTrustBet(this.Module, this.Operater, this.Room, this.MinBet, this.CurrentIndex, position);
                    if (this.BetWatch is Form form)
                    {
                        form.ShowDialog();
                        form.Activate();
                    }
                }
                else
                {
                    this.BetWatch = new LuckEatSmallPositionBet(this.Module, this.Operater, this.Room, this.MinBet, this.CurrentIndex, position);
                    if (this.BetWatch is Form form)
                    {
                        form.ShowDialog();
                        form.Activate();
                    }
                }
            }
            else
            {
                string msg = UIHelper.LocalString($"{this.Room.RoomId}处于关闭状态,无法下注", $"Room:{Room.RoomId} closed, cannot bet");
                Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                DarkMessageBox.ShowInformation(msg, "");
            }
        }
        void moreBet(byte position)
        {
            new LuckEatSmallMoreBettings(this, this.Operater.Wallet, position, this.Room, this.CurrentIndex, this.DibsAccount).ShowDialog();
        }
        public override void AfterOnBlock(Block block)
        {
            base.AfterOnBlock(block);
            if (OXRunTime.RunMode != RunMode.Server)
            {
                if (this.PeroidBlocks == 100)
                {
                    var k = this.CurrentIndex % this.PeroidBlocks;
                    var rev = block.Index % this.PeroidBlocks;
                    if (k < rev) k += this.PeroidBlocks;
                    var rm = k-rev ;
                    this.DoInvoke(() =>
                    {
                        //if (this.Visible)
                        //{
                        DockText = $"{this.Room.RoomId.ToString()}-{UIHelper.LocalString(this.Room.Request.Kind.StringValue(), this.Room.Request.Kind.EngStringValue())}--{rm}";
                        //}
                    });
                }
            }
        }
    }
}
