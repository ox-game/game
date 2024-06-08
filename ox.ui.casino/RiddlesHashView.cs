using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Text;

namespace OX.UI.Casino
{
    public partial class RiddlesHashView : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        int[] SangongSeed = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        string[] marksixSeed = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", };
        string[] pokerSeed = new string[] { "A1", "21", "31", "41", "51", "61", "71", "81", "91", "01", "J1", "Q1", "K1", "A2", "22", "32", "42", "52", "62", "72", "82", "92", "02", "J2", "Q2", "K2", "A3", "23", "33", "43", "53", "63", "73", "83", "93", "03", "J3", "Q3", "K3", "A4", "24", "34", "44", "54", "64", "74", "84", "94", "04", "J4", "Q4", "K4" };
        string[] letter = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        //static Dictionary<uint, RoomLine> Lines = new Dictionary<uint, RoomLine>();
        public Module Module { get; set; }
        protected INotecase Operater;
        protected uint CurrentIndex;
        bool Run = false;
        uint LastRiddlesHashIndex;
        uint CurrentCollisionIndex;
        long CollinsionCount;
        RiddlesHash rh;
        #region Constructor Region

        public RiddlesHashView()
        {
            InitializeComponent();
            
            this.bt_pre.Text = UIHelper.LocalString("< 上 1 段", "< Previous");
            this.bt_pre10.Text = UIHelper.LocalString("< 上 10 段", "< Previous 10");
            this.bt_pre100.Text = UIHelper.LocalString("< 上 100 段", "< Previous 100");
            this.bt_next.Text = UIHelper.LocalString("下 1 段 >", "Next >");
            this.bt_next10.Text = UIHelper.LocalString("下 10 段 >", "Next 10 >");
            this.bt_next100.Text = UIHelper.LocalString("下 100 段 >", "Next 100 >");
            this.bt_last.Text = UIHelper.LocalString("最近", "Current");
            this.DockText = UIHelper.LocalString("谜底哈希", "Riddles Hash");
            this.RoundPanel.SizeChanged += RoundPanel_SizeChanged;
            this.SizeChanged += GameRoom_SizeChanged;
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
            var index = Blockchain.Singleton.HeaderHeight + 9000;
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
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin != default)
            {
                var riddlesHashs = bizPlugin.GetRangeRiddlesHash(this.CurrentIndex);
                foreach (var r in riddlesHashs.OrderBy(m => m.Value.Index))
                {
                    RiddlesHashButton rhb = new RiddlesHashButton(this.Module, this.Operater, r.Value);
                    this.RoundPanel.Controls.Add(rhb);
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
                if (this.Module.Bapp.ContainBizTransaction(block, out BizTransaction[] bts))
                {
                    foreach (BizTransaction bt in bts)
                    {
                        if (bt is ReplyTransaction rt && rt.DataType == (byte)CasinoType.RiddlesAndHash)
                        {
                            RiddlesAndHash rh = rt.Data.AsSerializable<RiddlesAndHash>();
                            if (rh.IsNotNull() && rh.RiddlesHash.IsNotNull())
                            {
                                var index = rh.RiddlesHash.Index;
                                LastRiddlesHashIndex = index;
                                var rem = index % 10000;
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



        private void bt_pre_Click(object sender, System.EventArgs e)
        {
            if (this.CurrentIndex > 1000)
                this.CurrentIndex -= 1000;
            else this.CurrentIndex = 0;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();

        }

        private void bt_next_Click(object sender, System.EventArgs e)
        {
            this.CurrentIndex += 1000;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();
        }



        private void bt_pre10_Click(object sender, EventArgs e)
        {
            if (this.CurrentIndex > 1000 * 10)
                this.CurrentIndex -= 1000 * 10;
            else this.CurrentIndex = 0;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();

        }

        private void bt_pre100_Click(object sender, EventArgs e)
        {
            if (this.CurrentIndex > 1000 * 100)
                this.CurrentIndex -= 1000 * 100;
            else this.CurrentIndex = 0;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();

        }

        private void bt_next10_Click(object sender, EventArgs e)
        {
            this.CurrentIndex += 1000 * 10;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();
        }

        private void bt_next100_Click(object sender, EventArgs e)
        {
            this.CurrentIndex += 1000 * 100;
            this.lb_index.Text = this.CurrentIndex.ToString();
            this.ShowIndex();
        }

        private void bt_DoCollision_Click(object sender, EventArgs e)
        {
         
        }
       

        private void RiddlesHashView_Load(object sender, EventArgs e)
        {
            
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            ResetIndex();
        }
    }
}
