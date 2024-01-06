
namespace OX.UI.Casino
{
    partial class ViewCasinoTrustPool
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
            this.lb_truster = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_trustee = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_balance_OXC = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_scope = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_balance_Bit = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_truster_pub = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_truster_addr = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_trustee_addr = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_trustee_pub = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_scope = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_balance_oxc = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_balance_bit = new OX.Wallets.UI.Controls.DarkTextBox();
            this.tb_trustaddr = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_trustaddr = new OX.Wallets.UI.Controls.DarkLabel();
            this.bt_query = new OX.Wallets.UI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
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
            // lb_truster
            // 
            this.lb_truster.AutoSize = true;
            this.lb_truster.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_truster.Location = new System.Drawing.Point(37, 30);
            this.lb_truster.Name = "lb_truster";
            this.lb_truster.Size = new System.Drawing.Size(106, 24);
            this.lb_truster.TabIndex = 2;
            this.lb_truster.Text = "darkLabel1";
            // 
            // lb_trustee
            // 
            this.lb_trustee.AutoSize = true;
            this.lb_trustee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_trustee.Location = new System.Drawing.Point(37, 125);
            this.lb_trustee.Name = "lb_trustee";
            this.lb_trustee.Size = new System.Drawing.Size(106, 24);
            this.lb_trustee.TabIndex = 3;
            this.lb_trustee.Text = "darkLabel1";
            // 
            // lb_balance_OXC
            // 
            this.lb_balance_OXC.AutoSize = true;
            this.lb_balance_OXC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_balance_OXC.Location = new System.Drawing.Point(33, 442);
            this.lb_balance_OXC.Name = "lb_balance_OXC";
            this.lb_balance_OXC.Size = new System.Drawing.Size(106, 24);
            this.lb_balance_OXC.TabIndex = 5;
            this.lb_balance_OXC.Text = "darkLabel2";
            // 
            // lb_scope
            // 
            this.lb_scope.AutoSize = true;
            this.lb_scope.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_scope.Location = new System.Drawing.Point(37, 215);
            this.lb_scope.Name = "lb_scope";
            this.lb_scope.Size = new System.Drawing.Size(106, 24);
            this.lb_scope.TabIndex = 4;
            this.lb_scope.Text = "darkLabel1";
            // 
            // lb_balance_Bit
            // 
            this.lb_balance_Bit.AutoSize = true;
            this.lb_balance_Bit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_balance_Bit.Location = new System.Drawing.Point(33, 492);
            this.lb_balance_Bit.Name = "lb_balance_Bit";
            this.lb_balance_Bit.Size = new System.Drawing.Size(106, 24);
            this.lb_balance_Bit.TabIndex = 7;
            this.lb_balance_Bit.Text = "darkLabel2";
            // 
            // tb_truster_pub
            // 
            this.tb_truster_pub.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_truster_pub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_truster_pub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_truster_pub.Location = new System.Drawing.Point(170, 28);
            this.tb_truster_pub.Name = "tb_truster_pub";
            this.tb_truster_pub.ReadOnly = true;
            this.tb_truster_pub.Size = new System.Drawing.Size(738, 30);
            this.tb_truster_pub.TabIndex = 8;
            this.tb_truster_pub.TextChanged += new System.EventHandler(this.tb_truster_pub_TextChanged);
            // 
            // lb_truster_addr
            // 
            this.lb_truster_addr.AutoSize = true;
            this.lb_truster_addr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_truster_addr.Location = new System.Drawing.Point(170, 63);
            this.lb_truster_addr.Name = "lb_truster_addr";
            this.lb_truster_addr.Size = new System.Drawing.Size(106, 24);
            this.lb_truster_addr.TabIndex = 9;
            this.lb_truster_addr.Text = "darkLabel1";
            // 
            // lb_trustee_addr
            // 
            this.lb_trustee_addr.AutoSize = true;
            this.lb_trustee_addr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_trustee_addr.Location = new System.Drawing.Point(170, 153);
            this.lb_trustee_addr.Name = "lb_trustee_addr";
            this.lb_trustee_addr.Size = new System.Drawing.Size(106, 24);
            this.lb_trustee_addr.TabIndex = 11;
            this.lb_trustee_addr.Text = "darkLabel1";
            // 
            // tb_trustee_pub
            // 
            this.tb_trustee_pub.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_trustee_pub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_trustee_pub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_trustee_pub.Location = new System.Drawing.Point(170, 119);
            this.tb_trustee_pub.Name = "tb_trustee_pub";
            this.tb_trustee_pub.ReadOnly = true;
            this.tb_trustee_pub.Size = new System.Drawing.Size(738, 30);
            this.tb_trustee_pub.TabIndex = 10;
            this.tb_trustee_pub.TextChanged += new System.EventHandler(this.tb_trustee_pub_TextChanged);
            // 
            // tb_scope
            // 
            this.tb_scope.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_scope.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_scope.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_scope.Location = new System.Drawing.Point(170, 213);
            this.tb_scope.Name = "tb_scope";
            this.tb_scope.ReadOnly = true;
            this.tb_scope.Size = new System.Drawing.Size(738, 30);
            this.tb_scope.TabIndex = 12;
            this.tb_scope.TextChanged += new System.EventHandler(this.tb_scope_TextChanged);
            // 
            // tb_balance_oxc
            // 
            this.tb_balance_oxc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_balance_oxc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_balance_oxc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_balance_oxc.Location = new System.Drawing.Point(170, 436);
            this.tb_balance_oxc.Name = "tb_balance_oxc";
            this.tb_balance_oxc.ReadOnly = true;
            this.tb_balance_oxc.Size = new System.Drawing.Size(738, 30);
            this.tb_balance_oxc.TabIndex = 13;
            this.tb_balance_oxc.Text = "0";
            // 
            // tb_balance_bit
            // 
            this.tb_balance_bit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_balance_bit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_balance_bit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_balance_bit.Location = new System.Drawing.Point(170, 486);
            this.tb_balance_bit.Name = "tb_balance_bit";
            this.tb_balance_bit.ReadOnly = true;
            this.tb_balance_bit.Size = new System.Drawing.Size(738, 30);
            this.tb_balance_bit.TabIndex = 14;
            this.tb_balance_bit.Text = "0";
            // 
            // tb_trustaddr
            // 
            this.tb_trustaddr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_trustaddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_trustaddr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tb_trustaddr.Location = new System.Drawing.Point(170, 383);
            this.tb_trustaddr.Name = "tb_trustaddr";
            this.tb_trustaddr.ReadOnly = true;
            this.tb_trustaddr.Size = new System.Drawing.Size(738, 30);
            this.tb_trustaddr.TabIndex = 16;
            // 
            // lb_trustaddr
            // 
            this.lb_trustaddr.AutoSize = true;
            this.lb_trustaddr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_trustaddr.Location = new System.Drawing.Point(33, 389);
            this.lb_trustaddr.Name = "lb_trustaddr";
            this.lb_trustaddr.Size = new System.Drawing.Size(106, 24);
            this.lb_trustaddr.TabIndex = 15;
            this.lb_trustaddr.Text = "darkLabel2";
            // 
            // bt_query
            // 
            this.bt_query.Location = new System.Drawing.Point(374, 294);
            this.bt_query.Name = "bt_query";
            this.bt_query.Padding = new System.Windows.Forms.Padding(5);
            this.bt_query.Size = new System.Drawing.Size(209, 34);
            this.bt_query.SpecialBorderColor = null;
            this.bt_query.SpecialFillColor = null;
            this.bt_query.SpecialTextColor = null;
            this.bt_query.TabIndex = 17;
            this.bt_query.Text = "darkButton1";
            this.bt_query.Click += new System.EventHandler(this.bt_query_Click);
            // 
            // ViewCasinoTrustPool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 651);
            this.Controls.Add(this.bt_query);
            this.Controls.Add(this.tb_trustaddr);
            this.Controls.Add(this.lb_trustaddr);
            this.Controls.Add(this.tb_balance_bit);
            this.Controls.Add(this.tb_balance_oxc);
            this.Controls.Add(this.tb_scope);
            this.Controls.Add(this.lb_trustee_addr);
            this.Controls.Add(this.tb_trustee_pub);
            this.Controls.Add(this.lb_truster_addr);
            this.Controls.Add(this.tb_truster_pub);
            this.Controls.Add(this.lb_balance_Bit);
            this.Controls.Add(this.lb_balance_OXC);
            this.Controls.Add(this.lb_scope);
            this.Controls.Add(this.lb_trustee);
            this.Controls.Add(this.lb_truster);
            this.Name = "ViewCasinoTrustPool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PlayerQuery";
            this.Load += new System.EventHandler(this.GuaranteeQuery_Load);
            this.Controls.SetChildIndex(this.lb_truster, 0);
            this.Controls.SetChildIndex(this.lb_trustee, 0);
            this.Controls.SetChildIndex(this.lb_scope, 0);
            this.Controls.SetChildIndex(this.lb_balance_OXC, 0);
            this.Controls.SetChildIndex(this.lb_balance_Bit, 0);
            this.Controls.SetChildIndex(this.tb_truster_pub, 0);
            this.Controls.SetChildIndex(this.lb_truster_addr, 0);
            this.Controls.SetChildIndex(this.tb_trustee_pub, 0);
            this.Controls.SetChildIndex(this.lb_trustee_addr, 0);
            this.Controls.SetChildIndex(this.tb_scope, 0);
            this.Controls.SetChildIndex(this.tb_balance_oxc, 0);
            this.Controls.SetChildIndex(this.tb_balance_bit, 0);
            this.Controls.SetChildIndex(this.lb_trustaddr, 0);
            this.Controls.SetChildIndex(this.tb_trustaddr, 0);
            this.Controls.SetChildIndex(this.bt_query, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wallets.UI.Controls.DarkLabel lb_truster;
        private Wallets.UI.Controls.DarkLabel lb_trustee;
        private Wallets.UI.Controls.DarkLabel lb_balance_OXC;
        private Wallets.UI.Controls.DarkLabel lb_scope;
        private Wallets.UI.Controls.DarkLabel lb_balance_Bit;
        private Wallets.UI.Controls.DarkTextBox tb_truster_pub;
        private Wallets.UI.Controls.DarkLabel lb_truster_addr;
        private Wallets.UI.Controls.DarkLabel lb_trustee_addr;
        private Wallets.UI.Controls.DarkTextBox tb_trustee_pub;
        private Wallets.UI.Controls.DarkTextBox tb_scope;
        private Wallets.UI.Controls.DarkTextBox tb_balance_oxc;
        private Wallets.UI.Controls.DarkTextBox tb_balance_bit;
        private Wallets.UI.Controls.DarkTextBox tb_trustaddr;
        private Wallets.UI.Controls.DarkLabel lb_trustaddr;
        private Wallets.UI.Controls.DarkButton bt_query;
    }
}