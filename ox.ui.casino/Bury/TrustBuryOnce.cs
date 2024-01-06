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
using OX.VM;
using OX.Casino;
using OX.UI.Casino;
using OX.Persistence;

namespace OX.UI.Bury
{
    public partial class TrustBuryOnce : DarkForm, IBetWatch
    {
        public class TrustAccountDescription
        {
            public UInt160 TrustAddress;
            public AssetTrustContract AssetTrustContract;
            public override string ToString()
            {
                return $"{TrustAddress.ToAddress()}  /   {Contract.CreateSignatureRedeemScript(AssetTrustContract.Truster).ToScriptHash().ToAddress()}";
            }
        }
        Module Module;
        INotecase Operater;
        UInt160 BetAddress;
        UInt256 AssetId;
        Fixed8 MinBet;
        public TrustBuryOnce(Module module, INotecase operater, UInt160 betAddress, UInt256 assetId)
        {
            this.Module = module;
            this.Operater = operater;
            this.BetAddress = betAddress;
            this.AssetId = assetId;
            InitializeComponent();

            Dictionary<UInt160, AssetTrustContract> atcts = new Dictionary<UInt160, AssetTrustContract>();
            if (this.Operater.Wallet is OpenWallet openWallet)
            {
                foreach (var act in openWallet.GetAssetTrustContacts())
                {
                    if (act.Value.Targets.Contains(this.BetAddress))
                    {
                        atcts[act.Key] = act.Value;
                    }
                    else if (act.Value.SideScopes.IsNotNullAndEmpty())
                    {
                        foreach (var t in act.Value.SideScopes)
                        {
                            var ssl = Blockchain.Singleton.CurrentSnapshot.GetSides(t);
                            if (ssl.IsNotNull() && ssl.SideStateList.IsNotNullAndEmpty())
                            {
                                if (ssl.SideStateList.Select(m => m.SideScriptHash).Contains(this.BetAddress))
                                {
                                    atcts[act.Key] = act.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (atcts.IsNotNullAndEmpty())
            {
                foreach (var act in atcts)
                {
                    this.cbAccounts.Items.Add(new TrustAccountDescription { TrustAddress = act.Key, AssetTrustContract = act.Value });
                }
                this.cbAccounts.SelectedIndex = 0;
            }
        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.Text = UIHelper.LocalString($"信托埋雷", $"Trust bet in bury");
            this.lb_plainCode.Text = UIHelper.LocalString($"明码:", $"Plain Code:");
            this.lb_cipherCode.Text = UIHelper.LocalString($"暗码:", $"Secret Code:");
            this.lb_balance.Text = UIHelper.LocalString("账户余额:", "Balance:");
            this.lb_betamt.Text = UIHelper.LocalString("埋雷金额:", "Bury Amount:");
            this.lb_from.Text = UIHelper.LocalString("信托账户:", "Trust Account:");
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
            this.tb_balance.Text = "0";
            var ali = this.cbAccounts.SelectedItem as TrustAccountDescription;
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(ali.TrustAddress, () => null);
            if (acts.IsNotNull())
            {
                this.tb_balance.Text = acts.GetBalance(this.AssetId).ToString();
            }
        }

        private void bt_NewRoom_Click(object sender, EventArgs e)
        {

            var ali = this.cbAccounts.SelectedItem as TrustAccountDescription;
            if (ali.IsNull()) return;
            if (Blockchain.Singleton.HeaderHeight <= ali.AssetTrustContract.LastTransferIndex + 10) return;
            if (!byte.TryParse(this.tb_PlainCode.Text, out byte plainCode))
                return;
            if (!byte.TryParse(this.tb_CipherCode.Text, out byte cipherCode))
                return;
            if (!Fixed8.TryParse(this.tb_amount.Text, out Fixed8 amt))
                return;
            var from = this.cbAccounts.SelectedItem as TrustAccountDescription;
            var address = from?.TrustAddress.ToAddress();
            if (address.IsNullOrEmpty())
                return;
            if (!Fixed8.TryParse(this.tb_balance.Text, out Fixed8 balanc))
                return;
            if (amt > balanc)
                return;
            if (amt < MinBet)
            {
                DarkMessageBox.ShowInformation(UIHelper.LocalString($"最低埋雷额为{MinBet}OXC", $"the least bury amount {MinBet}OXC"), "");
                return;
            }
            var trusteeAddress = Contract.CreateSignatureRedeemScript(from.AssetTrustContract.Trustee).ToScriptHash();
            var trustee = this.Operater.Wallet.GetAccount(trusteeAddress);
            if (trustee.IsNotNull() && !trustee.WatchOnly && this.Operater.Wallet is OpenWallet openWallet)
            {
                using (ScriptBuilder sb = new ScriptBuilder())
                {
                    var account = LockAssetHelper.CreateAccount(this.Operater.Wallet as OpenWallet, from.AssetTrustContract.GetContract(), trustee.GetKey());//lock asset account have a some private key with master account
                    if (account != null)
                    {
                        List<UTXO> utxos = new List<UTXO>();
                        foreach (var r in openWallet.GetAssetTrustUTXO().Where(m => m.Value.AssetId.Equals(this.AssetId) && m.Value.ScriptHash.Equals(from.TrustAddress)))
                        {
                            utxos.Add(new UTXO
                            {
                                Address = r.Value.ScriptHash,
                                Value = r.Value.Value.GetInternalValue(),
                                TxId = r.Key.TxId,
                                N = r.Key.N
                            });
                        }
                        List<string> excludedUtxoKeys = new List<string>();
                        if (utxos.SortSearch(amt.GetInternalValue() +2* Fixed8.D, excludedUtxoKeys, out UTXO[] selectedUtxos, out long remainder))
                        {

                            PrivateBuryRequest PrivateBuryRequest = new PrivateBuryRequest { Rand = (uint)new Random().Next(0, int.MaxValue), CipherBuryPoint = cipherCode };
                            VerifyPrivateBuryRequest VerifyPrivateBuryRequest = new VerifyPrivateBuryRequest { From = from.TrustAddress, PrivateBuryRequest = PrivateBuryRequest };
                            var key = trustee.GetKey();
                            if (key.IsNull()) return;
                            var sharekey = key.DiffieHellman(casino.CasinoSettleAccountPubKey);
                            var encryptDataForCipherCode = PrivateBuryRequest.ToArray().Encrypt(sharekey);
                            BuryRequest request = new BuryRequest
                            {
                                From = from.TrustAddress,
                                BetAddress = this.BetAddress,
                                PlainBuryPoint = plainCode,
                                VerifyHash = VerifyPrivateBuryRequest.Hash,
                                CryptoData = encryptDataForCipherCode
                            };

                            List<TransactionOutput> outputs = new List<TransactionOutput>();
                            outputs.Add(new TransactionOutput { AssetId = this.AssetId, Value = amt, ScriptHash = this.BetAddress });
                            if (remainder > 0)
                            {
                                outputs.Add(new TransactionOutput { AssetId = this.AssetId, Value = new Fixed8(remainder), ScriptHash = from.TrustAddress });
                            }
                            List<CoinReference> inputs = new List<CoinReference>();
                            foreach (var utxo in selectedUtxos)
                            {
                                inputs.Add(new CoinReference { PrevHash = utxo.TxId, PrevIndex = utxo.N });
                            }
                            RangeTransaction tx = new RangeTransaction
                            {
                                Attributes = new TransactionAttribute[] {
                                             new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedData, Data =request.ToArray()},
                                            new TransactionAttribute { Usage = TransactionAttributeUsage.Tip5, Data = from.AssetTrustContract.Trustee.EncodePoint(true) },
                                            new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedScriptHash, Data = from.TrustAddress.ToArray() }
                                        },
                                Outputs = outputs.ToArray(),
                                Inputs = inputs.ToArray(),
                                Witnesses = new Witness[0]
                            };
                            tx = LockAssetHelper.Build(tx, new AvatarAccount[] { account });
                            if (tx.IsNotNull())
                            {
                                this.Operater.Wallet.ApplyTransaction(tx);
                                this.Operater.Relay(tx);
                                from.AssetTrustContract.LastTransferIndex = Blockchain.Singleton.HeaderHeight;
                                if (this.Operater != default)
                                {
                                    string msg = UIHelper.LocalString($"广播信托埋雷交易成功  {tx.Hash}", $"Relay  trust bury transaction completed  {tx.Hash}");
                                    //Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                                    DarkMessageBox.ShowInformation(msg, "");
                                    this.Close();
                                }
                            }
                        }

                    }
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
