using OX.Bapps;
using OX.IO;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.UI.Casino;
using OX.SmartContract;
using OX.Wallets.NEP6;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OX.Ledger;
using OX.IO.Json;
using System.Net.WebSockets;

namespace OX.UI.GameMining
{
    public class GameMiningModule : Module
    {
        public override string ModuleName { get { return "gameminingmodule"; } }
        public override uint Index { get { return 104; } }

        public INotecase Operater;
        protected SangongGM EatsmallFixedView;
        protected SangongGM EatsmallFloatingView;

        public GameMiningModule(Bapp bapp) : base(bapp)
        {
        }
        public override void InitEvents() { }
        public override void InitWindows()
        {
            ToolStripMenuItem gameMiningMenu = new ToolStripMenuItem();
            gameMiningMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));

            gameMiningMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            gameMiningMenu.Name = "gameMiningMenu";
            gameMiningMenu.Size = new System.Drawing.Size(39, 21);
            gameMiningMenu.Text = UIHelper.LocalString("竞技挖矿", "Game Mining");
          
            ToolStripMenuItem eatsmallMiningMenu = new ToolStripMenuItem();
            eatsmallMiningMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            eatsmallMiningMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            eatsmallMiningMenu.Name = "eatsmallMiningMenu";
            eatsmallMiningMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            eatsmallMiningMenu.Size = new System.Drawing.Size(170, 22);
            eatsmallMiningMenu.Text = UIHelper.LocalString("原位竞技挖矿", "EatSmall Fixed Mining");
            eatsmallMiningMenu.Click += EatsmallMiningMenu_Click;

            ToolStripMenuItem eatsmallFloatingMiningMenu = new ToolStripMenuItem();
            eatsmallFloatingMiningMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            eatsmallFloatingMiningMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            eatsmallFloatingMiningMenu.Name = "eatsmallFloatingMiningMenu";
            eatsmallFloatingMiningMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            eatsmallFloatingMiningMenu.Size = new System.Drawing.Size(170, 22);
            eatsmallFloatingMiningMenu.Text = UIHelper.LocalString("本位竞技挖矿", "EatSmall Floating Mining");
            eatsmallFloatingMiningMenu.Click += EatsmallFloatingMiningMenu_Click;
            gameMiningMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
               eatsmallMiningMenu,
               eatsmallFloatingMiningMenu
            });
            this.Container.TopMenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            gameMiningMenu});
        }

        private void LootingFloatingMiningMenu_Click(object sender, EventArgs e)
        {
        }

        private void EatsmallFloatingMiningMenu_Click(object sender, EventArgs e)
        {
            if (this.EatsmallFloatingView == default)
            {
                this.EatsmallFloatingView = new SangongGM(this.Operater, GameMiningKind.Floating);
                this.EatsmallFloatingView.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.EatsmallFloatingView.ChangeWallet(this.Operater);
            }
            this.Container.DockPanel.AddContent(this.EatsmallFloatingView);
        }

        private void LootingMiningMenu_Click(object sender, EventArgs e)
        {
        }

        private void EatsmallMiningMenu_Click(object sender, EventArgs e)
        {
            if (this.EatsmallFixedView == default)
            {
                this.EatsmallFixedView = new SangongGM(this.Operater, GameMiningKind.Fixed);
                this.EatsmallFixedView.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.EatsmallFixedView.ChangeWallet(this.Operater);
            }
            this.Container.DockPanel.AddContent(this.EatsmallFixedView);
        }

        public override void OnCrossBappMessage(CrossBappMessage message)
        {
            if (this.EatsmallFixedView.IsNotNull()) this.EatsmallFixedView.OnCrossBappMessage(message);
            if (this.EatsmallFloatingView.IsNotNull()) this.EatsmallFloatingView.OnCrossBappMessage(message);
        }


        public override void OnBappEvent(BappEvent be)
        {
            if (this.EatsmallFixedView.IsNotNull()) this.EatsmallFixedView.OnBappEvent(be);
            if (this.EatsmallFloatingView.IsNotNull()) this.EatsmallFloatingView.OnBappEvent(be);
        }

        public override void HeartBeat(HeartBeatContext context)
        {
            if (this.EatsmallFixedView.IsNotNull()) this.EatsmallFixedView.HeartBeat(context);
            if (this.EatsmallFloatingView.IsNotNull()) this.EatsmallFloatingView.HeartBeat(context);
        }
        public override void OnBlock(Block block)
        {
            if (this.EatsmallFixedView.IsNotNull()) this.EatsmallFixedView.OnBlock(block);
            if (this.EatsmallFloatingView.IsNotNull()) this.EatsmallFloatingView.OnBlock(block);
        }
        public override void BeforeOnBlock(Block block)
        {
            if (this.EatsmallFixedView.IsNotNull()) this.EatsmallFixedView.BeforeOnBlock(block);
            if (this.EatsmallFloatingView.IsNotNull()) this.EatsmallFloatingView.BeforeOnBlock(block);
        }
        public override void AfterOnBlock(Block block)
        {
            if (this.EatsmallFixedView.IsNotNull()) this.EatsmallFixedView.AfterOnBlock(block);
            if (this.EatsmallFloatingView.IsNotNull()) this.EatsmallFloatingView.AfterOnBlock(block);
        }
        public override void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            if (this.EatsmallFixedView.IsNotNull()) this.EatsmallFixedView.ChangeWallet(operater);
            if (this.EatsmallFloatingView.IsNotNull()) this.EatsmallFloatingView.ChangeWallet(operater);
        }
        public override void OnRebuild()
        {
            if (this.EatsmallFixedView.IsNotNull()) this.EatsmallFixedView.OnRebuild();
            if (this.EatsmallFloatingView.IsNotNull()) this.EatsmallFloatingView.OnRebuild();
        }
        public override void OnLoadBappModuleWalletSection(JObject bappSectionObject)
        {

        }

    }
}
