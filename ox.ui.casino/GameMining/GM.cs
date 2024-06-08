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
//using OX.UI.Agent;
using Akka.IO;
using OX;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;
using System.DirectoryServices.ActiveDirectory;
using OX.Plugins;
using OX.UI.Casino;
using OX.IO.Data.LevelDB;

namespace OX.UI.GameMining
{
    public abstract partial class GM : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        public INotecase Operater { get; set; }
        public GameMiningKind GameMiningKind { get; private set; }
        protected Dictionary<GameMiningTaskKey, GameMiningTask> Tasks = new Dictionary<GameMiningTaskKey, GameMiningTask>();
        public GameMiningTaskKey CurrentTaskKey { get; set; } = default;
        public GameMiningTask CurrentTask { get; set; } = default;
        protected ulong MineNonce;
        public Riddles Riddles;
        protected IEnumerable<KeyValuePair<GameMiningSeedKey, GameMiningSeedValue>> Seeds = default;
        protected IEnumerable<KeyValuePair<GameMiningAirdrop, GameMiningAirdropResult>> Airdrops = default;
        bool needReloadTask = false;
        bool needReloadData = false;
        #region Constructor Region

        public GM()
        {
            InitializeComponent();
            this.panel1.SizeChanged += Panel1_SizeChanged;
            this.VisibleChanged += GM_VisibleChanged;
        }

        private void GM_VisibleChanged(object sender, EventArgs e)
        {
            this.DoRebuildUI();
        }

        public GM(INotecase notecase, GameMiningKind gameMiningKind)
           : this()
        {
            this.Operater = notecase;
            this.GameMiningKind = gameMiningKind;
            this.DockText = UIHelper.LocalString(this.GameMiningKind.StringValue(), this.GameMiningKind.EngStringValue());
            this.ReloadTasks();
            this.InitUI();
        }
        protected abstract void InitUI();
        protected abstract void RebuildUI();
        public void ReLoadCurrentData()
        {
            var plugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (plugin.IsNotNull())
            {
                this.Riddles = default;
                this.MineNonce = 0;
                var riddleRecord = plugin.GetRiddles(this.CurrentTaskKey.BetIndex);
                if (riddleRecord.IsNotNull())
                {
                    Riddles = riddleRecord;
                    this.MineNonce = BlockHelper.GetMineNonce(this.CurrentTaskKey.BetIndex);
                }
                this.Seeds = plugin.GetAll<GameMiningSeedKey, GameMiningSeedValue>(CasinoBizPersistencePrefixes.Casino_GameMining_Seed, SliceBuilder.Begin().Add((byte)this.GameMiningKind).Add(this.CurrentTaskKey.BetIndex).ToArray());
                this.Airdrops = plugin.GetAll<GameMiningAirdrop, GameMiningAirdropResult>(CasinoBizPersistencePrefixes.Casino_GameMining_Airdrop, SliceBuilder.Begin().Add((byte)this.GameMiningKind).Add(this.CurrentTaskKey.BetIndex).ToArray());
            }
        }
        void DoRebuildUI()
        {
            this.DoInvoke(() =>
            {
                if (this.Visible)
                {
                    this.RebuildUI();
                }
            });
        }
        private void Panel1_SizeChanged(object sender, System.EventArgs e)
        {
            this.RebuildUI();
        }

        protected void ReloadTasks()
        {
            this.Tasks.Clear();
            var bizPlugin = Bapp.GetBappProvider<OX.UI.Casino.CasinoBapp, ICasinoProvider>();
            if (bizPlugin.IsNotNull())
            {
                var bs = bizPlugin.GetAll<GameMiningTaskKey, GameMiningTask>(CasinoBizPersistencePrefixes.Casino_GameMining_Task, new byte[] { (byte)this.GameMiningKind });
                if (bs.IsNotNullAndEmpty())
                {
                    this.Tasks = new Dictionary<GameMiningTaskKey, GameMiningTask>(bs.OrderBy(m => m.Key.BetIndex));
                }
            }
        }
        protected bool GetRecentTask(out GameMiningTaskKey key, out GameMiningTask value)
        {
            key = default;
            value = default;
            if (this.Tasks.IsNotNullAndEmpty())
            {
                var first = this.Tasks.OrderBy(m => m.Key.BetIndex).FirstOrDefault(m => m.Key.BetIndex > Blockchain.Singleton.HeaderHeight);
                if (!first.Equals(new KeyValuePair<GameMiningTaskKey, GameMiningTask>()))
                {
                    key = first.Key;
                    value = first.Value;
                    return true;
                }
            }
            return false;
        }
        public void GoRecent()
        {
            if (this.GetRecentTask(out GameMiningTaskKey key, out GameMiningTask value))
            {
                this.CurrentTaskKey = key;
                this.CurrentTask = value;
                this.ReLoadCurrentData();
                this.DoRebuildUI();
            }
            else if (this.Tasks.IsNotNullAndEmpty())
            {
                var last = this.Tasks.OrderBy(m => m.Key.BetIndex).Last();
                this.CurrentTaskKey = last.Key;
                this.CurrentTask = last.Value;
                this.ReLoadCurrentData();
                this.DoRebuildUI();
            }
        }
        public void GoPrev()
        {
            if (this.CurrentTaskKey.IsNotNull() && this.Tasks.IsNotNullAndEmpty())
            {
                var first = this.Tasks.OrderBy(m => m.Key.BetIndex).LastOrDefault(m => m.Key.BetIndex < this.CurrentTaskKey.BetIndex);
                if (!first.Equals(new KeyValuePair<GameMiningTaskKey, GameMiningTask>()))
                {
                    this.CurrentTaskKey = first.Key;
                    this.CurrentTask = first.Value;
                    this.ReLoadCurrentData();
                    this.DoRebuildUI();
                }
            }
        }
        public void GoNext()
        {
            if (this.CurrentTaskKey.IsNotNull() && this.Tasks.IsNotNullAndEmpty())
            {
                var first = this.Tasks.OrderBy(m => m.Key.BetIndex).FirstOrDefault(m => m.Key.BetIndex > this.CurrentTaskKey.BetIndex);
                if (!first.Equals(new KeyValuePair<GameMiningTaskKey, GameMiningTask>()))
                {
                    this.CurrentTaskKey = first.Key;
                    this.CurrentTask = first.Value;
                    this.ReLoadCurrentData();
                    this.DoRebuildUI();
                }
            }
        }
        #endregion

        #region Event Handler Region
        public override void Close()
        {
            var result = DarkMessageBox.ShowWarning(UIHelper.LocalString($"确定要退出竞争挖矿吗?", $"Are you sure you want to exit the game mining?"), UIHelper.LocalString("退出房间", "exit room"), DarkDialogButton.YesNo);
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
                //ShowIndex();
            }
        }

        public virtual void OnCrossBappMessage(CrossBappMessage message)
        {
        }


        public virtual void HeartBeat(HeartBeatContext context)
        {

        }
        public void BeforeOnBlock(Block block)
        {
            if (needReloadTask)
            {
                this.ReloadTasks();
                needReloadTask = false;
            }
            if (needReloadData)
            {
                this.ReLoadCurrentData();
                this.DoRebuildUI();
                needReloadData = false;
            }
        }
        public void OnBlock(Block block)
        {

        }
        public virtual void AfterOnBlock(Block block)
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
                            if (bizModel.Model is GameMiningTask gameMiningTask)
                            {
                                needReloadTask = true;
                            }
                        }
                    }
                }
            }
            foreach (var tx in block.Transactions)
            {
                if (tx is LockAssetTransaction lat)
                {
                    if (!lat.IsTimeLock)
                    {
                        var sh = Contract.CreateSignatureRedeemScript(lat.Recipient).ToScriptHash();
                        var contractSH = lat.GetContract().ScriptHash;
                        if (lat.Witnesses.Select(m => m.ScriptHash).Contains(sh))//self lock
                        {
                            for (ushort k = 0; k < lat.Outputs.Length; k++)
                            {
                                TransactionOutput output = lat.Outputs[k];
                                if (output.ScriptHash.Equals(contractSH))
                                {
                                    var attr = lat.Attributes.FirstOrDefault(m => m.Usage == TransactionAttributeUsage.Tip5);
                                    if (attr.IsNotNull())
                                    {
                                        try
                                        {
                                            var gms = attr.Data.AsSerializable<GameMiningSeed>();
                                            if (gms.IsNotNull())
                                            {
                                                if (gms.BetIndex > block.Index && gms.BetIndex - block.Index < GameMiningTask.MAXBETRANGE && gms.Position >= 0 && gms.Position <= 9 && output.Value >= GameMiningTask.MINSEED)
                                                {
                                                    needReloadData = true;
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
                        if (lat.IsIssue)
                        {
                            for (ushort k = 0; k < lat.Outputs.Length; k++)
                            {
                                TransactionOutput output = lat.Outputs[k];
                                if (output.ScriptHash.Equals(contractSH) && output.AssetId.Equals(casino.GamblerLuckBonusAsset) && lat.Attributes.IsNotNullAndEmpty())
                                {
                                    var attr = lat.Attributes.FirstOrDefault(m => m.Usage == TransactionAttributeUsage.Tip6);
                                    if (attr.IsNotNull())
                                    {
                                        try
                                        {
                                            var gma = attr.Data.AsSerializable<GameMiningAirdrop>();
                                            if (gma.IsNotNull())
                                            {
                                                needReloadData = true;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (tx is IssueTransaction ist)
                {
                    for (ushort k = 0; k < ist.Outputs.Length; k++)
                    {
                        TransactionOutput output = ist.Outputs[k];
                        if (output.AssetId.Equals(casino.GamblerLuckBonusAsset) && ist.Attributes.IsNotNullAndEmpty())
                        {
                            var attr = ist.Attributes.FirstOrDefault(m => m.Usage == TransactionAttributeUsage.Tip6);
                            if (attr.IsNotNull())
                            {
                                try
                                {
                                    var gma = attr.Data.AsSerializable<GameMiningAirdrop>();
                                    if (gma.IsNotNull())
                                    {
                                        needReloadData = true;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
        }
        public virtual void ChangeWallet(INotecase operater)
        {
            this.GoRecent();
        }
        public virtual void OnFlashMessage(FlashMessage flashMessage)
        {

        }
        public virtual void OnRebuild() { }
        #endregion



    }
}
