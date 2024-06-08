using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public partial class RiddlesView : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        //static Dictionary<uint, RoomLine> Lines = new Dictionary<uint, RoomLine>();
        public Module Module { get; set; }
        protected INotecase Operater;
        protected uint CurrentIndex;
        public GameKind GameKind;
        #region Constructor Region

        public RiddlesView()
        {
            InitializeComponent();
            this.bt_pre.Text = UIHelper.LocalString("< 上 1 段", "< Previous");
            this.bt_pre10.Text = UIHelper.LocalString("< 上 10 段", "< Previous 10");
            this.bt_pre100.Text = UIHelper.LocalString("< 上 100 段", "< Previous 100");
            this.bt_next.Text = UIHelper.LocalString("下 1 段 >", "Next >");
            this.bt_next10.Text = UIHelper.LocalString("下 10 段 >", "Next 10 >");
            this.bt_next100.Text = UIHelper.LocalString("下 100 段 >", "Next 100 >");
            this.cb_auto.Text = UIHelper.LocalString("自动定位到最近", "Auto Focus Current");
            this.DockText = UIHelper.LocalString("谜底", "Riddles");
            this.RoundPanel.SizeChanged += RoundPanel_SizeChanged;
            this.SizeChanged += GameRoom_SizeChanged;
            this.cb_gamekind.Items.Clear();
            foreach (var gk in EnumHelper.All<GameKind>().RiddlesValid())
            {
                this.cb_gamekind.Items.Add(new EnumItem<GameKind>(gk));
            }
            this.cb_gamekind.SelectedIndex = 0;
            EnumItem<GameKind> item = this.cb_gamekind.SelectedItem as EnumItem<GameKind>;
            GameKind = item.Target;
        }

        private void GameRoom_SizeChanged(object sender, EventArgs e)
        {
        }

        protected virtual void RoundPanel_SizeChanged(object sender, System.EventArgs e)
        {
            foreach (Control ctrl in this.RoundPanel.Controls)
            {
                if (ctrl is DarkTitle dt)
                    dt.Width = this.RoundPanel.Width - 10;
                if (ctrl is Panel pl)
                    pl.Width = this.RoundPanel.Width - 10;
            }
            int w = this.RoundPanel.Size.Width - 30;
            IEnumerator itr = this.RoundPanel.Controls.GetEnumerator();
            List<Control> cs = new List<Control>();
            while (itr.MoveNext())
            {
                cs.Add(itr.Current as Control);
            }
            this.RoundPanel.Controls.Clear();
            foreach (var c in cs)
            {
                c.Width = this.RoundPanel.Width / 4 - 10;
                this.RoundPanel.Controls.Add(c);
            }
        }

        #endregion

        #region Event Handler Region

        public override void Close()
        {
            base.Close();
        }

        #endregion

        public void ResetIndex()
        {
            var index = Blockchain.Singleton.HeaderHeight;
            var c = index % 1000;
            if (c > 0)
                index = index - c + 1000;
            if (this.CurrentIndex != index)
            {
                this.CurrentIndex = index;
                this.lb_index.Text = this.CurrentIndex.ToString();
                ShowIndex();
            }
        }
        public void ShowIndex()
        {
            this.RoundPanel.Controls.Clear();
            var bizPlugin = OX.Bapps.Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin != default)
            {
                var riddlesHashs = bizPlugin.GetRangeRiddles(this.CurrentIndex);
                foreach (var r in riddlesHashs.OrderBy(m => m.Value.Index))
                {
                    var guessKey = r.Value.GetGuessKey(this.GameKind);
                    if (guessKey.IsNotNull())
                    {
                        RiddlesButton rhb = new RiddlesButton(this.Operater, r.Value, r.Value.Index, guessKey, this.GameKind);
                        this.RoundPanel.Controls.Add(rhb);
                    }
                }
            }
            this.RoundPanel_SizeChanged(this.RoundPanel, System.EventArgs.Empty);
        }

        #region IBlockChainTrigger
        public void OnBappEvent(BappEvent be)
        {
            if (be.ContainEventType(CasinoBappEventType.ReBuildIndex, out BappEventItem[] eventItems))
            {
                ShowIndex();
            }
        }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
        }
        public void HeartBeat(HeartBeatContext context)
        {

        }
        public void OnFlashMessage(FlashMessage flashMessage)
        {

        }
        public void BeforeOnBlock(Block block) { }
        public void OnBlock(Block block) { }
        public void AfterOnBlock(Block block)
        {
            this.DoInvoke(() =>
            {
                if (this.cb_auto.Checked)
                {
                    ResetIndex();
                }
                else
                {
                    if (this.Module.Bapp.ContainBizTransaction(block, out BizTransaction[] bts))
                    {
                        foreach (var tx in bts)
                        {
                            if (tx is ReplyTransaction bt)
                            {
                                if (bt.DataType == (byte)CasinoType.RiddlesAndHash)
                                {
                                    var RiddlesAndHash = bt.Data.AsSerializable<RiddlesAndHash>();
                                    if (RiddlesAndHash.IsNotNull() && RiddlesAndHash.Riddles.IsNotNull())
                                    {
                                        uint index = RiddlesAndHash.Riddles.Index;
                                        var rem = index % 1000;
                                        index -= rem;
                                        if (index == this.CurrentIndex)
                                            this.DoInvoke(() =>
                                            {
                                                this.ShowIndex();
                                            });
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }
        public void ChangeWallet(INotecase operater)
        {
            bool needResetIndex = false;
            if (this.Operater.IsNull())
            {
                needResetIndex = true;
            }
            this.Operater = operater;
            if (needResetIndex)
                this.ResetIndex();
        }
        public void OnRebuild() { }
        #endregion

        private void cb_auto_CheckedChanged(object sender, System.EventArgs e)
        {
            if (sender is DarkCheckBox cb)
            {
                if (cb.Checked)
                {
                    ResetIndex();
                }
            }
        }

        private void bt_pre_Click(object sender, System.EventArgs e)
        {
            this.cb_auto.Checked = false;
            if (this.CurrentIndex > 1000)
                this.CurrentIndex -= 1000;
            else this.CurrentIndex = 0;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();

        }

        private void bt_next_Click(object sender, System.EventArgs e)
        {
            this.cb_auto.Checked = false;
            this.CurrentIndex += 1000;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();
        }



        private void bt_pre10_Click(object sender, EventArgs e)
        {
            this.cb_auto.Checked = false;
            if (this.CurrentIndex > 1000 * 10)
                this.CurrentIndex -= 1000 * 10;
            else this.CurrentIndex = 0;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();

        }

        private void bt_pre100_Click(object sender, EventArgs e)
        {
            this.cb_auto.Checked = false;
            if (this.CurrentIndex > 1000 * 100)
                this.CurrentIndex -= 1000 * 100;
            else this.CurrentIndex = 0;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();

        }

        private void bt_next10_Click(object sender, EventArgs e)
        {
            this.cb_auto.Checked = false;
            this.CurrentIndex += 1000 * 10;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();
        }

        private void bt_next100_Click(object sender, EventArgs e)
        {
            this.cb_auto.Checked = false;
            this.CurrentIndex += 1000 * 100;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();
        }

        private void cb_gamekind_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumItem<GameKind> item = this.cb_gamekind.SelectedItem as EnumItem<GameKind>;
            GameKind = item.Target;
            this.ShowIndex();
        }
    }
}
