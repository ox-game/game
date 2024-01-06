using OX.IO;
using OX.Wallets;
using OX.Wallets.UI.Forms;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using OX.Bapps;

namespace OX.UI.Casino
{
    public partial class VerifyRiddles : DarkDialog
    {
        public UInt160 BetAddress;
        Riddles RiddlesRecord;
        ulong MineNonce;
        public VerifyRiddles(UInt160 betAddres, Riddles riddlesRecord, ulong mineNonce)
        {
            InitializeComponent();
            this.BetAddress = betAddres;
            this.MineNonce = mineNonce;
            this.RiddlesRecord = riddlesRecord;
            this.lb_index_v.Text = riddlesRecord.Index.ToString();
            this.tb_riddles.Text = riddlesRecord.ToArray().ToHexString();
            StringBuilder sb = new StringBuilder();
            foreach (var g in riddlesRecord.GuessKeys)
            {
                var riddlesName = UIHelper.LocalString(g.RiddlesKind.StringValue(), g.RiddlesKind.EngStringValue());
                sb.AppendLine($"{riddlesName} : {g.SpecialPosition}/{g.SpecialChar}/{g.ReRandomSanGongOrLottoInnerString(riddlesRecord.Index, mineNonce)} \n");
            }
            this.tb_riddles_items.Text = sb.ToString();
            var plugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (plugin.IsNotNull())
            {
                var results = plugin.GetRoundClearResults(BetAddress, riddlesRecord.Index);
                this.cb_txhasList.Items.Clear();
                foreach (var result in results)
                {
                    this.cb_txhasList.Items.Add(result.Value.TxHash.ToString());
                }
            }
        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.Text = UIHelper.LocalString("验证谜底", "Verify Riddles");
            this.btnOk.Text = UIHelper.LocalString("确定", "OK");
            this.lb_index.Text = UIHelper.LocalString("区块链高度:", "Blockchain Height:");
            this.lb_riddles.Text = UIHelper.LocalString("谜底数据:", "Riddles Data:");
            this.lb_riddles_hash.Text = UIHelper.LocalString("谜底哈希:", "Riddles Hash:");
            this.bt_verify.Text = UIHelper.LocalString("验证", "Verify");
            this.lb_txhash.Text = UIHelper.LocalString("开奖哈希:", "Prize Hashs:");
            this.lb_mineNonce.Text = UIHelper.LocalString("区块随机数:", "Block Random:");
            this.tb_mineNonce.Text = GuessKey.BuildRiddlesSeed(this.RiddlesRecord.Index, this.MineNonce).ToString();
        }

        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void darkComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bt_NewRoom_Click(object sender, EventArgs e)
        {

        }

        private void cb_period_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_commissionkind_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pnlFooter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bt_copy_Click(object sender, EventArgs e)
        {
            var proof = this.tb_riddles.Text;
            if (proof.IsNotNullAndEmpty())
            {
                try
                {
                    var riddles = proof.HexToBytes().AsSerializable<Riddles>();
                    if (riddles.Hash.ToString() == this.tb_riddles_hash.Text.Trim())
                    {
                        DarkMessageBox.ShowInformation(UIHelper.LocalString("验证成功", "Validation  Success"), "");
                    }
                    else
                    {
                        DarkMessageBox.ShowInformation(UIHelper.LocalString("验证失败", "Validation  Failed"), "");
                    }
                }
                catch (Exception) { DarkMessageBox.ShowInformation(UIHelper.LocalString("验证失败", "Validation Failed"), ""); }
            }
        }

        private void cb_txhasList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var txid = this.cb_txhasList.SelectedItem as string;
            Clipboard.SetText(txid);
            string msg = txid + UIHelper.LocalString("  已复制", "  copied");
            DarkMessageBox.ShowInformation(msg, "");
        }
    }
}
