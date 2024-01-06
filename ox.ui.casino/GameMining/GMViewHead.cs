using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using OX.Wallets.UI.Config;
using OX.Wallets.UI.Controls;
using OX.Wallets;
using System.Globalization;

namespace OX.UI.GameMining
{
    public class GMViewHead : Control
    {
        GM GMView;
        Action<PaintEventArgs> Action;
        public Panel Panel { get; private set; }
        public DarkButton pre1Btn { get; private set; }
        public DarkLabel betIndexLab { get; private set; }
        public DarkButton nxt1Btn { get; private set; }

        public DarkButton recentBtn { get; private set; }
        public GMViewHead(GM roomview, Action<PaintEventArgs> action)
        {
            this.GMView = roomview;
            this.Action = action;
            this.SuspendLayout();

            this.pre1Btn = new DarkButton();
            this.pre1Btn.Text = "<";
            this.pre1Btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.pre1Btn.Width = 60;
            this.pre1Btn.Height = 30;
            this.pre1Btn.Top = 6;
            this.pre1Btn.Left = 430;
            this.pre1Btn.Click += Pre1Btn_Click;
            this.Controls.Add(this.pre1Btn);

            this.betIndexLab = new DarkLabel();
            this.betIndexLab.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.betIndexLab.Top = 6;
            this.betIndexLab.Left = 530;
            this.Controls.Add(this.betIndexLab);

            this.nxt1Btn = new DarkButton();
            this.nxt1Btn.Text = ">";
            this.nxt1Btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.nxt1Btn.Width = 60;
            this.nxt1Btn.Height = 30;
            this.nxt1Btn.Top = 6;
            this.nxt1Btn.Left = 720;
            this.nxt1Btn.Click += Nxt1Btn_Click;
            this.Controls.Add(this.nxt1Btn);

            this.recentBtn = new DarkButton();
            this.recentBtn.Text = ">|<";
            this.recentBtn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.recentBtn.Width = 60;
            this.recentBtn.Height = 30;
            this.recentBtn.Top = 6;
            this.recentBtn.Left = 820;
            this.recentBtn.Click += RecentBtn_Click;
            this.Controls.Add(this.recentBtn);


            if (casino.BizAddresses.Keys.FirstOrDefault(m => this.GMView.Operater.Wallet.ContainsAndHeld(m)).IsNotNull())
            {
                var adminButton = new DarkButton();
                adminButton.Text = UIHelper.LocalString("发布任务", "Issue Task");
                adminButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                adminButton.Width = 150;
                adminButton.Height = 30;
                adminButton.Top = 6;
                adminButton.Left = this.ClientSize.Width - 180;
                adminButton.Click += AdminButton_Click;
                this.Controls.Add(adminButton);
            }

            this.Panel = new FlowLayoutPanel();
            //this.Panel.BackColor = Color.Yellow;
            this.Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
           | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel.Location = new System.Drawing.Point(260, 47);
            this.Panel.Left = 410;
            this.Panel.Top = 47;
            this.Panel.Size = new System.Drawing.Size(ClientSize.Width - 265, ClientSize.Height - 50);
            this.Panel.Width = ClientSize.Width - 420;
            this.Panel.Height = ClientSize.Height - 50;
            this.Controls.Add(this.Panel);
            this.Panel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void AdminButton_Click(object sender, EventArgs e)
        {
            new NewGameMiningTasks(this.GMView.Operater).ShowDialog();
        }

        private void Nxt1Btn_Click(object sender, EventArgs e)
        {
            this.GMView.GoNext();
        }

        private void Pre1Btn_Click(object sender, EventArgs e)
        {
            this.GMView.GoPrev();
        }

        private void RecentBtn_Click(object sender, EventArgs e)
        {
            this.GMView.GoRecent();
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = new Rectangle(3, 3, 400, 244);
            using (var p = new Pen(Colors.GreyHighlight))
            {
                g.DrawRectangle(p, rect);
            }
            this.Action?.Invoke(e);
        }
    }
}
