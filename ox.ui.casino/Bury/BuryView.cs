using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.Wallets.UI.Forms;
using OX.IO;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OX.SmartContract;
using OX.Casino;
using OX.UI.Casino;
using OX.UI.Casino.Bury;

namespace OX.UI.Bury
{
    public partial class BuryView : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        public INotecase Operater { get; set; }
        public UInt160 BetAddress { get; private set; }
        public UInt256 AssetId { get; private set; } = Blockchain.OXC;
        FlowLayoutPanel TaskPanel;
        FlowLayoutPanel Report200PlainPanel;
        FlowLayoutPanel Report200ChiperPanel;
        FlowLayoutPanel ReportAllPlainPanel;
        FlowLayoutPanel ReportAllChiperPanel;
        uint needReload = uint.MaxValue-10;//To prevent overflow, subtract 10
        #region Constructor Region

        public BuryView(INotecase notecase)
        {
            this.Operater = notecase;
            InitializeComponent();
            ReloadUI();
            this.SuspendLayout();
            this.ResumeLayout();
            this.SizeChanged += Panel1_SizeChanged;

        }

        public BuryView(INotecase notecase, UInt160 betAddress)
           : this(notecase)
        {
            this.BetAddress = betAddress;
            this.DockText = UIHelper.LocalString("爆雷", "Bury");
            this.bt_DoBury.Text = UIHelper.LocalString("埋雷", "Bury");
            this.bt_DoTrustBury.Text = UIHelper.LocalString("信托埋雷", "Trust Bury");
            this.bt_mybury.Text = UIHelper.LocalString("埋雷记录", "Bury Records");
            this.bt_myluck.Text = UIHelper.LocalString("命中记录", "Hit Records");
            this.bt_copyBetAddress.Text = UIHelper.LocalString("复制埋雷地址", "Copy Bury Address");
            this.lb_buryAddress.Text = UIHelper.LocalString($"埋雷地址:    {betAddress.ToAddress()}", $"Bury Address:      {betAddress.ToAddress()}");
        }
        void ReloadUI()
        {
            if (TaskPanel.IsNull())
            {
                TaskPanel = new FlowLayoutPanel();
                this.Controls.Add(TaskPanel);
            }
            //this.TaskPanel.SuspendLayout();
            this.TaskPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.TaskPanel.Location = new System.Drawing.Point(10, 100);
            this.TaskPanel.Name = "TaskPanel";
            this.TaskPanel.Size = new System.Drawing.Size(this.ClientSize.Width / 2 - 10, this.ClientSize.Height - 110);
            this.TaskPanel.BorderStyle = BorderStyle.FixedSingle;
            this.TaskPanel.ResumeLayout(false);



            if (Report200PlainPanel.IsNull())
            {
                Report200PlainPanel = new FlowLayoutPanel();
                this.Controls.Add(Report200PlainPanel);
            }
            this.Report200PlainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Report200PlainPanel.Location = new System.Drawing.Point(this.ClientSize.Width / 2 + 10, 100);
            this.Report200PlainPanel.Name = "Report200PlainPanel";
            this.Report200PlainPanel.Size = new System.Drawing.Size(this.ClientSize.Width / 2 - 20, this.ClientSize.Height / 4 - 35);
            this.Report200PlainPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Report200PlainPanel.ResumeLayout(false);

            if (Report200ChiperPanel.IsNull())
            {
                Report200ChiperPanel = new FlowLayoutPanel();
                this.Controls.Add(Report200ChiperPanel);
            }
            this.Report200ChiperPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Report200ChiperPanel.Location = new System.Drawing.Point(this.ClientSize.Width / 2 + 10, this.ClientSize.Height / 4 + 75);
            this.Report200ChiperPanel.Name = "Report200ChiperPanel";
            this.Report200ChiperPanel.Size = new System.Drawing.Size(this.ClientSize.Width / 2 - 20, this.ClientSize.Height / 4 - 35);
            this.Report200ChiperPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Report200ChiperPanel.ResumeLayout(false);

            if (ReportAllPlainPanel.IsNull())
            {
                ReportAllPlainPanel = new FlowLayoutPanel();
                this.Controls.Add(ReportAllPlainPanel);
            }
            this.ReportAllPlainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReportAllPlainPanel.Location = new System.Drawing.Point(this.ClientSize.Width / 2 + 10, this.ClientSize.Height / 2 + 50);
            this.ReportAllPlainPanel.Name = "ReportAllPlainPanel";
            this.ReportAllPlainPanel.Size = new System.Drawing.Size(this.ClientSize.Width / 2 - 20, this.ClientSize.Height / 4 - 35);
            this.ReportAllPlainPanel.BorderStyle = BorderStyle.FixedSingle;
            this.ReportAllPlainPanel.ResumeLayout(false);

            if (ReportAllChiperPanel.IsNull())
            {
                ReportAllChiperPanel = new FlowLayoutPanel();
                this.Controls.Add(ReportAllChiperPanel);
            }
            this.ReportAllChiperPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReportAllChiperPanel.Location = new System.Drawing.Point(this.ClientSize.Width / 2 + 10, this.ClientSize.Height / 2 + this.ClientSize.Height / 4 + 25);
            this.ReportAllChiperPanel.Name = "ReportAllChiperPanel";
            this.ReportAllChiperPanel.Size = new System.Drawing.Size(this.ClientSize.Width / 2 - 20, this.ClientSize.Height / 4 - 35);
            this.ReportAllChiperPanel.BorderStyle = BorderStyle.FixedSingle;
            this.ReportAllChiperPanel.ResumeLayout(false);
        }

        private void Panel1_SizeChanged(object sender, System.EventArgs e)
        {
            this.ReloadUI();
        }



        #endregion

        #region Event Handler Region
        public override void Close()
        {
            var result = DarkMessageBox.ShowWarning(UIHelper.LocalString($"确定要退出爆雷吗?", $"Are you sure you want to exit the bury?"), UIHelper.LocalString("退出爆雷", "exit bury"), DarkDialogButton.YesNo);
            if (result == DialogResult.No)
                return;
            base.Close();
        }

        #endregion
        #region IBlockChainTrigger
        public virtual void OnBappEvent(BappEvent be)
        {
            if (be.ContainEventType(CasinoBappEventType.ReBuildIndex, out BappEventItem[] eventItems))
            {
            }
        }

        public virtual void OnCrossBappMessage(CrossBappMessage message)
        {
        }


        public virtual void HeartBeat(HeartBeatContext context)
        {

        }
        public void OnFlashMessage(FlashMessage flashMessage)
        {

        }
        public void BeforeOnBlock(Block block)
        {
            if (block.Index > needReload + 5)////To prevent overflow, subtract 10
            {
                needReload = uint.MaxValue-10;
                ReloadBurys();
            }
        }
        public void OnBlock(Block block) { }
        public virtual void AfterOnBlock(Block block)
        {

            if (this.Visible)
            {
                if (this.Module.Bapp.ContainBizTransaction(block, out BizTransaction[] bts))
                {
                    foreach (var tx in bts)
                    {
                        if (tx is AskTransaction at)
                        {
                            var bizshs = this.Module.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();
                            if (at.GetDataModel<BuryRequest>(bizshs, (byte)CasinoType.Bury, out BuryRequest buryrequest) && buryrequest.BetAddress == this.BetAddress)
                            {
                                if (needReload == uint.MaxValue-10)
                                    needReload = block.Index;
                            }
                        }
                    }
                }
                foreach (var tx in block.Transactions)
                {
                    if (tx is RangeTransaction rt)
                    {
                        try
                        {
                            var attr = rt.Attributes.Where(m => m.Usage == TransactionAttributeUsage.RelatedData).FirstOrDefault();
                            if (attr.IsNotNull())
                            {
                                var request = attr.Data.AsSerializable<BuryRequest>();
                                if (request.BetAddress == this.BetAddress)
                                {
                                    if (needReload == uint.MaxValue-10)
                                        needReload = block.Index;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }

        }
        public virtual void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            ReloadBurys();
        }
        public virtual void OnRebuild() { }


        #endregion
        #region
        void ReloadBurys()
        {
            this.DoInvoke(() =>
            {
                if (this.Visible)
                {
                    this.TaskPanel.Controls.Clear();
                    var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                    if (bizPlugin != default)
                    {
                        var number = bizPlugin.BuryNumber;
                        uint p = number > 200 ? number - 200 : 0;
                        int k = 0;
                        Dictionary<BuryCodeKey, uint> dic = new Dictionary<BuryCodeKey, uint>();
                        for (uint i = number; i > p; i--)
                        {
                            var buryRecord = bizPlugin.GetBury(this.BetAddress, i);
                            if (buryRecord.IsNotNull())
                            {
                                k++;
                                var replyBury = bizPlugin.GetRoomReplyBury(buryRecord.TxId);
                                this.TaskPanel.Controls.Add(new BuryButton(this, buryRecord, replyBury, k));
                                BuryCodeKey bck = new BuryCodeKey { BetAddress = this.BetAddress, CodeKind = 0, Code = buryRecord.Request.PlainBuryPoint };

                                if (dic.TryGetValue(bck, out uint c))
                                {
                                    dic[bck] = c + 1;
                                }
                                else
                                {
                                    dic[bck] = 1;
                                }
                                if (replyBury.IsNotNull())
                                {
                                    BuryCodeKey bcw = new BuryCodeKey { BetAddress = this.BetAddress, CodeKind = 1, Code = replyBury.ReplyBury.PrivateBuryRequest.CipherBuryPoint };

                                    if (dic.TryGetValue(bcw, out uint cc))
                                    {
                                        dic[bcw] = cc + 1;
                                    }
                                    else
                                    {
                                        dic[bcw] = 1;
                                    }
                                }
                            }
                        }
                        this.Report200PlainPanel.Controls.Clear();
                        this.Report200PlainPanel.Controls.Add(new DarkButton { Width = 525, Height = 30, Text = UIHelper.LocalString("当前明码统计", "Current plain code statistics") });
                        foreach (var pair in dic.Where(m => m.Key.CodeKind == 0).OrderByDescending(m => m.Value).Take(40))
                        {
                            this.Report200PlainPanel.Controls.Add(new DarkButton { Width = 100, Height = 30, Text = $"{pair.Key.Code}({pair.Value})" });
                        }
                        this.Report200ChiperPanel.Controls.Clear();
                        this.Report200ChiperPanel.Controls.Add(new DarkButton { Width = 525, Height = 30, Text = UIHelper.LocalString("当前暗码统计", "Current secret code statistics") });
                        foreach (var pair in dic.Where(m => m.Key.CodeKind == 1).OrderByDescending(m => m.Value).Take(40))
                        {
                            this.Report200ChiperPanel.Controls.Add(new DarkButton { Width = 100, Height = 30, Text = $"{pair.Key.Code}({pair.Value})" });
                        }
                        var ps = bizPlugin.GetRoomCodeCount(this.BetAddress);
                        this.ReportAllPlainPanel.Controls.Clear();
                        this.ReportAllPlainPanel.Controls.Add(new DarkButton { Width = 525, Height = 30, Text = UIHelper.LocalString("历史明码统计", "Historical plain code statistics") });
                        foreach (var pair in ps.Where(m => m.Key.CodeKind == 0).OrderByDescending(m => m.Value).Take(40))
                        {
                            this.ReportAllPlainPanel.Controls.Add(new DarkButton { Width = 100, Height = 30, Text = $"{pair.Key.Code}({pair.Value})" });
                        }
                        this.ReportAllChiperPanel.Controls.Clear();
                        this.ReportAllChiperPanel.Controls.Add(new DarkButton { Width = 525, Height = 30, Text = UIHelper.LocalString("历史暗码统计", "Historical secret code statistics") });
                        foreach (var pair in ps.Where(m => m.Key.CodeKind == 1).OrderByDescending(m => m.Value).Take(40))
                        {
                            this.ReportAllChiperPanel.Controls.Add(new DarkButton { Width = 100, Height = 30, Text = $"{pair.Key.Code}({pair.Value})" });
                        }
                    }
                }
            });
        }
        #endregion
        private void bt_DoBury_Click(object sender, EventArgs e)
        {
            new BuryOnce(this.Module, this.Operater, this.BetAddress, Blockchain.OXC).ShowDialog();
        }

        private void bt_mybury_Click(object sender, EventArgs e)
        {
            new MyBuryQuery(this.BetAddress, this.Operater).ShowDialog();
        }

        private void bt_myluck_Click(object sender, EventArgs e)
        {
            new MyBuryHitQuery(this.BetAddress, this.Operater).ShowDialog();
        }

        private void bt_DoTrustBury_Click(object sender, EventArgs e)
        {
            new TrustBuryOnce(this.Module, this.Operater, this.BetAddress, Blockchain.OXC).ShowDialog();
        }

        private void bt_copyBetAddress_Click(object sender, EventArgs e)
        {
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin != default)
            {
                var buryRecord = bizPlugin.GetBury(this.BetAddress, 217);
            }
            var addr = this.BetAddress.ToAddress();
            Clipboard.SetText(addr);
            string msg = addr + UIHelper.LocalString("  已复制", "  copied");
            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
            OX.Wallets.UI.Forms.DarkMessageBox.ShowInformation(msg, "");
        }
    }
}
