using OX.Wallets.UI.Config;

namespace OX.UI.Casino
{
    partial class LuckEatSmallPositionBet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LuckEatSmallPositionBet));
            this.panel = new System.Windows.Forms.Panel();
            this.tb_amount = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_balance = new OX.Wallets.UI.Controls.DarkTextBox();
            this.bt_bet = new OX.Wallets.UI.Controls.DarkButton();
            this.cbAccounts = new OX.Wallets.UI.Controls.DarkComboBox();
            this.lb_from = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_betamt = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_balance = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_Index = new OX.Wallets.UI.Controls.DarkLabel();
            this.cb_Height = new OX.Wallets.UI.Controls.DarkComboBox();
            this.lb_roomId = new OX.Wallets.UI.Controls.DarkLabel();
            this.cb_specialchar = new OX.Wallets.UI.Controls.DarkComboBox();
            this.lb_specialchar = new OX.Wallets.UI.Controls.DarkLabel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.cb_specialchar);
            this.panel.Controls.Add(this.lb_specialchar);
            this.panel.Controls.Add(this.tb_amount);
            this.panel.Controls.Add(this.tb_balance);
            this.panel.Controls.Add(this.bt_bet);
            this.panel.Controls.Add(this.cbAccounts);
            this.panel.Controls.Add(this.lb_from);
            this.panel.Controls.Add(this.lb_betamt);
            this.panel.Controls.Add(this.lb_balance);
            this.panel.Controls.Add(this.lb_Index);
            this.panel.Controls.Add(this.cb_Height);
            this.panel.Controls.Add(this.lb_roomId);
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
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
            // lb_Index
            // 
            resources.ApplyResources(this.lb_Index, "lb_Index");
            this.lb_Index.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_Index.Name = "lb_Index";
            // 
            // cb_Height
            // 
            this.cb_Height.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(this.cb_Height, "cb_Height");
            this.cb_Height.Name = "cb_Height";
            this.cb_Height.SpecialBorderColor = null;
            this.cb_Height.SpecialFillColor = null;
            this.cb_Height.SpecialTextColor = null;
            // 
            // lb_roomId
            // 
            resources.ApplyResources(this.lb_roomId, "lb_roomId");
            this.lb_roomId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_roomId.Name = "lb_roomId";
            // 
            // cb_specialchar
            // 
            this.cb_specialchar.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(this.cb_specialchar, "cb_specialchar");
            this.cb_specialchar.Name = "cb_specialchar";
            this.cb_specialchar.SpecialBorderColor = null;
            this.cb_specialchar.SpecialFillColor = null;
            this.cb_specialchar.SpecialTextColor = null;
            // 
            // lb_specialchar
            // 
            resources.ApplyResources(this.lb_specialchar, "lb_specialchar");
            this.lb_specialchar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_specialchar.Name = "lb_specialchar";
            // 
            // LuckEatSmallPositionBet
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LuckEatSmallPositionBet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClaimForm_FormClosing);
            this.Load += new System.EventHandler(this.ClaimForm_Load);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private OX.Wallets.UI.Controls.DarkLabel lb_roomId;
        private OX.Wallets.UI.Controls.DarkLabel lb_balance;
        private OX.Wallets.UI.Controls.DarkLabel lb_Index;
        private OX.Wallets.UI.Controls.DarkComboBox cb_Height;
        private OX.Wallets.UI.Controls.DarkLabel lb_betamt;
        private OX.Wallets.UI.Controls.DarkComboBox cbAccounts;
        private OX.Wallets.UI.Controls.DarkLabel lb_from;
        private OX.Wallets.UI.Controls.DarkButton bt_bet;
        private OX.Wallets.UI.Controls.DarkTextBox tb_amount;
        private OX.Wallets.UI.Controls.DarkTextBox tb_balance;
        private Wallets.UI.Controls.DarkComboBox cb_specialchar;
        private Wallets.UI.Controls.DarkLabel lb_specialchar;
    }
}