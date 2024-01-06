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
using OX.Cryptography.AES;
using OX.Cryptography.ECC;
using System.Security.Principal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OX.Casino;
using System.Runtime;

namespace OX.UI.Casino
{
    public partial class RoomPledgeForm : DarkDialog, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        INotecase Operator;
        MixRoom Room;
        Fixed8 TotalLockVolume;
        Fixed8 RoomOXSMinLockAmount;
        uint PeroidBlocks;
        bool isFull = false;
        public RoomPledgeForm(INotecase notecase, MixRoom room)
        {
            this.Operator = notecase;
            this.Room = room;

            InitializeComponent();
            this.Text = UIHelper.LocalString("房间锁仓入伙", "Room partner lock");
            this.btnOk.Text = UIHelper.LocalString("确定", "OK");
            this.lb_roomId.Text = UIHelper.LocalString("房间号:", "Room Id:");
            this.lb_accounts.Text = UIHelper.LocalString("入伙账户:", "Partner Account:");
            this.lb_amount.Text = UIHelper.LocalString("锁仓金额:", "Lock Amount:");
            this.lb_blockexpire.Text = UIHelper.LocalString("锁仓区块数:", "Lock Blocks:");
            this.lb_betAddress.Text = UIHelper.LocalString("下注地址:", "Bet Address:");
            this.lb_balance.Text = UIHelper.LocalString("OXS 余额:", "Asset Balance:");
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
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin.IsNull()) Close();

            var settings = bizPlugin.GetAllCasinoSettings();
            var RoomOXSMinLock = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.RoomOXSMinLock }));
            if (RoomOXSMinLock.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                Close();
            RoomOXSMinLockAmount = Fixed8.FromDecimal(decimal.Parse(RoomOXSMinLock.Value.Value));

            var PublicRoomPledgePeriod = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PublicRoomPledgePeriod }));
            if (PublicRoomPledgePeriod.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                Close();
            var PublicRoomPledgePeriodBlocks = uint.Parse(PublicRoomPledgePeriod.Value.Value);
            var PrivateRoomPledgePeriod = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PrivateRoomPledgePeriod }));
            if (PrivateRoomPledgePeriod.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                Close();
            var PrivateRoomPledgePeriodBlocks = uint.Parse(PrivateRoomPledgePeriod.Value.Value);

            PeroidBlocks = this.Room.Request.Permission == RoomPermission.Public ? PublicRoomPledgePeriodBlocks : PrivateRoomPledgePeriodBlocks;

            this.btnOk.Enabled = false;
            this.tb_roomId.Text = this.Room.RoomId.ToString();
            this.tb_betAddress.Text = this.Room.BetAddress.ToAddress();

            this.cb_accounts.Items.Clear();
            if (Operator.IsNotNull() && Operator.Wallet.IsNotNull() && Operator.Wallet is OpenWallet openWallet)
            {
                foreach (var act in openWallet.GetHeldAccounts())
                {
                    this.cb_accounts.Items.Add(new AccountListItem(act));
                }
            }
            isFull = bizPlugin.VerifyPartnerLock(this.Room, out IEnumerable<RoomPartnerLockRecord> validRecords, out Fixed8 haveLockTotal, out Fixed8 needLockTotal, out uint EarliestExpiration);
            this.tb_amount.Text = this.RoomOXSMinLockAmount.ToString();
            this.tb_blockexpire.Text = (this.PeroidBlocks + 100).ToString();
            this.lb_total_lock.Text = UIHelper.LocalString($"总计有效入伙锁仓数：{haveLockTotal}/{needLockTotal}", $"Total Valid Partner Lock：{haveLockTotal}/{needLockTotal}");
            if (validRecords.IsNotNullAndEmpty())
            {
                foreach (var record in validRecords.OrderBy(m => m.StartIndex))
                {
                    this.lv_lockrecords.Items.Add(new Wallets.UI.Controls.DarkListItem { Tag = record, Text = $"{record.Amount} OXS , {record.StartIndex}-{record.EndIndex} , {record.Partner.ToAddress()}" });
                }
            }
        }

        private void cb_accounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tb_balance.Clear();
            var item = this.cb_accounts.SelectedItem as AccountListItem;
            if (item.IsNotNull())
                this.tb_balance.Text = this.Operator.Wallet.GeAccountAvailable(item.Account.ScriptHash, Blockchain.OXS).ToString();
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
                AssetId = Blockchain.OXS,
                Value = new BigDecimal(Fixed8.Parse(tb_amount.Text).GetData(), 8).ToFixed8(),
                ScriptHash = Contract.CreateSignatureRedeemScript(ecp).ToScriptHash()
            };
        }
        private void tb_blockexpire_TextChanged(object sender, EventArgs e)
        {
            bool ok = true;
            if (!Fixed8.TryParse(this.tb_amount.Text, out Fixed8 amount) || amount < this.RoomOXSMinLockAmount) ok = false;
            if (!uint.TryParse(this.tb_blockexpire.Text, out uint expiration) || expiration < this.PeroidBlocks + 100) ok = false;
            if (!Fixed8.TryParse(this.tb_balance.Text, out Fixed8 balance) || balance < amount) ok = false;
            this.btnOk.Enabled = ok && !isFull;
        }
    }
}
