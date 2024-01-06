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

namespace OX.UI.Casino
{
    public abstract partial class RoomView : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        protected RoomViewHead ViewHead;
        protected Control ViewBody;
        public Module Module { get; set; }
        public INotecase Operater { get; set; }
        public abstract GameKind GameKind { get; }
        public MixRoom Room;
        protected uint CurrentIndex;
        protected ulong MineNonce;
        protected uint PeroidBlocks;
        protected Fixed8 MinBet = Fixed8.FromDecimal(1);
        protected string DibsAccount;
        public abstract byte MintBetSetting { get; }
        public IEnumerable<KeyValuePair<BetKey, Betting>> Bettings { get; protected set; } = default;
        public IEnumerable<KeyValuePair<RoundClearKey, RoundClearResult>> ClearResult { get; protected set; } = default;
        public List<UInt256> RoundClearTxIds { get; protected set; } = default;
        public Riddles Riddles;
        protected IBetWatch BetWatch;
        int showDiscount;
        #region Constructor Region

        public RoomView(INotecase notecase, MixRoom room)
        {
            this.Operater = notecase;
            this.Room = room;
            InitRoomView(room);
            InitializeComponent();
            this.SuspendLayout();
            this.ViewHead = new RoomViewHead(this, OnViewHeadPaint);
            this.panel1.Controls.Add(this.ViewHead);
            this.ViewHead.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ViewHead.Top = 0;
            this.ViewHead.Left = 0;
            this.ViewHead.Width = ClientSize.Width;
            this.ViewHead.Height = 250;
            this.ViewHead.pre1Btn.Click += Pre1Btn_Click;
            this.ViewHead.pre2Btn.Click += Pre2Btn_Click;
            this.ViewHead.pre3Btn.Click += Pre3Btn_Click;
            this.ViewHead.nxt1Btn.Click += Nxt1Btn_Click;
            this.ViewHead.nxt2Btn.Click += Nxt2Btn_Click;
            this.ViewHead.nxt3Btn.Click += Nxt3Btn_Click;
            this.ViewHead.exportBtn.Click += ExportBtn_Click;

            this.ViewBody = this.CreateViewBody();
            this.panel1.Controls.Add(this.ViewBody);
            this.ViewBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.ViewBody.Top = 250;
            this.ViewBody.Left = 0;
            this.ViewBody.Width = ClientSize.Width;
            this.ViewBody.Height = ClientSize.Height - 250;


            this.ResumeLayout();
            this.panel1.SizeChanged += Panel1_SizeChanged;

        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var index = Blockchain.Singleton.HeaderHeight;
            sb.AppendLine(UIHelper.LocalString($"游戏状态  {this.Room.RoomId}--{this.CurrentIndex}--{index} :", $"Game State  {this.Room.RoomId}--{this.CurrentIndex}--{index} :"));
            sb.AppendLine("-------------------------------------------------------------");
            this.SubExport(sb);
            if (this.Riddles.IsNotNull())
            {
                var m1 = UIHelper.LocalString("谜底", "Riddles");
                var g = this.Riddles.GetGuessKey(this.Room.Request.Kind);
                sb.AppendLine($"{m1} : {g.SpecialPosition}/{g.SpecialChar}/{g.ReRandomSanGongOrLottoString(this.MineNonce, index)}");
                sb.AppendLine("-------------------------------------------------------------");
            }
            if (this.ClearResult.IsNotNullAndEmpty())
            {
                sb.AppendLine(UIHelper.LocalString("获奖明细:", "Prize Details:"));
                foreach (var result in this.ClearResult)
                {
                    var tx = Blockchain.Singleton.GetTransaction(result.Value.TxHash);
                    if (tx is ReplyTransaction rt)
                    {
                        var bizshs = this.Module.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();

                        if (rt.GetDataModel<RoundClear>(bizshs, (byte)CasinoType.RoundClear, out RoundClear roundClear))
                        {
                            if (roundClear.BetAddress == this.Room.BetAddress && roundClear.Index == this.CurrentIndex)
                            {
                                foreach (var output in tx.Outputs)
                                {
                                    sb.AppendLine($"{output.ScriptHash.ToAddress()}      :      {output.Value}");
                                }
                            }
                        }
                    }
                }
                sb.AppendLine("-------------------------------------------------------------");
            }
            if (this.Bettings.IsNotNullAndEmpty())
            {
                var total = this.Bettings.Sum(m => m.Value.Amount);
                sb.AppendLine(UIHelper.LocalString($"总投注额:  {total}", $"Total Betting Amount:  {total}"));
                sb.AppendLine(UIHelper.LocalString("下注明细:", "Betting Details:"));
                foreach (var b in this.Bettings)
                {
                    sb.AppendLine($"{b.Value.BetRequest.From.ToAddress()}      :      {b.Value.Amount}         -->{b.Value.BetRequest.BetPoint}");
                }
                sb.AppendLine("-------------------------------------------------------------");
            }
            var text = sb.ToString();
            var m2 = UIHelper.LocalString("游戏状态", "Game State");
            var fn = $"{m2}--{this.Room.RoomId}--{this.CurrentIndex}--{index}.txt";
            ExportHelper.CopyFile(fn, text);
            string msg = UIHelper.LocalString($"{fn}  已经复制到粘贴板", $"{fn}   has been copied to the pasteboard");
            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
        }

        public RoomView(INotecase notecase, MixRoom room, bool ok)
           : this(notecase, room)
        {
            if (room.Request.Kind != this.GameKind)
                throw new System.Exception(UIHelper.LocalString("游戏类型不匹配", "Game type invalid"));
            this.Room = room;
            PeroidBlocks = Game.PeroidBlocks(room.Request);
            DockText = $"{room.RoomId.ToString()}-{UIHelper.LocalString(room.Request.Kind.StringValue(), room.Request.Kind.EngStringValue())}";
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin.IsNotNull())
            {
                var settings = bizPlugin.GetAllCasinoSettings();
                var setting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { MintBetSetting }));
                if (!setting.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                {
                    Fixed8.TryParse(setting.Value.Value, out MinBet);
                }
                setting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { CasinoSettingTypes.DibsAccount }));
                if (!setting.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                {
                    DibsAccount = setting.Value.Value;
                }
            }

            ResetAnShowIndex();
        }
        public abstract void OnViewHeadPaint(PaintEventArgs e);
        public abstract void InitRoomView(MixRoom room);
        public abstract Control CreateViewBody();
        public abstract void ReloadGameData();
        public abstract void ReloadGameUI();
        public abstract void SubExport(StringBuilder sb);
        public void ReloadData()
        {
            var plugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (plugin.IsNotNull())
            {
                this.Bettings = plugin.GetBettings(this.Room.BetAddress, this.CurrentIndex);
                this.ClearResult = plugin.GetRoundClearResults(this.Room.BetAddress, this.CurrentIndex);
                List<UInt256> txids = new List<UInt256>();
                foreach (var cr in this.ClearResult)
                {
                    var tx = Blockchain.Singleton.GetSnapshot().Transactions.TryGet(cr.Value.TxHash);
                    if (tx.IsNotNull())
                        foreach (var id in tx.Transaction.Inputs.Select(m => m.PrevHash))
                        {
                            if (!txids.Contains(id)) txids.Add(id);
                        }
                }
                this.RoundClearTxIds = txids;
                this.Riddles = default;
                this.MineNonce = 0;
                var riddleRecord = plugin.GetRiddles(this.CurrentIndex);
                if (riddleRecord.IsNotNull())
                {
                    Riddles = riddleRecord;
                    this.MineNonce = BlockHelper.GetMineNonce(this.CurrentIndex);
                }
            }

            ReloadGameData();
        }
        private void reloadRoundClear()
        {
            this.ViewHead.Panel.Controls.Clear();
            var cols = this.ViewHead.Panel.ClientSize.Width / 90;
            var rows = this.ViewHead.Panel.ClientSize.Height / 30;
            var c = rows * cols;
            int k = 0;
            if (this.ClearResult.IsNotNullAndEmpty())
            {
                bool more = false;
                foreach (var result in this.ClearResult)
                {
                    var tx = Blockchain.Singleton.GetTransaction(result.Value.TxHash);
                    if (tx is ReplyTransaction rt)
                    {
                        var bizshs = this.Module.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();

                        if (rt.GetDataModel<RoundClear>(bizshs, (byte)CasinoType.RoundClear, out RoundClear roundClear))
                        {
                            if (roundClear.BetAddress == this.Room.BetAddress && roundClear.Index == this.CurrentIndex)
                            {
                                foreach (var output in tx.Outputs)
                                {
                                    if (k < c - 1)
                                    {
                                        if (output.AssetId == this.Room.Request.AssetId)
                                        {
                                            var pb = new PrizeButton(output.ScriptHash.ToAddress(), output.Value, this.Operater.Wallet, this.Room, this.DibsAccount);
                                            this.ViewHead.Panel.Controls.Add(pb);
                                            k++;
                                        }
                                    }
                                    if (k >= c - 1)
                                    {
                                        more = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (more)
                {
                    var morbtn = new DarkButton();
                    morbtn.Width = 80;
                    morbtn.Height = 25;
                    morbtn.Text = "...";
                    morbtn.Margin = new Padding() { All = 5 };
                    morbtn.Click += Morbtn_Click;
                    this.ViewHead.Panel.Controls.Add(morbtn);
                }
            }
            this.ViewHead.Panel.Invalidate();
        }

        private void Morbtn_Click(object sender, EventArgs e)
        {
            new MorePrize(this.Module, this.Operater.Wallet, this.Room, this.CurrentIndex, this.DibsAccount).ShowDialog();
        }

        private void ReloadUI()
        {
            this.ReloadGameUI();
            this.reloadRoundClear();
            this.ViewHead.Invalidate();
            this.ViewBody?.Invalidate();
        }
        public bool ResetIndex()
        {
            var index = Blockchain.Singleton.HeaderHeight;

            if (PeroidBlocks == 10)
            {
                var pb = PeroidBlocks * 2;
                var c = index % pb;
                index -= c;
                if (this.Room.Request.Flag % 2 == 1)
                {
                    int cc = c > 10 ? 30 : 10;
                    index += (uint)cc;
                }
                else
                {
                    if (c > 0) index += pb;
                }
            }
            else
            {
                var rem = index % PeroidBlocks;
                var newIndex = index - rem + GetReviseIndex();
                if (newIndex < index)
                    newIndex += PeroidBlocks;
                index = newIndex;
            }
            if (this.CurrentIndex != index)
            {
                this.CurrentIndex = index;
                return true;
            }
            return false;
        }
        public uint GetReviseIndex()
        {
            return this.Room.ReviseIndex();
        }
        public void ResetAnShowIndex()
        {
            var index = Blockchain.Singleton.HeaderHeight;
            if (PeroidBlocks == 10)
            {
                var pb = PeroidBlocks * 2;
                var c = index % pb;
                index -= c;
                if (this.Room.Request.Flag % 2 == 1)
                {
                    int cc = c > 10 ? 30 : 10;
                    index += (uint)cc;
                }
                else
                {
                    if (c > 0) index += pb;
                }
            }
            else
            {
                var rem = index % PeroidBlocks;
                var newIndex = index - rem + GetReviseIndex();
                if (newIndex < index)
                    newIndex += PeroidBlocks;
                index = newIndex;
            }

            if (this.CurrentIndex != index)
            {
                this.CurrentIndex = index;
                ShowIndex();
            }
        }
        public void ShowIndex()
        {
            this.ReloadData();
            this.ReloadUI();
        }
        private void Panel1_SizeChanged(object sender, System.EventArgs e)
        {
            this.ReloadUI();
        }

        protected bool AllowBet()
        {
            var plugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (plugin.IsNull()) return false;
            var partnerOk = plugin.VerifyPartnerLock(this.Room, out IEnumerable<RoomPartnerLockRecord> validRecords, out Fixed8 havLockTotal, out Fixed8 needLockTotal, out uint earliestExpiration);
            return partnerOk;
        }

        #endregion

        #region Event Handler Region
        public override void Close()
        {
            var result = DarkMessageBox.ShowWarning(UIHelper.LocalString($"确定要退出房间{this.Room.RoomId}吗?", $"Are you sure you want to exit the room{this.Room.RoomId}?"), UIHelper.LocalString("退出房间", "exit room"), DarkDialogButton.YesNo);
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
                ShowIndex();
            }
        }

        public virtual void OnCrossBappMessage(CrossBappMessage message)
        {
        }


        public virtual void HeartBeat(HeartBeatContext context)
        {
            if (BetWatch.IsNotNull())
            {
                BetWatch.HeartBeat(context);
            }
            if (this.showDiscount > 0)
            {
                this.showDiscount--;
                if (this.showDiscount == 0)
                {
                    this.ShowIndex();
                }
            }
        }
        public void BeforeOnBlock(Block block) { }
        public void OnBlock(Block block) { }
        public virtual void AfterOnBlock(Block block)
        {
            this.DoInvoke(() =>
            {
                if (this.Visible)
                {
                    bool show = false;
                    if (this.Module.Bapp.ContainBizTransaction(block, out BizTransaction[] bts))
                    {
                        foreach (var bt in bts)
                        {
                            if (bt is ReplyTransaction rt)
                            {
                                //if (this.Operater.IsNull()) return;
                                //if (this.Operater.Wallet.IsNull()) return;
                                var bizshs = this.Module.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();

                                if (rt.GetDataModel<RoundClear>(bizshs, (byte)CasinoType.RoundClear, out RoundClear roundClear) && roundClear.Index == this.CurrentIndex)
                                {
                                    if (roundClear.BetAddress == this.Room.BetAddress)
                                    {
                                        show = true;
                                    }
                                }
                                if (rt.GetDataModel<RiddlesAndHash>(bizshs, (byte)CasinoType.RiddlesAndHash, out RiddlesAndHash riddlesandhash) && riddlesandhash.Riddles.IsNotNull() && riddlesandhash.Riddles.Index == this.CurrentIndex)
                                {
                                    show = true;
                                }
                            }
                        }
                        foreach (var bt in bts)
                        {
                            if (bt is AskTransaction at)
                            {
                                //if (this.Operater.IsNull()) return;
                                //if (this.Operater.Wallet.IsNull()) return;
                                var bizshs = this.Module.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();
                                if (at.GetDataModel<BetRequest>(bizshs, (byte)CasinoType.Bet, out BetRequest bet) && bet.Index == this.CurrentIndex)
                                {
                                    if (bet.BetAddress == this.Room.BetAddress)
                                    {
                                        show = true;
                                    }
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
                                    var request = attr.Data.AsSerializable<BetRequest>();
                                    if (request.BetAddress == this.Room.BetAddress && request.Index == this.CurrentIndex)
                                    {
                                        show = true;
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                    if (!show && this.ViewHead.cb_auto.Checked)
                    {
                        if (block.Index % this.PeroidBlocks == 2)
                        {
                            if (ResetIndex())
                                show = true;
                        }
                    }
                    if (show)
                    {
                        this.showDiscount = 3;
                    }
                }
            });
        }
        public virtual void ChangeWallet(INotecase operater)
        {
            bool needResetIndex = false;
            if (this.Operater.IsNull())
            {
                needResetIndex = true;
            }
            this.Operater = operater;
            if (needResetIndex)
                this.ResetAnShowIndex();
        }
        public virtual void OnRebuild() { }
        #endregion

        private void Nxt3Btn_Click(object sender, EventArgs e)
        {
            this.ViewHead.cb_auto.Checked = false;
            var preCurrentIndex = this.CurrentIndex;
            var preReviseIndex = GetReviseIndex();
            this.CurrentIndex += this.PeroidBlocks * 100;
            var nextReviseIndex = GetReviseIndex();
            if (preReviseIndex != nextReviseIndex)
            {
                this.CurrentIndex += nextReviseIndex;
            }
            this.ShowIndex();
        }

        private void Nxt2Btn_Click(object sender, EventArgs e)
        {
            this.ViewHead.cb_auto.Checked = false;
            var preCurrentIndex = this.CurrentIndex;
            var preReviseIndex = GetReviseIndex();
            this.CurrentIndex += this.PeroidBlocks * 10;
            var nextReviseIndex = GetReviseIndex();
            if (preReviseIndex != nextReviseIndex)
            {
                this.CurrentIndex += nextReviseIndex;
            }
            this.ShowIndex();
        }

        private void Nxt1Btn_Click(object sender, EventArgs e)
        {
            this.ViewHead.cb_auto.Checked = false;
            if (this.PeroidBlocks == 10)
            {
                this.CurrentIndex += this.PeroidBlocks * 2;
            }
            else
            {
                var preCurrentIndex = this.CurrentIndex;
                var preReviseIndex = GetReviseIndex();
                this.CurrentIndex += this.PeroidBlocks;
                var nextReviseIndex = GetReviseIndex();
                if (preReviseIndex != nextReviseIndex)
                {
                    this.CurrentIndex += nextReviseIndex;
                }
            }
            this.ShowIndex();
        }

        private void Pre3Btn_Click(object sender, EventArgs e)
        {
            this.ViewHead.cb_auto.Checked = false;
            if (this.CurrentIndex > this.PeroidBlocks * 100)
            {
                var preCurrentIndex = this.CurrentIndex;
                var preReviseIndex = GetReviseIndex();
                this.CurrentIndex -= this.PeroidBlocks * 100;
                var nextReviseIndex = GetReviseIndex();
                if (preReviseIndex != nextReviseIndex)
                {
                    this.CurrentIndex -= preReviseIndex;
                }
            }
            else this.CurrentIndex = 0;
            this.ShowIndex();
        }

        private void Pre2Btn_Click(object sender, EventArgs e)
        {
            this.ViewHead.cb_auto.Checked = false;
            if (this.CurrentIndex > this.PeroidBlocks * 10)
            {
                var preCurrentIndex = this.CurrentIndex;
                var preReviseIndex = GetReviseIndex();
                this.CurrentIndex -= this.PeroidBlocks * 10;
                var nextReviseIndex = GetReviseIndex();
                if (preReviseIndex != nextReviseIndex)
                {
                    this.CurrentIndex -= preReviseIndex;
                }
            }
            else this.CurrentIndex = 0;
            this.ShowIndex();
        }

        private void Pre1Btn_Click(object sender, EventArgs e)
        {
            this.ViewHead.cb_auto.Checked = false;
            var pb = this.PeroidBlocks;
            if (this.PeroidBlocks == 10)
                pb = this.PeroidBlocks * 2;
            if (this.CurrentIndex > pb)
            {
                var preCurrentIndex = this.CurrentIndex;
                var preReviseIndex = GetReviseIndex();
                this.CurrentIndex -= pb;
                var nextReviseIndex = GetReviseIndex();
                if (preReviseIndex != nextReviseIndex)
                {
                    this.CurrentIndex -= preReviseIndex;
                }
            }
            else this.CurrentIndex = 0;

            this.ShowIndex();
        }

    }
}
