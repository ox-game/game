
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
using Akka.Util;

namespace OX.Web.Pages
{
    public partial class MobileLuckEatSmall
    {
        IMessageService MessageService { get; set; }
        Fixed8 masterBalance = Fixed8.Zero;
        Fixed8 poolBalance = Fixed8.Zero;
        byte BankerPosition = 0;
        char[] keys = default;
        string specialCodePosition = string.Empty;
        Dictionary<byte, EatSmallPositionContext> PostionContexts = new Dictionary<byte, EatSmallPositionContext>();
        Fixed8 MinBet = Fixed8.Zero;
        bool DrawerVisible = false;
        EatSmallBetViewModel BetModel = new EatSmallBetViewModel() {  SpecialCode="a"};
        protected override void InitRoomPage()
        {
            BankerPosition = this.Room.Request.Flag;
            this.GetMinBet(CasinoSettingTypes.EatSmallMinBet, out MinBet);
        }

        protected override void ReloadGameData()
        {
            masterBalance = GetBalance(Room.BankerAddress);
            poolBalance = GetBalance(Room.PoolAddress);
        }
        protected override void ReloadGameUI()
        {
            this.PostionContexts.Clear();
            keys = default;
            specialCodePosition = string.Empty;
            if (this.Riddles.IsNotNull() && this.MineNonce > 0)
            {
                var guessKey = this.Riddles.GetGuessKey(this.Room.Request.Kind);
                if (guessKey.IsNotNull())
                {
                    keys = guessKey.ReRandomSanGongOrLotto(this.MineNonce, this.CurrentIndex);
                    specialCodePosition = $"{guessKey.SpecialChar} - {guessKey.SpecialPosition}";
                }
            }

            for (byte i = 0; i < 10; i++)
            {
                if (i != this.BankerPosition)
                {
                    List<Betting> bettings = new List<Betting>();
                    if (this.Bettings != default)
                    {
                        var bs = this.Bettings.Where(m =>
                        {
                            var cs = m.Value.BetRequest.BetPoint.ToCharArray();
                            return cs.IsNotNullAndEmpty() && cs[0].ToString() == i.ToString();
                        }).Select(m => m.Value);
                        if (bs.IsNotNullAndEmpty())
                        {
                            bettings.AddRange(bs);
                        }
                    }
                    var context = new EatSmallPositionContext() { Position = i, Bettings = bettings.ToArray() };
                    if (keys.IsNotNullAndEmpty())
                    {
                        context.Key = keys[i];
                    }
                    PostionContexts[i] = context;
                }
            }

        }
        void CloseBet()
        {
            this.DrawerVisible = false;
        }
        private async void Bet()
        {

            if (this.BetModel.Amount > 0 && this.Account.IsNotNull())
            {
                var amount = Fixed8.One * this.BetModel.Amount;
                if (this.AssetBalance > amount)
                {
                    doBet(amount);
                }
            }

            CloseBet();
            await Task.CompletedTask;
        }
        private  void doBet(Fixed8 amt)
        {
            if (this.Provider.IsNotNull())
            {
                if (Provider.GetRoomDestory(this.Room.RoomId).IsNotNull())
                {
                    return;
                }
            }
            if (PeroidBlocks == 10)
            {
                if (this.CurrentIndex - Blockchain.Singleton.HeaderHeight < 5) return;
            }
            else
            {
                if (this.CurrentIndex - Blockchain.Singleton.HeaderHeight < 17) return;
            }
            if (this.CurrentIndex % PeroidBlocks != GameHelper.ReviseIndex(this.Room))
                return;

            if (amt < MinBet)
            {
                var msg = this.WebLocalString($"最低投注额为{MinBet} ", $"the least bet amount {MinBet} ");
                return;
            }
            try
            {
                if (Room.ValidPrivateRoom(this.Account.ScriptHash))
                {
                    amt = Fixed8.One * (amt.GetInternalValue() / Fixed8.D);
                    List<TransactionOutput> outputs = new List<TransactionOutput>();
                    TransactionOutput output = new TransactionOutput()
                    {
                        AssetId = Room.Request.AssetId,
                        ScriptHash = this.Room.BetAddress,
                        Value = amt
                    };
                    outputs.Add(output);
                    var betFee = Room.GetPrivateRoomBetFee();
                    if (betFee > Fixed8.Zero)
                    {
                        output = new TransactionOutput()
                        {
                            AssetId = Blockchain.OXC,
                            ScriptHash = Room.Holder,
                            Value = betFee
                        };
                        outputs.Add(output);
                    }
                    var spc = this.BetModel.SpecialCode;
                    if (spc.IsNullOrEmpty()) return;
                    var spcc = spc.ToCharArray()[0];
                    if (!GuessKey.SpecialChars.Contains(spcc)) return;
                    BetRequest request = new BetRequest() { BetPoint = $"{this.BetModel.Position.ToString()}|{spcc}", From = this.Account.ScriptHash, Index = this.BetModel.BetIndex, BetAddress = this.Room.BetAddress, Passport = default };
                    var box = WebBox.Boxes.FirstOrDefault();
                    if (box.IsNotNull() && box.Notecase.IsNotNull() && box.Notecase.Wallet.IsNotNull() && box.Notecase.Wallet is OpenWallet openWallet)
                    {
                        SingleAskTransactionWrapper stw = new SingleAskTransactionWrapper(this.Account, outputs.ToArray());
                        var pubkey = Bapp.GetBapp<CasinoBapp>().ValidBizScriptHashs.FirstOrDefault();
                        var sh = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash();
                        var tx = openWallet.MakeSingleAskTransaction(stw, sh, (byte)CasinoType.Bet, request, this.BetModel.BetIndex);

                        if (tx.IsNotNull() && this.BetModel.BetIndex > Blockchain.Singleton.HeaderHeight)
                        {
                            box.Notecase.SignAndSendTx(tx);
                            //var msg = this.WebLocalString($"广播简易码下注交易成功  {tx.Hash}", $"Relay easy code bet transaction completed  {tx.Hash}");
                            //await MessageService.Info(msg);
                            StateHasChanged();
                        }

                    }
                }
            }
            catch
            {
                return;
            }
        }
        void OpenBet(byte position)
        {
            this.DrawerVisible = true;
            this.BetModel.Position = position;
            this.BetModel.BetIndex = this.CurrentIndex;
        }
    }
}
