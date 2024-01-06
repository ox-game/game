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
using OX.Ledger;

namespace OX.UI.Casino
{
    public class RoomViewHead : Control
    {
        RoomView RoomView;
        Action<PaintEventArgs> Action;
        public Panel Panel { get; private set; }
        public DarkButton pre3Btn { get; private set; }
        public DarkButton pre2Btn { get; private set; }
        public DarkButton pre1Btn { get; private set; }
        public DarkButton nxt3Btn { get; private set; }
        public DarkButton nxt2Btn { get; private set; }
        public DarkButton nxt1Btn { get; private set; }
        public DarkButton exportBtn { get; private set; }
        public DarkCheckBox cb_auto { get; private set; }
        public RoomViewHead(RoomView roomview, Action<PaintEventArgs> action)
        {
            this.RoomView = roomview;
            this.Action = action;
            this.SuspendLayout();
            var assetLabel = new DarkLabel();
            var assetState = Blockchain.Singleton.CurrentSnapshot.Assets.TryGet(roomview.Room.Request.AssetId);
            if (assetState.IsNotNull())
            {
                assetLabel.Text = $"{new EnumItem<RoomPermission>(RoomView.Room.Request.Permission).ToString()}  {assetState.GetName()}    {roomview.Room.Request.AssetId.ToString()}";
                assetLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                assetLabel.Width = 860;
                assetLabel.Height = 30;
                assetLabel.Top = 6;
                assetLabel.Left = 270;
                this.Controls.Add(assetLabel);
            }

            this.pre3Btn = new DarkButton();
            this.pre3Btn.Text = "<<<";
            this.pre3Btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.pre3Btn.Width = 60;
            this.pre3Btn.Height = 30;
            this.pre3Btn.Top = 46;
            this.pre3Btn.Left = 270;
            this.Controls.Add(this.pre3Btn);
            this.pre2Btn = new DarkButton();
            this.pre2Btn.Text = "<<";
            this.pre2Btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.pre2Btn.Width = 60;
            this.pre2Btn.Height = 30;
            this.pre2Btn.Top = 46;
            this.pre2Btn.Left = 350;
            this.Controls.Add(this.pre2Btn);
            this.pre1Btn = new DarkButton();
            this.pre1Btn.Text = "<";
            this.pre1Btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.pre1Btn.Width = 60;
            this.pre1Btn.Height = 30;
            this.pre1Btn.Top = 46;
            this.pre1Btn.Left = 430;
            this.Controls.Add(this.pre1Btn);

            this.nxt1Btn = new DarkButton();
            this.nxt1Btn.Text = ">";
            this.nxt1Btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.nxt1Btn.Width = 60;
            this.nxt1Btn.Height = 30;
            this.nxt1Btn.Top = 46;
            this.nxt1Btn.Left = 720;
            this.Controls.Add(this.nxt1Btn);

            this.exportBtn = new DarkButton();
            this.exportBtn.Text = UIHelper.LocalString("导出状态", "Export State");
            this.exportBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.exportBtn.Width = 150;
            this.exportBtn.Height = 30;
            this.exportBtn.Top = 6;
            this.exportBtn.Left = this.ClientSize.Width - 300;
            this.Controls.Add(this.exportBtn);

            this.nxt2Btn = new DarkButton();
            this.nxt2Btn.Text = ">>";
            this.nxt2Btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.nxt2Btn.Width = 60;
            this.nxt2Btn.Height = 30;
            this.nxt2Btn.Top = 46;
            this.nxt2Btn.Left = 800;
            this.Controls.Add(this.nxt2Btn);
            this.nxt3Btn = new DarkButton();
            this.nxt3Btn.Text = ">>>";
            this.nxt3Btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.nxt3Btn.Width = 60;
            this.nxt3Btn.Height = 30;
            this.nxt3Btn.Top = 46;
            this.nxt3Btn.Left = 880;
            this.Controls.Add(this.nxt3Btn);
            this.cb_auto = new DarkCheckBox();
            this.cb_auto.AutoSize = true;
            this.cb_auto.Checked = true;
            this.cb_auto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_auto.Top = 46;
            this.cb_auto.Left = 960;
            this.cb_auto.Name = "cb_auto";
            this.cb_auto.Size = new System.Drawing.Size(118, 29);
            this.cb_auto.SpecialBorderColor = null;
            this.cb_auto.SpecialFillColor = null;
            this.cb_auto.SpecialTextColor = null;
            this.cb_auto.TabIndex = 13;
            this.cb_auto.Text = UIHelper.LocalString("自动定位到最近", "Auto Focus Current");
            this.Controls.Add(this.cb_auto);
            this.cb_auto.CheckedChanged += Cb_auto_CheckedChanged;

            this.Panel = new FlowLayoutPanel();
            //this.Panel.BackColor = Color.Yellow;
            this.Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
           | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel.Location = new System.Drawing.Point(260, 47);
            this.Panel.Left = 260;
            this.Panel.Top = 87;
            this.Panel.Size = new System.Drawing.Size(ClientSize.Width - 265, ClientSize.Height - 50);
            this.Panel.Width = ClientSize.Width - 265;
            this.Panel.Height = ClientSize.Height - 50;
            this.Controls.Add(this.Panel);
            this.Panel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void Cb_auto_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is DarkCheckBox cb)
            {
                if (cb.Checked)
                {
                    this.RoomView.ResetAnShowIndex();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = new Rectangle(3, 3, 244, 244);
            using (var p = new Pen(Colors.GreyHighlight))
            {
                g.DrawRectangle(p, rect);
            }
            this.Action?.Invoke(e);
        }
    }
}
