
namespace OX.UI.WebAgent
{
    partial class RoomAgentSetting
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
            pl_rooms = new System.Windows.Forms.FlowLayoutPanel();
            tb_rid = new Wallets.UI.Controls.DarkTextBox();
            lb_address = new Wallets.UI.Controls.DarkLabel();
            tb_roomId = new Wallets.UI.Controls.DarkTextBox();
            lb_roomid = new Wallets.UI.Controls.DarkLabel();
            bt_add = new Wallets.UI.Controls.DarkButton();
            bt_clear = new Wallets.UI.Controls.DarkButton();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(166, 18);
            btnCancel.Click += btnCancel_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new System.Drawing.Point(18, 18);
            // 
            // btnYes
            // 
            btnYes.Location = new System.Drawing.Point(18, 18);
            // 
            // btnNo
            // 
            btnNo.Location = new System.Drawing.Point(18, 18);
            // 
            // btnRetry
            // 
            btnRetry.Location = new System.Drawing.Point(708, 18);
            // 
            // btnIgnore
            // 
            btnIgnore.Location = new System.Drawing.Point(708, 18);
            // 
            // pl_rooms
            // 
            pl_rooms.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pl_rooms.AutoScroll = true;
            pl_rooms.Location = new System.Drawing.Point(12, 98);
            pl_rooms.Name = "pl_rooms";
            pl_rooms.Size = new System.Drawing.Size(894, 436);
            pl_rooms.TabIndex = 9;
            // 
            // tb_rid
            // 
            tb_rid.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_rid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_rid.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_rid.Location = new System.Drawing.Point(189, 34);
            tb_rid.Margin = new System.Windows.Forms.Padding(6);
            tb_rid.Name = "tb_rid";
            tb_rid.Size = new System.Drawing.Size(253, 30);
            tb_rid.TabIndex = 11;
            // 
            // lb_address
            // 
            lb_address.AutoSize = true;
            lb_address.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_address.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lb_address.Location = new System.Drawing.Point(31, 36);
            lb_address.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lb_address.Name = "lb_address";
            lb_address.Size = new System.Drawing.Size(86, 24);
            lb_address.TabIndex = 10;
            lb_address.Text = "Claim to:";
            // 
            // tb_roomId
            // 
            tb_roomId.BackColor = System.Drawing.Color.FromArgb(69, 73, 74);
            tb_roomId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tb_roomId.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            tb_roomId.Location = new System.Drawing.Point(1135, 34);
            tb_roomId.Margin = new System.Windows.Forms.Padding(6);
            tb_roomId.Name = "tb_roomId";
            tb_roomId.Size = new System.Drawing.Size(256, 30);
            tb_roomId.TabIndex = 13;
            // 
            // lb_roomid
            // 
            lb_roomid.AutoSize = true;
            lb_roomid.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            lb_roomid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lb_roomid.Location = new System.Drawing.Point(977, 36);
            lb_roomid.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lb_roomid.Name = "lb_roomid";
            lb_roomid.Size = new System.Drawing.Size(86, 24);
            lb_roomid.TabIndex = 12;
            lb_roomid.Text = "Claim to:";
            // 
            // bt_add
            // 
            bt_add.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            bt_add.Location = new System.Drawing.Point(480, 31);
            bt_add.Name = "bt_add";
            bt_add.Padding = new System.Windows.Forms.Padding(5);
            bt_add.Size = new System.Drawing.Size(160, 34);
            bt_add.SpecialBorderColor = null;
            bt_add.SpecialFillColor = null;
            bt_add.SpecialTextColor = null;
            bt_add.TabIndex = 37;
            bt_add.Text = "darkButton1";
            bt_add.Click += bt_add_Click;
            // 
            // bt_clear
            // 
            bt_clear.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            bt_clear.Location = new System.Drawing.Point(746, 31);
            bt_clear.Name = "bt_clear";
            bt_clear.Padding = new System.Windows.Forms.Padding(5);
            bt_clear.Size = new System.Drawing.Size(160, 34);
            bt_clear.SpecialBorderColor = null;
            bt_clear.SpecialFillColor = null;
            bt_clear.SpecialTextColor = null;
            bt_clear.TabIndex = 38;
            bt_clear.Text = "darkButton1";
            bt_clear.Click += bt_clear_Click;
            // 
            // RoomAgentSetting
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(926, 638);
            Controls.Add(bt_clear);
            Controls.Add(bt_add);
            Controls.Add(tb_roomId);
            Controls.Add(lb_roomid);
            Controls.Add(tb_rid);
            Controls.Add(lb_address);
            Controls.Add(pl_rooms);
            DialogButtons = Wallets.UI.Forms.DarkDialogButton.OkCancel;
            Name = "RoomAgentSetting";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "PlayerQuery";
            Load += RoomAgentSetting_Load;
            Controls.SetChildIndex(pl_rooms, 0);
            Controls.SetChildIndex(lb_address, 0);
            Controls.SetChildIndex(tb_rid, 0);
            Controls.SetChildIndex(lb_roomid, 0);
            Controls.SetChildIndex(tb_roomId, 0);
            Controls.SetChildIndex(bt_add, 0);
            Controls.SetChildIndex(bt_clear, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel pl_rooms;
        private Wallets.UI.Controls.DarkTextBox tb_rid;
        private Wallets.UI.Controls.DarkLabel lb_address;
        private Wallets.UI.Controls.DarkTextBox tb_roomId;
        private Wallets.UI.Controls.DarkLabel lb_roomid;
        private Wallets.UI.Controls.DarkButton bt_add;
        private Wallets.UI.Controls.DarkButton bt_clear;
    }
}