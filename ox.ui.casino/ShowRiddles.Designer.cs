using OX.Wallets.UI.Config;

namespace OX.UI.Casino
{
    partial class ShowRiddles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowRiddles));
            panel = new System.Windows.Forms.Panel();
            bt_copy = new Wallets.UI.Controls.DarkButton();
            tb_riddlesData = new Wallets.UI.Controls.DarkTextBox();
            lb_riddlesData = new Wallets.UI.Controls.DarkLabel();
            tb_nonce = new Wallets.UI.Controls.DarkTextBox();
            lb_nonce = new Wallets.UI.Controls.DarkLabel();
            tb_index = new Wallets.UI.Controls.DarkTextBox();
            tb_riddles = new Wallets.UI.Controls.DarkTextBox();
            lb_riddles = new Wallets.UI.Controls.DarkLabel();
            lb_index = new Wallets.UI.Controls.DarkLabel();
            bt_copyNonce = new Wallets.UI.Controls.DarkButton();
            panel.SuspendLayout();
            SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(btnCancel, "btnCancel");
            // 
            // btnClose
            // 
            resources.ApplyResources(btnClose, "btnClose");
            // 
            // btnYes
            // 
            resources.ApplyResources(btnYes, "btnYes");
            // 
            // btnNo
            // 
            resources.ApplyResources(btnNo, "btnNo");
            // 
            // btnRetry
            // 
            resources.ApplyResources(btnRetry, "btnRetry");
            // 
            // btnIgnore
            // 
            resources.ApplyResources(btnIgnore, "btnIgnore");
            // 
            // panel
            // 
            panel.Controls.Add(bt_copyNonce);
            panel.Controls.Add(bt_copy);
            panel.Controls.Add(tb_riddlesData);
            panel.Controls.Add(lb_riddlesData);
            panel.Controls.Add(tb_nonce);
            panel.Controls.Add(lb_nonce);
            panel.Controls.Add(tb_index);
            panel.Controls.Add(tb_riddles);
            panel.Controls.Add(lb_riddles);
            panel.Controls.Add(lb_index);
            resources.ApplyResources(panel, "panel");
            panel.Name = "panel";
            // 
            // bt_copy
            // 
            resources.ApplyResources(bt_copy, "bt_copy");
            bt_copy.Name = "bt_copy";
            bt_copy.SpecialBorderColor = null;
            bt_copy.SpecialFillColor = null;
            bt_copy.SpecialTextColor = null;
            bt_copy.Click += bt_copy_Click;
            // 
            // tb_riddlesData
            // 
            tb_riddlesData.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_riddlesData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_riddlesData.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_riddlesData, "tb_riddlesData");
            tb_riddlesData.Name = "tb_riddlesData";
            tb_riddlesData.ReadOnly = true;
            // 
            // lb_riddlesData
            // 
            resources.ApplyResources(lb_riddlesData, "lb_riddlesData");
            lb_riddlesData.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_riddlesData.Name = "lb_riddlesData";
            // 
            // tb_nonce
            // 
            tb_nonce.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_nonce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_nonce.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_nonce, "tb_nonce");
            tb_nonce.Name = "tb_nonce";
            tb_nonce.ReadOnly = true;
            // 
            // lb_nonce
            // 
            resources.ApplyResources(lb_nonce, "lb_nonce");
            lb_nonce.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_nonce.Name = "lb_nonce";
            // 
            // tb_index
            // 
            tb_index.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_index.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_index.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_index, "tb_index");
            tb_index.Name = "tb_index";
            tb_index.ReadOnly = true;
            // 
            // tb_riddles
            // 
            tb_riddles.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_riddles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_riddles.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_riddles, "tb_riddles");
            tb_riddles.Name = "tb_riddles";
            tb_riddles.ReadOnly = true;
            // 
            // lb_riddles
            // 
            resources.ApplyResources(lb_riddles, "lb_riddles");
            lb_riddles.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_riddles.Name = "lb_riddles";
            // 
            // lb_index
            // 
            resources.ApplyResources(lb_index, "lb_index");
            lb_index.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_index.Name = "lb_index";
            // 
            // bt_copyNonce
            // 
            resources.ApplyResources(bt_copyNonce, "bt_copyNonce");
            bt_copyNonce.Name = "bt_copyNonce";
            bt_copyNonce.SpecialBorderColor = null;
            bt_copyNonce.SpecialFillColor = null;
            bt_copyNonce.SpecialTextColor = null;
            bt_copyNonce.Click += bt_copyNonce_Click;
            // 
            // ShowRiddles
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ShowRiddles";
            FormClosing += ClaimForm_FormClosing;
            Load += ClaimForm_Load;
            Controls.SetChildIndex(panel, 0);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private OX.Wallets.UI.Controls.DarkLabel lb_index;
        private OX.Wallets.UI.Controls.DarkLabel lb_riddles;
        private OX.Wallets.UI.Controls.DarkTextBox tb_riddles;
        private Wallets.UI.Controls.DarkTextBox tb_index;
        private Wallets.UI.Controls.DarkTextBox tb_nonce;
        private Wallets.UI.Controls.DarkLabel lb_nonce;
        private Wallets.UI.Controls.DarkLabel lb_riddlesData;
        private Wallets.UI.Controls.DarkTextBox tb_riddlesData;
        private Wallets.UI.Controls.DarkButton bt_copy;
        private Wallets.UI.Controls.DarkButton bt_copyNonce;
    }
}