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
using OX.UI.Casino;

namespace OX.UI.Bury
{
    public partial class ReplyBuryDetail : DarkForm, IBetWatch
    {
        BuryMergeTx BuryMergeTx;
        BuryRecord BuryRecord;
        public ReplyBuryDetail(BuryRecord buryRecord, BuryMergeTx mergeTx)
        {
            this.BuryRecord = buryRecord;
            this.BuryMergeTx = mergeTx;
            InitializeComponent();
            this.Text = UIHelper.LocalString("验证埋雷", "Verify Bury");
        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.lb_plaincode.Text = UIHelper.LocalString("明码:", "Plain Code:");
            this.lb_ciphercode.Text = UIHelper.LocalString($"暗码:", $"Secret Code:");
            this.lb_rand.Text = UIHelper.LocalString($"随机码:", $"Random Code:");
            this.lb_verifyhash.Text = UIHelper.LocalString($"验证哈希:", $"Verify Hash:");
            this.lb_txhash.Text = UIHelper.LocalString($"交易哈希:", $"Tx Hash:");
            this.bt_verify.Text = UIHelper.LocalString("验证", "Verify");
            this.bt_bet.Text = UIHelper.LocalString("关闭", "Close");

            //this.lb_roomid_v.Text = this.BuryRecord.Request.RoomId.ToString();
            this.lb_plaincode_v.Text = this.BuryRecord.Request.PlainBuryPoint.ToString();
            this.lb_ciphercode_v.Text = this.BuryMergeTx.ReplyBury.PrivateBuryRequest.CipherBuryPoint.ToString();
            this.lb_rand_v.Text = this.BuryMergeTx.ReplyBury.PrivateBuryRequest.Rand.ToString();
            this.lb_verifyhash_v.Text = this.BuryRecord.Request.VerifyHash.ToString();
            this.lb_txhash_v.Text = this.BuryRecord.TxId.ToString();
            foreach (var output in this.BuryMergeTx.Outputs)
            {
                this.flowLayoutPanel1.Controls.Add(new DarkButton { Width = this.flowLayoutPanel1.Width - 20, Height = 30, Text = $"{output.Value.ToString()}   /   {output.ScriptHash.ToAddress()}" });
            }
        }


        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        public void HeartBeat(HeartBeatContext context)
        {

        }

        private void bt_bet_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_verify_Click(object sender, EventArgs e)
        {
            PrivateBuryRequest PrivateBuryRequest = new PrivateBuryRequest { Rand = this.BuryMergeTx.ReplyBury.PrivateBuryRequest.Rand, CipherBuryPoint = this.BuryMergeTx.ReplyBury.PrivateBuryRequest.CipherBuryPoint };
            VerifyPrivateBuryRequest VerifyPrivateBuryRequest = new VerifyPrivateBuryRequest { From = this.BuryRecord.Request.From, PrivateBuryRequest = PrivateBuryRequest };
            string msg = string.Empty;
            if (VerifyPrivateBuryRequest.Hash.Equals(this.BuryRecord.Request.VerifyHash))
            {
                msg = UIHelper.LocalString("验证成功", "Verify Success");
            }
            else
            {
                msg = UIHelper.LocalString("验证失败", "Verify failed");
            }
            DarkMessageBox.ShowInformation(msg, "");
        }
    }
}
