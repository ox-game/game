using OX.Wallets.UI.Config;

namespace OX.UI.Bury
{
    partial class ReplyBuryDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplyBuryDetail));
            panel = new System.Windows.Forms.Panel();
            lb_txhash_v = new Wallets.UI.Controls.DarkLabel();
            lb_txhash = new Wallets.UI.Controls.DarkLabel();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            bt_verify = new Wallets.UI.Controls.DarkButton();
            lb_verifyhash_v = new Wallets.UI.Controls.DarkLabel();
            lb_verifyhash = new Wallets.UI.Controls.DarkLabel();
            lb_rand_v = new Wallets.UI.Controls.DarkLabel();
            lb_rand = new Wallets.UI.Controls.DarkLabel();
            lb_ciphercode_v = new Wallets.UI.Controls.DarkLabel();
            lb_ciphercode = new Wallets.UI.Controls.DarkLabel();
            lb_plaincode_v = new Wallets.UI.Controls.DarkLabel();
            lb_plaincode = new Wallets.UI.Controls.DarkLabel();
            bt_bet = new Wallets.UI.Controls.DarkButton();
            panel.SuspendLayout();
            SuspendLayout();
            // 
            // panel
            // 
            panel.Controls.Add(lb_txhash_v);
            panel.Controls.Add(lb_txhash);
            panel.Controls.Add(flowLayoutPanel1);
            panel.Controls.Add(bt_verify);
            panel.Controls.Add(lb_verifyhash_v);
            panel.Controls.Add(lb_verifyhash);
            panel.Controls.Add(lb_rand_v);
            panel.Controls.Add(lb_rand);
            panel.Controls.Add(lb_ciphercode_v);
            panel.Controls.Add(lb_ciphercode);
            panel.Controls.Add(lb_plaincode_v);
            panel.Controls.Add(lb_plaincode);
            panel.Controls.Add(bt_bet);
            resources.ApplyResources(panel, "panel");
            panel.Name = "panel";
            // 
            // lb_txhash_v
            // 
            resources.ApplyResources(lb_txhash_v, "lb_txhash_v");
            lb_txhash_v.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_txhash_v.Name = "lb_txhash_v";
            // 
            // lb_txhash
            // 
            resources.ApplyResources(lb_txhash, "lb_txhash");
            lb_txhash.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_txhash.Name = "lb_txhash";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(flowLayoutPanel1, "flowLayoutPanel1");
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // bt_verify
            // 
            resources.ApplyResources(bt_verify, "bt_verify");
            bt_verify.Name = "bt_verify";
            bt_verify.SpecialBorderColor = null;
            bt_verify.SpecialFillColor = null;
            bt_verify.SpecialTextColor = null;
            bt_verify.Click += bt_verify_Click;
            // 
            // lb_verifyhash_v
            // 
            resources.ApplyResources(lb_verifyhash_v, "lb_verifyhash_v");
            lb_verifyhash_v.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_verifyhash_v.Name = "lb_verifyhash_v";
            // 
            // lb_verifyhash
            // 
            resources.ApplyResources(lb_verifyhash, "lb_verifyhash");
            lb_verifyhash.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_verifyhash.Name = "lb_verifyhash";
            // 
            // lb_rand_v
            // 
            resources.ApplyResources(lb_rand_v, "lb_rand_v");
            lb_rand_v.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_rand_v.Name = "lb_rand_v";
            // 
            // lb_rand
            // 
            resources.ApplyResources(lb_rand, "lb_rand");
            lb_rand.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_rand.Name = "lb_rand";
            // 
            // lb_ciphercode_v
            // 
            resources.ApplyResources(lb_ciphercode_v, "lb_ciphercode_v");
            lb_ciphercode_v.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_ciphercode_v.Name = "lb_ciphercode_v";
            // 
            // lb_ciphercode
            // 
            resources.ApplyResources(lb_ciphercode, "lb_ciphercode");
            lb_ciphercode.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_ciphercode.Name = "lb_ciphercode";
            // 
            // lb_plaincode_v
            // 
            resources.ApplyResources(lb_plaincode_v, "lb_plaincode_v");
            lb_plaincode_v.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_plaincode_v.Name = "lb_plaincode_v";
            // 
            // lb_plaincode
            // 
            resources.ApplyResources(lb_plaincode, "lb_plaincode");
            lb_plaincode.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_plaincode.Name = "lb_plaincode";
            // 
            // bt_bet
            // 
            resources.ApplyResources(bt_bet, "bt_bet");
            bt_bet.Name = "bt_bet";
            bt_bet.SpecialBorderColor = null;
            bt_bet.SpecialFillColor = null;
            bt_bet.SpecialTextColor = null;
            bt_bet.Click += bt_bet_Click;
            // 
            // ReplyBuryDetail
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ReplyBuryDetail";
            FormClosing += ClaimForm_FormClosing;
            Load += ClaimForm_Load;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private OX.Wallets.UI.Controls.DarkButton bt_bet;
        private Wallets.UI.Controls.DarkButton bt_verify;
        private Wallets.UI.Controls.DarkLabel lb_verifyhash_v;
        private Wallets.UI.Controls.DarkLabel lb_verifyhash;
        private Wallets.UI.Controls.DarkLabel lb_rand_v;
        private Wallets.UI.Controls.DarkLabel lb_rand;
        private Wallets.UI.Controls.DarkLabel lb_ciphercode_v;
        private Wallets.UI.Controls.DarkLabel lb_ciphercode;
        private Wallets.UI.Controls.DarkLabel lb_plaincode_v;
        private Wallets.UI.Controls.DarkLabel lb_plaincode;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Wallets.UI.Controls.DarkLabel lb_txhash_v;
        private Wallets.UI.Controls.DarkLabel lb_txhash;
    }
}