using System.Collections.Generic;
using OX.Wallets.UI.Controls;
namespace OX.UI.Bury
{
    partial class BuryView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_DoBury = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_mybury = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_myluck = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_DoTrustBury = new OX.Wallets.UI.Controls.DarkButton();
            this.lb_buryAddress = new OX.Wallets.UI.Controls.DarkLabel();
            this.bt_copyBetAddress = new OX.Wallets.UI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // bt_DoBury
            // 
            this.bt_DoBury.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bt_DoBury.Location = new System.Drawing.Point(16, 53);
            this.bt_DoBury.Name = "bt_DoBury";
            this.bt_DoBury.Padding = new System.Windows.Forms.Padding(5);
            this.bt_DoBury.Size = new System.Drawing.Size(145, 31);
            this.bt_DoBury.SpecialBorderColor = null;
            this.bt_DoBury.SpecialFillColor = null;
            this.bt_DoBury.SpecialTextColor = null;
            this.bt_DoBury.TabIndex = 10;
            this.bt_DoBury.Text = "button1";
            this.bt_DoBury.Click += new System.EventHandler(this.bt_DoBury_Click);
            // 
            // bt_mybury
            // 
            this.bt_mybury.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bt_mybury.Location = new System.Drawing.Point(351, 53);
            this.bt_mybury.Name = "bt_mybury";
            this.bt_mybury.Padding = new System.Windows.Forms.Padding(5);
            this.bt_mybury.Size = new System.Drawing.Size(145, 31);
            this.bt_mybury.SpecialBorderColor = null;
            this.bt_mybury.SpecialFillColor = null;
            this.bt_mybury.SpecialTextColor = null;
            this.bt_mybury.TabIndex = 11;
            this.bt_mybury.Text = "button1";
            this.bt_mybury.Click += new System.EventHandler(this.bt_mybury_Click);
            // 
            // bt_myluck
            // 
            this.bt_myluck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bt_myluck.Location = new System.Drawing.Point(519, 53);
            this.bt_myluck.Name = "bt_myluck";
            this.bt_myluck.Padding = new System.Windows.Forms.Padding(5);
            this.bt_myluck.Size = new System.Drawing.Size(145, 31);
            this.bt_myluck.SpecialBorderColor = null;
            this.bt_myluck.SpecialFillColor = null;
            this.bt_myluck.SpecialTextColor = null;
            this.bt_myluck.TabIndex = 12;
            this.bt_myluck.Text = "button1";
            this.bt_myluck.Click += new System.EventHandler(this.bt_myluck_Click);
            // 
            // bt_DoTrustBury
            // 
            this.bt_DoTrustBury.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bt_DoTrustBury.Location = new System.Drawing.Point(183, 53);
            this.bt_DoTrustBury.Name = "bt_DoTrustBury";
            this.bt_DoTrustBury.Padding = new System.Windows.Forms.Padding(5);
            this.bt_DoTrustBury.Size = new System.Drawing.Size(145, 31);
            this.bt_DoTrustBury.SpecialBorderColor = null;
            this.bt_DoTrustBury.SpecialFillColor = null;
            this.bt_DoTrustBury.SpecialTextColor = null;
            this.bt_DoTrustBury.TabIndex = 13;
            this.bt_DoTrustBury.Text = "button1";
            this.bt_DoTrustBury.Click += new System.EventHandler(this.bt_DoTrustBury_Click);
            // 
            // lb_buryAddress
            // 
            this.lb_buryAddress.AutoSize = true;
            this.lb_buryAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_buryAddress.Location = new System.Drawing.Point(20, 14);
            this.lb_buryAddress.Name = "lb_buryAddress";
            this.lb_buryAddress.Size = new System.Drawing.Size(98, 25);
            this.lb_buryAddress.TabIndex = 14;
            this.lb_buryAddress.Text = "darkLabel1";
            // 
            // bt_copyBetAddress
            // 
            this.bt_copyBetAddress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bt_copyBetAddress.Location = new System.Drawing.Point(686, 53);
            this.bt_copyBetAddress.Name = "bt_copyBetAddress";
            this.bt_copyBetAddress.Padding = new System.Windows.Forms.Padding(5);
            this.bt_copyBetAddress.Size = new System.Drawing.Size(145, 31);
            this.bt_copyBetAddress.SpecialBorderColor = null;
            this.bt_copyBetAddress.SpecialFillColor = null;
            this.bt_copyBetAddress.SpecialTextColor = null;
            this.bt_copyBetAddress.TabIndex = 15;
            this.bt_copyBetAddress.Text = "button1";
            this.bt_copyBetAddress.Click += new System.EventHandler(this.bt_copyBetAddress_Click);
            // 
            // BuryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bt_copyBetAddress);
            this.Controls.Add(this.lb_buryAddress);
            this.Controls.Add(this.bt_DoTrustBury);
            this.Controls.Add(this.bt_myluck);
            this.Controls.Add(this.bt_mybury);
            this.Controls.Add(this.bt_DoBury);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "BuryView";
            this.Size = new System.Drawing.Size(895, 486);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkButton bt_DoBury;
        private DarkButton bt_mybury;
        private DarkButton bt_myluck;
        private DarkButton bt_DoTrustBury;
        private DarkLabel lb_buryAddress;
        private DarkButton bt_copyBetAddress;
    }
}
