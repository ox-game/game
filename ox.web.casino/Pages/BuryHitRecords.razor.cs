
using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using OX.Wallets;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using OX.Network.P2P.Payloads;
using OX;
using OX.IO;
using OX.Cryptography.ECC;
using OX.Ledger;
using OX.SmartContract;
using OX.Cryptography.AES;
using OX.Web.Models;
using OX.Wallets.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using OX.Wallets.Authentication;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using OX.Wallets.States;
using OX.Bapps;
using OX.UI.WebAgent;
using OX.UI.Casino;
using OX.Casino;
using OX.Wallets.Eths;
using OX.Web.ViewModels;
using OX.UI.Messages;
using Microsoft.AspNetCore.Mvc.Routing;
using Akka.Actor;
using System.Drawing;
using OX.Wallets.UI.Controls;
using System.Windows.Forms;
using Akka.IO;
using OX.Wallets.UI.Forms;
using NBitcoin;
using Org.BouncyCastle.Asn1.Cms;
using AntDesign;
using OX.UI.Bury;
using NuGet.Protocol.Plugins;
using System.Security.Cryptography;
using OX.UI.Casino.Bury;
using Akka.Actor.Dsl;

namespace OX.Web.Pages
{
    public partial class BuryHitRecords
    {
        public override string PageTitle => this.WebLocalString("命中记录", "Hit Records");

        protected ICasinoProvider Provider { get; set; }
        BuryMergeTx[] Records = new BuryMergeTx[0];

        protected override void OnCasinoInit()
        {
            this.Provider = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            var ps = Provider.GetEthMapHitBuryRecords(casino.BuryBetAddress, this.EthID.MapAddress);
            if (ps.IsNotNullAndEmpty())
            {
                Records=ps.OrderByDescending(m => m.BlockIndex).ToArray();
            }
        }
    }
}
