using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets.UI;
using OX.Wallets.UI.Forms;
using OX.Wallets;
using OX.Bapps;
using OX.Network.P2P.Payloads;
using OX.Wallets.Models;
using OX.Ledger;
using OX.IO;
using OX.SmartContract;
using OX.Cryptography;
using OX.Cryptography.ECC;
using System.Security.Principal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OX.UI.GameMining
{
    public partial class SeedSelfForm : DarkDialog, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        INotecase Operator;
        UInt256 AssetId;
        Fixed8 MinAmount;
        uint MinExpire;
        byte Position;
        uint BetIndex;
        public SeedSelfForm(INotecase notecase, uint betIndex, byte position, UInt256 assetId, Fixed8 minAmount, uint minExpire)
        {
            this.Operator = notecase;
            this.BetIndex = betIndex;
            this.Position = position;
            this.AssetId = assetId;
            this.MinAmount = minAmount;
            this.MinExpire = minExpire;
            InitializeComponent();
            this.Text = UIHelper.LocalString("锁仓种矿", "Lock Seed");
            this.btnOk.Text = UIHelper.LocalString("确定", "OK");
            this.lb_assetName.Text = UIHelper.LocalString("资产名:", "Asset Name:");
            this.lb_accounts.Text = UIHelper.LocalString("账户:", "Account:");
            this.lb_amount.Text = UIHelper.LocalString("锁仓金额:", "Lock Amount:");
            this.lb_blockexpire.Text = UIHelper.LocalString("锁仓区块高度:", "Lock Block Index:");
            this.lb_assetId.Text = UIHelper.LocalString("资产Id:", "Asset Id:");
            this.lb_balance.Text = UIHelper.LocalString("主余额:", "Master Balance:");
            this.lb_position.Text = UIHelper.LocalString($"{betIndex}种矿{position}号位", $"Seed  position {position} on {betIndex}");
        }
        #region IBlockChainTrigger
        public void OnBappEvent(BappEvent be)
        {

        }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
        }
        public void HeartBeat(HeartBeatContext context)
        {

        }
        public void BeforeOnBlock(Block block) { }
        public void OnBlock(Block block) { }
        public void AfterOnBlock(Block block)
        {

        }
        public void ChangeWallet(INotecase operater)
        {

            this.Operator = operater;

        }
        public void OnRebuild()
        {

        }
        #endregion

        private void RegMinerForm_Load(object sender, EventArgs e)
        {
            this.btnOk.Enabled = false;
            var assetState = Blockchain.Singleton.Store.GetAssets().TryGet(this.AssetId);
            if (assetState.IsNotNull())
            {
                this.tb_assetName.Text = assetState.GetName();
                this.tb_assetId.Text = this.AssetId.ToString();
            }
            this.cb_accounts.Items.Clear();
            if (Operator.IsNotNull() && Operator.Wallet.IsNotNull() && Operator.Wallet is OpenWallet openWallet)
            {
                foreach (var act in openWallet.GetHeldAccounts())
                {
                    this.cb_accounts.Items.Add(new AccountListItem(act));
                }
            }
            this.tb_amount.Text = this.MinAmount.ToString();
            this.tb_blockexpire.Text = this.MinExpire.ToString();
        }

        private void cb_accounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tb_balance.Clear();
            var item = this.cb_accounts.SelectedItem as AccountListItem;
            if (item.IsNotNull())
                this.tb_balance.Text = this.Operator.Wallet.GeAccountAvailable(item.Account.ScriptHash, this.AssetId).ToString();
            tb_blockexpire_TextChanged(null, null);
        }
        public TransactionOutput GetOutput(out ECPoint ecp, out uint expiration)
        {
            ecp = default;
            expiration = 0;
            var item = this.cb_accounts.SelectedItem as AccountListItem;
            if (item.IsNull()) return default;
            ecp = item.Account.GetKey().PublicKey;
            if (!uint.TryParse(this.tb_blockexpire.Text, out expiration)) return default;
            if (!Fixed8.TryParse(this.tb_amount.Text, out Fixed8 amount)) return default;
            return new TransactionOutput
            {
                AssetId = this.AssetId,
                Value = new BigDecimal(Fixed8.Parse(tb_amount.Text).GetData(), 8).ToFixed8(),
                ScriptHash = Contract.CreateSignatureRedeemScript(ecp).ToScriptHash()
            };
        }


        private void tb_blockexpire_TextChanged(object sender, EventArgs e)
        {
            bool ok = true;
            if (!Fixed8.TryParse(this.tb_amount.Text, out Fixed8 amount) || amount < MinAmount ) ok = false;
            if (!uint.TryParse(this.tb_blockexpire.Text, out uint expiration) || expiration < MinExpire) ok = false;
            if (!Fixed8.TryParse(this.tb_balance.Text, out Fixed8 balance) || balance < amount) ok = false;
            this.btnOk.Enabled = ok;
        }
    }
}
