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
using OX.UI.Casino;
using OX.Wallets.States;
using OX.UI.Messages;

namespace OX.UI.WebAgent
{
    public class WebAgentModule : Module
    {
        public override string ModuleName { get { return "webagentmodule"; } }
        public override uint Index { get { return 103; } }
        public INotecase Operater { get; private set; }
        public List<uint> Rooms = new List<uint>();

        bool needReloadBury = false;
        public WebAgentModule(Bapp bapp) : base(bapp)
        {
        }
        public override void InitEvents() { }
        public override void InitWindows()
        {
            ToolStripMenuItem casinoAgentMenu = new ToolStripMenuItem();
            casinoAgentMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));

            casinoAgentMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            casinoAgentMenu.Name = "webAgentMenu";
            casinoAgentMenu.Size = new System.Drawing.Size(39, 21);
            casinoAgentMenu.Text = UIHelper.LocalString("娱乐代理", "Casino Agent");


            ToolStripMenuItem agentRoomMenu = new ToolStripMenuItem();
            agentRoomMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            agentRoomMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            agentRoomMenu.Name = "agentRoomMenu";
            agentRoomMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control| System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            agentRoomMenu.Size = new System.Drawing.Size(170, 22);
            agentRoomMenu.Text = UIHelper.LocalString("设置代理房间", "Agent Room Setting");
            agentRoomMenu.Click += AgentRoomMenu_Click;

            ToolStripMenuItem roomsMenu = new ToolStripMenuItem();
            roomsMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            roomsMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            roomsMenu.Name = "roomsMenu";
            roomsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control| System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            roomsMenu.Size = new System.Drawing.Size(170, 22);
            roomsMenu.Text = UIHelper.LocalString("进入代理房间", "Go Agented Room");
            roomsMenu.Click += RoomsMenu_Click;

            casinoAgentMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    agentRoomMenu,
                    roomsMenu
            });
            this.Container.TopMenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            casinoAgentMenu});
        }

        private void RoomsMenu_Click(object sender, EventArgs e)
        {
            OXRunTime.GoWeb("/_pc/casino/rooms");
        }

        private void AgentRoomMenu_Click(object sender, EventArgs e)
        {
            new RoomAgentSetting(this).ShowDialog();
        }





        public override void OnCrossBappMessage(CrossBappMessage message)
        {
        }


        public override void OnBappEvent(BappEvent be)
        {


        }

        public override void HeartBeat(HeartBeatContext context)
        {


        }
        public override void OnFlashMessage(FlashMessage flashMessage)
        {

        }
        public override void OnBlock(Block block)
        {
        }
        public override void BeforeOnBlock(Block block)
        {
            if (needReloadBury)
            {
                needReloadBury = false;
                BuildRoomBuryMessage();
            }
        }
        public override void AfterOnBlock(Block block)
        {
            if (this.Bapp.ContainBizTransaction(block, out BizTransaction[] bts))
            {
                foreach (var bt in bts)
                {
                    if (bt is ReplyTransaction rt)
                    {
                        var bizshs = this.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();

                        if (rt.GetDataModel<RoundClear>(bizshs, (byte)CasinoType.RoundClear, out RoundClear roundClear))
                        {
                            BuildRoomPrizeMessage(roundClear);
                        }
                        //if (rt.GetDataModel<RiddlesAndHash>(bizshs, (byte)CasinoType.RiddlesAndHash, out RiddlesAndHash riddlesandhash) && riddlesandhash.Riddles.IsNotNull() && riddlesandhash.Riddles.Index == this.CurrentIndex)
                        //{
                        //    show = true;
                        //}
                    }
                }
                foreach (var bt in bts)
                {
                    if (bt is AskTransaction at)
                    {
                        var bizshs = this.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();
                        if (at.GetDataModel<BetRequest>(bizshs, (byte)CasinoType.Bet, out BetRequest bet))
                        {
                            BuildRoomBetMessage(bet);
                        }
                        else if (at.GetDataModel<BuryRequest>(bizshs, (byte)CasinoType.Bury, out BuryRequest buryrequest) && buryrequest.BetAddress == casino.BuryBetAddress)
                        {
                            needReloadBury = true;
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
                            BuildRoomBetMessage(request);
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        var attr = rt.Attributes.Where(m => m.Usage == TransactionAttributeUsage.RelatedData).FirstOrDefault();
                        if (attr.IsNotNull())
                        {
                            var buryrequest = attr.Data.AsSerializable<BuryRequest>();
                            if (buryrequest.BetAddress == casino.BuryBetAddress)
                                needReloadBury = true;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        void BuildRoomBuryMessage()
        {
            var plugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (plugin.IsNotNull())
            {
                BuryMessage message = new BuryMessage();
                StateDispatcher.Instance.PostServerStateMessage(message);
            }
        }
        void BuildRoomPrizeMessage(RoundClear roundClear)
        {
            var plugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (plugin.IsNotNull())
            {
                var room = plugin.AllRooms.Where(m => m.BetAddress == roundClear.BetAddress);
                if (room.IsNotNull())
                {
                    RoomPrizeMessage message = new RoomPrizeMessage
                    {
                        RoundClear = roundClear
                    };
                    StateDispatcher.Instance.PostServerStateMessage(message);
                }
            }
        }
        void BuildRoomBetMessage(BetRequest betRequest)
        {
            var plugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (plugin.IsNotNull())
            {
                var room = plugin.AllRooms.Where(m => m.BetAddress == betRequest.BetAddress);
                if (room.IsNotNull())
                {
                    RoomBetMessage message = new RoomBetMessage
                    {
                        BetRequest = betRequest
                    };
                    StateDispatcher.Instance.PostServerStateMessage(message);
                }
            }
        }
        public override void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;

        }
        public override void OnRebuild()
        {

        }
        public override void OnLoadBappModuleWalletSection(JObject bappSectionObject)
        {
            var webAgent = bappSectionObject["webagent"];
            if (webAgent.IsNotNull())
            {
                var rooms = webAgent["rooms"].AsString();
                if (rooms.IsNotNullAndEmpty())
                {
                    foreach (var r in rooms.Split(","))
                    {
                        if (uint.TryParse(r, out uint roomId))
                        {
                            this.Rooms.Add(roomId);
                        }
                    }
                }
            }
        }
        public void SaveSetting()
        {
            if (this.Operater.Wallet is OpenWallet openWallet)
            {
                var webAgent = new JObject();
                var str = string.Join(",", this.Rooms);
                webAgent["rooms"] = str;
                this.moduleWalletSection["webagent"] = webAgent;
                openWallet.Save();
            }
        }

    }
}
