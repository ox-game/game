namespace OX.UI.Casino
{
    partial class SignRoomAuth
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignRoomAuth));
            this.panel = new System.Windows.Forms.Panel();
            this.bt_clear = new OX.Wallets.UI.Controls.DarkButton();
            this.bt_add_addr = new OX.Wallets.UI.Controls.DarkButton();
            this.tb_sh_to_add = new OX.Wallets.UI.Controls.DarkTextBox();
            this.lv_members = new OX.Wallets.UI.Controls.DarkListView();
            this.lb_memberAddr = new OX.Wallets.UI.Controls.DarkLabel();
            this.darkLabel2 = new OX.Wallets.UI.Controls.DarkLabel();
            this.cb_betFee = new OX.Wallets.UI.Controls.DarkComboBox();
            this.lb_betFee = new OX.Wallets.UI.Controls.DarkLabel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.panel.Controls.Add(this.darkLabel2);
            this.panel.Controls.Add(this.cb_betFee);
            this.panel.Controls.Add(this.lb_betFee);
            this.panel.Controls.Add(this.bt_clear);
            this.panel.Controls.Add(this.bt_add_addr);
            this.panel.Controls.Add(this.tb_sh_to_add);
            this.panel.Controls.Add(this.lv_members);
            this.panel.Controls.Add(this.lb_memberAddr);
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
            // 
            // bt_clear
            // 
            resources.ApplyResources(this.bt_clear, "bt_clear");
            this.bt_clear.Name = "bt_clear";
            this.bt_clear.SpecialBorderColor = null;
            this.bt_clear.SpecialFillColor = null;
            this.bt_clear.SpecialTextColor = null;
            this.bt_clear.Click += new System.EventHandler(this.bt_clear_Click);
            // 
            // bt_add_addr
            // 
            resources.ApplyResources(this.bt_add_addr, "bt_add_addr");
            this.bt_add_addr.Name = "bt_add_addr";
            this.bt_add_addr.SpecialBorderColor = null;
            this.bt_add_addr.SpecialFillColor = null;
            this.bt_add_addr.SpecialTextColor = null;
            this.bt_add_addr.Click += new System.EventHandler(this.bt_add_addr_Click);
            // 
            // tb_sh_to_add
            // 
            this.tb_sh_to_add.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tb_sh_to_add.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_sh_to_add.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.tb_sh_to_add, "tb_sh_to_add");
            this.tb_sh_to_add.Name = "tb_sh_to_add";
            // 
            // lv_members
            // 
            resources.ApplyResources(this.lv_members, "lv_members");
            this.lv_members.Name = "lv_members";
            // 
            // lb_memberAddr
            // 
            resources.ApplyResources(this.lb_memberAddr, "lb_memberAddr");
            this.lb_memberAddr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_memberAddr.Name = "lb_memberAddr";
            // 
            // darkLabel2
            // 
            resources.ApplyResources(this.darkLabel2, "darkLabel2");
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Name = "darkLabel2";
            // 
            // cb_betFee
            // 
            this.cb_betFee.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cb_betFee.FormattingEnabled = true;
            this.cb_betFee.Items.AddRange(new object[] {
            resources.GetString("cb_betFee.Items"),
            resources.GetString("cb_betFee.Items1"),
            resources.GetString("cb_betFee.Items2"),
            resources.GetString("cb_betFee.Items3"),
            resources.GetString("cb_betFee.Items4"),
            resources.GetString("cb_betFee.Items5"),
            resources.GetString("cb_betFee.Items6"),
            resources.GetString("cb_betFee.Items7"),
            resources.GetString("cb_betFee.Items8"),
            resources.GetString("cb_betFee.Items9")});
            resources.ApplyResources(this.cb_betFee, "cb_betFee");
            this.cb_betFee.Name = "cb_betFee";
            this.cb_betFee.SpecialBorderColor = null;
            this.cb_betFee.SpecialFillColor = null;
            this.cb_betFee.SpecialTextColor = null;
            // 
            // lb_betFee
            // 
            resources.ApplyResources(this.lb_betFee, "lb_betFee");
            this.lb_betFee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lb_betFee.Name = "lb_betFee";
            // 
            // SignRoomAuth
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.DialogButtons = OX.Wallets.UI.Forms.DarkDialogButton.OkCancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SignRoomAuth";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClaimForm_FormClosing);
            this.Load += new System.EventHandler(this.ClaimForm_Load);
            this.Controls.SetChildIndex(this.panel, 0);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private OX.Wallets.UI.Controls.DarkLabel lb_memberAddr;
        private Wallets.UI.Controls.DarkButton bt_clear;
        private Wallets.UI.Controls.DarkButton bt_add_addr;
        private Wallets.UI.Controls.DarkTextBox tb_sh_to_add;
        private Wallets.UI.Controls.DarkListView lv_members;
        private Wallets.UI.Controls.DarkLabel darkLabel2;
        private Wallets.UI.Controls.DarkComboBox cb_betFee;
        private Wallets.UI.Controls.DarkLabel lb_betFee;
    }
}