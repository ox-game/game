
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

namespace OX.Web.Pages
{
    public partial class Rooms
    {
        public override string PageTitle => this.WebLocalString("房间列表", "Room List");
        List<MixRoom> ValidRooms = new List<MixRoom>();
        protected override void OnCasinoInit()
        {
            Init();
        }
        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            await base.MetaMaskService_AccountChangedEvent(arg);
            Init();
        }
        void Init()
        {
            List<MixRoom> Rooms = new List<MixRoom>();
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin.IsNull()) return;
            var ui = Bapp.GetBappUi<CasinoBapp, CasinoUI>();
            var module = ui.Modules.FirstOrDefault(m => m.ModuleName == "webagentmodule");
            if (module.IsNotNull() && module is WebAgentModule webAgentModule)
            {
                foreach (var r in webAgentModule.Rooms)
                {
                    var room = bizPlugin.AllRooms.FirstOrDefault(m => m.RoomId == r);
                    if (room.IsNotNull() && VerifyRoom(bizPlugin, room))
                    {
                        Rooms.Add(room);
                    }
                }
            }
            this.ValidRooms = Rooms;
        }
        public bool VerifyRoom(ICasinoProvider provider, MixRoom room)
        {
            if (room.Request.Permission== RoomPermission.Private && this.EthID.IsNull()) return false;
            if (!ValidPrivateRoom(room, this.EthID)) return false;
            return provider.VerifyPartnerLock(room, out IEnumerable<RoomPartnerLockRecord> validRecords, out Fixed8 haveLockTotal, out Fixed8 needLockTotal, out uint EarliestExpiration);
        }
        public bool ValidPrivateRoom(MixRoom room, EthID ethID)
        {
            if (room.Request.Permission == RoomPermission.Private)
            {
                bool ok = false;
                if (room.RoomMemberSetting.IsNotNull())
                {
                    ok =ethID.IsNotNull()&& room.RoomMemberSetting.Members.Contains(ethID.MapAddress);
                }
                return ok;
            }
            else
            {
                return true;
            }
        }
    }
}
