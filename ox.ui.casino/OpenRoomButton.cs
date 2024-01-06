using OX.Bapps;
using OX.Casino;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using System;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public class OpenRoomButton : DarkButton
    {
        Module Module;
        MixRoom Room;
        INotecase Operater;
        public OpenRoomButton(Module module, INotecase operate, MixRoom room)
        {
            this.Module = module;
            this.Operater = operate;
            this.Room = room;
            this.Text = room.RoomId.ToString();
            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this, UIHelper.LocalString(this.Room.Request.Kind.StringValue(), this.Room.Request.Kind.EngStringValue()));
            this.Click += RiddlesHashButton_Click;
            this.Width = 150;
            this.Margin = new Padding() { All = 5 };
        }

        private void RiddlesHashButton_Click(object sender, EventArgs e)
        {
            var roomid = this.Room.RoomId.ToString();
            Clipboard.SetText(roomid);
            string msg = roomid + UIHelper.LocalString("  已复制", "  copied");
            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });

            DarkMessageBox.ShowInformation(msg, "");
        }
    }
}
