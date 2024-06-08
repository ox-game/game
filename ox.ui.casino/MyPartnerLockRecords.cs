using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.Wallets.UI.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using OX.Network.P2P;
using OX.Cryptography.ECC;
using OX.Cryptography;
using OX.Casino;

namespace OX.UI.Casino
{
    public partial class MyPartnerLockRecords : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public class PartnerLockInfo
        {
            public Fixed8 Amount;
            public decimal Ratio;
            public UInt160 Partner;
            public uint EndIndex;
        }
        public Module Module { get; set; }
        private INotecase Operater;
        //Dictionary<uint, DarkTreeNode> watchnodes = new Dictionary<uint, DarkTreeNode>();
        #region Constructor Region

        public MyPartnerLockRecords()
        {
            InitializeComponent();
            this.DockArea = DarkDockArea.Left;
            this.treePools.MouseDown += TreeAsset_MouseDown;
        }


        private void TreeAsset_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DarkTreeNode[] nodes = treePools.SelectedNodes.ToArray();
                if (nodes != null && nodes.Length == 1)
                {
                    DarkContextMenu menu = new DarkContextMenu();
                    var node = nodes.FirstOrDefault();
                    //if (node.NodeType != default)
                    //{
                    //    ToolStripMenuItem sm;
                    //    uint roomId = 0;
                    //    if ((int)node.NodeType == 1)
                    //    {
                    //        roomId = (uint)node.Tag;
                    //    }
                    //    else if ((int)node.NodeType == 2)
                    //    {
                    //        var r = node.Tag as RoomPledgeGuaranteeRequestAndOutput;
                    //        roomId = r.CreateRoomGuaranteeRequest.RoomId;
                    //        sm = new ToolStripMenuItem(UIHelper.LocalString("查看待释放的原生OXC矿产", "View primary minerals to be released"));
                    //        sm.Tag = node.Tag;
                    //        sm.Click += Sm_Click1;
                    //        menu.Items.Add(sm);
                    //    }
                    //    sm = new ToolStripMenuItem(UIHelper.LocalString("追加质押担保", "Append Pledge Guarantee"));

                    //    sm.Tag = roomId;
                    //    sm.Click += Sm_Click;
                    //    menu.Items.Add(sm);
                    //}
                    if (menu.Items.Count > 0)
                        menu.Show(this.treePools, e.Location);
                }
            }
        }

        //private void Sm_Click1(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
        //    var r = ToolStripMenuItem.Tag as RoomPledgeGuaranteeRequestAndOutput;
        //    var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
        //    if (bizPlugin != default)
        //    {
        //        var v = bizPlugin.Get<RoomPledgeGuaranteeRequestAndOutput>(CasinoBizPersistencePrefixes.Casino_RoomPledge_Guarantee_RoomId, new UintHashKey { Value = r.CreateRoomGuaranteeRequest.RoomId, TxId = r.TxId });
        //        if (v.IsNotNull())
        //        {
        //            var vs = new RoomPledgeGuaranteeRequestAndOutput[] { v };
        //            var amt = ClaimHelper.CalculateBonusUnspend(vs, Blockchain.Singleton.Height + 1);
        //            DarkMessageBox.ShowInformation($"OXC:  {amt}", "");
        //        }
        //    }
        //}

        //private void Sm_Click(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem ToolStripMenuItem = sender as ToolStripMenuItem;
        //    uint roomId = (uint)ToolStripMenuItem.Tag;
        //    var form = new CreateRoomGuaranteeForm();
        //    form.RoomId = roomId;
        //    form.Module = this.Module;
        //    form.ChangeWallet(this.Operater);
        //    form.ShowDialog();
        //}

        public void Clear()
        {
            this.treePools.Nodes.Clear();
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
        public void OnFlashMessage(FlashMessage flashMessage)
        {

        }
        public void BeforeOnBlock(Block block)
        {

        }
        public void OnBlock(Block block)
        {

        }
        public void AfterOnBlock(Block block)
        {
            foreach (var tx in block.Transactions)
            {
                if (tx is LockAssetTransaction lat)
                {
                    if (!lat.IsTimeLock && !lat.IsIssue)
                    {
                        var shs = lat.RelatedScriptHashes;
                        if (shs.IsNotNullAndEmpty())
                        {
                            var sh = shs.FirstOrDefault();
                            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                            if (bizPlugin != default)
                            {
                                if (bizPlugin.MixRooms.TryGetValue(sh, out MixRoom room))
                                {
                                    DoGuarantees();
                                }
                            }
                        }
                    }
                }
            }
        }


        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            if (this.Operater.IsNotNull() && this.Operater.Wallet.IsNotNull())
                DoGuarantees();
        }
        public void OnRebuild()
        {
            this.DoInvoke(() =>
            {
                this.treePools.Nodes.Clear();
            });
        }
        void DoGuarantees()
        {
            this.DoInvoke(() =>
            {
                //this.watchnodes.Clear();
                this.treePools.Nodes.Clear();

                var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                if (bizPlugin != default)
                {
                    var records = bizPlugin.GetAll<RoomPartnerLockRecord, LockAssetTransaction>(CasinoBizPersistencePrefixes.Casino_RoomPartnerLock_Record);
                    if (records.IsNotNullAndEmpty())
                    {
                        var settings = bizPlugin.GetAllCasinoSettings();
                        var PrivateRoomOXSLockVolume = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PrivateRoomOXSLockVolume }));
                        if (PrivateRoomOXSLockVolume.Equals(new KeyValuePair<byte[], CasinoSettingRecord>())) return;
                        var PrivateRoomOXSLockAmount = Fixed8.FromDecimal(decimal.Parse(PrivateRoomOXSLockVolume.Value.Value));

                        var PublicRoomOXSLockVolume = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.PublicRoomOXSLockVolume }));
                        if (PublicRoomOXSLockVolume.Equals(new KeyValuePair<byte[], CasinoSettingRecord>())) return;
                        var PublicRoomOXSLockAmount = Fixed8.FromDecimal(decimal.Parse(PublicRoomOXSLockVolume.Value.Value));


                        List<RoomPartnerLockRecord> list = new List<RoomPartnerLockRecord>();
                        foreach (var r in records.Where(m => m.Key.EndIndex > Blockchain.Singleton.HeaderHeight))
                        {
                            if (this.Operater.Wallet.ContainsAndHeld(r.Key.Partner)) list.Add(r.Key);
                        }
                        Dictionary<MixRoom, List<PartnerLockInfo>> lockInfo = new Dictionary<MixRoom, List<PartnerLockInfo>>();
                        foreach (var roomLocks in records.Select(m => m.Key).GroupBy(m => m.BetAddress))
                        {
                            if (bizPlugin.MixRooms.TryGetValue(roomLocks.Key, out MixRoom room))
                            {
                                Fixed8 TotalLockVolume = Fixed8.Zero;
                                if (room.Request.Permission == RoomPermission.Public)
                                {
                                    TotalLockVolume = PublicRoomOXSLockAmount;
                                }
                                else
                                {
                                    TotalLockVolume = PrivateRoomOXSLockAmount;
                                }
                                Fixed8 t = Fixed8.Zero;
                                foreach (var g in roomLocks.OrderBy(m => m.StartIndex * 1000 + m.TxIndex))
                                {
                                    if (TotalLockVolume > t)
                                    {
                                        var amt = g.Amount;
                                        if (t + amt > TotalLockVolume)
                                        {
                                            amt = TotalLockVolume - t;
                                        }
                                        var ratio = room.Request.DividentSlope.ComputeBonusRatio(TotalLockVolume, t, amt);
                                        var rt = System.Decimal.Round(ratio * 100, 4);
                                        t += amt;
                                        if (this.Operater.Wallet.ContainsAndHeld(g.Partner))
                                        {
                                            if (!lockInfo.TryGetValue(room, out List<PartnerLockInfo> infos))
                                            {
                                                infos = new List<PartnerLockInfo>();
                                                lockInfo[room] = infos;
                                            }
                                            infos.Add(new PartnerLockInfo { Amount = g.Amount, EndIndex = g.EndIndex, Partner = g.Partner, Ratio = rt });
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var d in lockInfo)
                        {
                            var node = new DarkTreeNode($"{d.Key.RoomId}--{d.Key.BetAddress.ToAddress()}");
                            foreach (var li in d.Value)
                            {
                                var subnode = new DarkTreeNode($"{li.Amount} OXS,   {li.Ratio}%   ,{li.Partner.ToAddress()}   ,{li.EndIndex}");
                                node.Nodes.Add(subnode);
                            }
                            this.treePools.Nodes.Add(node);
                        }
                    }
                }
            });
        }

        #endregion
    }
}
