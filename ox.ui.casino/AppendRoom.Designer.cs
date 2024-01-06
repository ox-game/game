using OX.Wallets;

namespace OX.UI.Casino
{
    partial class AppendRoom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppendRoom));
            this.panel = new System.Windows.Forms.Panel();
            this.tb_roomId = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lb_roomId = new OX.Wallets.UI.Controls.DarkLabel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.tb_roomId);
            this.panel.Controls.Add(this.lb_roomId);
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
            // 
            // tb_roomId
            // 
            this.tb_roomId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_roomId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_roomId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_roomId, "tb_roomId");
            this.tb_roomId.Name = "tb_roomId";
            // 
            // lb_roomId
            // 
            resources.ApplyResources(this.lb_roomId, "lb_roomId");
            this.lb_roomId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_roomId.Name = "lb_roomId";
            // 
            // AppendRoom
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppendRoom";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClaimForm_FormClosing);
            this.Load += new System.EventHandler(this.ClaimForm_Load);
            this.Controls.SetChildIndex(this.panel, 0);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private OX.Wallets.UI.Controls.DarkLabel lb_roomId;
        private OX.Wallets.UI.Controls.DarkTextBox tb_roomId;
    }
}