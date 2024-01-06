
namespace OX.UI.Casino
{
    partial class RoomPledgeForm
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
            this.lb_roomId = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_roomId = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_betAddress = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_accounts = new OX.Wallets.UI.Controls.DarkLabel();
            this.cb_accounts = new OX.Wallets.UI.Controls.DarkComboBox();
            this.lb_amount = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_blockexpire = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_betAddress = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_amount = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_balance = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_balance = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_blockexpire = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lv_lockrecords = new OX.Wallets.UI.Controls.DarkListView();
            this.lb_total_lock = new OX.Wallets.UI.Controls.DarkLabel();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(18, 18);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(18, 18);
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(18, 18);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(18, 18);
            // 
            // btnRetry
            // 
            this.btnRetry.Location = new System.Drawing.Point(708, 18);
            // 
            // btnIgnore
            // 
            this.btnIgnore.Location = new System.Drawing.Point(708, 18);
            // 
            // lb_roomId
            // 
            this.lb_roomId.AutoSize = true;
            this.lb_roomId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_roomId.Location = new System.Drawing.Point(30, 24);
            this.lb_roomId.Name = "lb_roomId";
            this.lb_roomId.Size = new System.Drawing.Size(106, 24);
            this.lb_roomId.TabIndex = 2;
            this.lb_roomId.Text = "darkLabel1";
            // 
            // tb_roomId
            // 
            this.tb_roomId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_roomId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_roomId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_roomId.Location = new System.Drawing.Point(232, 22);
            this.tb_roomId.Name = "tb_roomId";
            this.tb_roomId.ReadOnly = true;
            this.tb_roomId.Size = new System.Drawing.Size(254, 30);
            this.tb_roomId.TabIndex = 3;
            // 
            // tb_betAddress
            // 
            this.tb_betAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_betAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_betAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_betAddress.Location = new System.Drawing.Point(232, 72);
            this.tb_betAddress.Name = "tb_betAddress";
            this.tb_betAddress.ReadOnly = true;
            this.tb_betAddress.Size = new System.Drawing.Size(769, 30);
            this.tb_betAddress.TabIndex = 5;
            // 
            // lb_accounts
            // 
            this.lb_accounts.AutoSize = true;
            this.lb_accounts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_accounts.Location = new System.Drawing.Point(30, 128);
            this.lb_accounts.Name = "lb_accounts";
            this.lb_accounts.Size = new System.Drawing.Size(106, 24);
            this.lb_accounts.TabIndex = 6;
            this.lb_accounts.Text = "darkLabel1";
            // 
            // cb_accounts
            // 
            this.cb_accounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cb_accounts.FormattingEnabled = true;
            this.cb_accounts.Location = new System.Drawing.Point(232, 125);
            this.cb_accounts.Name = "cb_accounts";
            this.cb_accounts.Size = new System.Drawing.Size(771, 31);
            this.cb_accounts.SpecialBorderColor = null;
            this.cb_accounts.SpecialFillColor = null;
            this.cb_accounts.SpecialTextColor = null;
            this.cb_accounts.TabIndex = 7;
            this.cb_accounts.SelectedIndexChanged += new System.EventHandler(this.cb_accounts_SelectedIndexChanged);
            // 
            // lb_amount
            // 
            this.lb_amount.AutoSize = true;
            this.lb_amount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_amount.Location = new System.Drawing.Point(30, 188);
            this.lb_amount.Name = "lb_amount";
            this.lb_amount.Size = new System.Drawing.Size(106, 24);
            this.lb_amount.TabIndex = 8;
            this.lb_amount.Text = "darkLabel1";
            // 
            // lb_blockexpire
            // 
            this.lb_blockexpire.AutoSize = true;
            this.lb_blockexpire.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_blockexpire.Location = new System.Drawing.Point(30, 248);
            this.lb_blockexpire.Name = "lb_blockexpire";
            this.lb_blockexpire.Size = new System.Drawing.Size(106, 24);
            this.lb_blockexpire.TabIndex = 10;
            this.lb_blockexpire.Text = "darkLabel1";
            // 
            // lb_betAddress
            // 
            this.lb_betAddress.AutoSize = true;
            this.lb_betAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_betAddress.Location = new System.Drawing.Point(30, 78);
            this.lb_betAddress.Name = "lb_betAddress";
            this.lb_betAddress.Size = new System.Drawing.Size(106, 24);
            this.lb_betAddress.TabIndex = 11;
            this.lb_betAddress.Text = "darkLabel1";
            // 
            // tb_amount
            // 
            this.tb_amount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_amount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_amount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_amount.Location = new System.Drawing.Point(232, 186);
            this.tb_amount.Name = "tb_amount";
            this.tb_amount.Size = new System.Drawing.Size(254, 30);
            this.tb_amount.TabIndex = 12;
            this.tb_amount.Text = "1000";
            this.tb_amount.TextChanged += new System.EventHandler(this.tb_blockexpire_TextChanged);
            // 
            // lb_balance
            // 
            this.lb_balance.AutoSize = true;
            this.lb_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_balance.Location = new System.Drawing.Point(533, 192);
            this.lb_balance.Name = "lb_balance";
            this.lb_balance.Size = new System.Drawing.Size(106, 24);
            this.lb_balance.TabIndex = 13;
            this.lb_balance.Text = "darkLabel1";
            // 
            // tb_balance
            // 
            this.tb_balance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_balance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_balance.Location = new System.Drawing.Point(747, 186);
            this.tb_balance.Name = "tb_balance";
            this.tb_balance.ReadOnly = true;
            this.tb_balance.Size = new System.Drawing.Size(254, 30);
            this.tb_balance.TabIndex = 14;
            // 
            // tb_blockexpire
            // 
            this.tb_blockexpire.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_blockexpire.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_blockexpire.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_blockexpire.Location = new System.Drawing.Point(232, 246);
            this.tb_blockexpire.Name = "tb_blockexpire";
            this.tb_blockexpire.Size = new System.Drawing.Size(254, 30);
            this.tb_blockexpire.TabIndex = 15;
            this.tb_blockexpire.Text = "1001000";
            this.tb_blockexpire.TextChanged += new System.EventHandler(this.tb_blockexpire_TextChanged);
            // 
            // lv_lockrecords
            // 
            this.lv_lockrecords.Location = new System.Drawing.Point(30, 286);
            this.lv_lockrecords.Name = "lv_lockrecords";
            this.lv_lockrecords.Size = new System.Drawing.Size(971, 395);
            this.lv_lockrecords.TabIndex = 16;
            this.lv_lockrecords.Text = "darkListView1";
            // 
            // lb_total_lock
            // 
            this.lb_total_lock.AutoSize = true;
            this.lb_total_lock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_total_lock.Location = new System.Drawing.Point(533, 248);
            this.lb_total_lock.Name = "lb_total_lock";
            this.lb_total_lock.Size = new System.Drawing.Size(106, 24);
            this.lb_total_lock.TabIndex = 17;
            this.lb_total_lock.Text = "darkLabel1";
            // 
            // RoomPledgeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 773);
            this.Controls.Add(this.lb_total_lock);
            this.Controls.Add(this.lv_lockrecords);
            this.Controls.Add(this.tb_blockexpire);
            this.Controls.Add(this.tb_balance);
            this.Controls.Add(this.lb_balance);
            this.Controls.Add(this.tb_amount);
            this.Controls.Add(this.lb_betAddress);
            this.Controls.Add(this.lb_blockexpire);
            this.Controls.Add(this.lb_amount);
            this.Controls.Add(this.cb_accounts);
            this.Controls.Add(this.lb_accounts);
            this.Controls.Add(this.tb_betAddress);
            this.Controls.Add(this.tb_roomId);
            this.Controls.Add(this.lb_roomId);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RoomPledgeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RegMinerForm";
            this.Load += new System.EventHandler(this.RegMinerForm_Load);
            this.Controls.SetChildIndex(this.lb_roomId, 0);
            this.Controls.SetChildIndex(this.tb_roomId, 0);
            this.Controls.SetChildIndex(this.tb_betAddress, 0);
            this.Controls.SetChildIndex(this.lb_accounts, 0);
            this.Controls.SetChildIndex(this.cb_accounts, 0);
            this.Controls.SetChildIndex(this.lb_amount, 0);
            this.Controls.SetChildIndex(this.lb_blockexpire, 0);
            this.Controls.SetChildIndex(this.lb_betAddress, 0);
            this.Controls.SetChildIndex(this.tb_amount, 0);
            this.Controls.SetChildIndex(this.lb_balance, 0);
            this.Controls.SetChildIndex(this.tb_balance, 0);
            this.Controls.SetChildIndex(this.tb_blockexpire, 0);
            this.Controls.SetChildIndex(this.lv_lockrecords, 0);
            this.Controls.SetChildIndex(this.lb_total_lock, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wallets.UI.Controls.DarkLabel lb_roomId;
        private Wallets.UI.Controls.DarkTextBox tb_roomId;
        private Wallets.UI.Controls.DarkTextBox tb_betAddress;
        private Wallets.UI.Controls.DarkLabel lb_accounts;
        private Wallets.UI.Controls.DarkComboBox cb_accounts;
        private Wallets.UI.Controls.DarkLabel lb_amount;
        private Wallets.UI.Controls.DarkLabel lb_blockexpire;
        private Wallets.UI.Controls.DarkLabel lb_betAddress;
        private Wallets.UI.Controls.DarkTextBox tb_amount;
        private Wallets.UI.Controls.DarkLabel lb_balance;
        private Wallets.UI.Controls.DarkTextBox tb_balance;
        private Wallets.UI.Controls.DarkTextBox tb_blockexpire;
        private Wallets.UI.Controls.DarkListView lv_lockrecords;
        private Wallets.UI.Controls.DarkLabel lb_total_lock;
    }
}