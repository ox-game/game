using OX.Wallets;
using OX.Wallets.UI.Controls;
using System;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public class RiddlesButton : DarkButton
    {
        Riddles Riddles;
        GuessKey GuessKey;
        INotecase Operater;
        uint Index;
        GameKind GameKind;
        public RiddlesButton(INotecase operate, Riddles riddles, uint index, GuessKey guessKey, GameKind gameKind)
        {
            this.Operater = operate;
            this.Riddles = riddles;
            this.Index = index;
            this.GuessKey = guessKey;
            this.GameKind = gameKind;
            string str = string.Empty;
            switch (gameKind)
            {
                case GameKind.EatSmall:
                    str = $"{index}:      {guessKey.Keys}";
                    break;
                //case GameKind.LuckEatSmall:
                //    str = $"{index}:      {guessKey.SpecialChar}-{guessKey.SpecialPosition}-{guessKey.Keys}";
                //    break;
                //case GameKind.Luck10x:
                //    str = $"{index}:      {guessKey.Keys}";
                //    break;
                //case GameKind.Looting:
                //    str = $"{index}:      {guessKey.SpecialPosition}-{guessKey.Keys}";
                //    break;
                case GameKind.Lotto:
                    str = $"{index}:      {guessKey.SpecialChar}-{guessKey.SpecialPosition}-{guessKey.Keys}";
                    break;
                case GameKind.MarkSix:
                    var cs = guessKey.Keys.ToCharArray();
                    if (cs.Length == 14)
                    {
                        str = $"{index}:      {cs[0]}{cs[1]}-{cs[2]}{cs[3]}-{cs[4]}{cs[5]}-{cs[6]}{cs[7]}-{cs[8]}{cs[9]}-{cs[10]}{cs[11]}+{cs[12]}{cs[13]}";
                    }
                    break;

            }
            this.Text = str;
            ToolTip toolTip1 = new ToolTip();

            //toolTip1.AutoPopDelay = 5000;
            //toolTip1.InitialDelay = 1000;
            //toolTip1.ReshowDelay = 500;
            //toolTip1.ShowAlways = true;
            //toolTip1.SetToolTip(this, riddlesHash.Riddles.Index.ToString());
            this.Click += RiddlesHashButton_Click;
            //this.Width = 150;
            this.Margin = new Padding() { All = 5 };
        }

        private void RiddlesHashButton_Click(object sender, EventArgs e)
        {
            //if (this.Index >= 4309010 && (this.GuessKey.RiddlesKind == RiddlesKind.EatSmall || this.GuessKey.RiddlesKind == RiddlesKind.Lotto))
            //{
            new ShowRiddles(this.Riddles, this.Index, this.GuessKey, this.GameKind).ShowDialog();
            //}
        }
    }
}
