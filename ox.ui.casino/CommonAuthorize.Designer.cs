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
            lb_from = new Wallets.UI.Controls.DarkLabel();
            cbAccounts = new Wallets.UI.Controls.DarkComboBox();
            bt_bet = new Wallets.UI.Controls.DarkButton();
            tb_address = new Wallets.UI.Controls.DarkTextBox();
            tb_markproof = new Wallets.UI.Controls.DarkTextBox();
            lb_markproof = new Wallets.UI.Controls.DarkLabel();
            panel = new System.Windows.Forms.Panel();
            lb_balance = new Wallets.UI.Controls.DarkLabel();
            panel.SuspendLayout();
            SuspendLayout();
            // 
            // lb_from
            // 
            resources.ApplyResources(lb_from, "lb_from");
            lb_from.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_from.Name = "lb_from";
            // 
            // cbAccounts
            // 
            cbAccounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(cbAccounts, "cbAccounts");
            cbAccounts.Name = "cbAccounts";
            cbAccounts.SpecialBorderColor = null;
            cbAccounts.SpecialFillColor = null;
            cbAccounts.SpecialTextColor = null;
            cbAccounts.SelectedIndexChanged += darkComboBox1_SelectedIndexChanged;
            // 
            // bt_bet
            // 
            resources.ApplyResources(bt_bet, "bt_bet");
            bt_bet.Name = "bt_bet";
            bt_bet.SpecialBorderColor = null;
            bt_bet.SpecialFillColor = null;
            bt_bet.SpecialTextColor = null;
            bt_bet.Click += bt_NewRoom_Click;
            // 
            // tb_address
            // 
            tb_address.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_address.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_address.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_address, "tb_address");
            tb_address.Name = "tb_address";
            tb_address.TextChanged += tb_amount_TextChanged;
            // 
            // tb_markproof
            // 
            tb_markproof.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_markproof.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_markproof.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_markproof, "tb_markproof");
            tb_markproof.Name = "tb_markproof";
            // 
            // lb_markproof
            // 
            resources.ApplyResources(lb_markproof, "lb_markproof");
            lb_markproof.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_markproof.Name = "lb_markproof";
            // 
            // panel
            // 
            panel.Controls.Add(lb_markproof);
            panel.Controls.Add(tb_markproof);
            panel.Controls.Add(tb_address);
            panel.Controls.Add(bt_bet);
            panel.Controls.Add(cbAccounts);
            panel.Controls.Add(lb_from);
            panel.Controls.Add(lb_balance);
            resources.ApplyResources(panel, "panel");
            panel.Name = "panel";
            // 
            // lb_balance
            // 
            resources.ApplyResources(lb_balance, "lb_balance");
            lb_balance.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_balance.Name = "lb_balance";
            // 
            // CommonAuthorize
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CommonAuthorize";
            FormClosing += ClaimForm_FormClosing;
            Load += ClaimForm_Load;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ResumeLayout(false);
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