
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
using Akka.Util;

namespace OX.Web.Pages
{
    public partial class MobileLotto
    {
        IMessageService MessageService { get; set; }
        Fixed8 masterBalance = Fixed8.Zero;
        Fixed8 poolBalance = Fixed8.Zero;
        char[] keys = default;
        string LotteryResult = string.Empty;
        Fixed8 MinBet = Fixed8.Zero;
        bool DrawerVisible = false;
        LottoBetViewModel BetModel = new LottoBetViewModel() { CheckDatas = GetCheckOptions(), SpecialCode = "a", SpecialPosition = "0" };
        protected override void InitRoomPage()
        {
            BuildBetPoint();
            this.GetMinBet(CasinoSettingTypes.LottoMinBet, out MinBet);
        }

        protected override void ReloadGameData()
        {
            //masterBalance = GetBalance(Room.BankerAddress);
            poolBalance = GetBalance(Room.PoolAddress);
        }
        protected override void ReloadGameUI()
        {
            LotteryResult = string.Empty;
            if (this.Riddles.IsNotNull() && this.MineNonce > 0)
            {
                var guessKey = this.Riddles.GetGuessKey(this.Room.Request.Kind);
                if (guessKey.IsNotNull())
                {
                    LotteryResult = $"{guessKey.SpecialChar}-{guessKey.SpecialPosition}-{guessKey.ReRandomSanGongOrLottoString(this.MineNonce, this.CurrentIndex)}";
                }
            }
        }
        static CkeckData[] GetCheckOptions()
        {
            Random rd = new Random();
            List<CkeckData> list = new List<CkeckData>();
            for (uint i = 0; i < 10; i++)
            {
                var nonce = rd.Next(0, 10);
                List<CheckboxOption> cos = new List<CheckboxOption>();
                for (int k = 0; k < 10; k++)
                {
                    var co = new CheckboxOption { Label = k.ToString(), Value = k.ToString() };
                    if (nonce == k)
                        co.Checked = true;
                    cos.Add(co);
                }
                CkeckData cd = new CkeckData { N = i, Options = cos.ToArray() };
                list.Add(cd);
            }
            return list.ToArray();
        }
        GuessKey.BetAtom GetBetAtom(uint position)
        {
            switch (position)
            {
                case 0:
                    return GuessKey.BetAtom.P0;
                case 1:
                    return GuessKey.BetAtom.P1;
                case 2:
                    return GuessKey.BetAtom.P2;
                case 3:
                    return GuessKey.BetAtom.P3;
                case 4:
                    return GuessKey.BetAtom.P4;
                case 5:
                    return GuessKey.BetAtom.P5;
                case 6:
                    return GuessKey.BetAtom.P6;
                case 7:
                    return GuessKey.BetAtom.P7;
                case 8:
                    return GuessKey.BetAtom.P8;
                case 9:
                    return GuessKey.BetAtom.P9;
                default:
                    return default;
            }
        }
        void BuildBetPoint()
        {
            var c = this.BetModel.SpecialCode.ToCharArray()[0];
            BetPack pack = new BetPack() { SpecialChar = c, SpecialPosition = byte.Parse(this.BetModel.SpecialPosition) };
            foreach (var data in this.BetModel.CheckDatas)
            {
                foreach (var p in data.Options)
                {
                    if (p.Checked)
                    {
                        var atom = GetBetAtom(uint.Parse(p.Value));
                        pack.BetPostion(data.N, atom);
                    }
                }
            }
            this.BetModel.BetPack = pack;
            this.BetModel.Amount = (uint)(pack.TotalAmount().GetInternalValue() / Fixed8.D);
            if (pack.TryMerge(out string betpoint))
            {

            }
        }
        void OnCheckChange(string[] checkedValues)
        {
            BuildBetPoint();
        }
        void OpenBet()
        {
            this.DrawerVisible = true;
            this.BetModel.BetIndex = this.CurrentIndex;
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
        private async void doBet(Fixed8 amt)
        {
            BuildBetPoint();
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
            if (!this.BetModel.BetPack.TryMerge(out string betpoint))
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
                    BetRequest request = new BetRequest() { BetPoint = betpoint, From = this.Account.ScriptHash, Index = this.BetModel.BetIndex, BetAddress = this.Room.BetAddress, Passport = default };
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
                            this.BetModel = new LottoBetViewModel() { CheckDatas = GetCheckOptions(), SpecialCode = "a", SpecialPosition = "0" };
                            this.BuildBetPoint();
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

    }
}
