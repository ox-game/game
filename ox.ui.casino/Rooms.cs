using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using OX.Network.P2P;
using OX.Casino;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using OX.Wallets.UI.Forms;

namespace OX.UI.Casino
{
    public partial class Rooms : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public class RoomKeyRecordPledge
        {
            public MixRoom Room;
            public RoomPledgeAccountReply RoomPledgeAccountReply;
        }
        public Module Module { get; set; }
        private INotecase Operater;
        List<Tuple<UInt160, DarkTreeNode>> nodes = new List<Tuple<UInt160, DarkTreeNode>>();
        #region Constructor Region

        public Rooms()
        {
            InitializeComponent();
            this.DockArea = DarkDockArea.Left;
            this.treeRooms.MouseDown += TreeAsset_MouseDown;
        }
        bool AllowBanker(MixRoom rkr)
        {
            if (rkr.Request.Kind == GameKind.EatSmall && rkr.Request.AssetId == Blockchain.OXC) return true;
            //if (rkr.RoomRecord.Kind == GameKind.LuckEatSmall) return true;
            //if (rkr.Request.Kind == GameKind.MarkSix) return true;
            return false;
        }
        private void TreeAsset_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DarkTreeNode[] nodes = treeRooms.SelectedNodes.ToArray();
                if (nodes != null && nodes.Length == 1)
                {
                    var node = nodes.FirstOrDefault();
                    DarkContextMenu menu = new DarkContextMenu();
                    ToolStripMenuItem sm;
                    if (node.NodeType != default)
                    {
                        RoomKeyRecordPledge rkr = node.Tag as RoomKeyRecordPledge;
                        sm = new ToolStripMenuItem(UIHelper.LocalString($"进入房间{rkr.Room.RoomId}", $"Enter Room{rkr.Room.RoomId}"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm_Click;
                        menu.Items.Add(sm);
                        sm = new ToolStripMenuItem(UIHelper.LocalString("复制下注地址", "Copy Bet Address"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm_Click2;
                        menu.Items.Add(sm);
                        sm = new ToolStripMenuItem(UIHelper.LocalString("复制房主地址", "Copy Room Owner Address"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm_Click4;
                        menu.Items.Add(sm);
                        //if (rkr.RoomKeyRecord.RoomRecord.ValidBuryGame())
                        //{
                        //    sm = new ToolStripMenuItem(UIHelper.LocalString("进去埋雷", "Go in Bury"));
                        //    sm.Tag = node.Tag;
                        //    sm.Click += Sm_Click5;
                        //    menu.Items.Add(sm);
                        //}
                        if (AllowBanker(rkr.Room))
                        {
                            var blocks = Game.PeroidBlocks(rkr.Room.Request);
                            if (blocks >= 100)
                            {
                                sm = new ToolStripMenuItem(UIHelper.LocalString("庄家充值", "Recharge for banker"));
                                sm.Tag = node.Tag;
                                sm.Click += SmRecharge_Click;
                                menu.Items.Add(sm);
                                sm = new ToolStripMenuItem(UIHelper.LocalString("庄家提现", "Withdraw for banker"));
                                sm.Tag = node.Tag;
                                sm.Click += SmWithdraw_Click;
                                menu.Items.Add(sm);
                            }
                        }
                        //sm = new ToolStripMenuItem(UIHelper.LocalString("设置房间状态", "Set Room State"));
                        //sm.Tag = node.Tag;
                        //sm.Click += SetRoomState_Click;
                        //menu.Items.Add(sm);
                        if (rkr.Room.Request.Permission == RoomPermission.Private)
                        {
                            sm = new ToolStripMenuItem(UIHelper.LocalString("管理成员", "Manage Members"));
                            sm.Tag = node.Tag;
                            sm.Click += Sm2_Click;
                            menu.Items.Add(sm);
                        }
                        //else
                        //{
                        //    if (rkr.RoomPledgeAccountReply.IsNull())
                        //    {
                        //        sm = new ToolStripMenuItem(UIHelper.LocalString("申请创建OXS众筹质押", "Apply to create OXS crowdfunding pledge"));
                        //        sm.Tag = node.Tag;
                        //        sm.Click += Sm_Click2;
                        //        menu.Items.Add(sm);
                        //    }
                        //    else
                        //    {
                        //        sm = new ToolStripMenuItem(UIHelper.LocalString("查看OXS众筹质押明细", "View OXS crowdfunding pledge detail"));
                        //        sm.Tag = node.Tag;
                        //        sm.Click += Sm_Click4;
                        //        menu.Items.Add(sm);
                        //    }
                        //}
                        sm = new ToolStripMenuItem(UIHelper.LocalString("查询房间数据", "Query Room Data"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm_Click1;
                        menu.Items.Add(sm);
                        //if ((int)node.NodeType == 2)
                        //{                           

                        //}
                    }
                    if (menu.Items.Count > 0)
                        menu.Show(this.treeRooms, e.Location);
                }
            }
        }

        private void Sm_Click4(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            RoomKeyRecordPledge roomMsg = (RoomKeyRecordPledge)ToolStripMenuItem.Tag;
            var str = roomMsg.Room.Holder.ToAddress();
            Clipboard.SetText(str);
            string msg = str + UIHelper.LocalString("  已复制", "  copied");
            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
            DarkMessageBox.ShowInformation(msg, "");
        }

        private void Sm_Click2(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            RoomKeyRecordPledge roomMsg = (RoomKeyRecordPledge)ToolStripMenuItem.Tag;
            var str = roomMsg.Room.BetAddress.ToAddress();
            Clipboard.SetText(str);
            string msg = str + UIHelper.LocalString("  已复制", "  copied");
            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
            DarkMessageBox.ShowInformation(msg, "");
        }

        //private void Sm_Click5(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
        //    RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
        //    if (this.Module is CasinoModule md)
        //    {
        //        md.GoinBury(roomMsg.RoomKeyRecord.RoomKey.RoomId);
        //    }
        //}

        //private void Sm_Click4(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
        //    RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
        //    new GuaranteeQuery(roomMsg.RoomKeyRecord.RoomKey.RoomId).ShowDialog();
        //}

        private void Sm_Click3(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
            try
            {
                var pledgeAdr = Contract.CreateSignatureRedeemScript(roomMsg.RoomPledgeAccountReply.Address).ToScriptHash().ToAddress();
                Clipboard.SetText(pledgeAdr);
                string msg = pledgeAdr + UIHelper.LocalString("  已复制", "  copied");
                Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                OX.Wallets.UI.Forms.DarkMessageBox.ShowInformation(msg, "");
            }
            catch (Exception) { }
        }

        //private void Sm_Click2(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
        //    RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
        //    new CreateCrowdfundingPledge(this.Module, this.Operater, roomMsg.RoomKeyRecord).ShowDialog();
        //}

        private void Sm_Click1(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
            new RoomQuery(roomMsg.Room.BetAddress).ShowDialog();
        }

        //private void SetRoomState_Click(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
        //    RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
        //    if (this.Module is CasinoModule md)
        //    {
        //        md.SetRoomState(roomMsg.RoomKeyRecord.RoomKey.RoomId);
        //    }
        //}
        //private void Sm3_Click(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
        //    RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
        //    if (this.Module is CasinoModule md)
        //    {
        //        md.SetRoomSplit(roomMsg.RoomKeyRecord.RoomKey.RoomId);
        //    }
        //}
        private void Sm_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
            if (this.Module is CasinoModule md)
            {
                md.OpenRoom(roomMsg.Room);
            }
        }
        private void SmRecharge_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
            if (this.Module is CasinoModule md)
            {
                TransferRequest request = new TransferRequest()
                {
                    From = roomMsg.Room.Holder,
                    Asset = roomMsg.Room.Request.AssetId,
                    To = roomMsg.Room.BankerAddress
                };
                CrossBappMessage message = new CrossBappMessage() { MessageType = -1, Attachment = request, From = this.Module.Bapp };
                Bapp.PushCrossBappMessage(message);
            }
        }
        private void SmWithdraw_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
            if (this.Module is CasinoModule md)
            {
                var act = this.Operater.Wallet.GetAccount(roomMsg.Room.Holder);
                if (act.IsNull() || act.WatchOnly) return;
                BankerWithdraw request = new BankerWithdraw() { BetAddress = roomMsg.Room.BetAddress, PublicKey = roomMsg.Room.HolderPubkey };
                SignatureValidator<BankerWithdraw> validtor = new SignatureValidator<BankerWithdraw>() { Target = request, Signature = request.Sign(act.GetKey()) };
                if (validtor.Verify())
                {
                    SingleAskTransactionWrapper stw = new SingleAskTransactionWrapper(act);
                    var pubkey = Bapp.GetBapp<CasinoBapp>().ValidBizScriptHashs.FirstOrDefault();
                    var sh = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash();
                    var tx = this.Operater.Wallet.MakeSingleAskTransaction(stw, sh, (byte)CasinoType.BankerWithdraw, validtor);

                    if (tx.IsNotNull())
                    {
                        this.Operater.SignAndSendTx(tx);
                        if (this.Operater != default)
                        {
                            string msg = $"{UIHelper.LocalString($"{roomMsg.Room.BetAddress.ToAddress()}庄家提现交易已广播", $"Room:{roomMsg.Room.BetAddress.ToAddress()}  relay banker withdraw transaction")}   {tx.Hash}";
                            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });

                            OX.Wallets.UI.Forms.DarkMessageBox.ShowInformation(msg, "");
                        }
                    }
                }
            }
        }
        private void Sm2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            RoomKeyRecordPledge roomMsg = ToolStripMenuItem.Tag as RoomKeyRecordPledge;
            new SignRoomAuth(this.Operater, this.Module, roomMsg.Room).ShowDialog();
        }
        public void Clear()
        {
            this.treeRooms.Nodes.Clear();
        }

        #endregion
        #region IBlockChainTrigger
        public void OnBappEvent(BappEvent be) { }

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
            if (this.Module.Bapp.ContainBizTransaction(block, out BizTransaction[] bts))
            {
                foreach (var tx in bts)
                {
                    if (tx is BillTransaction bt)
                    {
                        foreach (var record in bt.Records)
                        {
                            var bizModel = CasinoBizRecordHelper.BuildModel(record);
                            if (bizModel.IsNotNull() && bizModel.Model is RoomRecord roomRecord)
                            {
                                var key = bizModel.Key.AsSerializable<RoomKey>();
                                if (key.IsNotNull())
                                {
                                    if (this.Operater.Wallet.ContainsAndHeld(Contract.CreateSignatureRedeemScript(key.Holder).ToScriptHash()))
                                    {
                                        DoRooms();
                                    }
                                }
                            }
                        }
                    }
                    else if (tx is ReplyTransaction rt)
                    {
                        var bizshs = this.Module.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();
                        if (rt.GetDataModel<RoomPledgeAccountReply>(bizshs, (byte)CasinoType.RoomPledgeAccountReply, out RoomPledgeAccountReply reply))
                        {
                            DoRooms();
                        }
                    }
                }
            }
            foreach (var tx in block.Transactions)
            {
                foreach (var output in tx.Outputs)
                {
                    var node = this.nodes.FirstOrDefault(m => m.Item1.Equals(output.ScriptHash));
                    if (node.IsNotNull())
                    {
                        this.DoInvoke(() =>
                        {
                            var oxsBalance = getBalance(node.Item1, Blockchain.OXS);
                            var oxcBalance = getBalance(node.Item1, Blockchain.OXC);
                            var msg = UIHelper.LocalString($"OXS余额:{oxsBalance}     OXC余额:{oxcBalance}", $"OXS balance:{oxsBalance}      OXC balance:{oxcBalance}");
                            node.Item2.Text = msg;
                        });
                    }
                }
            }
        }


        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            if (this.Operater.IsNotNull() && this.Operater.Wallet.IsNotNull())
                DoRooms();
        }
        public void OnRebuild()
        {
            this.DoInvoke(() =>
            {
                this.treeRooms.Nodes.Clear();
            });
        }
        Fixed8 getBalance(UInt160 sh, UInt256 asset)
        {
            using var snapshot = Blockchain.Singleton.GetSnapshot();
            var acts = snapshot.Accounts.GetAndChange(sh, () => null);
            return acts.IsNotNull() ? acts.GetBalance(asset) : Fixed8.Zero;
        }
        void DoRooms()
        {
            this.nodes.Clear();
            this.DoInvoke(() =>
            {
                this.treeRooms.Nodes.Clear();
            });
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin != default)
            {
                foreach (var r in bizPlugin.GetWalletRooms().Select(m => new RoomKeyRecordPledge { Room = m }))
                {
                    //var roomState = bizPlugin.GetRoomState(r.RoomKeyRecord.RoomRecord.RoomId);
                    //r.RoomPledgeAccountReply = bizPlugin.Get<RoomPledgeAccountReply>(CasinoBizPersistencePrefixes.Casino_RoomPledgeAccountReply, BitConverter.GetBytes(r.RoomKeyRecord.RoomRecord.RoomId));
                    this.DoInvoke(() =>
                    {
                        var node = new DarkTreeNode(r.Room.RoomId.ToString());
                        node.NodeType = 1;
                        node.Tag = r;
                        var subNode = new DarkTreeNode(UIHelper.LocalString($"房主地址:  {r.Room.Holder.ToAddress()}", $"Holder Address:  {r.Room.Holder.ToAddress()}"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        var address = r.Room.BetAddress.ToAddress();
                        subNode = new DarkTreeNode(UIHelper.LocalString($"下注地址:  {address}", $"Bet Address:  {address}"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        var pooladdress = r.Room.PoolAddress.ToAddress();
                        subNode = new DarkTreeNode(UIHelper.LocalString($"奖池地址:  {pooladdress}", $"Pool Address:  {pooladdress}"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        var feeaddress = r.Room.FeeAddress.ToAddress();
                        subNode = new DarkTreeNode(UIHelper.LocalString($"房费地址:  {feeaddress}", $"Fee Address:  {feeaddress}"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        var bankerddress = r.Room.BankerAddress.ToAddress();
                        subNode = new DarkTreeNode(UIHelper.LocalString($"庄池地址:  {bankerddress}", $"Banker Address:  {bankerddress}"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        var kindItem = new EnumItem<GameKind>(r.Room.Request.Kind);
                        subNode = new DarkTreeNode(UIHelper.LocalString($"娱乐类型:  {kindItem.ToString()}", $"Game Type:  {kindItem.ToString()}"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        var assetState = Blockchain.Singleton.CurrentSnapshot.Assets.TryGet(r.Room.Request.AssetId);
                        if (assetState.IsNotNull())
                        {
                            subNode = new DarkTreeNode(UIHelper.LocalString($"下注资产名:  {assetState.GetName()}", $"Bet Asset Name:  {assetState.GetName()}"));
                            subNode.NodeType = 2;
                            subNode.Tag = r;
                            node.Nodes.Add(subNode);
                            subNode = new DarkTreeNode(UIHelper.LocalString($"下注资产Id:  {r.Room.Request.AssetId.ToString()}", $"Bet Asset Id:  {r.Room.Request.AssetId.ToString()}"));
                            subNode.NodeType = 2;
                            subNode.Tag = r;
                            node.Nodes.Add(subNode);
                        }
                        var stateItem = new EnumItem<RoomPermission>(r.Room.Request.Permission);
                        subNode = new DarkTreeNode(UIHelper.LocalString($"房间状态:  {stateItem.ToString()}", $"Room State:  {stateItem.ToString()}"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        var blocks = Game.PeroidBlocks(r.Room.Request);
                        if (blocks == 10) blocks = 20;
                        subNode = new DarkTreeNode(UIHelper.LocalString($"每局周期: {blocks} 区块", $"Period:  {blocks} blocks"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);

                        var msg = UIHelper.LocalString($"比例:  {r.Room.Request.CommissionValue}/1000", $"Ratio per round:  {r.Room.Request.CommissionValue}/1000");
                        subNode = new DarkTreeNode(msg);
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        var ok = bizPlugin.VerifyPartnerLock(r.Room, out IEnumerable<RoomPartnerLockRecord> validRecords, out Fixed8 haveLockTotal, out Fixed8 needLockTotal, out uint EarliestExpiration);
                        subNode = new DarkTreeNode(UIHelper.LocalString($"合伙人OXS锁仓门槛:{haveLockTotal}/{needLockTotal}", $"Partner OXS Lock Threshold:{haveLockTotal}/{needLockTotal}"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        subNode = new DarkTreeNode(UIHelper.LocalString($"合伙人总分红比例:{r.Room.Request.DividendRatio}%", $"Partner Total dividend proportion:{r.Room.Request.DividendRatio}%"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        subNode = new DarkTreeNode(UIHelper.LocalString($"分红递减坡度:{r.Room.Request.DividentSlope.StringValue()}", $"Divident Diminish Slope:{r.Room.Request.DividentSlope.EngStringValue()}"));
                        subNode.NodeType = 2;
                        subNode.Tag = r;
                        node.Nodes.Add(subNode);
                        //if (r.RoomPledgeAccountReply.IsNotNull())
                        //{
                        //    var pledgeAdr = Contract.CreateSignatureRedeemScript(r.RoomPledgeAccountReply.Address).ToScriptHash();
                        //    subNode = new DarkTreeNode(UIHelper.LocalString($"OXS众筹质押地址:{pledgeAdr.ToAddress()}", $"OXS crowdfunding pledge address:{pledgeAdr.ToAddress()}"));
                        //    subNode.NodeType = 2;
                        //    subNode.Tag = r;
                        //    node.Nodes.Add(subNode);
                        //    subNode = new DarkTreeNode(UIHelper.LocalString($"众筹合伙人合计分红:{r.RoomPledgeAccountReply.DividendRatio}%", $"Total dividends of crowdfunding partners:{r.RoomPledgeAccountReply.DividendRatio}%"));
                        //    subNode.NodeType = 2;
                        //    subNode.Tag = r;
                        //    node.Nodes.Add(subNode);
                        //    var enumItem = new EnumItem<DividentSlope>(r.RoomPledgeAccountReply.DividentSlope);
                        //    subNode = new DarkTreeNode(UIHelper.LocalString($"{EnumHelper.EnumName<DividentSlope>()}:{enumItem.ToString()}", $"{EnumHelper.EnumEngName<DividentSlope>()}:{enumItem.ToString()}"));
                        //    subNode.NodeType = 2;
                        //    subNode.Tag = r;
                        //    node.Nodes.Add(subNode);

                        //    subNode = new DarkTreeNode(UIHelper.LocalString($"质押清算的区块高度:{r.RoomPledgeAccountReply.SettleIndex}", $"Block height of pledge liquidation:{r.RoomPledgeAccountReply.SettleIndex}"));
                        //    subNode.NodeType = 2;
                        //    subNode.Tag = r;
                        //    node.Nodes.Add(subNode);
                        //    var oxsBalance = getBalance(pledgeAdr, Blockchain.OXS);
                        //    var oxcBalance = getBalance(pledgeAdr, Blockchain.OXC);
                        //    subNode = new DarkTreeNode(UIHelper.LocalString($"OXS余额:{oxsBalance}     OXC余额:{oxcBalance}", $"OXS balance:{oxsBalance}      OXC balance:{oxcBalance}"));
                        //    subNode.NodeType = 2;
                        //    subNode.Tag = r;
                        //    node.Nodes.Add(subNode);
                        //    nodes.Add(new Tuple<UInt160, DarkTreeNode>(pledgeAdr, subNode));
                        //}
                        this.treeRooms.Nodes.Add(node);
                    });
                }
            }
        }

        #endregion
    }
}
