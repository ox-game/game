using OX.Wallets.UI.Config;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OX.Wallets;
using System.Linq;
using System;
using System.IO;
using OX.IO;

namespace OX.UI.GameMining
{
    public class GMBetting : ISerializable
    {
        public byte Position;
        public Fixed8 Amount;
        public UInt160 SH;
        public virtual int Size => sizeof(byte) + Amount.Size + SH.Size;
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Position);
            writer.Write(Amount);
            writer.Write(SH);
        }
        public void Deserialize(BinaryReader reader)
        {
            Position = reader.ReadByte();
            Amount = reader.ReadSerializable<Fixed8>();
            SH = reader.ReadSerializable<UInt160>();
        }
    }
    public class GMSangongData
    {
        public byte Position { get; set; }
    }
    public class GMSangongCell : Control
    {
        GM RoomView;
        GMSangongTable SangongTable;
        public Panel Panel;
        public GMSangongData Data;
        public DarkButton BetButton;
        public DarkButton TrustBetButton;
        public DarkButton MoreButton;
        public GMBetting[] Bettings { get; set; } = new GMBetting[0];
        Action<byte, bool> BetAction;
        Action<byte> MoreBetAction;
        public GMSangongCell(GMSangongTable sangongtable, GM roomView, GMSangongData data, Action<byte, bool> betaction, Action<byte> morebetaction)
        {
            this.RoomView = roomView;
            this.SangongTable = sangongtable;
            this.Data = data;
            this.BetAction = betaction;
            this.MoreBetAction = morebetaction;
            this.SuspendLayout();
            this.BetButton = new DarkButton();
            this.BetButton.Text = UIHelper.LocalString("种矿", "Seed");
            this.BetButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.BetButton.Width = 100;
            this.BetButton.Height = 30;
            this.BetButton.Top = 6;
            this.BetButton.Left = ClientSize.Width - 145;
            this.Controls.Add(this.BetButton);
            this.BetButton.Click += BetButton_Click;


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
                var betbtn = new GMSangongBettingButton(this.RoomView, bet);
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
    public class GMSangongTable : Control
    {
        public Action<GMSangongCell, PaintEventArgs> Action { get; private set; }
        GMSangongData[] Datas;
        List<GMSangongCell> cells = new List<GMSangongCell>();
        public Dictionary<byte, GMSangongCell> Cells => cells.ToDictionary(m => m.Data.Position);
        #region Constructor Region

        public GMSangongTable(GM roomView, GMSangongData[] datas, Action<GMSangongCell, PaintEventArgs> action, Action<byte, bool> betaction, Action<byte> morebetaction)
        {
            this.Datas = datas;
            this.Action = action;
            foreach (var d in Datas)
            {
                var cell = new GMSangongCell(this, roomView, d, betaction, morebetaction);
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
            var w = ClientSize.Width / 5;
            var h = ClientSize.Height / 2;
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                var row = i / 5;
                var ren = i % 5;
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
