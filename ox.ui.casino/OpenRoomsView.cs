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

namespace OX.UI.Casino
{
    public partial class OpenRoomsView : DarkDocument, INotecaseTrigger, IModuleComponent
    {
        //static Dictionary<uint, RoomLine> Lines = new Dictionary<uint, RoomLine>();
        public Module Module { get; set; }
        protected INotecase Operater;
        protected uint CurrentIndex;
        #region Constructor Region

        public OpenRoomsView()
        {
            InitializeComponent();
            this.DockText = UIHelper.LocalString("开放房间列表", "Opened Room List");
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


        public void ShowIndex()
        {
            this.RoundPanel.Controls.Clear();
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin != default)
            {
                foreach (var r in bizPlugin.AllRooms.Where(m => m.Request.Permission == RoomPermission.Public).OrderByDescending(m => m.RoomId))
                {
                    OpenRoomButton rhb = new OpenRoomButton(this.Module, this.Operater, r);
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

            });
        }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            this.ResetList();
        }
        public void ResetList()
        {
            ShowIndex();
        }
        public void OnRebuild() { }
        #endregion

    }
}
