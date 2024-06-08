
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
    public partial class Bury
    {
        public override string PageTitle => this.WebLocalString("爆雷", "Bury");
        [Inject]
        IMessageService MessageService { get; set; }
        protected ICasinoProvider Provider { get; set; }
        public EthAssetBalanceState BalanceState = new EthAssetBalanceState();
        bool DrawerVisible = false;
        Fixed8 MinBet = Fixed8.Zero;
        BuryMergeModel[] BuryMergeModels { get; set; } = new BuryMergeModel[0];
        BuryViewModel BuryModel { get; set; } = new BuryViewModel();
        string[] Plains200 { get; set; } = new string[0];
        string[] Chipers200 { get; set; } = new string[0];
        string[] PlainsAll { get; set; } = new string[0];
        string[] ChipersAll { get; set; } = new string[0];

        protected override void OnCasinoInit()
        {
            this.Provider = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            this.GetMinBet(CasinoSettingTypes.BuryMinBet, out MinBet);
            this.ReloadBurys();
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
        protected override async Task MetaMaskService_AccountChangedEvent(string arg)
        {
            await base.MetaMaskService_AccountChangedEvent(arg);
            this.BalanceState = new EthAssetBalanceState();
            if (this.EthID.IsNotNull() && this.Box.Notecase.Wallet is OpenWallet openWallet)
            {
                var balanceState = openWallet.QueryBalanceState(this.EthID);
                if (balanceState.IsNotNull())
                {
                    this.BalanceState = balanceState.TryGetBalance(Blockchain.OXC);
                }
            }
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
            if (this.EthID.IsNotNull() && this.Box.Notecase.Wallet is OpenWallet openWallet)
            {
                var balanceState = openWallet.QueryBalanceState(this.EthID);
                if (balanceState.IsNotNull())
                {
                    this.BalanceState = balanceState.TryGetBalance(Blockchain.OXC);
                }
            }
        }
        void CloseBury()
        {
            this.DrawerVisible = false;
        }
        private async void GoBury()
        {
            if (this.Valid && this.EthID.MapAddress.IsNotNull())
            {

                var amount = Fixed8.One * this.BuryModel.Amount;
                if (this.BalanceState.AvailableBalance >= amount + Fixed8.One * 2)
                {
                    doBury(this.BuryModel.Amount);
                }

            }
            CloseBury();
            await Task.CompletedTask;
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
                if (this.Box.Notecase.Wallet is OpenWallet openWallet)
                {
                    var key = openWallet.GetHeldAccounts().First().GetKey();
                    PrivateBuryRequest PrivateBuryRequest = new PrivateBuryRequest { Rand = (uint)new Random().Next(0, int.MaxValue), CipherBuryPoint = this.BuryModel.SecretCode };
                    VerifyPrivateBuryRequest VerifyPrivateBuryRequest = new VerifyPrivateBuryRequest { From = this.EthID.MapAddress, PrivateBuryRequest = PrivateBuryRequest };
                    if (key.IsNull()) return;
                    var encryptDataForCipherCode = PrivateBuryRequest.ToArray().Encrypt(key, casino.CasinoMasterAccountPubKey);
                    BuryRequest request = new BuryRequest
                    {
                        From = this.EthID.MapAddress,
                        BetAddress = casino.BuryBetAddress,
                        PlainBuryPoint = this.BuryModel.PlainCode,
                        VerifyHash = VerifyPrivateBuryRequest.Hash,
                        CryptoData = encryptDataForCipherCode
                    };

                    var allutxos = openWallet.GetAllEthereumMapUTXOs();
                    if (allutxos.IsNotNullAndEmpty())
                    {
                        var us = allutxos.Where(m => m.Value.Output.AssetId == Blockchain.OXC && m.Value.EthAddress == this.EthID.EthAddress && m.Value.LockExpirationIndex < Blockchain.Singleton.Height);
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
                            Dictionary<UInt160, Contract> contracts = new Dictionary<UInt160, Contract>();
                            if (utxos.SortSearch(amount.GetInternalValue() + Fixed8.D * 2, excludedUtxoKeys, out EthMapUTXO[] selectedUtxos, out long remainder))
                            {
                                TransactionOutput output = new TransactionOutput()
                                {
                                    AssetId = Blockchain.OXC,
                                    ScriptHash = casino.BuryBetAddress,
                                    Value = amount
                                };
                                outputs.Add(output);
                                if (remainder > 0)
                                {
                                    outputs.Add(new TransactionOutput { AssetId = Blockchain.OXC, Value = new Fixed8(remainder), ScriptHash = this.EthID.MapAddress });
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


                            if (ok)
                            {
                                var oxKey = openWallet.GetHeldAccounts().First().GetKey();
                                RangeTransaction tx = new RangeTransaction
                                {
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
                                                     new TransactionAttribute { Usage = TransactionAttributeUsage.Tip5, Data =oxKey.PublicKey.EncodePoint(true) },
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
                                        var msg = this.WebLocalString($"广播以太坊映射账户埋雷交易成功  {tx.Hash}", $"Relay transfer ethereum map account bury transaction completed  {tx.Hash}");
                                        await MessageService.Info(msg);
                                        this.BuryModel = new BuryViewModel();
                                        StateHasChanged();
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
            //await Task.CompletedTask;
        }
        protected override void StateDispatcher_ServerStateNotice(IServerStateMessage message)
        {
            base.StateDispatcher_ServerStateNotice(message);
            if (message is BuryMessage)
            {
                this.ReloadBurys();
                InvokeAsync(StateHasChanged);
            }
        }
         
    }
}
