
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

namespace OX.Web.Pages
{
    public partial class MobileBury
    {
        public override string PageTitle => this.WebLocalString("爆雷", "Bury");
        [Inject]
        IMessageService MessageService { get; set; }
        protected ICasinoProvider Provider { get; set; }
        bool DrawerVisible = false;
        Fixed8 MinBet = Fixed8.Zero;
        public Fixed8 AssetBalance = Fixed8.Zero;
        BuryMergeModel[] BuryMergeModels { get; set; } = new BuryMergeModel[0];
        BuryViewModel BuryModel { get; set; } = new BuryViewModel();
        string[] Plains200 { get; set; } = new string[0];
        string[] Chipers200 { get; set; } = new string[0];
        string[] PlainsAll { get; set; } = new string[0];
        string[] ChipersAll { get; set; } = new string[0];


        public override void StateDispatcher_MixStateNotice(IMixStateMessage obj)
        {
            base.StateDispatcher_MixStateNotice(obj);
            if (this.Account.IsNotNull())
                AssetBalance = GetBalance(this.Account.ScriptHash);
        }

        public override async Task OnInitCompleted()
        {
            StateDispatcher.ServerStateNotice += StateDispatcher_ServerStateNotice;
            StateDispatcher.MixStateNotice += StateDispatcher_MixStateNotice;
            if (this.Account.IsNotNull())
                AssetBalance = GetBalance(this.Account.ScriptHash);
            this.Provider = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            this.GetMinBet(CasinoSettingTypes.BuryMinBet, out MinBet);
            this.ReloadBurys();
            await Task.CompletedTask;
        }
        protected bool GetMinBet(byte MintBetSetting, out Fixed8 MinBet)
        {
            MinBet = Fixed8.Zero;
            var settings = this.Provider.GetAllCasinoSettings();
            var setting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { MintBetSetting }));
            if (!setting.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
            {
                return Fixed8.TryParse(setting.Value.Value, out MinBet);
            }
            return false;
        }
        public override async Task OnAuthInitialized()
        {
            await Task.CompletedTask;
        }

        protected Fixed8 GetBalance(UInt160 address)
        {
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(address, () => null);
            return acts.IsNotNull() ? acts.GetBalance(Blockchain.OXC) : Fixed8.Zero;
        }
        public override void OnDispose()
        {
            StateDispatcher.ServerStateNotice -= StateDispatcher_ServerStateNotice;
            StateDispatcher.MixStateNotice -= StateDispatcher_MixStateNotice;
        }
        void ReloadBurys()
        {
            List<BuryMergeModel> list = new List<BuryMergeModel>();

            var number = this.Provider.BuryNumber;
            uint p = number > 200 ? number - 200 : 0;
            int k = 0;
            Dictionary<BuryCodeKey, uint> dic = new Dictionary<BuryCodeKey, uint>();
            for (uint i = number; i > p; i--)
            {
                var buryRecord = this.Provider.GetBury(casino.BuryBetAddress, i);
                if (buryRecord.IsNotNull())
                {
                    k++;
                    var replyBury = this.Provider.GetRoomReplyBury(buryRecord.TxId);
                    list.Add(new BuryMergeModel { BuryRecord = buryRecord, BuryMergeTx = replyBury, index = k });
                    BuryCodeKey bck = new BuryCodeKey { BetAddress = casino.BuryBetAddress, CodeKind = 0, Code = buryRecord.Request.PlainBuryPoint };

                    if (dic.TryGetValue(bck, out uint c))
                    {
                        dic[bck] = c + 1;
                    }
                    else
                    {
                        dic[bck] = 1;
                    }
                    if (replyBury.IsNotNull())
                    {
                        BuryCodeKey bcw = new BuryCodeKey { BetAddress = casino.BuryBetAddress, CodeKind = 1, Code = replyBury.ReplyBury.PrivateBuryRequest.CipherBuryPoint };

                        if (dic.TryGetValue(bcw, out uint cc))
                        {
                            dic[bcw] = cc + 1;
                        }
                        else
                        {
                            dic[bcw] = 1;
                        }
                    }
                }
            }
            BuryMergeModels = list.ToArray();
            List<string> plain200 = new List<string>();
            foreach (var pair in dic.Where(m => m.Key.CodeKind == 0).OrderByDescending(m => m.Value).Take(40))
            {
                plain200.Add($"{pair.Key.Code}({pair.Value})");
            }
            List<string> chiper200 = new List<string>();
            foreach (var pair in dic.Where(m => m.Key.CodeKind == 1).OrderByDescending(m => m.Value).Take(40))
            {
                chiper200.Add($"{pair.Key.Code}({pair.Value})");
            }
            var ps = this.Provider.GetRoomCodeCount(casino.BuryBetAddress);
            List<string> plainAll = new List<string>();
            foreach (var pair in ps.Where(m => m.Key.CodeKind == 0).OrderByDescending(m => m.Value).Take(40))
            {
                plainAll.Add($"{pair.Key.Code}({pair.Value})");
            }
            List<string> chiperAll = new List<string>();
            foreach (var pair in ps.Where(m => m.Key.CodeKind == 1).OrderByDescending(m => m.Value).Take(40))
            {
                chiperAll.Add($"{pair.Key.Code}({pair.Value})");
            }
            this.Plains200 = plain200.ToArray();
            this.Chipers200 = chiper200.ToArray();
            this.PlainsAll = plainAll.ToArray();
            this.ChipersAll = chiperAll.ToArray();
        }
        void OpenBury()
        {
            this.DrawerVisible = true;
        }
        void CloseBury()
        {
            this.DrawerVisible = false;
        }
        private void GoBury()
        {
            var amount = Fixed8.One * this.BuryModel.Amount;
            if (this.AssetBalance >= amount + Fixed8.One * 2)
            {
                doBury(this.BuryModel.Amount);
            }
            CloseBury();
        }
        private async void doBury(uint amt)
        {

            if (Fixed8.One * amt < MinBet)
            {
                var msg = this.WebLocalString($"最低埋雷额为{MinBet} ", $"the least bury amount {MinBet} ");
                return;
            }
            try
            {
                var amount = Fixed8.One * amt;
                var box = WebBox.Boxes.FirstOrDefault();
                if (box.IsNotNull() && box.Notecase.IsNotNull() && box.Notecase.Wallet.IsNotNull() && box.Notecase.Wallet is OpenWallet openWallet)
                {
                    TransactionOutput output = new TransactionOutput()
                    {
                        AssetId = Blockchain.OXC,
                        ScriptHash = casino.BuryBetAddress,
                        Value = amount
                    };

                    PrivateBuryRequest PrivateBuryRequest = new PrivateBuryRequest { Rand = (uint)new Random().Next(0, int.MaxValue), CipherBuryPoint = this.BuryModel.SecretCode };
                    VerifyPrivateBuryRequest VerifyPrivateBuryRequest = new VerifyPrivateBuryRequest { From = this.Account.ScriptHash, PrivateBuryRequest = PrivateBuryRequest };
                    var key = this.Account.GetKey();
                    if (key.IsNull()) return;
                    var encryptDataForCipherCode = PrivateBuryRequest.ToArray().Encrypt(key,casino.CasinoMasterAccountPubKey);
                    BuryRequest request = new BuryRequest
                    {
                        From = this.Account.ScriptHash,
                        BetAddress = casino.BuryBetAddress,
                        PlainBuryPoint = this.BuryModel.PlainCode,
                        VerifyHash = VerifyPrivateBuryRequest.Hash,
                        CryptoData = encryptDataForCipherCode
                    };
                    SingleAskTransactionWrapper stw = new SingleAskTransactionWrapper(this.Account, output);
                    var pubkey = Bapp.GetBapp<CasinoBapp>().ValidBizScriptHashs.FirstOrDefault();
                    var sh = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash();
                    var tx = openWallet.MakeSingleAskTransaction(stw, sh, (byte)CasinoType.Bury, request);

                    if (tx.IsNotNull())
                    {
                        box.Notecase.SignAndSendTx(tx);
                        //var msg = this.WebLocalString($"广播简易码下注交易成功  {tx.Hash}", $"Relay easy code bet transaction completed  {tx.Hash}");
                        //await MessageService.Info(msg);
                        StateHasChanged();

                    }

                }
            }
            catch
            {
                return;
            }
            //await Task.CompletedTask;
        }
        protected void StateDispatcher_ServerStateNotice(IServerStateMessage message)
        {
            if (message is BuryMessage)
            {
                this.ReloadBurys();
                InvokeAsync(StateHasChanged);
            }
        }

    }
}
