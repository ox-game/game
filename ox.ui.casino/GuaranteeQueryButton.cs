using OX.Bapps;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using System;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public class GuaranteeQueryButton : DarkButton
    {
        RoomPledgeGuaranteeRequestAndOutput RPG;
        public GuaranteeQueryButton(RoomPledgeGuaranteeRequestAndOutput rpg)
        {
            this.RPG = rpg;
            this.Text = $"{rpg.Output.Value}  OXS ,   {rpg.CreateRoomGuaranteeRequest.Guarantor.ToAddress()}";
            this.Width = 600;
            this.Height = 50;
            this.Margin = new Padding() { All = 5 };
        }

    }
}
