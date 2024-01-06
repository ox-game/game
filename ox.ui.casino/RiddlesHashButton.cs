using OX.Bapps;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using System;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public class RiddlesHashButton : DarkButton
    {
        Module Module;
        RiddlesHash RiddlesHash;
        INotecase Operater;
        public RiddlesHashButton(Module module, INotecase operate, RiddlesHash riddlesHash)
        {
            this.Module = module;
            this.Operater = operate;
            this.RiddlesHash = riddlesHash;
            this.Text = riddlesHash.Index.ToString();
            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this, riddlesHash.VerifyHash.ToString());
            this.Click += RiddlesHashButton_Click;
            this.Width = 150;
            this.Margin = new Padding() { All = 5 };
        }

        private void RiddlesHashButton_Click(object sender, EventArgs e)
        {
            var hash = RiddlesHash.VerifyHash.ToString();
            Clipboard.SetText(hash);
            string msg = hash + UIHelper.LocalString("  已复制", "  copied");
            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });

            DarkMessageBox.ShowInformation(msg, "");
        }
    }
}
