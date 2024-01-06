using OX.Wallets.UI.Config;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OX.Wallets;
using System.Linq;
using System;

namespace OX.UI.Casino
{
    public class LuckEatSmallData
    {
        public byte Position { get; set; }
    }
    public class LuckEatSmallCell : Control
    {
        RoomView RoomView;
        LuckEatSmallTable SangongTable;
        public Panel Panel;
        public LuckEatSmallData Data;
        public DarkButton BetButton;
        public DarkButton TrustBetButton;
        public DarkButton MoreButton;
        public Betting[] Bettings { get; set; } = new Betting[0];
        Action<byte, bool> BetAction;
        Action<byte> MoreBetAction;
        public LuckEatSmallCell(LuckEatSmallTable sangongtable, RoomView roomView, LuckEatSmallData data, Action<byte, bool> betaction, Action<byte> morebetaction)
        {
            this.RoomView = roomView;
            this.SangongTable = sangongtable;
            this.Data = data;
            this.BetAction = betaction;
            this.MoreBetAction = morebetaction;
            this.SuspendLayout();
            this.BetButton = new DarkButton();
            this.BetButton.Text = UIHelper.LocalString("下注", "Bet");
            this.BetButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.BetButton.Width = 80;
            this.BetButton.Height = 30;
            this.BetButton.Top = 6;
            this.BetButton.Left = ClientSize.Width - 125;
            this.Controls.Add(this.BetButton);
            this.BetButton.Click += BetButton_Click;
            this.TrustBetButton = new DarkButton();
            this.TrustBetButton.Text = UIHelper.LocalString("信托下注", "Trust Bet");
            this.TrustBetButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.TrustBetButton.Width = 100;
            this.TrustBetButton.Height = 30;
            this.TrustBetButton.Top = 6;
            this.TrustBetButton.Left = ClientSize.Width - 230;
            this.Controls.Add(this.TrustBetButton);
            this.TrustBetButton.Click += TrustBetButton_Click;
            this.MoreButton = new DarkButton();
            this.MoreButton.Text = "...";
            this.MoreButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.MoreButton.Width = 33;
            this.MoreButton.Height = 30;
            this.MoreButton.Top = 6;
            this.MoreButton.Left = ClientSize.Width - 40;
            this.Controls.Add(this.MoreButton);
            this.MoreButton.Click += MoreButton_Click;

            this.Panel = new FlowLayoutPanel();
            //this.Panel.BackColor = Color.Yellow;
            this.Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
           | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel.Location = new System.Drawing.Point(6, 40);
            this.Panel.Left = 6;
            this.Panel.Top = 40;
            this.Panel.Size = new System.Drawing.Size(ClientSize.Width - 12, ClientSize.Height - 45);
            this.Panel.Width = ClientSize.Width - 12;
            this.Panel.Height = ClientSize.Height - 46;
            this.Controls.Add(this.Panel);
            this.Panel.SizeChanged += Panel_SizeChanged;
            //this.fillBettingPanel();
            this.Panel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void TrustBetButton_Click(object sender, EventArgs e)
        {
            //if (this.RoomView.Room.Request.Permission == RoomPermission.Public)
                this.BetAction(this.Data.Position, true);
        }

        private void MoreButton_Click(object sender, EventArgs e)
        {
            this.MoreBetAction(this.Data.Position);
        }

        private void BetButton_Click(object sender, EventArgs e)
        {
            this.BetAction(this.Data.Position, false);
        }

        public void ClearBetting()
        {
            this.Panel.Controls.Clear();
        }
        private void Panel_SizeChanged(object sender, System.EventArgs e)
        {
            //this.fillBettingPanel();
        }
        public void FillBettingPanel()
        {
            this.Panel.Controls.Clear();
            var cols = this.Panel.ClientSize.Width / 90;
            var rows = this.Panel.ClientSize.Height / 30;
            var bets = this.Bettings;
            if (this.Bettings.Length > cols * rows)
                bets = this.Bettings.Take(cols * rows).ToArray();
            foreach (var bet in bets.OrderByDescending(m => m.Amount))
            {
                var betbtn = new LuckEatSmallBettingButton(this.RoomView, bet);
                this.Panel.Controls.Add(betbtn);
                //betbtn.Invalidate();
            }
            this.Invalidate();
            this.Panel.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = new Rectangle(3, 3, ClientSize.Width - 6, ClientSize.Height - 6);
            var rectPosition = new Rectangle(3, 3, 28, 32);
            var rectRiddles = new Rectangle(31, 3, 28, 32);

            using (var b = new SolidBrush(Color.Red))
            {
                using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                {
                    //g.DrawString(this.Data.Position, Font, b, new PointF(5, 5));
                    g.DrawString(this.Data.Position.ToString(), bigFont, b, new PointF(5, 5));
                }

            }

            using (var p = new Pen(Colors.GreyHighlight))
            {
                g.DrawRectangle(p, rect);
                g.DrawRectangle(p, rectPosition);
                g.DrawRectangle(p, rectRiddles);
            }
            this.SangongTable.Action?.Invoke(this, e);
        }
    }
    public class LuckEatSmallTable : Control
    {
        public Action<LuckEatSmallCell, PaintEventArgs> Action { get; private set; }
        LuckEatSmallData[] Datas;
        List<LuckEatSmallCell> cells = new List<LuckEatSmallCell>();
        public Dictionary<byte, LuckEatSmallCell> Cells => cells.ToDictionary(m => m.Data.Position);
        #region Constructor Region

        public LuckEatSmallTable(RoomView roomView, LuckEatSmallData[] datas, Action<LuckEatSmallCell, PaintEventArgs> action, Action<byte, bool> betaction, Action<byte> morebetaction)
        {
            this.Datas = datas;
            this.Action = action;
            foreach (var d in Datas)
            {
                var cell = new LuckEatSmallCell(this, roomView, d, betaction, morebetaction);
                //cell.Width = w;
                //cell.Height = h;
                //cell.Invalidate();
                this.Controls.Add(cell);
                cells.Add(cell);
            }
            this.SizeChanged += SangongTable_SizeChanged;
        }

        private void SangongTable_SizeChanged(object sender, System.EventArgs e)
        {
            var w = ClientSize.Width / 3;
            var h = ClientSize.Height / 3;
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                var row = i / 3;
                var ren = i % 3;
                cell.Width = w;
                cell.Height = h;
                cell.Top = h * row;
                cell.Left = w * ren;
                cell.Invalidate();
            }
        }

        #endregion

        #region Paint Region

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    var g = e.Graphics;
        //    var w = ClientSize.Width / 3;
        //    var h = ClientSize.Height / 3;
        //    using (var p = new Pen(Colors.GreyHighlight))
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            var p1 = new PointF(0, h * i);
        //            var p2 = new PointF(ClientSize.Width, h * i);
        //            g.DrawLine(p, p1, p2);
        //        }
        //        for (int i = 0; i < 4; i++)
        //        {
        //            var p1 = new PointF(w * i, 0);
        //            var p2 = new PointF(w * i, ClientSize.Height);
        //            g.DrawLine(p, p1, p2);
        //        }
        //    }
        //    //foreach (var d in Datas)
        //    //{
        //    //    var cell = new SangongCell(d);
        //    //    cell.Width = w;
        //    //    cell.Height = h;
        //    //    cell.Invalidate();
        //    //}
        //}


        #endregion
    }
}
