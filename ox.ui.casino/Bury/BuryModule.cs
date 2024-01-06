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

namespace OX.UI.Bury
{
    public class BuryModule : Module
    {
        public override string ModuleName { get { return "burymodule"; } }
        public override uint Index { get { return 103; } }

        public INotecase Operater;
        protected BuryView FixedView;
        protected BuryView FloatingView;

        public BuryModule(Bapp bapp) : base(bapp)
        {
        }
        public override void InitEvents() { }
        public override void InitWindows()
        {
            ToolStripMenuItem gameMiningMenu = new ToolStripMenuItem();
            gameMiningMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));

            gameMiningMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            gameMiningMenu.Name = "buryMenu";
            gameMiningMenu.Size = new System.Drawing.Size(39, 21);
            gameMiningMenu.Text = UIHelper.LocalString("爆雷", "Bury");

            ToolStripMenuItem ywBuryMenu = new ToolStripMenuItem();
            ywBuryMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            ywBuryMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            ywBuryMenu.Name = "ywBuryMenu";
            ywBuryMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            ywBuryMenu.Size = new System.Drawing.Size(170, 22);
            ywBuryMenu.Text = UIHelper.LocalString("原位爆雷", "Fixed Bury");
            ywBuryMenu.Click += EatsmallMiningMenu_Click;

            ToolStripMenuItem bwBuryMenu = new ToolStripMenuItem();
            bwBuryMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            bwBuryMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            //exitmenu.Image = global::Example.Icons.NewFile_6276;
            bwBuryMenu.Name = "bwBuryMenu";
            bwBuryMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            bwBuryMenu.Size = new System.Drawing.Size(170, 22);
            bwBuryMenu.Text = UIHelper.LocalString("本位爆雷", "Floating Bury");
            bwBuryMenu.Click += EatsmallFloatingMiningMenu_Click;
            gameMiningMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
               ywBuryMenu,
               bwBuryMenu
            });
            this.Container.TopMenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            gameMiningMenu});
        }

        private void LootingFloatingMiningMenu_Click(object sender, EventArgs e)
        {
        }

        private void EatsmallFloatingMiningMenu_Click(object sender, EventArgs e)
        {
            if (this.FloatingView == default)
            {
                this.FloatingView = new BuryView(this.Operater, BuryKind.Floating);
                this.FloatingView.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.FloatingView.ChangeWallet(this.Operater);
            }
            this.Container.DockPanel.AddContent(this.FloatingView);
        }

        private void LootingMiningMenu_Click(object sender, EventArgs e)
        {
        }

        private void EatsmallMiningMenu_Click(object sender, EventArgs e)
        {
            if (this.FixedView == default)
            {
                this.FixedView = new BuryView(this.Operater, BuryKind.Fixed);
                this.FixedView.Module = this;
                if (this.Operater != default && this.Operater.Wallet != default)
                    this.FixedView.ChangeWallet(this.Operater);
            }
            this.Container.DockPanel.AddContent(this.FixedView);
        }

        public override void OnCrossBappMessage(CrossBappMessage message)
        {
            if (this.FixedView.IsNotNull()) this.FixedView.OnCrossBappMessage(message);
            if (this.FloatingView.IsNotNull()) this.FloatingView.OnCrossBappMessage(message);
        }


        public override void OnBappEvent(BappEvent be)
        {
            if (this.FixedView.IsNotNull()) this.FixedView.OnBappEvent(be);
            if (this.FloatingView.IsNotNull()) this.FloatingView.OnBappEvent(be);
        }

        public override void HeartBeat(HeartBeatContext context)
        {
            if (this.FixedView.IsNotNull()) this.FixedView.HeartBeat(context);
            if (this.FloatingView.IsNotNull()) this.FloatingView.HeartBeat(context);
        }
        public override void OnBlock(Block block)
        {
            if (this.FixedView.IsNotNull()) this.FixedView.OnBlock(block);
            if (this.FloatingView.IsNotNull()) this.FloatingView.OnBlock(block);
        }
        public override void BeforeOnBlock(Block block)
        {
            if (this.FixedView.IsNotNull()) this.FixedView.BeforeOnBlock(block);
            if (this.FloatingView.IsNotNull()) this.FloatingView.BeforeOnBlock(block);
        }
        public override void AfterOnBlock(Block block)
        {
            if (this.FixedView.IsNotNull()) this.FixedView.AfterOnBlock(block);
            if (this.FloatingView.IsNotNull()) this.FloatingView.AfterOnBlock(block);
        }
        public override void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
            if (this.FixedView.IsNotNull()) this.FixedView.ChangeWallet(operater);
            if (this.FloatingView.IsNotNull()) this.FloatingView.ChangeWallet(operater);
        }
        public override void OnRebuild()
        {
            if (this.FixedView.IsNotNull()) this.FixedView.OnRebuild();
            if (this.FloatingView.IsNotNull()) this.FloatingView.OnRebuild();
        }
        public override void OnLoadBappModuleWalletSection(JObject bappSectionObject)
        {

        }

    }
}
