
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
    public partial class Lotto
    {
        public override string PageTitle => new EnumItem<GameKind>(GameKind.Lotto).ToWebString(this);
        [Inject]
        IMessageService MessageService { get; set; }
        //Fixed8 masterBalance = Fixed8.Zero;
        Fixed8 poolBalance = Fixed8.Zero;
        public EthAssetBalanceState BalanceState = new EthAssetBalanceState();
        char[] keys = default;
        string LotteryResult = string.Empty;
        bool DrawerVisible = false;
        Fixed8 MinBet = Fixed8.Zero;
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
        void OpenBet()
        {
            this.DrawerVisible = true;
            this.BetModel.BetIndex = this.CurrentIndex;
            this.BalanceState = this.GetMapBalance();
        }
        void CloseBet()
        {
            this.DrawerVisible = false;
        }
        private async void Bet()
        {
            if (this.Valid && VerifyRoom(this.Provider, this.Room))
            {
                if (this.BetModel.Amount > 0 && this.EthID.MapAddress.IsNotNull())
                {
                    var amount = Fixed8.One * this.BetModel.Amount;
                    if (this.BalanceState.AvailableBalance > amount)
                    {
                        doBet(amount);
                    }
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
                if (Room.ValidPrivateRoom(this.EthID.MapAddress))
                {
                    amt = Fixed8.One * (amt.GetInternalValue() / Fixed8.D);

                    var spc = this.BetModel.SpecialCode;
                    if (spc.IsNullOrEmpty()) return;
                    var spcc = spc.ToCharArray()[0];
                    if (!GuessKey.SpecialChars.Contains(spcc)) return;
                    BetRequest request = new BetRequest() { BetPoint = betpoint, From = this.EthID.MapAddress, Index = this.BetModel.BetIndex, BetAddress = this.Room.BetAddress, Passport = default };
                    if (this.Box.Notecase.Wallet is OpenWallet openWallet)
                    {
                        var allutxos = openWallet.GetAllEthereumMapUTXOs();
                        if (allutxos.IsNotNullAndEmpty())
                        {
                            var us = allutxos.Where(m => m.Value.Output.AssetId == this.Room.Request.AssetId && m.Value.EthAddress == this.EthID.EthAddress && m.Value.LockExpirationIndex < Blockchain.Singleton.Height);
                            if (us.IsNotNullAndEmpty())
                            {
                                List<EthMapUTXO> utxos = new List<EthMapUTXO>();
                                foreach (var r in us)
                                {
                                    utxos.Add(new EthMapUTXO
                                    {
                                        Address = r.Value.Output.ScriptHash,
                                        Value = r.Value.Output.Value.GetInternalValue(),
                                        TxId = r.Key.PrevHash,
                                        N = r.Key.PrevIndex,
                                        EthAddress = r.Value.EthAddress,
                                        LockExpirationIndex = r.Value.LockExpirationIndex
                                    });
                                }
                                List<string> excludedUtxoKeys = new List<string>();
                                List<TransactionOutput> outputs = new List<TransactionOutput>();
                                List<CoinReference> inputs = new List<CoinReference>();
                                bool ok = false;
                                int m = 0;
                                var betFee = Room.GetPrivateRoomBetFee();
                                Dictionary<UInt160, Contract> contracts = new Dictionary<UInt160, Contract>();
                                if (Room.Request.AssetId == Blockchain.OXC)
                                {
                                    if (utxos.SortSearch(amt.GetInternalValue() + Fixed8.D + betFee.GetInternalValue(), excludedUtxoKeys, out EthMapUTXO[] selectedUtxos, out long remainder))
                                    {
                                        TransactionOutput output = new TransactionOutput()
                                        {
                                            AssetId = Room.Request.AssetId,
                                            ScriptHash = this.Room.BetAddress,
                                            Value = amt
                                        };
                                        outputs.Add(output);

                                        if (betFee > Fixed8.Zero)
                                        {
                                            output = new TransactionOutput()
                                            {
                                                AssetId = Room.Request.AssetId,
                                                ScriptHash = Room.Holder,
                                                Value = betFee
                                            };
                                            outputs.Add(output);
                                        }
                                        if (remainder > 0)
                                        {
                                            outputs.Add(new TransactionOutput { AssetId = Room.Request.AssetId, Value = new Fixed8(remainder), ScriptHash = this.EthID.MapAddress });
                                        }
                                        foreach (var utxo in selectedUtxos)
                                        {
                                            inputs.Add(new CoinReference { PrevHash = utxo.TxId, PrevIndex = utxo.N });
                                            EthereumMapTransaction emt = new EthereumMapTransaction { EthereumAddress = utxo.EthAddress, LockExpirationIndex = utxo.LockExpirationIndex };
                                            var c = emt.GetContract();
                                            var esh = c.ScriptHash;
                                            if (!contracts.ContainsKey(esh))
                                                contracts[esh] = c;
                                        }
                                        ok = true;
                                    }
                                }
                                else
                                {
                                    if (utxos.SortSearch(amt.GetInternalValue(), excludedUtxoKeys, out EthMapUTXO[] selectedUtxos, out long remainder))
                                    {
                                        TransactionOutput output = new TransactionOutput()
                                        {
                                            AssetId = Room.Request.AssetId,
                                            ScriptHash = this.Room.BetAddress,
                                            Value = amt
                                        };
                                        outputs.Add(output);
                                        if (remainder > 0)
                                        {
                                            outputs.Add(new TransactionOutput { AssetId = Room.Request.AssetId, Value = new Fixed8(remainder), ScriptHash = this.EthID.MapAddress });
                                        }
                                        foreach (var utxo in selectedUtxos)
                                        {
                                            inputs.Add(new CoinReference { PrevHash = utxo.TxId, PrevIndex = utxo.N });
                                            EthereumMapTransaction emt = new EthereumMapTransaction { EthereumAddress = utxo.EthAddress, LockExpirationIndex = utxo.LockExpirationIndex };
                                            var c = emt.GetContract();
                                            var esh = c.ScriptHash;
                                            if (!contracts.ContainsKey(esh))
                                                contracts[esh] = c;
                                        }
                                        m++;
                                    }
                                    var usFee = allutxos.Where(m => m.Value.Output.AssetId == Blockchain.OXC && m.Value.EthAddress == this.EthID.EthAddress && m.Value.LockExpirationIndex < Blockchain.Singleton.Height);
                                    if (usFee.IsNotNullAndEmpty())
                                    {
                                        List<EthMapUTXO> feeutxos = new List<EthMapUTXO>();
                                        foreach (var r in usFee)
                                        {
                                            feeutxos.Add(new EthMapUTXO
                                            {
                                                Address = r.Value.Output.ScriptHash,
                                                Value = r.Value.Output.Value.GetInternalValue(),
                                                TxId = r.Key.PrevHash,
                                                N = r.Key.PrevIndex,
                                                EthAddress = r.Value.EthAddress,
                                                LockExpirationIndex = r.Value.LockExpirationIndex
                                            });
                                        }


                                        if (feeutxos.SortSearch(Fixed8.D + betFee.GetInternalValue(), excludedUtxoKeys, out EthMapUTXO[] selectedUtxosFee, out long remainderFee))
                                        {
                                            if (betFee > Fixed8.Zero)
                                            {
                                                outputs.Add(new TransactionOutput()
                                                {
                                                    AssetId = Blockchain.OXC,
                                                    ScriptHash = Room.Holder,
                                                    Value = betFee
                                                });
                                            }
                                            if (remainderFee > 0)
                                            {
                                                outputs.Add(new TransactionOutput { AssetId = Blockchain.OXC, Value = new Fixed8(remainderFee), ScriptHash = this.EthID.MapAddress });
                                            }
                                            foreach (var utxo in selectedUtxos)
                                            {
                                                inputs.Add(new CoinReference { PrevHash = utxo.TxId, PrevIndex = utxo.N });
                                                EthereumMapTransaction emt = new EthereumMapTransaction { EthereumAddress = utxo.EthAddress, LockExpirationIndex = utxo.LockExpirationIndex };
                                                var c = emt.GetContract();
                                                var esh = c.ScriptHash;
                                                if (!contracts.ContainsKey(esh))
                                                    contracts[esh] = c;
                                            }
                                            m++;
                                        }
                                    }
                                }
                                if (ok || m == 2)
                                {
                                    var oxKey = openWallet.GetHeldAccounts().First().GetKey();
                                    RangeTransaction tx = new RangeTransaction
                                    {
                                        MaxIndex = request.Index,
                                        Outputs = outputs.ToArray(),
                                        Inputs = inputs.ToArray(),
                                        Witnesses = new Witness[0]
                                    };
                                    var stringToSign = tx.InputOutputHash.ToArray().ToHexString();
                                    var signatureData = await this.MetaMaskService.PersonalSign(stringToSign);
                                    var signer = new Nethereum.Signer.EthereumMessageSigner();
                                    var ethaddress = signer.EncodeUTF8AndEcRecover(stringToSign, signatureData);
                                    if (ethaddress.ToLower() == this.EthID.EthAddress.ToLower())
                                    {
                                        tx.Attributes = new TransactionAttribute[] {
                                                    new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedData, Data =request.ToArray()},
                                                    new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedPublicKey, Data =oxKey.PublicKey.EncodePoint(true) },
                                                    new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedScriptHash, Data =this.EthID.MapAddress.ToArray() },
                                                    new TransactionAttribute { Usage = TransactionAttributeUsage.EthSignature, Data = System.Text.Encoding.UTF8.GetBytes(signatureData) }
                                                };
                                        List<AvatarAccount> avatars = new List<AvatarAccount>();
                                        foreach (var c in contracts)
                                        {
                                            avatars.Add(LockAssetHelper.CreateAccount(openWallet, c.Value, oxKey));
                                        }
                                        tx = LockAssetHelper.Build(tx, avatars.ToArray());
                                        if (tx.IsNotNull())
                                        {
                                            this.Box.Notecase.Wallet.ApplyTransaction(tx);
                                            this.Box.Notecase.Relay(tx);
                                            this.EthID.SetLastTransactionIndex(Blockchain.Singleton.Height);
                                            var msg = this.WebLocalString($"广播以太坊映射账户乐透交易成功  {tx.Hash}", $"Relay transfer ethereum map account letto completed  {tx.Hash}");
                                            await MessageService.Info(msg);
                                            this.BetModel = new LottoBetViewModel() { CheckDatas = GetCheckOptions(), SpecialCode = "a", SpecialPosition = "0" };
                                            this.BuildBetPoint();
                                            StateHasChanged();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return;
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
        protected override void StateDispatcher_ServerStateNotice(IServerStateMessage message)
        {
            base.StateDispatcher_ServerStateNotice(message);
            if (message is RoomBetMessage betMessage)
            {
                var betAddress = betMessage.BetRequest.BetAddress.ToAddress();
                var betIndex = betMessage.BetRequest.Index;
                if (betAddress == this.betaddress && betIndex == this.CurrentIndex)
                {
                    this.ShowIndex();
                    InvokeAsync(StateHasChanged);
                }
            }
            else if (message is RoomPrizeMessage prizeMessage)
            {
                var betAddress = prizeMessage.RoundClear.BetAddress.ToAddress();
                var betIndex = prizeMessage.RoundClear.Index;
                if (betAddress == this.betaddress && betIndex == this.CurrentIndex)
                {
                    this.ShowIndex();
                    InvokeAsync(StateHasChanged);
                }
            }
        }
        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            await base.MetaMaskService_AccountChangedEvent(arg);
            this.BalanceState = this.GetMapBalance();
        }
    }
}
