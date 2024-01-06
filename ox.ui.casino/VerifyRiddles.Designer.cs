namespace OX.UI.Casino
{
    partial class VerifyRiddles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerifyRiddles));
            this.panel = new System.Windows.Forms.Panel();
            this.tb_riddles_items = new OX.Wallets.UI.Controls.DarkTextBox();
            this.cb_txhasList = new OX.Wallets.UI.Controls.DarkComboBox();
            this.lb_txhash = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_index_v = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_riddles_hash = new OX.Wallets.UI.Controls.DarkTextBox();
            this.bt_verify = new OX.Wallets.UI.Controls.DarkButton();
            this.tb_riddles = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_riddles_hash = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_riddles = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_index = new OX.Wallets.UI.Controls.DarkLabel();
            this.lb_mineNonce = new OX.Wallets.UI.Controls.DarkLabel();
            this.tb_mineNonce = new OX.Wallets.UI.Controls.DarkTextBox();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            // 
            // btnYes
            // 
            resources.ApplyResources(this.btnYes, "btnYes");
            // 
            // btnNo
            // 
            resources.ApplyResources(this.btnNo, "btnNo");
            // 
            // btnRetry
            // 
            resources.ApplyResources(this.btnRetry, "btnRetry");
            // 
            // btnIgnore
            // 
            resources.ApplyResources(this.btnIgnore, "btnIgnore");
            // 
            // panel
            // 
            this.panel.Controls.Add(this.tb_mineNonce);
            this.panel.Controls.Add(this.lb_mineNonce);
            this.panel.Controls.Add(this.tb_riddles_items);
            this.panel.Controls.Add(this.cb_txhasList);
            this.panel.Controls.Add(this.lb_txhash);
            this.panel.Controls.Add(this.lb_index_v);
            this.panel.Controls.Add(this.tb_riddles_hash);
            this.panel.Controls.Add(this.bt_verify);
            this.panel.Controls.Add(this.tb_riddles);
            this.panel.Controls.Add(this.lb_riddles_hash);
            this.panel.Controls.Add(this.lb_riddles);
            this.panel.Controls.Add(this.lb_index);
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
            // 
            // tb_riddles_items
            // 
            this.tb_riddles_items.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_riddles_items.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_riddles_items.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_riddles_items, "tb_riddles_items");
            this.tb_riddles_items.Name = "tb_riddles_items";
            this.tb_riddles_items.ReadOnly = true;
            // 
            // cb_txhasList
            // 
            this.cb_txhasList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cb_txhasList.FormattingEnabled = true;
            resources.ApplyResources(this.cb_txhasList, "cb_txhasList");
            this.cb_txhasList.Name = "cb_txhasList";
            this.cb_txhasList.SpecialBorderColor = null;
            this.cb_txhasList.SpecialFillColor = null;
            this.cb_txhasList.SpecialTextColor = null;
            this.cb_txhasList.SelectedIndexChanged += new System.EventHandler(this.cb_txhasList_SelectedIndexChanged);
            // 
            // lb_txhash
            // 
            resources.ApplyResources(this.lb_txhash, "lb_txhash");
            this.lb_txhash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_txhash.Name = "lb_txhash";
            // 
            // lb_index_v
            // 
            resources.ApplyResources(this.lb_index_v, "lb_index_v");
            this.lb_index_v.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_index_v.Name = "lb_index_v";
            // 
            // tb_riddles_hash
            // 
            this.tb_riddles_hash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_riddles_hash.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_riddles_hash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_riddles_hash, "tb_riddles_hash");
            this.tb_riddles_hash.Name = "tb_riddles_hash";
            // 
            // bt_verify
            // 
            resources.ApplyResources(this.bt_verify, "bt_verify");
            this.bt_verify.Name = "bt_verify";
            this.bt_verify.SpecialBorderColor = null;
            this.bt_verify.SpecialFillColor = null;
            this.bt_verify.SpecialTextColor = null;
            this.bt_verify.Click += new System.EventHandler(this.bt_copy_Click);
            // 
            // tb_riddles
            // 
            this.tb_riddles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_riddles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_riddles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_riddles, "tb_riddles");
            this.tb_riddles.Name = "tb_riddles";
            this.tb_riddles.ReadOnly = true;
            // 
            // lb_riddles_hash
            // 
            resources.ApplyResources(this.lb_riddles_hash, "lb_riddles_hash");
            this.lb_riddles_hash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_riddles_hash.Name = "lb_riddles_hash";
            // 
            // lb_riddles
            // 
            resources.ApplyResources(this.lb_riddles, "lb_riddles");
            this.lb_riddles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_riddles.Name = "lb_riddles";
            // 
            // lb_index
            // 
            resources.ApplyResources(this.lb_index, "lb_index");
            this.lb_index.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_index.Name = "lb_index";
            // 
            // lb_mineNonce
            // 
            resources.ApplyResources(this.lb_mineNonce, "lb_mineNonce");
            this.lb_mineNonce.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_mineNonce.Name = "lb_mineNonce";
            // 
            // tb_mineNonce
            // 
            this.tb_mineNonce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_mineNonce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_mineNonce.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_mineNonce, "tb_mineNonce");
            this.tb_mineNonce.Name = "tb_mineNonce";
            this.tb_mineNonce.ReadOnly = true;
            // 
            // VerifyRiddles
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VerifyRiddles";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClaimForm_FormClosing);
            this.Load += new System.EventHandler(this.ClaimForm_Load);
            this.Controls.SetChildIndex(this.panel, 0);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private OX.Wallets.UI.Controls.DarkLabel lb_index;
        private OX.Wallets.UI.Controls.DarkLabel lb_riddles;
        private OX.Wallets.UI.Controls.DarkLabel lb_riddles_hash;
        private OX.Wallets.UI.Controls.DarkTextBox tb_riddles;
        private OX.Wallets.UI.Controls.DarkButton bt_verify;
        private OX.Wallets.UI.Controls.DarkTextBox tb_riddles_hash;
        private OX.Wallets.UI.Controls.DarkLabel lb_index_v;
        private Wallets.UI.Controls.DarkComboBox cb_txhasList;
        private Wallets.UI.Controls.DarkLabel lb_txhash;
        private Wallets.UI.Controls.DarkTextBox tb_riddles_items;
        private Wallets.UI.Controls.DarkTextBox tb_mineNonce;
        private Wallets.UI.Controls.DarkLabel lb_mineNonce;
    }
}