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

    public class LottoTable : Control
    {
        RoomView RoomView;
        public Action<PaintEventArgs> Action { get; private set; }
        public Panel Panel;
        Betting[] Bettings;
        #region Constructor Region

        public LottoTable(RoomView roomView, Action<PaintEventArgs> action, Action<byte> morebetaction)
        {
            this.RoomView = roomView;
            this.Action = action;
            this.SuspendLayout();
            this.Panel = new FlowLayoutPanel();
            //this.Panel.BackColor = Color.Yellow;
            this.Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
           | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel.Location = new System.Drawing.Point(0, 0);
            this.Panel.Left = 0;
            this.Panel.Top = 0;
            this.Panel.Size = new System.Drawing.Size(ClientSize.Width, ClientSize.Height);
            this.Panel.Width = ClientSize.Width;
            this.Panel.Height = ClientSize.Height;
            this.Panel.AutoScroll = true;
            this.Controls.Add(this.Panel);
            this.Panel.SizeChanged += Panel_SizeChanged;
            this.Panel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        public void FillBettings(Betting[] bettings)
        {
            this.Bettings = bettings;
            this.Panel.Controls.Clear();
            var cols = this.Panel.ClientSize.Width / 90;
            var rows = this.Panel.ClientSize.Height / 30;
            var bets = this.Bettings;
            if (this.Bettings.Length > cols * rows)
                bets = this.Bettings.Take(cols * rows).ToArray();
            foreach (var bet in bets.OrderByDescending(m => m.Amount))
            {
                var betbtn = new LottoBettingButton(this.RoomView, bet);
                this.Panel.Controls.Add(betbtn);
                //betbtn.Invalidate();
            }
            this.Invalidate();
            this.Panel.Invalidate();
        }
        private void Panel_SizeChanged(object sender, EventArgs e)
        {
        }


        #endregion

    }
}
