using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using OX.Wallets.Models;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public partial class NewRoomForm : DarkForm, INotecaseTrigger, IModuleComponent
    {
        public class AssetItem
        {
            public UInt256 AssetId { get; private set; }
            public string AssetName { get; private set; }
            public AssetItem(UInt256 assetid, string assetName)
            {
                this.AssetId = assetid;
                this.AssetName = assetName;
            }
            public override bool Equals(object obj)
            {
                if (obj is AssetItem ai)
                {
                    return ai.AssetId == this.AssetId;
                }
                return base.Equals(obj);
            }
            public override int GetHashCode()
            {
                return this.AssetId.GetHashCode();
            }
            public override string ToString()
            {
                return $"{this.AssetName}     {this.AssetId.ToString()}";
            }
        }
        INotecase Operater;
        public Module Module { get; set; }
        public NewRoomForm()
        {
            InitializeComponent();
        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.Text = UIHelper.LocalString("注册娱乐房间", "Register Casino Room");
            this.lb_roomkind.Text = UIHelper.LocalString("房间游戏类型:", "Room Game Type:");
            this.lb_period.Text = UIHelper.LocalString("游戏速度:", "Game Speed:");
            this.lb_commissionvalue.Text = UIHelper.LocalString("抽佣值:", "Commission Value:");
            this.lb_from.Text = UIHelper.LocalString("持房账户:", "Holder Address:");
            this.lb_roomState.Text = UIHelper.LocalString("房间状态:", "Room State:");
            this.bt_NewRoom.Text = UIHelper.LocalString("马上注册", "Register Now");
            this.lb_flagPoint.Text = UIHelper.LocalString("标记位:", "Flag Point:");
            this.lb_bonusmultiple.Text = UIHelper.LocalString("奖励倍数:", "Bonus Multiple:");
            this.darkGroupBox1.Text = UIHelper.LocalString("白名单", "White List");
            this.bt_add_addr.Text = UIHelper.LocalString("增加", "Add");
            this.bt_clear.Text = UIHelper.LocalString("清空", "Clear");
            this.lb_partnerRatio.Text = UIHelper.LocalString("总分红比例:", "Total dividend proportion:");
            this.lb_slope.Text = UIHelper.LocalString("分红递减坡度:", "Divident Diminish Slope:");
            this.lb_assetKind.Text = UIHelper.LocalString("下注资产:", "Bet Asset:");
            this.lb_betFee.Text = UIHelper.LocalString("单注费:", "Bet Fee:");
            this.lb_bet_addr.Text = string.Empty;
            this.lb_pool_addr.Text = string.Empty;
            this.lb_fee_addr.Text = string.Empty;
            this.lb_banker_addr.Text = string.Empty;
            this.cb_flagPoint_v.Items.Clear();
            for (int i = 0; i < 100; i++)
            {
                this.cb_betFee.Items.Add(i.ToString());
            }
            this.cb_betFee.SelectedIndex = 0;
            for (int i = 0; i < 10; i++)
            {
                this.cb_flagPoint_v.Items.Add(i.ToString());
            }
            this.cb_flagPoint_v.SelectedIndex = 0;
            this.cb_gamekind.Items.Clear();
            foreach (var gk in EnumHelper.All<GameKind>().Valid())
            {
                this.cb_gamekind.Items.Add(new EnumItem<GameKind>(gk));
            }
            this.cb_gamekind.SelectedIndex = 0;

            this.cb_period.Items.Clear();
            foreach (var gk in EnumHelper.All<GameSpeed>())
            {
                this.cb_period.Items.Add(new EnumItem<GameSpeed>(gk));
            }
            this.cb_period.SelectedIndex = 0;

            this.cb_roomState.Items.Clear();
            foreach (var gk in EnumHelper.All<RoomPermission>())
            {
                this.cb_roomState.Items.Add(new EnumItem<RoomPermission>(gk));
            }
            this.cb_roomState.SelectedIndex = 0;
            this.nu_commissionvalue.Value = 1;
            this.cb_bonusmultiple.Items.Clear();
            for (int i = 2; i < 10; i++)
            {
                this.cb_bonusmultiple.Items.Add(i.ToString());
            }
            this.cb_bonusmultiple.SelectedIndex = 0;
            this.cb_slope.Items.Clear();
            foreach (var gk in EnumHelper.All<DividentSlope>())
            {
                this.cb_slope.Items.Add(new EnumItem<DividentSlope>(gk));
            }
            this.cb_slope.SelectedIndex = 0;
            resetAsset();
        }

        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        public void OnBappEvent(BappEvent be) { }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
        }
        public void HeartBeat(HeartBeatContext context)
        {

        }
        public void OnFlashMessage(FlashMessage flashMessage)
        {

        }
        public void OnBlock(Block block)
        {
        }
        public void BeforeOnBlock(Block block) { }
        public void AfterOnBlock(Block block) { }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            initAccounts();
        }
        public void OnRebuild() { }
        void initAccounts()
        {
            if (this.Operater.IsNotNull())
            {
                var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                if (bizPlugin.IsNotNull())
                {
                    var roomHolders = bizPlugin.AllRooms.Select(m => m.Holder);
                    this.DoInvoke(() =>
                    {
                        this.cbAccounts.Items.Clear();
                        foreach (var act in this.Operater.Wallet.GetHeldAccounts())
                        {
                            if (!roomHolders.Contains(act.ScriptHash))
                                this.cbAccounts.Items.Add(new AccountListItem(act));
                        }
                    });
                }
            }
        }

        private void darkComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var from = this.cbAccounts.SelectedItem as AccountListItem;
            var addr = BuildAddress(0, from).ToAddress();
            this.lb_bet_addr.Text = UIHelper.LocalString($"房间下注地址：{addr}", $"Room Bet Address: {addr}");
            addr = BuildAddress(1, from).ToAddress();
            this.lb_pool_addr.Text = UIHelper.LocalString($"奖金池地址：{addr}", $"Bonus Pool Address: {addr}");
            addr = BuildAddress(2, from).ToAddress();
            this.lb_fee_addr.Text = UIHelper.LocalString($"管理费地址：{addr}", $"Fee Address: {addr}");
            addr = BuildAddress(3, from).ToAddress();
            this.lb_banker_addr.Text = UIHelper.LocalString($"房主托管地址：{addr}", $"Owner Deposit Address: {addr}");
        }
        UInt160 BuildAddress(byte flag, AccountListItem ai)
        {
            var tx = new SlotSideTransaction()
            {
                 Slot = casino.CasinoMasterAccountPubKey,
                SideType = SideType.PublicKey,
                Data = ai.Account.GetKey().PublicKey.ToArray(),
                Flag = flag,
                AuthContract = Blockchain.SideAssetContractScriptHash,
                Attributes = new TransactionAttribute[0],
                Outputs = new TransactionOutput[0],
                Inputs = new CoinReference[0]
            };
            return tx.GetContract().ScriptHash;
        }

        private void bt_NewRoom_Click(object sender, EventArgs e)
        {
            if (this.cbAccounts.Text.IsNotNullAndEmpty())
            {
                if (!byte.TryParse(this.tb_partnerRatio.Text, out byte div) || div > 100 || div == 0) return;
                var slope = this.cb_slope.SelectedItem as EnumItem<DividentSlope>;
                var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                if (bizPlugin.IsNull())
                    return;
                var settings = bizPlugin.GetAllCasinoSettings();
                var publicfeesetting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PublicRoomFee }));
                if (publicfeesetting.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                    return;
                var privatefeesetting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PrivateRoomFee }));
                if (privatefeesetting.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                    return;
                var publicfee = Fixed8.FromDecimal(decimal.Parse(publicfeesetting.Value.Value));
                var privatefee = Fixed8.FromDecimal(decimal.Parse(privatefeesetting.Value.Value));
                var from = this.cbAccounts.SelectedItem as AccountListItem;
                var roomState = (this.cb_roomState.SelectedItem as EnumItem<RoomPermission>).Target;
                var balance = this.Operater.Wallet.GeAccountAvailable(from.Account.ScriptHash, Blockchain.OXC);
                var fee = roomState == RoomPermission.Public ? publicfee : privatefee;
                var assetId = Blockchain.OXC;
                if (roomState == RoomPermission.Private)
                {
                    var si = this.cb_assetKind.SelectedItem;
                    if (si.IsNull()) return;
                    var siitem = si as AssetItem;
                    if (siitem.AssetId == UInt256.Zero) return;
                    assetId = siitem.AssetId;
                }
                if (balance >= fee + Fixed8.One * 2)
                {
                    RegRoomRequest request = new RegRoomRequest();
                    var kind = this.cb_gamekind.SelectedItem as EnumItem<GameKind>;
                    var speed = this.cb_period.SelectedItem as EnumItem<GameSpeed>;

                    var v = (byte)this.nu_commissionvalue.Value;
                    request.AssetId = assetId;
                    request.Level = 1;
                    request.CommissionValue = v;
                    request.Kind = kind.Target;
                    request.Permission = roomState;
                    request.GameSpeed = speed.Target;
                    request.Flag = byte.Parse(cb_flagPoint_v.Text);
                    request.BonusMultiple = byte.Parse(cb_bonusmultiple.Text);
                    request.DividendRatio = div;
                    request.DividentSlope = slope.Target;
                    request.DataKind = 0;
                    if (roomState == RoomPermission.Private)
                    {
                        List<UInt160> list = new List<UInt160>();
                        foreach (var item in this.lv_members.Items)
                        {
                            var sh = item.Tag as UInt160;
                            if (sh.IsNotNull() && !list.Contains(sh)) list.Add(sh);
                        }
                        byte betFee = byte.Parse(this.cb_betFee.SelectedItem as string);
                        if (list.IsNotNullAndEmpty() || betFee > 0)
                        {
                            RoomMemberSetting rms = new RoomMemberSetting { Flag = betFee, Members = list.ToArray() };
                            request.DataKind = 1;
                            request.Data = rms.ToArray();
                        }
                    }

                    var tx = new SlotSideTransaction()
                    {
                        Slot = casino.CasinoMasterAccountPubKey,
                        SideType = SideType.PublicKey,
                        Data = from.Account.GetKey().PublicKey.ToArray(),
                        Flag = 0,
                        AuthContract = Blockchain.SideAssetContractScriptHash,
                        Attributes = new TransactionAttribute[0],
                        Outputs = new TransactionOutput[0],
                        Inputs = new CoinReference[0],
                        Attach = request.ToArray()
                    };
                    var addr = tx.GetContract().ScriptHash;

                    List<TransactionOutput> outputs = new List<TransactionOutput>();

                    outputs.Add(new TransactionOutput
                    {
                        ScriptHash = casino.CasinoMasterAccountAddress,
                        AssetId = Blockchain.OXC,
                        Value = fee
                    });
                    outputs.Add(new TransactionOutput
                    {
                        ScriptHash = addr,
                        AssetId = Blockchain.OXC,
                        Value = Fixed8.One
                    });
                    tx.Outputs = outputs.ToArray();
                    tx = this.Operater.Wallet.MakeTransaction(tx, from.Account.ScriptHash, from.Account.ScriptHash);

                    if (tx.IsNotNull())
                    {
                        this.Operater.SignAndSendTx(tx);
                        if (this.Operater != default)
                        {
                            string msg = $"{UIHelper.LocalString("广播娱乐城创建房间交易", "Relay casino create game room transaction")}   {tx.Hash}";
                            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });

                            DarkMessageBox.ShowInformation(msg, "");
                        }
                        this.Close();
                    }
                }
                else
                {
                    string msg = UIHelper.LocalString($"至少需要{fee} OXC余额注册游戏房间", $"At least {fee} OXC  balance is required to register game room");
                    Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });

                    DarkMessageBox.ShowError(msg, "");
                }
            }
        }

        private void cb_period_SelectedIndexChanged(object sender, EventArgs e)
        {
            var kind = this.cb_gamekind.SelectedItem as EnumItem<GameKind>;
            var mutiple = this.cb_period.SelectedItem as EnumItem<GameSpeed>;
            if (kind.IsNotNull() && mutiple.IsNotNull())
            {
                var blocks = Game.MinBlocks(kind.Target) * Game.MutlpleValue(mutiple.Target);
                if (blocks == 10)
                    blocks = blocks * 2;
                var msg = UIHelper.LocalString($"每局游戏间隔为{blocks}个区块,大约{(blocks * ProtocolSettings.Default.SecondsPerBlock) / 60}分钟", $"Every game is divided into {blocks} blocks, about {(blocks * ProtocolSettings.Default.SecondsPerBlock) / 60} minutes");
                this.lb_msg.Text = msg;
            }
            var showMultiple = kind.Target == GameKind.EatSmall;
            this.lb_bonusmultiple.Visible = showMultiple;
            this.cb_bonusmultiple.Visible = showMultiple;
        }

        private void cb_commissionkind_SelectedIndexChanged(object sender, EventArgs e)
        {
            int v = (int)this.nu_commissionvalue.Value;
            var msg = UIHelper.LocalString($"每局抽佣{v}/1000的投注额", $"Bet amount of {v}/1000 drawn per round");
            this.lb_commissionmsg.Text = msg;
            var ei = this.cb_roomState.SelectedItem as EnumItem<RoomPermission>;
            if (ei.IsNotNull())
            {
                this.darkGroupBox1.Visible = ei.Target == RoomPermission.Private;
                this.lb_assetKind.Visible = ei.Target == RoomPermission.Private;
                this.cb_assetKind.Visible = ei.Target == RoomPermission.Private;
                this.lb_betFee.Visible = ei.Target == RoomPermission.Private;
                this.cb_betFee.Visible = ei.Target == RoomPermission.Private;
                this.darkLabel2.Visible = ei.Target == RoomPermission.Private;
            }
        }
        void resetAsset()
        {
            this.cb_assetKind.Items.Clear();
            this.cb_assetKind.Items.Add(new AssetItem(UInt256.Zero, UIHelper.LocalString("未选择", "No select")));
            foreach (var assetState in Blockchain.Singleton.Store.GetAssets().Find().Where(m => !m.Key.Equals(Blockchain.OXS)).OrderByDescending(m => m.Key == Blockchain.OXC))
            {
                this.cb_assetKind.Items.Add(new AssetItem(assetState.Key, assetState.Value.GetName()));
            }
            this.cb_assetKind.SelectedIndex = 0;
        }
        private void bt_clear_Click(object sender, EventArgs e)
        {
            this.lv_members.Items.Clear();
        }

        private void bt_add_addr_Click(object sender, EventArgs e)
        {
            var s = this.tb_sh_to_add.Text;
            if (s.IsNotNullAndEmpty())
            {
                try
                {
                    var sh = s.ToScriptHash();
                    bool find = false;
                    foreach (var item in this.lv_members.Items)
                    {
                        var shItem = item.Tag as UInt160;
                        if (shItem.Equals(sh)) find = true;
                    }
                    if (!find)
                    {
                        this.lv_members.Items.Add(new Wallets.UI.Controls.DarkListItem { Tag = sh, Text = sh.ToAddress() });
                        if (this.lv_members.Items.Count >= 100) this.bt_add_addr.Enabled = false;
                    }
                }
                catch
                {
                    this.tb_sh_to_add.Clear();
                    DarkMessageBox.ShowError(UIHelper.LocalString("地址重复或者错误", "Duplicate or incorrect address"), "");
                }
            }

        }

        private void tb_partnerRatio_TextChanged(object sender, EventArgs e)
        {
            if (!uint.TryParse(this.tb_partnerRatio.Text, out uint div) || div > 100 || div == 0)
            {
                var s = this.tb_partnerRatio.Text;
                if (s.Length > 0)
                {
                    s = s.Substring(0, s.Length - 1);
                    this.tb_partnerRatio.Clear();
                    this.tb_partnerRatio.AppendText(s);
                }
            }
        }
    }
}
