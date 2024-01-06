using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Bapps;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OX.UI.Casino.Bury
{
    public class MyBuryHitRecordButton : DarkButton
    {
        BuryMergeTx BuryMergeTx;
        UInt160 Player;

        public MyBuryHitRecordButton(BuryMergeTx bmt, UInt160 player)
        {
            Width = 80;
            Height = 25;
            BuryMergeTx = bmt;
            Player = player;
            var output = bmt.Outputs.FirstOrDefault(m => m.ScriptHash.Equals(player));
            if (output.IsNotNull())
                Text = $"{output.Value}";

            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this, $"{player.ToAddress()}");

            Margin = new Padding() { All = 5 };
        }


    }
}
