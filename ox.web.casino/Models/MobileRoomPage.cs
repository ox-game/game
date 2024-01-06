
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
using OX.Bapps;
using OX.UI.Casino;
using OX.Casino;
using System.Linq;
using OX.Web.Pages;
using OX.Ledger;
using System.Collections.Generic;
using OX.Wallets.Eths;
using OX.SmartContract;
using OX.Network.P2P.Payloads;
using Akka.IO;
using NuGet.Protocol.Plugins;
using OX.UI.Messages;
using System.Collections;
using AntDesign;

namespace OX.Web.Models
{
    public abstract class MobileRoomPage : WebBoxComponentBase
    {
        [Inject]
        IMessageService MessageService { get; set; }
        [Parameter]
        public MixRoom Room { get; set; }
        MixRoom _room;
        [Parameter]
        public WalletAccount Account { get; set; }
        WalletAccount _account;
        [Parameter]
        public Block LastBlock { get; set; }
        Block _lastBLock;
        public Fixed8 AssetBalance = Fixed8.Zero;
        protected override Task OnParametersSetAsync()
        {
            var needReset = _room != Room || _account != Account;
            var needLoad = _lastBLock != LastBlock;
            if (needReset)
            {
                _room = Room;
                if (this.Room.IsNotNull())
                {
                    AssetBalance = GetBalance(this.Account.ScriptHash);
                }
                _account = Account;
                this.OnInit(true);
            }
            else if (needLoad)
            {
                _lastBLock = LastBlock;

                if (this.Room.IsNotNull())
                {
                    AssetBalance = GetBalance(this.Account.ScriptHash);
                }
                this.OnInit(false);
            }
            return base.OnParametersSetAsync();
        }
        public uint CurrentIndex { get; set; }
        protected ICasinoProvider Provider { get; set; }
        protected ulong MineNonce { get; set; }
        protected uint PeroidBlocks { get; set; }

        public IEnumerable<KeyValuePair<BetKey, Betting>> Bettings { get; protected set; } = default;
        public IEnumerable<KeyValuePair<RoundClearKey, RoundClearResult>> ClearResult { get; protected set; } = default;
        public List<UInt256> RoundClearTxIds { get; protected set; } = default;
        public List<TransactionOutput> Prizes { get; protected set; } = default;
        public Riddles Riddles;
        protected override Task OnInitializedAsync()
        {
            //this.OnInit();
            return base.OnInitializedAsync();
        }
        protected void OnInit(bool needReset)
        {
            this.Provider = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            PeroidBlocks = Game.PeroidBlocks(this.Room.Request);
            InitRoomPage();
            if (needReset)
                ResetAnShowIndex();
            else ShowIndex();
        }
        protected abstract void InitRoomPage();
        protected abstract void ReloadGameData();
        protected abstract void ReloadGameUI();
        uint GetReviseIndex()
        {
            return this.Room.ReviseIndex();
        }
        public void ResetAnShowIndex()
        {
            var index = Blockchain.Singleton.HeaderHeight;
            if (PeroidBlocks == 10)
            {
                var pb = PeroidBlocks * 2;
                var c = index % pb;
                index -= c;
                if (this.Room.Request.Flag % 2 == 1)
                {
                    int cc = c > 10 ? 30 : 10;
                    index += (uint)cc;
                }
                else
                {
                    if (c > 0) index += pb;
                }
            }
            else
            {
                var rem = index % PeroidBlocks;
                var newIndex = index - rem + GetReviseIndex();
                if (newIndex < index)
                    newIndex += PeroidBlocks;
                index = newIndex;
            }

            if (this.CurrentIndex != index)
            {
                this.CurrentIndex = index;
                ShowIndex();
            }
        }
        public void ShowIndex()
        {
            ReloadData();
            ReloadUI();
        }
        public void ReloadData()
        {
            this.Bettings = this.Provider.GetBettings(this.Room.BetAddress, this.CurrentIndex);
            this.ClearResult = this.Provider.GetRoundClearResults(this.Room.BetAddress, this.CurrentIndex);
            List<UInt256> txids = new List<UInt256>();
            foreach (var cr in this.ClearResult)
            {
                var tx = Blockchain.Singleton.GetSnapshot().Transactions.TryGet(cr.Value.TxHash);
                if (tx.IsNotNull())
                    foreach (var id in tx.Transaction.Inputs.Select(m => m.PrevHash))
                    {
                        if (!txids.Contains(id)) txids.Add(id);
                    }
            }
            this.RoundClearTxIds = txids;
            this.Riddles = default;
            this.MineNonce = 0;
            var riddleRecord = this.Provider.GetRiddles(this.CurrentIndex);
            if (riddleRecord.IsNotNull())
            {
                Riddles = riddleRecord;
                this.MineNonce = BlockHelper.GetMineNonce(this.CurrentIndex);
            }
            ReloadGameData();
        }
        protected virtual void ReloadUI()
        {
            this.ReloadGameUI();
            this.reloadRoundClear();
        }
        private void reloadRoundClear()
        {
            Prizes = new List<TransactionOutput>();
            if (this.ClearResult.IsNotNullAndEmpty())
            {
                foreach (var result in this.ClearResult)
                {
                    var tx = Blockchain.Singleton.GetTransaction(result.Value.TxHash);
                    if (tx is ReplyTransaction rt)
                    {
                        var casinoBapp = Bapp.GetBapp<CasinoBapp>();
                        var bizshs = casinoBapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();

                        if (rt.GetDataModel<RoundClear>(bizshs, (byte)CasinoType.RoundClear, out RoundClear roundClear))
                        {
                            if (roundClear.BetAddress == this.Room.BetAddress && roundClear.Index == this.CurrentIndex)
                            {
                                foreach (var output in tx.Outputs)
                                {
                                    if (output.AssetId == this.Room.Request.AssetId)
                                    {
                                        Prizes.Add(output);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        protected void Previous()
        {
            var pb = this.PeroidBlocks;
            if (this.PeroidBlocks == 10)
                pb = this.PeroidBlocks * 2;
            if (this.CurrentIndex > pb)
            {
                var preCurrentIndex = this.CurrentIndex;
                var preReviseIndex = GetReviseIndex();
                this.CurrentIndex -= pb;
                var nextReviseIndex = GetReviseIndex();
                if (preReviseIndex != nextReviseIndex)
                {
                    this.CurrentIndex -= preReviseIndex;
                }
            }
            else this.CurrentIndex = 0;

            this.ShowIndex();
        }
        protected void Next()
        {
            if (this.PeroidBlocks == 10)
            {
                this.CurrentIndex += this.PeroidBlocks * 2;
            }
            else
            {
                var preCurrentIndex = this.CurrentIndex;
                var preReviseIndex = GetReviseIndex();
                this.CurrentIndex += this.PeroidBlocks;
                var nextReviseIndex = GetReviseIndex();
                if (preReviseIndex != nextReviseIndex)
                {
                    this.CurrentIndex += nextReviseIndex;
                }
            }
            this.ShowIndex();
        }
        protected void Current()
        {
            this.ResetAnShowIndex();
        }
        protected Fixed8 GetBalance(UInt160 address)
        {
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(address, () => null);
            return acts.IsNotNull() ? acts.GetBalance(Room.Request.AssetId) : Fixed8.Zero;
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
    }
}
