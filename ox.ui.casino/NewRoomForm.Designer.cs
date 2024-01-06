namespace OX.UI.Casino
{
    partial class NewRoomForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRoomForm));
            panel = new System.Windows.Forms.Panel();
            darkLabel3 = new Wallets.UI.Controls.DarkLabel();
            darkLabel2 = new Wallets.UI.Controls.DarkLabel();
            cb_betFee = new Wallets.UI.Controls.DarkComboBox();
            lb_betFee = new Wallets.UI.Controls.DarkLabel();
            cb_assetKind = new Wallets.UI.Controls.DarkComboBox();
            lb_assetKind = new Wallets.UI.Controls.DarkLabel();
            tb_partnerRatio = new Wallets.UI.Controls.DarkTextBox();
            cb_slope = new Wallets.UI.Controls.DarkComboBox();
            lb_slope = new Wallets.UI.Controls.DarkLabel();
            lb_partnerRatio = new Wallets.UI.Controls.DarkLabel();
            darkGroupBox1 = new Wallets.UI.Controls.DarkGroupBox();
            bt_clear = new Wallets.UI.Controls.DarkButton();
            bt_add_addr = new Wallets.UI.Controls.DarkButton();
            tb_sh_to_add = new Wallets.UI.Controls.DarkTextBox();
            lv_members = new Wallets.UI.Controls.DarkListView();
            lb_banker_addr = new Wallets.UI.Controls.DarkLabel();
            lb_fee_addr = new Wallets.UI.Controls.DarkLabel();
            lb_pool_addr = new Wallets.UI.Controls.DarkLabel();
            lb_bet_addr = new Wallets.UI.Controls.DarkLabel();
            cb_bonusmultiple = new Wallets.UI.Controls.DarkComboBox();
            lb_bonusmultiple = new Wallets.UI.Controls.DarkLabel();
            cb_flagPoint_v = new Wallets.UI.Controls.DarkComboBox();
            lb_flagPoint = new Wallets.UI.Controls.DarkLabel();
            lb_roomState = new Wallets.UI.Controls.DarkLabel();
            cb_roomState = new Wallets.UI.Controls.DarkComboBox();
            lb_commissionmsg = new Wallets.UI.Controls.DarkLabel();
            lb_msg = new Wallets.UI.Controls.DarkLabel();
            bt_NewRoom = new Wallets.UI.Controls.DarkButton();
            cbAccounts = new Wallets.UI.Controls.DarkComboBox();
            lb_from = new Wallets.UI.Controls.DarkLabel();
            lb_commissionvalue = new Wallets.UI.Controls.DarkLabel();
            lb_period = new Wallets.UI.Controls.DarkLabel();
            cb_period = new Wallets.UI.Controls.DarkComboBox();
            cb_gamekind = new Wallets.UI.Controls.DarkComboBox();
            lb_roomkind = new Wallets.UI.Controls.DarkLabel();
            nu_commissionvalue = new Wallets.UI.Controls.DarkNumericUpDown();
            darkLabel1 = new Wallets.UI.Controls.DarkLabel();
            panel.SuspendLayout();
            darkGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nu_commissionvalue).BeginInit();
            SuspendLayout();
            // 
            // panel
            // 
            panel.Controls.Add(darkLabel3);
            panel.Controls.Add(darkLabel2);
            panel.Controls.Add(cb_betFee);
            panel.Controls.Add(lb_betFee);
            panel.Controls.Add(cb_assetKind);
            panel.Controls.Add(lb_assetKind);
            panel.Controls.Add(tb_partnerRatio);
            panel.Controls.Add(cb_slope);
            panel.Controls.Add(lb_slope);
            panel.Controls.Add(lb_partnerRatio);
            panel.Controls.Add(darkGroupBox1);
            panel.Controls.Add(lb_banker_addr);
            panel.Controls.Add(lb_fee_addr);
            panel.Controls.Add(lb_pool_addr);
            panel.Controls.Add(lb_bet_addr);
            panel.Controls.Add(cb_bonusmultiple);
            panel.Controls.Add(lb_bonusmultiple);
            panel.Controls.Add(cb_flagPoint_v);
            panel.Controls.Add(lb_flagPoint);
            panel.Controls.Add(lb_roomState);
            panel.Controls.Add(cb_roomState);
            panel.Controls.Add(lb_commissionmsg);
            panel.Controls.Add(lb_msg);
            panel.Controls.Add(bt_NewRoom);
            panel.Controls.Add(cbAccounts);
            panel.Controls.Add(lb_from);
            panel.Controls.Add(lb_commissionvalue);
            panel.Controls.Add(lb_period);
            panel.Controls.Add(cb_period);
            panel.Controls.Add(cb_gamekind);
            panel.Controls.Add(lb_roomkind);
            panel.Controls.Add(nu_commissionvalue);
            resources.ApplyResources(panel, "panel");
            panel.Name = "panel";
            // 
            // darkLabel3
            // 
            resources.ApplyResources(darkLabel3, "darkLabel3");
            darkLabel3.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            darkLabel3.Name = "darkLabel3";
            // 
            // darkLabel2
            // 
            resources.ApplyResources(darkLabel2, "darkLabel2");
            darkLabel2.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            darkLabel2.Name = "darkLabel2";
            // 
            // cb_betFee
            // 
            cb_betFee.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cb_betFee.FormattingEnabled = true;
            cb_betFee.Items.AddRange(new object[] { resources.GetString("cb_betFee.Items"), resources.GetString("cb_betFee.Items1"), resources.GetString("cb_betFee.Items2"), resources.GetString("cb_betFee.Items3"), resources.GetString("cb_betFee.Items4"), resources.GetString("cb_betFee.Items5"), resources.GetString("cb_betFee.Items6"), resources.GetString("cb_betFee.Items7"), resources.GetString("cb_betFee.Items8"), resources.GetString("cb_betFee.Items9") });
            resources.ApplyResources(cb_betFee, "cb_betFee");
            cb_betFee.Name = "cb_betFee";
            cb_betFee.SpecialBorderColor = null;
            cb_betFee.SpecialFillColor = null;
            cb_betFee.SpecialTextColor = null;
            // 
            // lb_betFee
            // 
            resources.ApplyResources(lb_betFee, "lb_betFee");
            lb_betFee.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_betFee.Name = "lb_betFee";
            // 
            // cb_assetKind
            // 
            cb_assetKind.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(cb_assetKind, "cb_assetKind");
            cb_assetKind.Name = "cb_assetKind";
            cb_assetKind.SpecialBorderColor = null;
            cb_assetKind.SpecialFillColor = null;
            cb_assetKind.SpecialTextColor = null;
            // 
            // lb_assetKind
            // 
            resources.ApplyResources(lb_assetKind, "lb_assetKind");
            lb_assetKind.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_assetKind.Name = "lb_assetKind";
            // 
            // tb_partnerRatio
            // 
            tb_partnerRatio.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_partnerRatio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_partnerRatio.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_partnerRatio, "tb_partnerRatio");
            tb_partnerRatio.Name = "tb_partnerRatio";
            tb_partnerRatio.TextChanged += tb_partnerRatio_TextChanged;
            // 
            // cb_slope
            // 
            cb_slope.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cb_slope.FormattingEnabled = true;
            cb_slope.Items.AddRange(new object[] { resources.GetString("cb_slope.Items"), resources.GetString("cb_slope.Items1"), resources.GetString("cb_slope.Items2"), resources.GetString("cb_slope.Items3"), resources.GetString("cb_slope.Items4"), resources.GetString("cb_slope.Items5"), resources.GetString("cb_slope.Items6"), resources.GetString("cb_slope.Items7"), resources.GetString("cb_slope.Items8"), resources.GetString("cb_slope.Items9") });
            resources.ApplyResources(cb_slope, "cb_slope");
            cb_slope.Name = "cb_slope";
            cb_slope.SpecialBorderColor = null;
            cb_slope.SpecialFillColor = null;
            cb_slope.SpecialTextColor = null;
            // 
            // lb_slope
            // 
            resources.ApplyResources(lb_slope, "lb_slope");
            lb_slope.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_slope.Name = "lb_slope";
            // 
            // lb_partnerRatio
            // 
            resources.ApplyResources(lb_partnerRatio, "lb_partnerRatio");
            lb_partnerRatio.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_partnerRatio.Name = "lb_partnerRatio";
            // 
            // darkGroupBox1
            // 
            darkGroupBox1.BorderColor = System.Drawing.Color.FromArgb(51, 51, 51);
            darkGroupBox1.Controls.Add(bt_clear);
            darkGroupBox1.Controls.Add(bt_add_addr);
            darkGroupBox1.Controls.Add(tb_sh_to_add);
            darkGroupBox1.Controls.Add(lv_members);
            resources.ApplyResources(darkGroupBox1, "darkGroupBox1");
            darkGroupBox1.Name = "darkGroupBox1";
            darkGroupBox1.TabStop = false;
            // 
            // bt_clear
            // 
            resources.ApplyResources(bt_clear, "bt_clear");
            bt_clear.Name = "bt_clear";
            bt_clear.SpecialBorderColor = null;
            bt_clear.SpecialFillColor = null;
            bt_clear.SpecialTextColor = null;
            bt_clear.Click += bt_clear_Click;
            // 
            // bt_add_addr
            // 
            resources.ApplyResources(bt_add_addr, "bt_add_addr");
            bt_add_addr.Name = "bt_add_addr";
            bt_add_addr.SpecialBorderColor = null;
            bt_add_addr.SpecialFillColor = null;
            bt_add_addr.SpecialTextColor = null;
            bt_add_addr.Click += bt_add_addr_Click;
            // 
            // tb_sh_to_add
            // 
            tb_sh_to_add.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_sh_to_add.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_sh_to_add.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(tb_sh_to_add, "tb_sh_to_add");
            tb_sh_to_add.Name = "tb_sh_to_add";
            // 
            // lv_members
            // 
            resources.ApplyResources(lv_members, "lv_members");
            lv_members.Name = "lv_members";
            // 
            // lb_banker_addr
            // 
            resources.ApplyResources(lb_banker_addr, "lb_banker_addr");
            lb_banker_addr.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_banker_addr.Name = "lb_banker_addr";
            // 
            // lb_fee_addr
            // 
            resources.ApplyResources(lb_fee_addr, "lb_fee_addr");
            lb_fee_addr.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_fee_addr.Name = "lb_fee_addr";
            // 
            // lb_pool_addr
            // 
            resources.ApplyResources(lb_pool_addr, "lb_pool_addr");
            lb_pool_addr.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_pool_addr.Name = "lb_pool_addr";
            // 
            // lb_bet_addr
            // 
            resources.ApplyResources(lb_bet_addr, "lb_bet_addr");
            lb_bet_addr.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_bet_addr.Name = "lb_bet_addr";
            // 
            // cb_bonusmultiple
            // 
            cb_bonusmultiple.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cb_bonusmultiple.FormattingEnabled = true;
            cb_bonusmultiple.Items.AddRange(new object[] { resources.GetString("cb_bonusmultiple.Items"), resources.GetString("cb_bonusmultiple.Items1"), resources.GetString("cb_bonusmultiple.Items2"), resources.GetString("cb_bonusmultiple.Items3"), resources.GetString("cb_bonusmultiple.Items4"), resources.GetString("cb_bonusmultiple.Items5"), resources.GetString("cb_bonusmultiple.Items6"), resources.GetString("cb_bonusmultiple.Items7"), resources.GetString("cb_bonusmultiple.Items8"), resources.GetString("cb_bonusmultiple.Items9") });
            resources.ApplyResources(cb_bonusmultiple, "cb_bonusmultiple");
            cb_bonusmultiple.Name = "cb_bonusmultiple";
            cb_bonusmultiple.SpecialBorderColor = null;
            cb_bonusmultiple.SpecialFillColor = null;
            cb_bonusmultiple.SpecialTextColor = null;
            // 
            // lb_bonusmultiple
            // 
            resources.ApplyResources(lb_bonusmultiple, "lb_bonusmultiple");
            lb_bonusmultiple.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_bonusmultiple.Name = "lb_bonusmultiple";
            // 
            // cb_flagPoint_v
            // 
            cb_flagPoint_v.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            cb_flagPoint_v.FormattingEnabled = true;
            cb_flagPoint_v.Items.AddRange(new object[] { resources.GetString("cb_flagPoint_v.Items"), resources.GetString("cb_flagPoint_v.Items1"), resources.GetString("cb_flagPoint_v.Items2"), resources.GetString("cb_flagPoint_v.Items3"), resources.GetString("cb_flagPoint_v.Items4"), resources.GetString("cb_flagPoint_v.Items5"), resources.GetString("cb_flagPoint_v.Items6"), resources.GetString("cb_flagPoint_v.Items7"), resources.GetString("cb_flagPoint_v.Items8"), resources.GetString("cb_flagPoint_v.Items9") });
            resources.ApplyResources(cb_flagPoint_v, "cb_flagPoint_v");
            cb_flagPoint_v.Name = "cb_flagPoint_v";
            cb_flagPoint_v.SpecialBorderColor = null;
            cb_flagPoint_v.SpecialFillColor = null;
            cb_flagPoint_v.SpecialTextColor = null;
            // 
            // lb_flagPoint
            // 
            resources.ApplyResources(lb_flagPoint, "lb_flagPoint");
            lb_flagPoint.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_flagPoint.Name = "lb_flagPoint";
            // 
            // lb_roomState
            // 
            resources.ApplyResources(lb_roomState, "lb_roomState");
            lb_roomState.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_roomState.Name = "lb_roomState";
            // 
            // cb_roomState
            // 
            cb_roomState.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(cb_roomState, "cb_roomState");
            cb_roomState.Name = "cb_roomState";
            cb_roomState.SpecialBorderColor = null;
            cb_roomState.SpecialFillColor = null;
            cb_roomState.SpecialTextColor = null;
            cb_roomState.SelectedIndexChanged += cb_commissionkind_SelectedIndexChanged;
            // 
            // lb_commissionmsg
            // 
            resources.ApplyResources(lb_commissionmsg, "lb_commissionmsg");
            lb_commissionmsg.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_commissionmsg.Name = "lb_commissionmsg";
            // 
            // lb_msg
            // 
            resources.ApplyResources(lb_msg, "lb_msg");
            lb_msg.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_msg.Name = "lb_msg";
            // 
            // bt_NewRoom
            // 
            resources.ApplyResources(bt_NewRoom, "bt_NewRoom");
            bt_NewRoom.Name = "bt_NewRoom";
            bt_NewRoom.SpecialBorderColor = null;
            bt_NewRoom.SpecialFillColor = null;
            bt_NewRoom.SpecialTextColor = null;
            bt_NewRoom.Click += bt_NewRoom_Click;
            // 
            // cbAccounts
            // 
            cbAccounts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(cbAccounts, "cbAccounts");
            cbAccounts.Name = "cbAccounts";
            cbAccounts.SpecialBorderColor = null;
            cbAccounts.SpecialFillColor = null;
            cbAccounts.SpecialTextColor = null;
            cbAccounts.SelectedIndexChanged += darkComboBox1_SelectedIndexChanged;
            // 
            // lb_from
            // 
            resources.ApplyResources(lb_from, "lb_from");
            lb_from.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_from.Name = "lb_from";
            // 
            // lb_commissionvalue
            // 
            resources.ApplyResources(lb_commissionvalue, "lb_commissionvalue");
            lb_commissionvalue.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_commissionvalue.Name = "lb_commissionvalue";
            // 
            // lb_period
            // 
            resources.ApplyResources(lb_period, "lb_period");
            lb_period.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_period.Name = "lb_period";
            // 
            // cb_period
            // 
            cb_period.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(cb_period, "cb_period");
            cb_period.Name = "cb_period";
            cb_period.SpecialBorderColor = null;
            cb_period.SpecialFillColor = null;
            cb_period.SpecialTextColor = null;
            cb_period.SelectedIndexChanged += cb_period_SelectedIndexChanged;
            // 
            // cb_gamekind
            // 
            cb_gamekind.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            resources.ApplyResources(cb_gamekind, "cb_gamekind");
            cb_gamekind.Name = "cb_gamekind";
            cb_gamekind.SpecialBorderColor = null;
            cb_gamekind.SpecialFillColor = null;
            cb_gamekind.SpecialTextColor = null;
            cb_gamekind.SelectedIndexChanged += cb_period_SelectedIndexChanged;
            // 
            // lb_roomkind
            // 
            resources.ApplyResources(lb_roomkind, "lb_roomkind");
            lb_roomkind.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_roomkind.Name = "lb_roomkind";
            // 
            // nu_commissionvalue
            // 
            resources.ApplyResources(nu_commissionvalue, "nu_commissionvalue");
            nu_commissionvalue.Maximum = new decimal(new int[] { 250, 0, 0, 0 });
            nu_commissionvalue.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nu_commissionvalue.Name = "nu_commissionvalue";
            nu_commissionvalue.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nu_commissionvalue.ValueChanged += cb_commissionkind_SelectedIndexChanged;
            // 
            // darkLabel1
            // 
            darkLabel1.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            resources.ApplyResources(darkLabel1, "darkLabel1");
            darkLabel1.Name = "darkLabel1";
            // 
            // NewRoomForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewRoomForm";
            FormClosing += ClaimForm_FormClosing;
            Load += ClaimForm_Load;
            panel.ResumeLayout(false);
            panel.PerformLayout();
            darkGroupBox1.ResumeLayout(false);
            darkGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nu_commissionvalue).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private OX.Wallets.UI.Controls.DarkNumericUpDown nu_commissionvalue;
        private OX.Wallets.UI.Controls.DarkLabel lb_roomkind;
        private OX.Wallets.UI.Controls.DarkLabel lb_period;
        private OX.Wallets.UI.Controls.DarkComboBox cb_period;
        private OX.Wallets.UI.Controls.DarkComboBox cb_gamekind;
        private OX.Wallets.UI.Controls.DarkLabel lb_commissionvalue;
        private OX.Wallets.UI.Controls.DarkComboBox cbAccounts;
        private OX.Wallets.UI.Controls.DarkLabel lb_from;
        private OX.Wallets.UI.Controls.DarkLabel lb_msg;
        private OX.Wallets.UI.Controls.DarkLabel lb_commissionmsg;
        private OX.Wallets.UI.Controls.DarkLabel lb_roomState;
        private OX.Wallets.UI.Controls.DarkComboBox cb_roomState;
        private Wallets.UI.Controls.DarkLabel lb_flagPoint;
        private Wallets.UI.Controls.DarkComboBox cb_flagPoint_v;
        private Wallets.UI.Controls.DarkComboBox cb_bonusmultiple;
        private Wallets.UI.Controls.DarkLabel lb_bonusmultiple;
        private Wallets.UI.Controls.DarkLabel lb_fee_addr;
        private Wallets.UI.Controls.DarkLabel lb_pool_addr;
        private Wallets.UI.Controls.DarkLabel lb_bet_addr;
        private Wallets.UI.Controls.DarkLabel lb_banker_addr;
        private Wallets.UI.Controls.DarkButton bt_NewRoom;
        private Wallets.UI.Controls.DarkGroupBox darkGroupBox1;
        private Wallets.UI.Controls.DarkButton bt_add_addr;
        private Wallets.UI.Controls.DarkTextBox tb_sh_to_add;
        private Wallets.UI.Controls.DarkListView lv_members;
        private Wallets.UI.Controls.DarkButton bt_clear;
        private Wallets.UI.Controls.DarkLabel lb_slope;
        private Wallets.UI.Controls.DarkLabel lb_partnerRatio;
        private Wallets.UI.Controls.DarkComboBox cb_slope;
        private Wallets.UI.Controls.DarkTextBox tb_partnerRatio;
        private Wallets.UI.Controls.DarkLabel darkLabel1;
        private Wallets.UI.Controls.DarkComboBox cb_assetKind;
        private Wallets.UI.Controls.DarkLabel lb_assetKind;
        private Wallets.UI.Controls.DarkLabel lb_betFee;
        private Wallets.UI.Controls.DarkComboBox cb_betFee;
        private Wallets.UI.Controls.DarkLabel darkLabel2;
        private Wallets.UI.Controls.DarkLabel darkLabel3;
    }
}