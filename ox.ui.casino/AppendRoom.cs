using OX.Wallets;
using OX.Wallets.UI.Forms;
using System;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public partial class AppendRoom : DarkDialog
    {

        public AppendRoom()
        {
            InitializeComponent();
        }
        public string RoomId { get { return this.tb_roomId.Text; } }

        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.Text = UIHelper.LocalString("添加房间", "Append Room");
            this.lb_roomId.Text = UIHelper.LocalString("房间Id:", "Room Id:");
        }

        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }



        private void pnlFooter_Paint(object sender, PaintEventArgs e)
        {

        }



    }
}
