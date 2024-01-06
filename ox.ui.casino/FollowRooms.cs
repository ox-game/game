using OX.Bapps;
using OX.IO;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.NEP6;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using System;
using System.Linq;
using System.Windows.Forms;
using OX.SmartContract;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OX.Wallets.UI.Forms;
using OX.Casino;
using OX.UI.GameMining;
using OX.Cryptography.ECC;
using OX.Ledger;
using System.Collections.Generic;

namespace OX.UI.Casino
{
    public partial class FollowRooms : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        private INotecase Operater;
        #region Constructor Region

        public FollowRooms()
        {
            InitializeComponent();
            this.DockArea = DarkDockArea.Left;
            this.treeRooms.MouseDown += TreeAsset_MouseDown;

        }

        private void TreeAsset_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DarkContextMenu menu = new DarkContextMenu();
                ToolStripMenuItem sm;
                DarkTreeNode[] nodes = treeRooms.SelectedNodes.ToArray();
                if (nodes != null && nodes.Length == 1)
                {
                    var node = nodes.FirstOrDefault();
                    if (node.NodeType != default)
                    {
                        MixRoom roomMsg = (MixRoom)node.Tag;
                        sm = new ToolStripMenuItem(UIHelper.LocalString($"进入房间{roomMsg.RoomId}", $"Enter Room{roomMsg.RoomId}"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm_Click;
                        menu.Items.Add(sm);
                        sm = new ToolStripMenuItem(UIHelper.LocalString("复制下注地址", "Copy Bet Address"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm_Click2;
                        menu.Items.Add(sm);
                        sm = new ToolStripMenuItem(UIHelper.LocalString("复制房主地址", "Copy Room Owner Address"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm_Click3;
                        menu.Items.Add(sm);
                        sm = new ToolStripMenuItem(UIHelper.LocalString("成为房间合伙人", "Become Room Partner"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm_Click4;
                        menu.Items.Add(sm);

                        sm = new ToolStripMenuItem(UIHelper.LocalString("取消关注", "Unfollow"));
                        sm.Tag = node.Tag;
                        sm.Click += Sm1_Click;
                        menu.Items.Add(sm);
                        //if (node.Tag is MixRoom roomKeyRecord)
                        //{
                        //    if (roomKeyRecord.RoomRecord.State == RoomPermission.Private)
                        //    {
                        //        sm = new ToolStripMenuItem(UIHelper.LocalString("管理房间授权", "Manage Room Authorization"));
                        //        sm.Tag = node.Tag;
                        //        sm.Click += Sm2_Click;
                        //        menu.Items.Add(sm);
                        //    }
                        //}

                        //if ((int)node.NodeType == 2)
                        //{                           

                        //}
                    }

                }
                sm = new ToolStripMenuItem(UIHelper.LocalString("添加房间", "Append Room"));
                sm.Click += Sm3_Click;
                menu.Items.Add(sm);
                if (menu.Items.Count > 0)
                    menu.Show(this.treeRooms, e.Location);
            }
        }

        private void Sm_Click3(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            MixRoom roomMsg = (MixRoom)ToolStripMenuItem.Tag;
            var str = roomMsg.Holder.ToAddress();
            Clipboard.SetText(str);
            string msg = str + UIHelper.LocalString("  已复制", "  copied");
            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
            DarkMessageBox.ShowInformation(msg, "");
        }

        private void Sm_Click4(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            MixRoom roomMsg = (MixRoom)ToolStripMenuItem.Tag;
            using (var dialog = new RoomPledgeForm(this.Operater, roomMsg))
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                var output = dialog.GetOutput(out ECPoint ecp, out uint expiration);
                LockAssetTransaction lat = new LockAssetTransaction
                {
                    LockContract = Blockchain.LockAssetContractScriptHash,
                    IsTimeLock = false,
                    LockExpiration = Blockchain.Singleton.HeaderHeight + expiration,
                    Recipient = ecp
                };

                TransactionAttribute attr = new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedScriptHash, Data = roomMsg.BetAddress.ToArray() };
                var from = Contract.CreateSignatureRedeemScript(ecp).ToScriptHash();
                output.ScriptHash = lat.GetContract().ScriptHash;
                lat.Outputs = new TransactionOutput[] { output };
                lat.Attributes = new TransactionAttribute[] { attr };
                lat = this.Operater.Wallet.MakeTransaction(lat, from, from);
                if (lat != null)
                {
                    if (lat.Inputs.Count() > 20)
                    {
                        string msg = $"{UIHelper.LocalString("交易输入项太多,请分为多次转账", "There are too many transaction input. Please transfer multiple times")}";
                        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                        DarkMessageBox.ShowInformation(msg, "");
                        return;
                    }
                    this.Operater.SignAndSendTx(lat);
                    if (this.Operater != default)
                    {
                        string msg = $"{UIHelper.LocalString("交易已广播", "Relay transaction completed")}   {lat.Hash}";
                        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                        DarkMessageBox.ShowInformation(msg, "");
                    }
                }
            }

        }



        private void Sm_Click2(object sender, EventArgs e)
        {
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            MixRoom roomMsg = (MixRoom)ToolStripMenuItem.Tag;
            var str = roomMsg.BetAddress.ToAddress();
            Clipboard.SetText(str);
            string msg = str + UIHelper.LocalString("  已复制", "  copied");
            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
            DarkMessageBox.ShowInformation(msg, "");
        }

        private void Sm_Click1(object sender, EventArgs e)
        {
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin.IsNull()) return;
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            MixRoom roomMsg = (MixRoom)ToolStripMenuItem.Tag;
            if (this.Module is CasinoModule md)
            {

                //if (roomMsg.Request.Permission == RoomPermission.Private)
                //{
                //    var splitIndex = bizPlugin.GetRoomSplit(roomMsg.RoomRecord.RoomId);
                //    bool ok = false;
                //    if (this.Operater.Wallet is OpenWallet OpenWallet)
                //    {
                //        foreach (var proof in OpenWallet.GetStones("roomproof"))
                //        {
                //            if (proof.Key.StartsWith(roomMsg.RoomRecord.RoomId.ToString()))
                //            {
                //                var bs = proof.Value.HexToBytes();
                //                var validator = bs.AsSerializable<SignatureValidator<SeasonPassport>>();
                //                if (OpenWallet.ContainsAndHeld(validator.Target.Gambler) && validator.Verify() && validator.Target.SplitIndex == splitIndex)
                //                    ok = true;
                //            }
                //        }
                //    }
                //    if (!ok)
                //        return;
                //}
                //md.GoinBury(roomMsg.BetAddress);
            }
        }

        private void Sm3_Click(object sender, EventArgs e)
        {
            using (AppendRoom dialog = new AppendRoom())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                var rId = dialog.RoomId;
                if (rId.IsNullOrEmpty()) return;
                if (!uint.TryParse(rId, out uint RoomId)) return;
                var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                if (bizPlugin != default)
                {
                    var roomRecord = bizPlugin.GetRoom(RoomId);
                    if (roomRecord.IsNotNull())
                    {
                        if (this.Operater.Wallet.IsNotNull() && this.Operater.Wallet is OpenWallet OpenWallet)
                        {
                            OpenWallet.AddStone("roomfollow", roomRecord.RoomId.ToString(), roomRecord.RoomId.ToString());
                            OpenWallet.Save();
                            AppendRoom(bizPlugin, roomRecord);
                        }
                    }
                }
            }
        }
        //private void Sm2_Click(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
        //    RoomKeyRecord roomMsg = (RoomKeyRecord)ToolStripMenuItem.Tag;
        //    if (this.Operater.Wallet is OpenWallet OpenWallet)
        //        new ManageRoomProof(OpenWallet, roomMsg.RoomRecord).ShowDialog();
        //}
        private void Sm_Click(object sender, EventArgs e)
        {
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin.IsNull()) return;
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            MixRoom roomMsg = (MixRoom)ToolStripMenuItem.Tag;
            if (this.Module is CasinoModule md)
            {
                md.OpenRoom(roomMsg);
            }
        }
        private void Sm1_Click(object sender, EventArgs e)
        {
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin.IsNull()) return;
            ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
            MixRoom roomMsg = (MixRoom)ToolStripMenuItem.Tag;
            if (this.Module is CasinoModule md)
            {
                if (this.Operater.Wallet is OpenWallet OpenWallet)
                {
                    foreach (var follow in OpenWallet.GetStones("roomfollow"))
                    {
                        if (follow.Key == roomMsg.RoomId.ToString())
                        {
                            OpenWallet.DeleteStone("roomfollow", follow.Key);
                            OpenWallet.Save();
                            ChangeWallet(this.Operater);
                            break;
                        }
                    }

                }
            }
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
        public void OnBlock(Block block)
        {
        }
        public void BeforeOnBlock(Block block) { }
        public void AfterOnBlock(Block block) { }


        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            this.treeRooms.Nodes.Clear();
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin != default)
            {
                if (this.Operater.Wallet.IsNotNull() && this.Operater.Wallet is OpenWallet OpenWallet)
                {
                    foreach (var stone in OpenWallet.GetStones("roomfollow"))
                    {
                        if (uint.TryParse(stone.Key, out uint roomId))
                        {
                            var roomRecord = bizPlugin.GetRoom(roomId);
                            if (roomRecord.IsNotNull())
                            {
                                AppendRoom(bizPlugin, roomRecord);
                            }
                        }
                    }
                }
            }
        }
        public void OnRebuild()
        {
            this.DoInvoke(() =>
            {
                this.treeRooms.Nodes.Clear();
            });
        }
        void AppendRoom(ICasinoProvider provier, MixRoom room)
        {
            foreach (var n in this.treeRooms.Nodes)
            {
                if (n.Tag is MixRoom rr)
                {
                    if (rr.BetAddress == room.BetAddress)
                        return;
                }
            }
            var node = new DarkTreeNode(room.RoomId.ToString());
            node.NodeType = 1;
            node.Tag = room;
            var subNode = new DarkTreeNode(UIHelper.LocalString($"房主地址:  {room.Holder.ToAddress()}", $"Holder Address:  {room.Holder.ToAddress()}"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            var address = room.BetAddress.ToAddress();
            subNode = new DarkTreeNode(UIHelper.LocalString($"下注地址:  {address}", $"Bet Address:  {address}"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            var pooladdress = room.PoolAddress.ToAddress();
            subNode = new DarkTreeNode(UIHelper.LocalString($"奖池地址:  {pooladdress}", $"Pool Address:  {pooladdress}"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            var feeaddress = room.FeeAddress.ToAddress();
            subNode = new DarkTreeNode(UIHelper.LocalString($"房费地址:  {feeaddress}", $"Fee Address:  {feeaddress}"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            var bankerddress = room.BankerAddress.ToAddress();
            subNode = new DarkTreeNode(UIHelper.LocalString($"庄池地址:  {bankerddress}", $"Banker Address:  {bankerddress}"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            //if (room.ValidBuryGame())
            //{
            //    var buryAddress = Contract.CreateSignatureRedeemScript(room.BetAddress).ToScriptHash().ToAddress();
            //    subNode = new DarkTreeNode(UIHelper.LocalString($"埋雷地址:  {buryAddress}", $"Bury Address:  {buryAddress}"));
            //    subNode.NodeType = 2;
            //    subNode.Tag = room;
            //    node.Nodes.Add(subNode);
            //}

            var kindItem = new EnumItem<GameKind>(room.Request.Kind);
            subNode = new DarkTreeNode(UIHelper.LocalString($"娱乐类型:  {kindItem.ToString()}", $"Game Type:  {kindItem.ToString()}"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            var assetState = Blockchain.Singleton.CurrentSnapshot.Assets.TryGet(room.Request.AssetId);
            if (assetState.IsNotNull())
            {
                subNode = new DarkTreeNode(UIHelper.LocalString($"下注资产名:  {assetState.GetName()}", $"Bet Asset Name:  {assetState.GetName()}"));
                subNode.NodeType = 2;
                subNode.Tag = room;
                node.Nodes.Add(subNode);
                subNode = new DarkTreeNode(UIHelper.LocalString($"下注资产Id:  {room.Request.AssetId.ToString()}", $"Bet Asset Id:  {room.Request.AssetId.ToString()}"));
                subNode.NodeType = 2;
                subNode.Tag = room;
                node.Nodes.Add(subNode);
            }
            //var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            //if (bizPlugin != default)
            //{
            //    var roomState = bizPlugin.GetRoomState(room.RoomRecord.RoomId);
            //    if (roomState.IsNotNull())
            //    {
            //        var AdminNum = roomState.Admins.IsNullOrEmpty() ? 0 : roomState.Admins.Length;
            //        subNode = new DarkTreeNode(UIHelper.LocalString($"管理员数量:  {AdminNum}", $"Nunber of Administrator:  {AdminNum}"));
            //        subNode.NodeType = 2;
            //        subNode.Tag = room;
            //        node.Nodes.Add(subNode);
            //    }
            //}
            var stateItem = new EnumItem<RoomPermission>(room.Request.Permission);
            subNode = new DarkTreeNode(UIHelper.LocalString($"房间状态:  {stateItem.ToString()}", $"Room State:  {stateItem.ToString()}"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            var blocks = Game.PeroidBlocks(room.Request);
            if (blocks == 10) blocks = 20;
            subNode = new DarkTreeNode(UIHelper.LocalString($"每局周期: {blocks} 区块", $"Period:  {blocks} blocks"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);

            var msg = UIHelper.LocalString($"抽佣比例:  {room.Request.CommissionValue}/1000", $"Commission Ratio per round:  {room.Request.CommissionValue}/1000");
            subNode = new DarkTreeNode(msg);
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            var ok = provier.VerifyPartnerLock(room, out IEnumerable<RoomPartnerLockRecord> validRecords, out Fixed8 haveLockTotal, out Fixed8 needLockTotal, out uint EarliestExpiration);
            subNode = new DarkTreeNode(UIHelper.LocalString($"合伙人OXS锁仓门槛:{haveLockTotal}/{needLockTotal}", $"Partner OXS Lock Threshold:{haveLockTotal}/{needLockTotal}"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            subNode = new DarkTreeNode(UIHelper.LocalString($"合伙人总分红比例:{room.Request.DividendRatio}%", $"Partner Total dividend proportion:{room.Request.DividendRatio}%"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            subNode = new DarkTreeNode(UIHelper.LocalString($"分红递减坡度:{room.Request.DividentSlope.StringValue()}", $"Divident Diminish Slope:{room.Request.DividentSlope.EngStringValue()}"));
            subNode.NodeType = 2;
            subNode.Tag = room;
            node.Nodes.Add(subNode);
            this.treeRooms.Nodes.Add(node);
        }

        #endregion
    }
}
