
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
using OX.Cryptography;
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
    public partial class MobileRoom
    {
        public override string PageTitle => this.WebLocalString("娱乐", "Casino");
        List<MixRoom> ValidRooms = new List<MixRoom>();
        MixRoom Room = default;
       
        public override async Task OnInitCompleted()
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
             
            await Task.CompletedTask;
        }
        public bool VerifyRoom(ICasinoProvider provider, MixRoom room)
        {
            if (!ValidPrivateRoom(room)) return false;
            return provider.VerifyPartnerLock(room, out IEnumerable<RoomPartnerLockRecord> validRecords, out Fixed8 haveLockTotal, out Fixed8 needLockTotal, out uint EarliestExpiration);
        }
        public bool ValidPrivateRoom(MixRoom room)
        {
            if (room.Request.Permission == RoomPermission.Private)
            {
                bool ok = false;
                if (room.RoomMemberSetting.IsNotNull())
                {
                    ok = this.Account.IsNotNull() && room.RoomMemberSetting.Members.Contains(this.Account.ScriptHash);
                }
                return ok;
            }
            else
            {
                return true;
            }
        }
        public override async Task OnAuthInitialized()
        {
            await Task.CompletedTask;
        }


        public override void OnDispose()
        {
        }
        void OpenRoom(MixRoom room)
        {
            this.Room = room;
            StateHasChanged();
        }
    }
}
