using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using OX.Wallets.Models;
using OX.Wallets.NEP6;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using OX.Cryptography;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public partial class ShowRiddles : DarkDialog
    {
        Riddles Riddles;
        uint Index;
        GuessKey GuessKey;
        GameKind GameKind;
        public ShowRiddles(Riddles riddles, uint index, GuessKey guessKey, GameKind gameKind)
        {
            InitializeComponent();
            this.Riddles = riddles;
            this.Index = index;
            this.GuessKey = guessKey;
            this.GameKind = gameKind;
        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.Text = Index.ToString();
            this.lb_index.Text = UIHelper.LocalString("区块高度:", "Block Index:");
            this.lb_nonce.Text = UIHelper.LocalString("区块随机数:", "Block Random:");
            this.lb_riddles.Text = UIHelper.LocalString("谜底:", "Riddles:");
            this.bt_copy.Text = UIHelper.LocalString("复制", "Copy");
            this.bt_copyNonce.Text = UIHelper.LocalString("复制", "Copy");
            this.lb_riddlesData.Text = UIHelper.LocalString("谜底数据:", "Riddles Data:");
            this.btnOk.Text = UIHelper.LocalString("确定", "OK");
            this.tb_index.Text = this.Index.ToString();
            this.tb_riddlesData.Text = this.Riddles.ToArray().ToHexString();
            var mineNonce = BlockHelper.GetMineNonce(this.Index);
            if (mineNonce > 0)
            {
                this.tb_nonce.Text = GuessKey.BuildRiddlesSeed(this.Index, mineNonce).ToString();
                var str = string.Empty;
                switch (GameKind)
                {
                    case GameKind.EatSmall:
                        str = $"{GuessKey.ReRandomSanGongOrLottoInnerString(this.Index, mineNonce)}";
                        break;
                    //case GameKind.LuckEatSmall:
                    //    str = $"{GuessKey.SpecialChar}-{GuessKey.SpecialPosition}-{GuessKey.ReRandomSanGongOrLottoInnerString(this.Index, mineNonce)}";
                    //    break;
                    //case GameKind.Luck10x:
                    //    str = $"{GuessKey.ReRandomSanGongOrLottoInnerString(this.Index, mineNonce)}";
                    //    break;
                    //case GameKind.Looting:
                    //    str = $"{GuessKey.SpecialPosition}-{GuessKey.ReRandomSanGongOrLottoInnerString(this.Index, mineNonce)}";
                    //    break;
                    case GameKind.Lotto:
                        str = $"{GuessKey.SpecialChar}-{GuessKey.SpecialPosition}-{GuessKey.ReRandomSanGongOrLottoInnerString(this.Index, mineNonce)}";
                        break;
                }
                this.tb_riddles.Text = str;
            }
        }


        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        public void HeartBeat(HeartBeatContext context)
        {

        }

        private void bt_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.tb_riddlesData.Text);
        }

        private void bt_copyNonce_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.tb_nonce.Text);
        }
    }
}
