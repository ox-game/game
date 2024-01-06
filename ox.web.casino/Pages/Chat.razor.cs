using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign;
using Microsoft.AspNetCore.Components.Authorization;
using OX.Wallets.States;
using OX.Wallets.Authentication;
using OX.MetaMask;
using System;
using OX.Casino;
using OX.UI.Messages;
using OX.Wallets;
using OX.Wallets.Eths;
using System.Reflection.Metadata;

namespace OX.Mix.Components
{
    public partial class Chat : IDisposable
    {
        [Parameter]
        public UInt160 BetAddress { get; set; }
        [Parameter]
        public EthID EthID { get; set; }
        protected RoomMessageEngine MessageEngine { get; set; }
        public bool HaveMessageEngine { get { return MessageEngine.IsNotNull(); } }
        RoomChatMessage[] Messages = new RoomChatMessage[0];
        public string InputMesssage { get; set; }
        protected override void OnParametersSet()
        {
            if (MessageEngine.IsNotNull())
                MessageEngine.MessageNotice -= MessageEngine_MessageNotice;
            if (this.BetAddress.IsNotNull())
            {
                this.MessageEngine = RoomMessageEngine.GetRoomMessageEngine(this.BetAddress);
                Messages = this.MessageEngine.Messages;
                this.MessageEngine.MessageNotice += MessageEngine_MessageNotice;
            }
        }

        private void MessageEngine_MessageNotice(RoomChatMessage message)
        {
            Messages = this.MessageEngine.Messages;
            InvokeAsync(StateHasChanged);
        }

        protected override async Task OnInitializedAsync()
        {
            await Task.CompletedTask;
        }


        public new void Dispose()
        {
            if (MessageEngine.IsNotNull())
                MessageEngine.MessageNotice -= MessageEngine_MessageNotice;
            base.Dispose();
        }
        protected void PushChat(string message)
        {
            if (this.EthID.IsNotNull() && this.HaveMessageEngine)
            {
                var msg = new RoomChatMessage
                {
                    AddressId = this.EthID.AddressID,
                    BetAddress = this.BetAddress,
                    TimeStamp = DateTime.Now.ToTimestamp(),
                    Message = message
                };
                this.MessageEngine.Push(msg);
            }
        }
        protected void Send()
        {
            this.PushChat(this.InputMesssage);
            this.InputMesssage = string.Empty;
        }
    }
}