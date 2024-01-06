using OX.Wallets.UI.Config;
using OX.Wallets.UI.Docking;
using OX.Wallets;

namespace OX.UI.Casino
{
    partial class Rooms
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
            this.treeRooms = new OX.Wallets.UI.Controls.DarkTreeView();
            this.SuspendLayout();
            // 
            // treeAsset
            // 
            this.treeRooms.AllowMoveNodes = true;
            this.treeRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeRooms.Location = new System.Drawing.Point(0, 25);
            this.treeRooms.MaxDragChange = 20;
            this.treeRooms.MultiSelect = true;
            this.treeRooms.Name = "treeRooms";
            this.treeRooms.ShowIcons = true;
            this.treeRooms.Size = new System.Drawing.Size(280, 425);
            this.treeRooms.TabIndex = 1;
            this.treeRooms.Text = "darkTreeView1";
            // 
            // DockAsset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeRooms);
            this.DefaultDockArea = OX.Wallets.UI.Docking.DarkDockArea.Right;
            this.DockText = UIHelper.LocalString("我拥有的娱乐房间", "My Own Casino Rooms");
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.Icon = global::OX.GT.Icons.properties_16xLG;
            this.Name = "Rooms";
            this.SerializationKey = "Rooms";
            this.Size = new System.Drawing.Size(280, 450);
            this.ResumeLayout(false);

        }

        #endregion

        private OX.Wallets.UI.Controls.DarkTreeView treeRooms;

    }
}
