using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets;
using OX.Wallets.UI.Forms;
using OX.Bapps;
using OX.IO.Data.LevelDB;
using Akka.Pattern;
using OX.Wallets.UI.Controls;
using OX.UI.Casino;

namespace OX.UI.WebAgent
{
    public partial class RoomAgentSetting : DarkDialog
    {
        WebAgentModule Module;
        List<uint> AgentRooms = new List<uint>();
        public RoomAgentSetting(WebAgentModule module)
        {
            this.Module = module;
            InitializeComponent();
            this.Text = UIHelper.LocalString("管理代理房间", "Manage Agent Rooms");
            this.btnCancel.Text = UIHelper.LocalString("关闭", "Close");
            this.btnOk.Text = UIHelper.LocalString("确定", "OK");
            this.bt_add.Text = UIHelper.LocalString("增加", "Add");
            this.bt_clear.Text = UIHelper.LocalString("清空", "Clear");
            this.lb_address.Text = UIHelper.LocalString("房间号:", "Room Id:");
            this.lb_roomid.Text = UIHelper.LocalString("房间号:", "Room Id:");
        }




        private void bt_add_Click(object sender, EventArgs e)
        {
            if (uint.TryParse(this.tb_rid.Text, out uint roomId))
            {
                if (!this.AgentRooms.Contains(roomId))
                {
                    var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                    if (bizPlugin.IsNotNull())
                    {
                        var room = bizPlugin.AllRooms.FirstOrDefault(m => m.RoomId == roomId);
                        if (room.IsNotNull() && (room.Request.Kind == GameKind.Lotto || room.Request.Kind == GameKind.EatSmall))
                        {
                            this.AgentRooms.Add(roomId);
                            this.pl_rooms.Controls.Add(new DarkButton { Text = roomId.ToString(), Height = 30, Width = 150 });
                        }
                    }
                }
            }
        }

        private void bt_clear_Click(object sender, EventArgs e)
        {
            this.pl_rooms.Controls.Clear();
            this.AgentRooms.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RoomAgentSetting_Load(object sender, EventArgs e)
        {

            this.AgentRooms.AddRange(this.Module.Rooms);
            foreach (var r in this.Module.Rooms)
            {
                this.pl_rooms.Controls.Add(new DarkButton { Text = r.ToString(), Height = 30, Width = 150 });
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Module.Rooms = this.AgentRooms;
            this.Module.SaveSetting();
            this.Close();
        }
    }
}
