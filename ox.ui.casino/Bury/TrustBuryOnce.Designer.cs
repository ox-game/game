using OX.Wallets.UI.Config;

namespace OX.UI.Bury
{
    partial class TrustBuryOnce
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrustBuryOnce));
            this.panel = new System.Windows.Forms.Panel();
            this.tb_CipherCode = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_PlainCode = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_amount = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_balance = new OX.Wallets.UI.Controls.DarkTextBox();
            this.bt_bet = new OX.Wallets.UI.Controls.DarkButton();
            this.cbAccounts = new OX.Wallets.UI.Controls.DarkComboBox();
            this.lb_from = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_betamt = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_balance = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_cipherCode = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_plainCode = new OX.Wallets.UI.Controls.DarkLabel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.tb_CipherCode);
            this.panel.Controls.Add(this.tb_PlainCode);
            this.panel.Controls.Add(this.tb_amount);
            this.panel.Controls.Add(this.tb_balance);
            this.panel.Controls.Add(this.bt_bet);
            this.panel.Controls.Add(this.cbAccounts);
            this.panel.Controls.Add(this.lb_from);
            this.panel.Controls.Add(this.lb_betamt);
            this.panel.Controls.Add(this.lb_balance);
            this.panel.Controls.Add(this.lb_cipherCode);
            this.panel.Controls.Add(this.lb_plainCode);
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
            // 
            // tb_CipherCode
            // 
            this.tb_CipherCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_CipherCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_CipherCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_CipherCode, "tb_CipherCode");
            this.tb_CipherCode.Name = "tb_CipherCode";
            this.tb_CipherCode.TextChanged += new System.EventHandler(this.tb_CipherCode_TextChanged);
            // 
            // tb_PlainCode
            // 
            this.tb_PlainCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_PlainCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_PlainCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_PlainCode, "tb_PlainCode");
            this.tb_PlainCode.Name = "tb_PlainCode";
            this.tb_PlainCode.TextChanged += new System.EventHandler(this.tb_PlainCode_TextChanged);
            // 
            // tb_amount
            // 
            this.tb_amount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_amount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_amount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_amount, "tb_amount");
            this.tb_amount.Name = "tb_amount";
            this.tb_amount.TextChanged += new System.EventHandler(this.tb_amount_TextChanged);
            // 
            // tb_balance
            // 
            this.tb_balance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_balance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_balance, "tb_balance");
            this.tb_balance.Name = "tb_balance";
            this.tb_balance.ReadOnly = true;
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
            // lb_from
            // 
            resources.ApplyResources(this.lb_from, "lb_from");
            this.lb_from.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_from.Name = "lb_from";
            // 
            // lb_betamt
            // 
            resources.ApplyResources(this.lb_betamt, "lb_betamt");
            this.lb_betamt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_betamt.Name = "lb_betamt";
            // 
            // lb_balance
            // 
            resources.ApplyResources(this.lb_balance, "lb_balance");
            this.lb_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_balance.Name = "lb_balance";
            // 
            // lb_cipherCode
            // 
            resources.ApplyResources(this.lb_cipherCode, "lb_cipherCode");
            this.lb_cipherCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_cipherCode.Name = "lb_cipherCode";
            // 
            // lb_plainCode
            // 
            resources.ApplyResources(this.lb_plainCode, "lb_plainCode");
            this.lb_plainCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_plainCode.Name = "lb_plainCode";
            // 
            // BuryOnce
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BuryOnce";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClaimForm_FormClosing);
            this.Load += new System.EventHandler(this.ClaimForm_Load);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private OX.Wallets.UI.Controls.DarkLabel lb_plainCode;
        private OX.Wallets.UI.Controls.DarkLabel lb_balance;
        private OX.Wallets.UI.Controls.DarkLabel lb_cipherCode;
        private OX.Wallets.UI.Controls.DarkLabel lb_betamt;
        private OX.Wallets.UI.Controls.DarkComboBox cbAccounts;
        private OX.Wallets.UI.Controls.DarkLabel lb_from;
        private OX.Wallets.UI.Controls.DarkButton bt_bet;
        private OX.Wallets.UI.Controls.DarkTextBox tb_amount;
        private OX.Wallets.UI.Controls.DarkTextBox tb_balance;
        private Wallets.UI.Controls.DarkTextBox tb_CipherCode;
        private Wallets.UI.Controls.DarkTextBox tb_PlainCode;
    }
}