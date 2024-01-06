
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using OX.Wallets;
using OX.Wallets.Authentication;
using OX.Wallets.States;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using OX.MetaMask;
using OX.UI.Messages;
using System.Collections;

namespace OX.Web.Models
{
    public abstract class CasinoComponentBase : StatesComponentBase
    {
        protected CasinoWebBox Box;
        //protected string EthAddress;
        //protected int? Chain;
        protected bool Valid
        {
            get
            {
                if (this.Box.IsNull()) return false;
                if (this.Box.Notecase.IsNull()) return false;
                if (this.Box.Notecase.Wallet.IsNull()) return false;
                if (this.EthID.IsNull()) return false;
                return true;
            }
        }
        protected override void OnStateInit()
        {
            if (OXRunTime.RunMode != RunMode.Server)
                NavigationManager.NavigateTo("/");
            Box = WebBox.GetWebBox<CasinoWebBox>();
            this.OnCasinoInit();
        }
        protected abstract void OnCasinoInit();
        protected override void StateDispatcher_NodeStateNotice(INodeStateMessage message)
        {
        }
        protected override void StateDispatcher_MixStateNotice(IMixStateMessage message)
        {
        }
        protected override void StateDispatcher_ServerStateNotice(IServerStateMessage message)
        {

        }
        protected override void IMetaMaskService_OnDisconnectEvent()
        {
            Console.WriteLine("Disconnect");
        }

        protected override void IMetaMaskService_OnConnectEvent()
        {
            Console.WriteLine("Connect");
        }

        protected override async Task MetaMaskService_ChainChangedEvent((long, Chain) arg)
        {
            Console.WriteLine("Chain Changed");
            await GetSelectedNetwork();
            StateHasChanged();
        }

        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            Console.WriteLine("Account Changed");
            await GetSelectedAddress();
            StateHasChanged();
        }

    }
}
