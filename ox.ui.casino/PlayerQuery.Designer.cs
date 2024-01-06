
namespace OX.UI.Casino
{
    partial class PlayerQuery
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_query = new OX.Wallets.UI.Controls.DarkButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tb_address = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_address = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_roomId = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_roomid = new OX.Wallets.UI.Controls.DarkLabel();
            this.bt_close = new OX.Wallets.UI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // bt_query
            // 
            this.bt_query.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_query.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bt_query.Location = new System.Drawing.Point(893, 794);
            this.bt_query.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.bt_query.Name = "bt_query";
            this.bt_query.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.bt_query.Size = new System.Drawing.Size(251, 42);
            this.bt_query.SpecialBorderColor = null;
            this.bt_query.SpecialFillColor = null;
            this.bt_query.SpecialTextColor = null;
            this.bt_query.TabIndex = 8;
            this.bt_query.Text = "Claim All";
            this.bt_query.Click += new System.EventHandler(this.bt_query_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 98);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1403, 669);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // tb_address
            // 
            this.tb_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_address.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_address.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_address.Location = new System.Drawing.Point(189, 34);
            this.tb_address.Margin = new System.Windows.Forms.Padding(6);
            this.tb_address.Name = "tb_address";
            this.tb_address.Size = new System.Drawing.Size(552, 30);
            this.tb_address.TabIndex = 11;
            // 
            // lb_address
            // 
            this.lb_address.AutoSize = true;
            this.lb_address.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_address.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_address.Location = new System.Drawing.Point(31, 36);
            this.lb_address.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_address.Name = "lb_address";
            this.lb_address.Size = new System.Drawing.Size(86, 24);
            this.lb_address.TabIndex = 10;
            this.lb_address.Text = "Claim to:";
            // 
            // tb_roomId
            // 
            this.tb_roomId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_roomId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_roomId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_roomId.Location = new System.Drawing.Point(1135, 34);
            this.tb_roomId.Margin = new System.Windows.Forms.Padding(6);
            this.tb_roomId.Name = "tb_roomId";
            this.tb_roomId.Size = new System.Drawing.Size(256, 30);
            this.tb_roomId.TabIndex = 13;
            // 
            // lb_roomid
            // 
            this.lb_roomid.AutoSize = true;
            this.lb_roomid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_roomid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_roomid.Location = new System.Drawing.Point(977, 36);
            this.lb_roomid.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb_roomid.Name = "lb_roomid";
            this.lb_roomid.Size = new System.Drawing.Size(86, 24);
            this.lb_roomid.TabIndex = 12;
            this.lb_roomid.Text = "Claim to:";
            // 
            // bt_close
            // 
            this.bt_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_close.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bt_close.Location = new System.Drawing.Point(1264, 794);
            this.bt_close.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.bt_close.Name = "bt_close";
            this.bt_close.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.bt_close.Size = new System.Drawing.Size(151, 42);
            this.bt_close.SpecialBorderColor = null;
            this.bt_close.SpecialFillColor = null;
            this.bt_close.SpecialTextColor = null;
            this.bt_close.TabIndex = 14;
            this.bt_close.Text = "Claim All";
            this.bt_close.Click += new System.EventHandler(this.bt_close_Click);
            // 
            // PlayerQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1435, 871);
            this.Controls.Add(this.bt_close);
            this.Controls.Add(this.tb_roomId);
            this.Controls.Add(this.lb_roomid);
            this.Controls.Add(this.tb_address);
            this.Controls.Add(this.lb_address);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.bt_query);
            this.Name = "PlayerQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PlayerQuery";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wallets.UI.Controls.DarkButton bt_query;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Wallets.UI.Controls.DarkTextBox tb_address;
        private Wallets.UI.Controls.DarkLabel lb_address;
        private Wallets.UI.Controls.DarkTextBox tb_roomId;
        private Wallets.UI.Controls.DarkLabel lb_roomid;
        private Wallets.UI.Controls.DarkButton bt_close;
    }
}