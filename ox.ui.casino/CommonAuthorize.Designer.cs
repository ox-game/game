namespace OX.UI.Casino
{
    partial class CommonAuthorize
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonAuthorize));
            this.lb_from = new OX.Wallets.UI.Controls.DarkLabel();
            this.cbAccounts = new OX.Wallets.UI.Controls.DarkComboBox();
            this.bt_bet = new OX.Wallets.UI.Controls.DarkButton();
            this.tb_address = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_markproof = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_markproof = new OX.Wallets.UI.Controls.DarkLabel();
            this.panel = new System.Windows.Forms.Panel();
            this.lb_balance = new OX.Wallets.UI.Controls.DarkLabel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_from
            // 
            resources.ApplyResources(this.lb_from, "lb_from");
            this.lb_from.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_from.Name = "lb_from";
            // 
            // cbAccounts
            // 
            this.cbAccounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(this.cbAccounts, "cbAccounts");
            this.cbAccounts.Name = "cbAccounts";
            this.cbAccounts.SpecialBorderColor = null;
            this.cbAccounts.SpecialFillColor = null;
            this.cbAccounts.SpecialTextColor = null;
            this.cbAccounts.SelectedIndexChanged += new System.EventHandler(this.darkComboBox1_SelectedIndexChanged);
            // 
            // bt_bet
            // 
            resources.ApplyResources(this.bt_bet, "bt_bet");
            this.bt_bet.Name = "bt_bet";
            this.bt_bet.SpecialBorderColor = null;
            this.bt_bet.SpecialFillColor = null;
            this.bt_bet.SpecialTextColor = null;
            this.bt_bet.Click += new System.EventHandler(this.bt_NewRoom_Click);
            // 
            // tb_address
            // 
            this.tb_address.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_address.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_address.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_address, "tb_address");
            this.tb_address.Name = "tb_address";
            this.tb_address.TextChanged += new System.EventHandler(this.tb_amount_TextChanged);
            // 
            // tb_markproof
            // 
            this.tb_markproof.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_markproof.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_markproof.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_markproof, "tb_markproof");
            this.tb_markproof.Name = "tb_markproof";
            // 
            // lb_markproof
            // 
            resources.ApplyResources(this.lb_markproof, "lb_markproof");
            this.lb_markproof.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_markproof.Name = "lb_markproof";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.lb_markproof);
            this.panel.Controls.Add(this.tb_markproof);
            this.panel.Controls.Add(this.tb_address);
            this.panel.Controls.Add(this.bt_bet);
            this.panel.Controls.Add(this.cbAccounts);
            this.panel.Controls.Add(this.lb_from);
            this.panel.Controls.Add(this.lb_balance);
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
            // 
            // lb_balance
            // 
            resources.ApplyResources(this.lb_balance, "lb_balance");
            this.lb_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_balance.Name = "lb_balance";
            // 
            // CommonAuthorize
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommonAuthorize";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClaimForm_FormClosing);
            this.Load += new System.EventHandler(this.ClaimForm_Load);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Wallets.UI.Controls.DarkLabel lb_from;
        private Wallets.UI.Controls.DarkComboBox cbAccounts;
        private Wallets.UI.Controls.DarkButton bt_bet;
        private Wallets.UI.Controls.DarkTextBox tb_address;
        private Wallets.UI.Controls.DarkTextBox tb_markproof;
        private Wallets.UI.Controls.DarkLabel lb_markproof;
        private System.Windows.Forms.Panel panel;
        private Wallets.UI.Controls.DarkLabel lb_balance;
    }
}