using OX.Bapps;
using OX.IO;
using OX.Network.P2P.Payloads;
using OX.Wallets;
//using OX.UI.Agent;
using OX.UI.Bury;
using OX.UI.GameMining;
using System.Collections.Generic;
using System.Linq;
using OX.UI.WebAgent;

namespace OX.UI.Casino
{
    public class CasinoUI : IBappUi
    {
        public Bapp Bapp { get; set; }
        Dictionary<string, IUIModule> _modules = new Dictionary<string, IUIModule>();
        public IUIModule[] Modules { get { return this._modules.Values.ToArray(); } }
        public CasinoUI(Bapp bapp)
        {
            this.Bapp = bapp;
            CasinoModule module = new CasinoModule(bapp);
            this._modules[module.ModuleName] = module;
            if (OXRunTime.RunMode == RunMode.Server)
            {
                WebAgentModule agentmodule = new WebAgentModule(bapp);
                this._modules[agentmodule.ModuleName] = agentmodule;
            }

            GameMiningModule gameminingmodule = new GameMiningModule(bapp);
            this._modules[gameminingmodule.ModuleName] = gameminingmodule;

        }
        public void OnBappEvent(BappEvent bappEvent)
        {
            foreach (var m in this.Modules)
                if (m is Module module)
                    module.OnBappEvent(bappEvent);
        }
        public void OnCrossBappMessage(CrossBappMessage message)
        {
            foreach (var m in this.Modules)
                if (m is Module module)
                    module.OnCrossBappMessage(message);
        }
        public void OnBlock(Block block)
        {
            foreach (var m in this.Modules)
                if (m is Module module)
                    module.OnBlock(block);
        }
        public void BeforeOnBlock(Block block)
        {
            foreach (var m in this.Modules)
                if (m is Module module)
                    module.BeforeOnBlock(block);
        }
        public void AfterOnBlock(Block block)
        {
            foreach (var m in this.Modules)
                if (m is Module module)
                    module.AfterOnBlock(block);
        }
        public void OnRebuild()
        {
            foreach (var m in this.Modules)
                if (m is Module module)
                    module.OnRebuild();
        }
        public void OnFlashMessage(FlashMessage flashMessage)
        {
            foreach (var m in this.Modules)
                if (m is Module module)
                    module.OnFlashMessage(flashMessage);
        }
    }
}
