using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
namespace OX.UI.Casino
{
    partial class RiddlesView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_pre = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_next = new OX.Wallets.UI.Controls.DarkButton();
            this.lb_index = new OX.Wallets.UI.Controls.DarkLabel();
            this.cb_auto = new OX.Wallets.UI.Controls.DarkCheckBox();
            this.RoundPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.bt_pre10 = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_pre100 = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_next10 = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_next100 = new OX.Wallets.UI.Controls.DarkButton();
            this.cb_gamekind = new OX.Wallets.UI.Controls.DarkComboBox();
            this.SuspendLayout();
            // 
            // bt_pre
            // 
            this.bt_pre.Location = new System.Drawing.Point(278, 23);
            this.bt_pre.Name = "bt_pre";
            this.bt_pre.Padding = new System.Windows.Forms.Padding(5);
            this.bt_pre.Size = new System.Drawing.Size(130, 27);
            this.bt_pre.SpecialBorderColor = null;
            this.bt_pre.SpecialFillColor = null;
            this.bt_pre.SpecialTextColor = null;
            this.bt_pre.TabIndex = 9;
            this.bt_pre.Text = "button1";
            this.bt_pre.Click += new System.EventHandler(this.bt_pre_Click);
            // 
            // bt_next
            // 
            this.bt_next.Location = new System.Drawing.Point(548, 23);
            this.bt_next.Name = "bt_next";
            this.bt_next.Padding = new System.Windows.Forms.Padding(5);
            this.bt_next.Size = new System.Drawing.Size(130, 27);
            this.bt_next.SpecialBorderColor = null;
            this.bt_next.SpecialFillColor = null;
            this.bt_next.SpecialTextColor = null;
            this.bt_next.TabIndex = 9;
            this.bt_next.Text = "button1";
            this.bt_next.Click += new System.EventHandler(this.bt_next_Click);
            // 
            // lb_index
            // 
            this.lb_index.AutoSize = true;
            this.lb_index.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_index.Location = new System.Drawing.Point(425, 24);
            this.lb_index.Name = "lb_index";
            this.lb_index.Size = new System.Drawing.Size(22, 25);
            this.lb_index.TabIndex = 8;
            this.lb_index.Text = "0";
            // 
            // cb_auto
            // 
            this.cb_auto.AutoSize = true;
            this.cb_auto.Checked = true;
            this.cb_auto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_auto.Location = new System.Drawing.Point(957, 23);
            this.cb_auto.Name = "cb_auto";
            this.cb_auto.Size = new System.Drawing.Size(118, 29);
            this.cb_auto.SpecialBorderColor = null;
            this.cb_auto.SpecialFillColor = null;
            this.cb_auto.SpecialTextColor = null;
            this.cb_auto.TabIndex = 13;
            this.cb_auto.Text = "auto fresh";
            this.cb_auto.CheckedChanged += new System.EventHandler(this.cb_auto_CheckedChanged);
            // 
            // RoundPanel
            // 
            this.RoundPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RoundPanel.AutoScroll = true;
            this.RoundPanel.Location = new System.Drawing.Point(0, 71);
            this.RoundPanel.Name = "RoundPanel";
            this.RoundPanel.Size = new System.Drawing.Size(1452, 472);
            this.RoundPanel.TabIndex = 0;
            // 
            // bt_pre10
            // 
            this.bt_pre10.Location = new System.Drawing.Point(143, 23);
            this.bt_pre10.Name = "bt_pre10";
            this.bt_pre10.Padding = new System.Windows.Forms.Padding(5);
            this.bt_pre10.Size = new System.Drawing.Size(130, 27);
            this.bt_pre10.SpecialBorderColor = null;
            this.bt_pre10.SpecialFillColor = null;
            this.bt_pre10.SpecialTextColor = null;
            this.bt_pre10.TabIndex = 9;
            this.bt_pre10.Text = "button1";
            this.bt_pre10.Click += new System.EventHandler(this.bt_pre10_Click);
            // 
            // bt_pre100
            // 
            this.bt_pre100.Location = new System.Drawing.Point(8, 23);
            this.bt_pre100.Name = "bt_pre100";
            this.bt_pre100.Padding = new System.Windows.Forms.Padding(5);
            this.bt_pre100.Size = new System.Drawing.Size(130, 27);
            this.bt_pre100.SpecialBorderColor = null;
            this.bt_pre100.SpecialFillColor = null;
            this.bt_pre100.SpecialTextColor = null;
            this.bt_pre100.TabIndex = 9;
            this.bt_pre100.Text = "button1";
            this.bt_pre100.Click += new System.EventHandler(this.bt_pre100_Click);
            // 
            // bt_next10
            // 
            this.bt_next10.Location = new System.Drawing.Point(683, 23);
            this.bt_next10.Name = "bt_next10";
            this.bt_next10.Padding = new System.Windows.Forms.Padding(5);
            this.bt_next10.Size = new System.Drawing.Size(130, 27);
            this.bt_next10.SpecialBorderColor = null;
            this.bt_next10.SpecialFillColor = null;
            this.bt_next10.SpecialTextColor = null;
            this.bt_next10.TabIndex = 9;
            this.bt_next10.Text = "button1";
            this.bt_next10.Click += new System.EventHandler(this.bt_next10_Click);
            // 
            // bt_next100
            // 
            this.bt_next100.Location = new System.Drawing.Point(818, 23);
            this.bt_next100.Name = "bt_next100";
            this.bt_next100.Padding = new System.Windows.Forms.Padding(5);
            this.bt_next100.Size = new System.Drawing.Size(130, 27);
            this.bt_next100.SpecialBorderColor = null;
            this.bt_next100.SpecialFillColor = null;
            this.bt_next100.SpecialTextColor = null;
            this.bt_next100.TabIndex = 9;
            this.bt_next100.Text = "button1";
            this.bt_next100.Click += new System.EventHandler(this.bt_next100_Click);
            // 
            // cb_gamekind
            // 
            this.cb_gamekind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_gamekind.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cb_gamekind.Location = new System.Drawing.Point(1274, 21);
            this.cb_gamekind.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cb_gamekind.Name = "cb_gamekind";
            this.cb_gamekind.Size = new System.Drawing.Size(151, 32);
            this.cb_gamekind.SpecialBorderColor = null;
            this.cb_gamekind.SpecialFillColor = null;
            this.cb_gamekind.SpecialTextColor = null;
            this.cb_gamekind.TabIndex = 9;
            this.cb_gamekind.SelectedIndexChanged += new System.EventHandler(this.cb_gamekind_SelectedIndexChanged);
            // 
            // RiddlesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cb_gamekind);
            this.Controls.Add(this.bt_next100);
            this.Controls.Add(this.bt_next10);
            this.Controls.Add(this.bt_pre100);
            this.Controls.Add(this.bt_pre10);
            this.Controls.Add(this.lb_index);
            this.Controls.Add(this.bt_next);
            this.Controls.Add(this.bt_pre);
            this.Controls.Add(this.cb_auto);
            this.Controls.Add(this.RoundPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "RiddlesView";
            this.Size = new System.Drawing.Size(1453, 543);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkButton bt_pre;
        private DarkButton bt_next;
        private DarkLabel lb_index;
        private DarkCheckBox cb_auto;
        protected System.Windows.Forms.FlowLayoutPanel RoundPanel;
        private DarkButton bt_pre10;
        private DarkButton bt_pre100;
        private DarkButton bt_next10;
        private DarkButton bt_next100;
        private DarkComboBox cb_gamekind;
    }
}
