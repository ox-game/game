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
using OX.Cryptography.AES;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OX.Casino;
using OX.UI.Casino;

namespace OX.UI.Bury
{
    public partial class BuryOnce : DarkForm, IBetWatch
    {
        Module Module;
        INotecase Operater;
        UInt160 BetAddress;
        UInt256 AssetId;
        Fixed8 MinBet;
        public BuryOnce(Module module, INotecase operater, UInt160 betAddress, UInt256 assetId)
        {
            this.Module = module;
            this.Operater = operater;
            this.BetAddress = betAddress;
            this.AssetId = assetId;
            InitializeComponent();

            foreach (var act in operater.Wallet.GetHeldAccounts())
            {
                this.cbAccounts.Items.Add(new AccountListItem(act));
            }

        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.Text = UIHelper.LocalString("埋雷", "Bury");
            this.lb_plainCode.Text = UIHelper.LocalString($"明码:", $"Plain Code:");
            this.lb_cipherCode.Text = UIHelper.LocalString($"暗码:", $"Secret Code:");
            this.lb_balance.Text = UIHelper.LocalString("账户余额:", "Balance:");
            this.lb_betamt.Text = UIHelper.LocalString("埋雷金额:", "Bury Amount:");
            this.lb_from.Text = UIHelper.LocalString("埋雷账户:", "Bury Account:");
            this.bt_bet.Text = UIHelper.LocalString($"马上埋雷", $"Do Bury");
            Random rd = new Random();
            this.tb_PlainCode.Text = rd.Next(0, 256).ToString();
            this.tb_CipherCode.Text = rd.Next(0, 256).ToString();
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin.IsNotNull())
            {
                var settings = bizPlugin.GetAllCasinoSettings();
                var setting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.BuryMinBet }));
                if (!setting.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                {
                    Fixed8.TryParse(setting.Value.Value, out MinBet);
                }
            }
            this.AcceptButton = bt_bet;
        }


        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        public void HeartBeat(HeartBeatContext context)
        {

        }

        private void darkComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ali = this.cbAccounts.SelectedItem as AccountListItem;
            this.tb_balance.Text = ali.IsNull() ? "0" : this.Operater.Wallet.GeAccountAvailable(ali.Account.Address.ToScriptHash(), this.AssetId).ToString();

        }

        private void bt_NewRoom_Click(object sender, EventArgs e)
        {
            if (!byte.TryParse(this.tb_PlainCode.Text, out byte plainCode))
                return;
            if (!byte.TryParse(this.tb_CipherCode.Text, out byte cipherCode))
                return;
            if (!Fixed8.TryParse(this.tb_amount.Text, out Fixed8 amt))
                return;
            var from = this.cbAccounts.SelectedItem as AccountListItem;
            var address = from?.Account.Address;
            if (address.IsNullOrEmpty())
                return;
            if (!Fixed8.TryParse(this.tb_balance.Text, out Fixed8 balanc))
                return;
            if (amt > balanc)
                return;
            if (amt < MinBet)
            {
                DarkMessageBox.ShowInformation(UIHelper.LocalString($"最低埋雷额为 {MinBet} OXC", $"the least bury amount {MinBet} OXC"), "");
                return;
            }
            TransactionOutput output = new TransactionOutput()
            {
                AssetId = this.AssetId,
                ScriptHash = this.BetAddress,
                Value = amt
            };

            PrivateBuryRequest PrivateBuryRequest = new PrivateBuryRequest { Rand = (uint)new Random().Next(0, int.MaxValue), CipherBuryPoint = cipherCode };
            VerifyPrivateBuryRequest VerifyPrivateBuryRequest = new VerifyPrivateBuryRequest { From = from.Account.ScriptHash, PrivateBuryRequest = PrivateBuryRequest };
            var key = from.Account.GetKey();
            if (key.IsNull()) return;
            var sharekey = key.DiffieHellman(casino.CasinoSettleAccountPubKey);
            var encryptDataForCipherCode = PrivateBuryRequest.ToArray().Encrypt(sharekey);
            BuryRequest request = new BuryRequest
            {
                From = from.Account.ScriptHash,
                BetAddress = this.BetAddress,
                PlainBuryPoint = plainCode,
                VerifyHash = VerifyPrivateBuryRequest.Hash,
                CryptoData = encryptDataForCipherCode
            };
            SingleAskTransactionWrapper stw = new SingleAskTransactionWrapper(from.Account, output);
            var pubkey = Bapp.GetBapp<CasinoBapp>().ValidBizScriptHashs.FirstOrDefault();
            var sh = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash();
            var tx = this.Operater.Wallet.MakeSingleAskTransaction(stw, sh, (byte)CasinoType.Bury, request);

            if (tx.IsNotNull())
            {
                this.Operater.SignAndSendTx(tx);
                if (this.Operater != default)
                {
                    string msg = $"{UIHelper.LocalString($"埋雷交易已广播", $" relay burying transaction")}   {tx.Hash}";
                    Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                    DarkMessageBox.ShowInformation(msg, "");
                    this.Close();
                }
            }
        }

        private void tb_amount_TextChanged(object sender, EventArgs e)
        {
            if (!Fixed8.TryParse(this.tb_amount.Text, out Fixed8 amt))
            {
                var s = this.tb_amount.Text;
                if (s.Length > 0)
                {
                    s = s.Substring(0, s.Length - 1);
                    this.tb_amount.Clear();
                    this.tb_amount.AppendText(s);
                }
            }
            if (Fixed8.TryParse(this.tb_balance.Text, out Fixed8 balanc))
            {
                if (amt > balanc)
                {
                    var s = this.tb_amount.Text;
                    if (s.Length > 0)
                    {
                        s = s.Substring(0, s.Length - 1);
                        this.tb_amount.Clear();
                        this.tb_amount.AppendText(s);
                    }
                }
            }
        }

        private void tb_PlainCode_TextChanged(object sender, EventArgs e)
        {
            if (!byte.TryParse(this.tb_PlainCode.Text, out byte code))
            {
                var s = this.tb_PlainCode.Text;
                if (s.Length > 0)
                {
                    s = s.Substring(0, s.Length - 1);
                    this.tb_PlainCode.Clear();
                    this.tb_PlainCode.AppendText(s);
                }
            }
        }

        private void tb_CipherCode_TextChanged(object sender, EventArgs e)
        {
            if (!byte.TryParse(this.tb_CipherCode.Text, out byte code))
            {
                var s = this.tb_CipherCode.Text;
                if (s.Length > 0)
                {
                    s = s.Substring(0, s.Length - 1);
                    this.tb_CipherCode.Clear();
                    this.tb_CipherCode.AppendText(s);
                }
            }
        }
    }
}
