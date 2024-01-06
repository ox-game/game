using OX.Bapps;
using OX.IO;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.SmartContract;
using OX.Wallets.NEP6;
using OX.Wallets.UI.Forms;
using OX.IO.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OX.Casino;
using OX.Ledger;
using OX.UI.Bury;

namespace OX.UI.Casino
{
    public class CasinoModule : Module
    {
        public override string ModuleName { get { return "casinomodule"; } }
        public override uint Index { get { return 102; } }
        public INotecase Operater { get; private set; }
        Dictionary<UInt160, RoomView> RoomViews = new Dictionary<UInt160, RoomView>();
        protected CasinoRule RuleSetting;
        protected NewRoomForm NewRoomForm;
        protected Rooms Rooms;
        protected BuryView BuryView;
        protected FollowRooms FollowRooms;
        protected RiddlesHashView RiddlesHash;
        protected RiddlesView Riddles;
        protected OpenRoomsView OpenRoomsView;
        protected MyPartnerLockRecords MyPartnerLockRecords;
        public HeartBeatContext HeartBeatContext { get; private set; }
        public CasinoModule(Bapp bapp) : base(bapp)
        {
        }
        public override void InitEvents() { }
        public override void InitWindows()
        {
            ToolStripMenuItem investMenu = new ToolStripMenuItem();
            investMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));

            investMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            investMenu.Name = "casinoMenu";
            investMenu.Size = new System.Drawing.Size(39, 21);
            investMenu.Text = UIHelper.LocalString("娱乐", "Casino");

            //爆雷
            ToolStripMenuItem buryMenu = new ToolStripMenuItem();
            buryMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            buryMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            buryMenu.Name = "buryMenu";
            buryMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            buryMenu.Size = new System.Drawing.Size(170, 22);
            buryMenu.Text = UIHelper.LocalString("爆雷", "Bury");
            buryMenu.Click += BuryMenu_Click;


            //关注的房间列表
            ToolStripMenuItem followRoomsMenu = new ToolStripMenuItem();
            followRoomsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            followRoomsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            followRoomsMenu.Name = "followRoomsMenu";
            followRoomsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            followRoomsMenu.Size = new System.Drawing.Size(170, 22);
            followRoomsMenu.Text = UIHelper.LocalString("我关注的娱乐房间", "My follow Casino Rooms");
            followRoomsMenu.Click += FollowRoomsMenu_Click;
            //谜底哈希
            ToolStripMenuItem riddlesHashMenu = new ToolStripMenuItem();
            riddlesHashMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            riddlesHashMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            riddlesHashMenu.Name = "riddlesHashMenu";
            riddlesHashMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            riddlesHashMenu.Size = new System.Drawing.Size(170, 22);
            riddlesHashMenu.Text = UIHelper.LocalString("谜底哈希", "Riddles Hash");
            riddlesHashMenu.Click += RiddlesHashMenu_Click;
            //谜底哈希
            ToolStripMenuItem riddlesMenu = new ToolStripMenuItem();
            riddlesMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            riddlesMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            riddlesMenu.Name = "riddlesMenu";
            riddlesMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            riddlesMenu.Size = new System.Drawing.Size(170, 22);
            riddlesMenu.Text = UIHelper.LocalString("谜底", "Riddles");
            riddlesMenu.Click += RiddlesMenu_Click;

            //开放房间列表
            ToolStripMenuItem openRoomListMenu = new ToolStripMenuItem();
            openRoomListMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            openRoomListMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //openRoomListMenu.Image = global::Example.Icons.NewFile_6276;
            openRoomListMenu.Name = "openRoomListMenu";
            openRoomListMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            openRoomListMenu.Size = new System.Drawing.Size(170, 22);
            openRoomListMenu.Text = UIHelper.LocalString("开放房间列表", "Opened Room List");
            openRoomListMenu.Click += OpenRoomListMenu_Click;
            //玩家查询
            ToolStripMenuItem playerQueryMenu = new ToolStripMenuItem();
            playerQueryMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            playerQueryMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //playerQueryMenu.Image = global::Example.Icons.NewFile_6276;
            playerQueryMenu.Name = "playerQueryMenu";
            playerQueryMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            playerQueryMenu.Size = new System.Drawing.Size(170, 22);
            playerQueryMenu.Text = UIHelper.LocalString("玩家查询", "Gambler Query");
            playerQueryMenu.Click += PlayerQueryMenu_Click;
            //通用授权
            ToolStripMenuItem commonAuthorizeMenu = new ToolStripMenuItem();
            commonAuthorizeMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            commonAuthorizeMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //commonAuthorizeMenu.Image = global::Example.Icons.NewFile_6276;
            commonAuthorizeMenu.Name = "commonAuthorizeMenu";
            commonAuthorizeMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            commonAuthorizeMenu.Size = new System.Drawing.Size(170, 22);
            commonAuthorizeMenu.Text = UIHelper.LocalString("通用授权", "Common Authorize");
            commonAuthorizeMenu.Click += CommonAuthorizeMenu_Click;

            //房主
            ToolStripMenuItem ownerMenu = new ToolStripMenuItem();
            ownerMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            ownerMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //commonAuthorizeMenu.Image = global::Example.Icons.NewFile_6276;
            ownerMenu.Name = "ownerMenu";
            ownerMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            ownerMenu.Size = new System.Drawing.Size(170, 22);
            ownerMenu.Text = UIHelper.LocalString("我是房主", "As Room Owner");
            //创建娱乐房间
            ToolStripMenuItem newRoomMenu = new ToolStripMenuItem();
            newRoomMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            newRoomMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            newRoomMenu.Name = "newRoomMenu";
            newRoomMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            newRoomMenu.Size = new System.Drawing.Size(170, 22);
            newRoomMenu.Text = UIHelper.LocalString("创建娱乐房间", "New Casino Room");
            newRoomMenu.Click += NewRoomMenu_Click;
            ownerMenu.DropDownItems.Add(newRoomMenu);
            //房间列表
            ToolStripMenuItem roomsMenu = new ToolStripMenuItem();
            roomsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            roomsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            roomsMenu.Name = "roomsMenu";
            roomsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            roomsMenu.Size = new System.Drawing.Size(170, 22);
            roomsMenu.Text = UIHelper.LocalString("我拥有的娱乐房间", "My Own Casino Rooms");
            roomsMenu.Click += RoomsMenu_Click;
            ownerMenu.DropDownItems.Add(roomsMenu);

            //房主合伙人
            ToolStripMenuItem ownerPartnerMenu = new ToolStripMenuItem();
            ownerPartnerMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            ownerPartnerMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //commonAuthorizeMenu.Image = global::Example.Icons.NewFile_6276;
            ownerPartnerMenu.Name = "ownerPartnerMenu";
            ownerPartnerMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            ownerPartnerMenu.Size = new System.Drawing.Size(170, 22);
            ownerPartnerMenu.Text = UIHelper.LocalString("我是房间合伙人", "As Room Partner");

            //参与房间合伙担保
            ToolStripMenuItem myGuaranteeRecordMenu = new ToolStripMenuItem();
            myGuaranteeRecordMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            myGuaranteeRecordMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //myGuaranteeRecordMenu.Image = global::Example.Icons.NewFile_6276;
            myGuaranteeRecordMenu.Name = "myGuaranteeRecordMenu";
            myGuaranteeRecordMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            myGuaranteeRecordMenu.Size = new System.Drawing.Size(170, 22);
            myGuaranteeRecordMenu.Text = UIHelper.LocalString("我的锁仓入伙记录", "My Partner Lock Records");
            myGuaranteeRecordMenu.Click += MyGuaranteeRecordMenu_Click;
            ownerPartnerMenu.DropDownItems.Add(myGuaranteeRecordMenu);

            //娱乐规则
            ToolStripMenuItem casinoTrustPoolMenu = new ToolStripMenuItem();
            casinoTrustPoolMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            casinoTrustPoolMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            casinoTrustPoolMenu.Name = "casinoTrustPoolMenu";
            casinoTrustPoolMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            casinoTrustPoolMenu.Size = new System.Drawing.Size(170, 22);
            casinoTrustPoolMenu.Text = UIHelper.LocalString("查看娱乐信托池", "View Casino Trust Pool");
            casinoTrustPoolMenu.Click += CasinoTrustPoolMenu_Click;


            investMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
               buryMenu,
              new ToolStripSeparator(),
                followRoomsMenu,
                riddlesHashMenu,
                riddlesMenu,
                openRoomListMenu,
                playerQueryMenu,
                commonAuthorizeMenu,
                ownerMenu,
                ownerPartnerMenu,
                casinoTrustPoolMenu
            });

            //娱乐规则
            ToolStripMenuItem ruleSettingMenu = new ToolStripMenuItem();
            ruleSettingMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            ruleSettingMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            ruleSettingMenu.Name = "ruleSettingMenu";
            ruleSettingMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            ruleSettingMenu.Size = new System.Drawing.Size(170, 22);
            ruleSettingMenu.Text = UIHelper.LocalString("娱乐规则", "Casino Rule");
            ruleSettingMenu.Click += RuleSettingMenu_Click;
            //娱乐社区
            ToolStripMenuItem casinoCommunityMenu = new ToolStripMenuItem();
            casinoCommunityMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            casinoCommunityMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //casinoCommunityMenu.Image = global::Example.Icons.NewFile_6276;
            casinoCommunityMenu.Name = "casinoCommunityMenu";
            casinoCommunityMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            casinoCommunityMenu.Size = new System.Drawing.Size(170, 22);
            casinoCommunityMenu.Text = UIHelper.LocalString("娱乐社区", "Casino Community");
            casinoCommunityMenu.Click += CasinoCommunityMenu_Click;
            investMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                new ToolStripSeparator(),
                ruleSettingMenu,
                casinoCommunityMenu
            });



            this.Container.TopMenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            investMenu});
        }


        private void BuryMenu_Click(object sender, EventArgs e)
        {
            if (this.BuryView == default)
            {
                this.BuryView = new BuryView(this.Operater, casino.BuryBetAddress);
                this.BuryView.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.BuryView.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.BuryView);
            }
            this.Container.DockPanel.AddContent(this.BuryView);
        }

        private void CasinoTrustPoolMenu_Click(object sender, EventArgs e)
        {
            new ViewCasinoTrustPool().ShowDialog();
        }

        private void MyGuaranteeRecordMenu_Click(object sender, EventArgs e)
        {
            if (this.MyPartnerLockRecords == default)
            {
                this.MyPartnerLockRecords = new MyPartnerLockRecords();
                this.MyPartnerLockRecords.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.MyPartnerLockRecords.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.MyPartnerLockRecords);
            }
            this.Container.DockPanel.AddContent(this.MyPartnerLockRecords);
        }

     

        private void PlayerQueryMenu_Click(object sender, EventArgs e)
        {
            new PlayerQuery().ShowDialog();
        }

        private void OpenRoomListMenu_Click(object sender, EventArgs e)
        {
            if (this.OpenRoomsView == default)
            {
                this.OpenRoomsView = new OpenRoomsView();
                this.OpenRoomsView.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.OpenRoomsView.ChangeWallet(this.Operater);
            }
            this.Container.DockPanel.AddContent(this.OpenRoomsView);
        }

        private void CommonAuthorizeMenu_Click(object sender, EventArgs e)
        {
            if (this.Operater.IsNotNull() && this.Operater.Wallet.IsNotNull())
                new CommonAuthorize(this, this.Operater).ShowDialog();
        }

        private void CasinoCommunityMenu_Click(object sender, EventArgs e)
        {
            Bapp.PushCrossBappMessage(new CrossBappMessage() { MessageType = 1, Attachment = casino.OfficalEventBoardId });
        }



        private void RiddlesMenu_Click(object sender, EventArgs e)
        {
            if (this.Riddles == default)
            {
                this.Riddles = new RiddlesView();
                this.Riddles.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.Riddles.ChangeWallet(this.Operater);
            }
            this.Container.DockPanel.AddContent(this.Riddles);
        }

        private void RiddlesHashMenu_Click(object sender, EventArgs e)
        {
            if (this.RiddlesHash == default)
            {
                this.RiddlesHash = new RiddlesHashView();
                this.RiddlesHash.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.RiddlesHash.ChangeWallet(this.Operater);
            }
            this.Container.DockPanel.AddContent(this.RiddlesHash);
        }

        private void FollowRoomsMenu_Click(object sender, EventArgs e)
        {
            if (this.FollowRooms == default)
            {
                this.FollowRooms = new FollowRooms();
                this.FollowRooms.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.FollowRooms.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.FollowRooms);
            }
            this.Container.DockPanel.AddContent(this.FollowRooms);
        }

        private void RoomsMenu_Click(object sender, EventArgs e)
        {
            if (this.Rooms == default)
            {
                this.Rooms = new Rooms();
                this.Rooms.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.Rooms.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.Rooms);
            }
            this.Container.DockPanel.AddContent(this.Rooms);
        }

        private void NewRoomMenu_Click(object sender, EventArgs e)
        {
            this.NewRoomForm = this.ShowDialog<NewRoomForm>(form =>
            {
                form.ChangeWallet(this.Operater);
            });
        }

        public static List<List<T>> SplitRange<T>(List<T> list, int rangeLength)
        {
            var count = list.Count();
            var r = count / rangeLength;
            var y = count % rangeLength;
            List<List<T>> lists = new List<List<T>>();
            for (int i = 0; i < r; i++)
            {
                lists.Add(list.GetRange(i * rangeLength, rangeLength));
            }
            if (y > 0)
                lists.Add(list.GetRange(r * rangeLength, y));
            return lists;
        }

        private void RuleSettingMenu_Click(object sender, EventArgs e)
        {
            if (this.RuleSetting == default)
            {
                this.RuleSetting = new CasinoRule();
                this.RuleSetting.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.RuleSetting.ChangeWallet(this.Operater);
                this.Container.ToolWindows.Add(this.RuleSetting);
            }
            this.Container.DockPanel.AddContent(this.RuleSetting);
        }


        public override void OnCrossBappMessage(CrossBappMessage message)
        {
        }


        public override void OnBappEvent(BappEvent be)
        {
            if (this.RuleSetting != default)
                this.RuleSetting.OnBappEvent(be);
            if (this.NewRoomForm != default)
                this.NewRoomForm.OnBappEvent(be);
            if (this.Rooms != default)
                this.Rooms.OnBappEvent(be);
            if (this.BuryView != default)
                this.BuryView.OnBappEvent(be);
            if (this.FollowRooms != default)
                this.FollowRooms.OnBappEvent(be);
            if (this.RiddlesHash != default)
                this.RiddlesHash.OnBappEvent(be);
            if (this.OpenRoomsView != default)
                this.OpenRoomsView.OnBappEvent(be);
            if (this.Riddles != default)
                this.Riddles.OnBappEvent(be);
            if (this.MyPartnerLockRecords != default)
                this.MyPartnerLockRecords.OnBappEvent(be);
            foreach (var room in this.RoomViews.Values)
            {
                room.OnBappEvent(be);
            }

        }

        public override void HeartBeat(HeartBeatContext context)
        {
            this.HeartBeatContext = context;
            if (this.RuleSetting != default)
                this.RuleSetting.HeartBeat(context);
            if (this.NewRoomForm != default)
                this.NewRoomForm.HeartBeat(context);
            if (this.Rooms != default)
                this.Rooms.HeartBeat(context);
            if (this.BuryView != default)
                this.BuryView.HeartBeat(context);
            if (this.FollowRooms != default)
                this.FollowRooms.HeartBeat(context);
            if (this.RiddlesHash != default)
                this.RiddlesHash.HeartBeat(context);
            if (this.OpenRoomsView != default)
                this.OpenRoomsView.HeartBeat(context);
            if (this.Riddles != default)
                this.Riddles.HeartBeat(context);
            if (this.MyPartnerLockRecords != default)
                this.MyPartnerLockRecords.HeartBeat(context);
            foreach (var room in this.RoomViews.Values)
            {
                room.HeartBeat(context);
            }

        }
        public override void OnBlock(Block block)
        {
            if (this.RuleSetting != default)
                this.RuleSetting.OnBlock(block);
            if (this.NewRoomForm != default)
                this.NewRoomForm.OnBlock(block);
            if (this.Rooms != default)
                this.Rooms.OnBlock(block);
            if (this.BuryView != default)
                this.BuryView.OnBlock(block);
            if (this.FollowRooms != default)
                this.FollowRooms.OnBlock(block);
            if (this.RiddlesHash != default)
                this.RiddlesHash.OnBlock(block);
            if (this.OpenRoomsView != default)
                this.OpenRoomsView.OnBlock(block);
            if (this.Riddles != default)
                this.Riddles.OnBlock(block);
            if (this.MyPartnerLockRecords != default)
                this.MyPartnerLockRecords.OnBlock(block);
            foreach (var room in this.RoomViews.Values)
            {
                room.OnBlock(block);
            }
        }
        public override void BeforeOnBlock(Block block)
        {
            if (this.RuleSetting != default)
                this.RuleSetting.BeforeOnBlock(block);
            if (this.NewRoomForm != default)
                this.NewRoomForm.BeforeOnBlock(block);
            if (this.Rooms != default)
                this.Rooms.BeforeOnBlock(block);
            if (this.BuryView != default)
                this.BuryView.BeforeOnBlock(block);
            if (this.FollowRooms != default)
                this.FollowRooms.BeforeOnBlock(block);
            if (this.RiddlesHash != default)
                this.RiddlesHash.BeforeOnBlock(block);
            if (this.OpenRoomsView != default)
                this.OpenRoomsView.BeforeOnBlock(block);
            if (this.Riddles != default)
                this.Riddles.BeforeOnBlock(block);
            if (this.MyPartnerLockRecords != default)
                this.MyPartnerLockRecords.BeforeOnBlock(block);
            foreach (var room in this.RoomViews.Values)
            {
                room.BeforeOnBlock(block);
            }
        }
        public override void AfterOnBlock(Block block)
        {
            if (this.RuleSetting != default)
                this.RuleSetting.AfterOnBlock(block);
            if (this.NewRoomForm != default)
                this.NewRoomForm.AfterOnBlock(block);
            if (this.Rooms != default)
                this.Rooms.AfterOnBlock(block);
            if (this.BuryView != default)
                this.BuryView.AfterOnBlock(block);
            if (this.FollowRooms != default)
                this.FollowRooms.AfterOnBlock(block);
            if (this.RiddlesHash != default)
                this.RiddlesHash.AfterOnBlock(block);
            if (this.OpenRoomsView != default)
                this.OpenRoomsView.AfterOnBlock(block);
            if (this.Riddles != default)
                this.Riddles.AfterOnBlock(block);
            if (this.MyPartnerLockRecords != default)
                this.MyPartnerLockRecords.AfterOnBlock(block);
            foreach (var room in this.RoomViews.Values)
            {
                room.AfterOnBlock(block);
            }
        }
        public override void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            if (this.RuleSetting != default)
                this.RuleSetting.ChangeWallet(operater);
            if (this.NewRoomForm != default)
                this.NewRoomForm.ChangeWallet(operater);
            if (this.Rooms != default)
                this.Rooms.ChangeWallet(operater);
            if (this.BuryView != default)
                this.BuryView.ChangeWallet(operater);
            if (this.FollowRooms != default)
                this.FollowRooms.ChangeWallet(operater);
            if (this.RiddlesHash != default)
                this.RiddlesHash.ChangeWallet(operater);
            if (this.OpenRoomsView != default)
                this.OpenRoomsView.ChangeWallet(operater);
            if (this.Riddles != default)
                this.Riddles.ChangeWallet(operater);
            if (this.MyPartnerLockRecords != default)
                this.MyPartnerLockRecords.ChangeWallet(operater);
            foreach (var room in this.RoomViews.Values)
            {
                room.ChangeWallet(operater);
            }
        }
        public override void OnRebuild()
        {
            if (this.RuleSetting != default)
                this.RuleSetting.OnRebuild();
            if (this.NewRoomForm != default)
                this.NewRoomForm.OnRebuild();
            if (this.Rooms != default)
                this.Rooms.OnRebuild();
            if (this.BuryView != default)
                this.BuryView.OnRebuild();
            if (this.FollowRooms != default)
                this.FollowRooms.OnRebuild();
            if (this.RiddlesHash != default)
                this.RiddlesHash.OnRebuild();
            if (this.OpenRoomsView != default)
                this.OpenRoomsView.OnRebuild();
            if (this.Riddles != default)
                this.Riddles.OnRebuild();
            if (this.MyPartnerLockRecords != default)
                this.MyPartnerLockRecords.OnRebuild();
            foreach (var room in this.RoomViews.Values)
            {
                room.OnRebuild();
            }
        }
        public override void OnLoadBappModuleWalletSection(JObject bappSectionObject)
        {
            
        }

        public void OpenRoom(MixRoom room)
        {
            if (!this.RoomViews.TryGetValue(room.BetAddress, out RoomView gr))
            {
                var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
                if (bizPlugin.IsNull()) return;
                if (!room.ValidPrivateRoom(this.Operater.Wallet)) return;
                if (bizPlugin.VerifyPartnerLock(room, out IEnumerable<RoomPartnerLockRecord> validRecords, out Fixed8 haveLockTotal, out Fixed8 needLockTotal, out uint EarliestExpiration))
                {
                    if (room.IsNotNull())
                    {
                        gr = CreateRoomView(room);
                        gr.Module = this;
                        if (this.Operater != default && this.Operater.Wallet != default)
                            gr.ChangeWallet(this.Operater);
                        this.RoomViews[room.BetAddress] = gr;
                    }                   
                }
            }
            if (gr.IsNotNull())
            {
                this.Container.DockPanel.AddContent(gr);
            }
        }

        RoomView CreateRoomView(MixRoom room)
        {
            if (room.Request.Kind == GameKind.EatSmall)
                return new LuckEatSmallRoomView(this.Operater, room, true);
            else if (room.Request.Kind == GameKind.Lotto)
                return new LottoRoomView(this.Operater, room, true);
            else if (room.Request.Kind == GameKind.TeamKill)
                return new TeamKillRoomView(this.Operater, room, true);
            //else if (room.RoomRecord.Kind == GameKind.LuckTeamKill)
            //    return new LuckTeamKillRoomView(this.Operater, room);
            return default;
        }
    }
}
