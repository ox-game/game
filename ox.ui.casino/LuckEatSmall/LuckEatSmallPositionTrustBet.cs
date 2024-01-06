using Akka.Actor.Dsl;
using OX.Bapps;
using OX.Casino;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.Persistence;
using OX.SmartContract;
using OX.VM;
using OX.Wallets;
using OX.Wallets.Models;
using OX.Wallets.NEP6;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OX.UI.Casino
{
    public partial class LuckEatSmallPositionTrustBet : DarkForm, IBetWatch
    {
        public class TrustAccountDescription
        {
            public UInt160 TrustAddress;
            public AssetTrustContract AssetTrustContract;
            public override string ToString()
            {
                return $"{TrustAddress.ToAddress()}  /   {Contract.CreateSignatureRedeemScript(AssetTrustContract.Truster).ToScriptHash().ToAddress()}";
            }
        }
        Module Module;
        INotecase Operater;
        MixRoom Room;
        uint PeroidBlocks;
        uint MinBetHeight;
        Fixed8 MinBet;
        uint initIndex;
        byte Position;
        //Dictionary<string, SignatureValidator<SeasonPassport>> passports = new Dictionary<string, SignatureValidator<SeasonPassport>>();
        public LuckEatSmallPositionTrustBet(Module module, INotecase operater, MixRoom room, Fixed8 minBet, uint index, byte position)
        {
            this.Module = module;
            this.Operater = operater;
            this.Room = room;
            this.MinBet = minBet;
            this.initIndex = index;
            this.Position = position;
            InitializeComponent();

            PeroidBlocks = Game.PeroidBlocks(room.Request);
            UpdateMinBetHeight();
            Dictionary<UInt160, AssetTrustContract> atcts = new Dictionary<UInt160, AssetTrustContract>();
            if (this.Operater.Wallet is OpenWallet openWallet)
            {
                foreach (var act in openWallet.GetAssetTrustContacts())
                {
                    if (act.Value.Targets.Contains(room.BetAddress))
                    {
                        atcts[act.Key] = act.Value;
                    }
                    else if (act.Value.SideScopes.IsNotNullAndEmpty())
                    {
                        foreach (var t in act.Value.SideScopes)
                        {
                            var ssl = Blockchain.Singleton.CurrentSnapshot.GetSides(t);
                            if (ssl.IsNotNull() && ssl.SideStateList.IsNotNullAndEmpty())
                            {
                                if (ssl.SideStateList.Select(m => m.SideScriptHash).Contains(room.BetAddress))
                                {
                                    atcts[act.Key] = act.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (atcts.IsNotNullAndEmpty())
            {
                foreach (var act in atcts)
                {
                    this.cbAccounts.Items.Add(new TrustAccountDescription { TrustAddress = act.Key, AssetTrustContract = act.Value });
                }
                this.cbAccounts.SelectedIndex = 0;
            }
        }
        bool inPrivateRoomMemberOrPublic(MixRoom room, UInt160 member)
        {
            if (room.Request.Permission == RoomPermission.Public) return true;
            return room.RoomMemberSetting.IsNotNull() && room.RoomMemberSetting.Members.IsNotNullAndEmpty() && room.RoomMemberSetting.Members.Contains(member);
        }

        private void ClaimForm_Load(object sender, EventArgs e)
        {
            var fee = Fixed8.One + this.Room.GetPrivateRoomBetFee();
            this.Text = UIHelper.LocalString($"大吃小信托下注  {this.Position}号位         单注费 {fee} OXC", $"EatSmall Trust Bet in Position {this.Position}          bet fee {fee} OXC");
            this.lb_roomId.Text = UIHelper.LocalString($"房间号:    {this.Room.RoomId}", $"Room Id:    {this.Room.RoomId}");
            this.lb_Index.Text = UIHelper.LocalString($"下注高度:", $"Bet Height:");
            this.lb_balance.Text = UIHelper.LocalString("账户余额:", "Balance:");
            this.lb_betamt.Text = UIHelper.LocalString("下注额:", "Bet Amount:");
            this.lb_from.Text = UIHelper.LocalString("信托账户:", "Trust Account:");
            this.bt_bet.Text = UIHelper.LocalString($"下注{this.Position}号位", $"Bet in position {this.Position}");
            this.lb_specialchar.Text = UIHelper.LocalString("特码:", "Special Code:");
            Random rd = new Random();
            this.cb_specialchar.Items.Clear();
            foreach (var c in GuessKey.SpecialChars)
            {
                this.cb_specialchar.Items.Add(c.ToString());
            }
            this.cb_specialchar.SelectedIndex = rd.Next(0, GuessKey.SpecialChars.Length);
            if (this.Room.Request.Permission == RoomPermission.Private)
            {
                this.lb_specialchar.Visible = false;
                this.cb_specialchar.Visible = false;
            }
        }


        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        void UpdateMinBetHeight()
        {
            var height = Blockchain.Singleton.HeaderHeight;
            var index = height;
            var remainder = index % PeroidBlocks;
            var zindex = index - remainder;
            var pb = PeroidBlocks;
            if (PeroidBlocks == 10)
            {
                zindex += PeroidBlocks;
                pb = PeroidBlocks * 2;
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
                zindex = index;
            }
            else
            {
                zindex += GameHelper.ReviseIndex(this.Room);
                if (zindex < height) zindex += PeroidBlocks;
            }

            var fremainder = zindex - height;
            if (fremainder <= (PeroidBlocks == 10 ? 5 : 17))
            {
                this.cb_Height.SpecialBorderColor = Color.Red;
                zindex += pb;
            }
            else
            {
                this.cb_Height.SpecialBorderColor = null;
            }

            if (this.MinBetHeight != zindex)
            {
                this.MinBetHeight = zindex;
                this.cb_Height.Items.Clear();
                for (uint i = 0; i < 10; i++)
                {
                    this.cb_Height.Items.Add(this.MinBetHeight + pb * i);
                }
                if (initIndex > this.MinBetHeight)
                {
                    this.cb_Height.SelectedItem = initIndex;
                }
                else
                    this.cb_Height.SelectedIndex = 0;
            }
            this.cb_Height.Refresh();
        }
        public void HeartBeat(HeartBeatContext context)
        {
            UpdateMinBetHeight();
            if (context.IsNormalSync)
            {
                EnableBet();
            }
            else
            {
                DisableBet();
            }

        }
        void DisableBet()
        {
            bt_bet.Enabled = false;
        }
        void EnableBet()
        {
            bt_bet.Enabled = true;
        }
        private void darkComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tb_balance.Text = "0";
            var ali = this.cbAccounts.SelectedItem as TrustAccountDescription;
            var acts = Blockchain.Singleton.CurrentSnapshot.Accounts.GetAndChange(ali.TrustAddress, () => null);
            if (acts.IsNotNull())
            {
                this.tb_balance.Text = acts.GetBalance(this.Room.Request.AssetId).ToString();
            }
        }

        private void bt_NewRoom_Click(object sender, EventArgs e)
        {
            var ali = this.cbAccounts.SelectedItem as TrustAccountDescription;
            if (ali.IsNull()) return;
            if (Blockchain.Singleton.HeaderHeight <= ali.AssetTrustContract.LastTransferIndex + 10) return;
            if (this.cb_Height.SelectedItem == null)
                return;
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin.IsNotNull())
            {
                if (bizPlugin.GetRoomDestory(this.Room.RoomId).IsNotNull())
                {
                    DarkMessageBox.ShowInformation(UIHelper.LocalString($"房间{Room.RoomId}已被永久封闭", $"Room{Room.RoomId} has been permanently closed"), "");
                    return;
                }
            }
            if (this.cb_Height.SelectedItem is uint index)
            {
                if (index % PeroidBlocks != GameHelper.ReviseIndex(this.Room))
                    return;
                if (!Fixed8.TryParse(this.tb_amount.Text, out Fixed8 amt))
                    return;
                var from = this.cbAccounts.SelectedItem as TrustAccountDescription;
                var address = from?.TrustAddress.ToAddress();
                if (address.IsNullOrEmpty())
                    return;
                if (!Fixed8.TryParse(this.tb_balance.Text, out Fixed8 balanc))
                    return;
                if (amt > balanc)
                    return;
                if (amt < MinBet)
                {
                    DarkMessageBox.ShowInformation(UIHelper.LocalString($"最低投注额为{MinBet}", $"the least bet amount {MinBet}"), "");
                    return;
                }
                try
                {
                    amt = Fixed8.One * (amt.GetInternalValue() / Fixed8.D);
                    var spc = this.cb_specialchar.Text;
                    if (spc.IsNullOrEmpty()) return;
                    var spcc = spc.ToCharArray()[0];
                    if (!GuessKey.SpecialChars.Contains(spcc)) return;
                    BetRequest request = new BetRequest() { BetPoint = $"{this.Position.ToString()}|{spcc}", From = from.TrustAddress, Index = index, BetAddress = this.Room.BetAddress, Passport = default };
                    var trusteeAddress = Contract.CreateSignatureRedeemScript(from.AssetTrustContract.Trustee).ToScriptHash();
                    var trustee = this.Operater.Wallet.GetAccount(trusteeAddress);
                    if (trustee.IsNotNull() && !trustee.WatchOnly && this.Operater.Wallet is OpenWallet openWallet)
                    {
                        using (ScriptBuilder sb = new ScriptBuilder())
                        {
                            var account = LockAssetHelper.CreateAccount(openWallet, from.AssetTrustContract.GetContract(), trustee.GetKey());//lock asset account have a some private key with master account
                            if (account != null)
                            {
                                List<UTXO> utxos = new List<UTXO>();
                                foreach (var r in openWallet.GetAssetTrustUTXO().Where(m => m.Value.AssetId.Equals(Room.Request.AssetId) && m.Value.ScriptHash.Equals(from.TrustAddress)))
                                {
                                    utxos.Add(new UTXO
                                    {
                                        Address = r.Value.ScriptHash,
                                        Value = r.Value.Value.GetInternalValue(),
                                        TxId = r.Key.TxId,
                                        N = r.Key.N
                                    });
                                }
                                List<string> excludedUtxoKeys = new List<string>();
                                List<TransactionOutput> outputs = new List<TransactionOutput>();
                                List<CoinReference> inputs = new List<CoinReference>();
                                bool ok = false;
                                int m = 0;
                                var betFee = Room.GetPrivateRoomBetFee();
                                if (Room.Request.AssetId == Blockchain.OXC)
                                {
                                    if (utxos.SortSearch(amt.GetInternalValue() + Fixed8.D + betFee.GetInternalValue(), excludedUtxoKeys, out UTXO[] selectedUtxos, out long remainder))
                                    {
                                        outputs.Add(new TransactionOutput { AssetId = Room.Request.AssetId, Value = amt, ScriptHash = Room.BetAddress });
                                        if (betFee > Fixed8.Zero)
                                        {
                                            outputs.Add(new TransactionOutput()
                                            {
                                                AssetId = Blockchain.OXC,
                                                ScriptHash = Room.Holder,
                                                Value = betFee
                                            });
                                        }
                                        if (remainder > 0)
                                        {
                                            outputs.Add(new TransactionOutput { AssetId = Room.Request.AssetId, Value = new Fixed8(remainder), ScriptHash = from.TrustAddress });
                                        }

                                        foreach (var utxo in selectedUtxos)
                                        {
                                            inputs.Add(new CoinReference { PrevHash = utxo.TxId, PrevIndex = utxo.N });
                                        }
                                        ok = true;
                                    }
                                }
                                else
                                {
                                    if (utxos.SortSearch(amt.GetInternalValue(), excludedUtxoKeys, out UTXO[] selectedUtxos, out long remainder))
                                    {
                                        outputs.Add(new TransactionOutput { AssetId = Room.Request.AssetId, Value = amt, ScriptHash = Room.BetAddress });
                                        if (remainder > 0)
                                        {
                                            outputs.Add(new TransactionOutput { AssetId = Room.Request.AssetId, Value = new Fixed8(remainder), ScriptHash = from.TrustAddress });
                                        }

                                        foreach (var utxo in selectedUtxos)
                                        {
                                            inputs.Add(new CoinReference { PrevHash = utxo.TxId, PrevIndex = utxo.N });
                                        }
                                        m++;
                                    }
                                    List<UTXO> feeutxos = new List<UTXO>();
                                    foreach (var r in openWallet.GetAssetTrustUTXO().Where(m => m.Value.AssetId.Equals(Blockchain.OXC) && m.Value.ScriptHash.Equals(from.TrustAddress)))
                                    {
                                        feeutxos.Add(new UTXO
                                        {
                                            Address = r.Value.ScriptHash,
                                            Value = r.Value.Value.GetInternalValue(),
                                            TxId = r.Key.TxId,
                                            N = r.Key.N
                                        });
                                    }
                                    if (feeutxos.SortSearch(Fixed8.D + betFee.GetInternalValue(), excludedUtxoKeys, out UTXO[] selectedUtxosFee, out long remainderFee))
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
                                            outputs.Add(new TransactionOutput { AssetId = Blockchain.OXC, Value = new Fixed8(remainderFee), ScriptHash = from.TrustAddress });
                                        }
                                        foreach (var utxo in selectedUtxosFee)
                                        {
                                            inputs.Add(new CoinReference { PrevHash = utxo.TxId, PrevIndex = utxo.N });
                                        }
                                        m++;
                                    }
                                }
                                if (ok || m == 2)
                                {
                                    RangeTransaction tx = new RangeTransaction
                                    {
                                        MaxIndex = request.Index,
                                        Attributes = new TransactionAttribute[] {
                                             new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedData, Data =request.ToArray()},
                                            new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedPublicKey, Data = from.AssetTrustContract.Truster.EncodePoint(true) },
                                            new TransactionAttribute { Usage = TransactionAttributeUsage.RelatedScriptHash, Data = from.TrustAddress.ToArray() }
                                        },
                                        Outputs = outputs.ToArray(),
                                        Inputs = inputs.ToArray(),
                                        Witnesses = new Witness[0]
                                    };
                                    tx = LockAssetHelper.Build(tx, new AvatarAccount[] { account });
                                    if (tx.IsNotNull())
                                    {
                                        this.Operater.Wallet.ApplyTransaction(tx);
                                        this.Operater.Relay(tx);
                                        from.AssetTrustContract.LastTransferIndex = Blockchain.Singleton.HeaderHeight;
                                        if (this.Operater != default)
                                        {
                                            string msg = UIHelper.LocalString($"广播信托下注交易成功  {tx.Hash}", $"Relay  trust bet transaction completed  {tx.Hash}");
                                            //Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                                            DarkMessageBox.ShowInformation(msg, "");
                                            this.Close();
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
        }

        private void tb_amount_TextChanged(object sender, EventArgs e)
        {
            if (!Fixed8.TryParse(this.tb_amount.Text, out Fixed8 amt))
            {
                var s = this.tb_amount.Text;
                if (s.Length > 0)
                {
                    s = s.Substring(0, s.Length - 1);
                    this.tb_amount.Clear();
                    this.tb_amount.AppendText(s);
                }
            }
            if (Fixed8.TryParse(this.tb_balance.Text, out Fixed8 balanc))
            {
                if (amt > balanc)
                {
                    var s = this.tb_amount.Text;
                    if (s.Length > 0)
                    {
                        s = s.Substring(0, s.Length - 1);
                        this.tb_amount.Clear();
                        this.tb_amount.AppendText(s);
                    }
                }
            }
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {

        }

        private void bt_rnd_Click_1(object sender, EventArgs e)
        {

        }
    }
}
