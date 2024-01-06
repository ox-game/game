using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Bapps;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace OX.UI.Casino.Bury
{
    public class MyBuryRecordButton : DarkButton
    {
        BuryRecord BuryRecord;

        public MyBuryRecordButton(BuryRecord br)
        {
            Width = 80;
            Height = 25;
            BuryRecord = br;
            Text = $"{br.BuryAmount}";

            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this, $"{br.Request.PlainBuryPoint}  :  {br.Request.From.ToAddress()}");

            Margin = new Padding() { All = 5 };
        }


    }
}
