using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.Wallets.UI.Forms;
using OX.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OX.SmartContract;
using OX.IO.Data.LevelDB;


namespace OX.UI.GameMining
{
    public partial class GMSangongMoreBettings : DarkDialog
    {
        public GMSangongMoreBettings(GM roomView, byte position, uint index)
        {
            InitializeComponent();
            this.Text = UIHelper.LocalString($"{index}局{position}号位", $"Round:{index},Position:{position}");
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");
            var plugin = Bapp.GetBappProvider<OX.UI.Casino.CasinoBapp, ICasinoProvider>();
            if (plugin.IsNotNull())
            {
                var Bettings = plugin.GetAll<GameMiningSeedKey, GameMiningSeedValue>(CasinoBizPersistencePrefixes.Casino_GameMining_Seed, SliceBuilder.Begin().Add((byte)roomView.GameMiningKind).Add(roomView.CurrentTaskKey.BetIndex).ToArray());
                if (Bettings != default)
                {
                    foreach (var bs in Bettings.Where(m =>
                       {
                           var cs = m.Key.Seed.Position.ToString();
                           return cs == position.ToString();
                       }).OrderByDescending(m => m.Value.Amount))
                    {
                        var pb = new GMSangongBettingButton(roomView, new GMBetting { Position = bs.Key.Seed.Position, Amount = bs.Value.Amount, SH = bs.Value.Player });
                        this.container.Controls.Add(pb);
                    }
                }
            }
        }
    }
}
