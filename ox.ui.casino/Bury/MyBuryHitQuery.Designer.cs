
namespace OX.UI.Casino.Bury
{
    partial class MyBuryHitQuery
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.bt_close = new OX.Wallets.UI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1522, 1037);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // bt_close
            // 
            this.bt_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_close.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bt_close.Location = new System.Drawing.Point(1383, 1076);
            this.bt_close.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.bt_close.Name = "bt_close";
            this.bt_close.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.bt_close.Size = new System.Drawing.Size(151, 42);
            this.bt_close.SpecialBorderColor = null;
            this.bt_close.SpecialFillColor = null;
            this.bt_close.SpecialTextColor = null;
            this.bt_close.TabIndex = 14;
            this.bt_close.Text = "Claim All";
            this.bt_close.Click += new System.EventHandler(this.bt_close_Click);
            // 
            // MyBuryQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1554, 1153);
            this.Controls.Add(this.bt_close);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "MyBuryQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PlayerQuery";
            this.Load += new System.EventHandler(this.MyBuryQuery_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Wallets.UI.Controls.DarkButton bt_close;
    }
}