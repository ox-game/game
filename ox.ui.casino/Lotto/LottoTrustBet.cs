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
    public partial class LottoTrustBet : DarkForm, IBetWatch
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
        Dictionary<string, SignatureValidator<SeasonPassport>> passports = new Dictionary<string, SignatureValidator<SeasonPassport>>();
        BetPack BetPack = default;
        public LottoTrustBet(Module module, INotecase operater, MixRoom room, Fixed8 minBet, uint index)
        {
            this.Module = module;
            this.Operater = operater;
            this.Room = room;
            this.MinBet = minBet;
            this.initIndex = index;
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
            this.Text = UIHelper.LocalString($"乐透信托下注      单注费 {fee} OXC", $"Lotto Trust Bet      bet fee {fee} OXC");
            this.lb_roomId.Text = UIHelper.LocalString($"房间号:    {this.Room.RoomId}", $"Room Id:    {this.Room.RoomId}");
            this.lb_Index.Text = UIHelper.LocalString($"下注高度:", $"Bet Height:");
            this.lb_balance.Text = UIHelper.LocalString("信托余额:", "Trust Balance:");
            this.lb_betamt.Text = UIHelper.LocalString("下注额:", "Bet Amount:");
            this.lb_from.Text = UIHelper.LocalString("信托账户:", "Trust Account:");
            this.lb_specialchar.Text = UIHelper.LocalString("下注方位:", "Bet Position:");
            this.bt_bet.Text = UIHelper.LocalString("下注", "Bet");

            this.lb_specialchar.Text = UIHelper.LocalString("特码:", "Special Code:");
            this.lb_specialposition.Text = UIHelper.LocalString("特位:", "Special Hit:");
            this.lb_p_0.Text = UIHelper.LocalString("码位 0:", "Seat 0:");
            this.lb_p_1.Text = UIHelper.LocalString("码位 1:", "Seat 1:");
            this.lb_p_2.Text = UIHelper.LocalString("码位 2:", "Seat 2:");
            this.lb_p_3.Text = UIHelper.LocalString("码位 3:", "Seat 3:");
            this.lb_p_4.Text = UIHelper.LocalString("码位 4:", "Seat 4:");
            this.lb_p_5.Text = UIHelper.LocalString("码位 5:", "Seat 5:");
            this.lb_p_6.Text = UIHelper.LocalString("码位 6:", "Seat 6:");
            this.lb_p_7.Text = UIHelper.LocalString("码位 7:", "Seat 7:");
            this.lb_p_8.Text = UIHelper.LocalString("码位 8:", "Seat 8:");
            this.lb_p_9.Text = UIHelper.LocalString("码位 9:", "Seat 9:");
            Random rd = new Random();
            this.cb_specialchar.Items.Clear();
            foreach (var c in GuessKey.SpecialChars)
            {
                this.cb_specialchar.Items.Add(c.ToString());
            }
            this.cb_specialchar.SelectedIndex = rd.Next(0, GuessKey.SpecialChars.Length);
            this.cb_specialposition.Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                this.cb_specialposition.Items.Add(i);
            }
            this.cb_specialposition.SelectedIndex = rd.Next(0, 10);


            for (uint i = 0; i < 10; i++)
            {
                var nonce = rd.Next(0, 10);
                for (uint j = 0; j < 10; j++)
                {
                    string name = $"cb_{i}_{j}";
                    var ctrl = this.panel.Controls.Find(name, false).FirstOrDefault();
                    var cb = ctrl as DarkCheckBox;
                    cb.Tag = GetBetAtom(j);

                    if (nonce == j)
                    {
                        cb.Checked = true;
                    }
                    cb.CheckedChanged += Cb_CheckStateChanged;
                    cb.CheckStateChanged += Cb_CheckStateChanged;
                }
            }
            Cb_CheckStateChanged(null, null);
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

        private void Cb_CheckStateChanged(object sender, EventArgs e)
        {
            var c = this.cb_specialchar.Text;
            if (c.IsNullOrEmpty() || !GuessKey.SpecialChars.Contains(c.ToCharArray()[0]))
            {
                this.BetPack = default;
                return;
            }
            var p = this.cb_specialposition.Text;
            if (p.IsNullOrEmpty() || !byte.TryParse(p, out byte specialPosition) || specialPosition > 9)
            {
                this.BetPack = default;
                return;
            }
            BetPack pack = new BetPack() { SpecialChar = c.ToCharArray()[0], SpecialPosition = specialPosition };
            for (uint i = 0; i < 10; i++)
                for (uint j = 0; j < 10; j++)
                {
                    string name = $"cb_{i}_{j}";
                    var ctrl = this.panel.Controls.Find(name, false).FirstOrDefault();
                    var cb = ctrl as DarkCheckBox;
                    if (cb.Checked)
                    {
                        GuessKey.BetAtom atom = (GuessKey.BetAtom)cb.Tag;
                        pack.BetPostion(i, atom);
                    }
                }
            if (!pack.Valid)
            {
                this.BetPack = default;
                return;
            }
            this.tb_amount.Text = pack.TotalAmount().ToString();
            this.BetPack = pack;
        }

        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        void UpdateMinBetHeight()
        {
            var index = Blockchain.Singleton.HeaderHeight;
            var remainder = index % PeroidBlocks;
            var zindex = index - remainder + PeroidBlocks;
            var fremainder = PeroidBlocks - remainder;
            if (fremainder <= (PeroidBlocks == 10 ? 5 : 17))
            {
                this.cb_Height.SpecialBorderColor = Color.Red;
                zindex += PeroidBlocks;
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
                    this.cb_Height.Items.Add(this.MinBetHeight + PeroidBlocks * i);
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
                this.tb_balance.Text = acts.GetBalance(Room.Request.AssetId).ToString();
            }
        }

        private void bt_NewRoom_Click(object sender, EventArgs e)
        {
            var ali = this.cbAccounts.SelectedItem as TrustAccountDescription;
            if (ali.IsNull()) return;
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
                if (index % PeroidBlocks > 0)
                    return;
                if (this.BetPack.IsNull() || !this.BetPack.Valid)
                    return;
                if (!this.BetPack.TryMerge(out string betpoint))
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
                if (amt != this.BetPack.TotalAmount())
                    return;
                try
                {
                    amt = Fixed8.One * (amt.GetInternalValue() / Fixed8.D);
                    BetRequest request = new BetRequest() { BetPoint = betpoint, From = from.TrustAddress, Index = index, BetAddress = this.Room.BetAddress, Passport = default };
                    var trusteeAddress = Contract.CreateSignatureRedeemScript(from.AssetTrustContract.Trustee).ToScriptHash();
                    var trustee = this.Operater.Wallet.GetAccount(trusteeAddress);
                    if (trustee.IsNotNull() && !trustee.WatchOnly && this.Operater.Wallet is OpenWallet openWallet)
                    {
                        using (ScriptBuilder sb = new ScriptBuilder())
                        {
                            var account = LockAssetHelper.CreateAccount(openWallet, from.AssetTrustContract.GetContract(), trustee.GetKey());//lock asset account have a some private key with master account
                            if (account != null)
                            {
                                List<AssetTrustUTXO> utxos = new List<AssetTrustUTXO>();
                                foreach (var r in openWallet.GetAssetTrustUTXO().Where(m => m.Value.SpentIndex ==0 && m.Value.OutPut.AssetId.Equals(this.Room.Request.AssetId) && m.Value.OutPut.ScriptHash.Equals(from.TrustAddress)))
                                {
                                    utxos.Add(new AssetTrustUTXO
                                    {
                                        AssetTrustOutput = r.Value,
                                        Address = r.Value.OutPut.ScriptHash,
                                        Value = r.Value.OutPut.Value.GetInternalValue(),
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
                                List<AssetTrustUTXO> waitSpents = new List<AssetTrustUTXO>();
                                if (Room.Request.AssetId == Blockchain.OXC)
                                {
                                    if (utxos.SortSearch(amt.GetInternalValue() + Fixed8.D + betFee.GetInternalValue(), excludedUtxoKeys, out AssetTrustUTXO[] selectedUtxos, out long remainder))
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
                                        waitSpents.AddRange(selectedUtxos);
                                        ok = true;
                                    }
                                }
                                else
                                {
                                    if (utxos.SortSearch(amt.GetInternalValue(), excludedUtxoKeys, out AssetTrustUTXO[] selectedUtxos, out long remainder))
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
                                        waitSpents.AddRange(selectedUtxos);
                                        m++;
                                    }
                                    List<AssetTrustUTXO> feeutxos = new List<AssetTrustUTXO>();
                                    foreach (var r in openWallet.GetAssetTrustUTXO().Where(m => m.Value.SpentIndex ==0 && m.Value.OutPut.AssetId.Equals(Blockchain.OXC) && m.Value.OutPut.ScriptHash.Equals(from.TrustAddress)))
                                    {
                                        feeutxos.Add(new AssetTrustUTXO
                                        {
                                            AssetTrustOutput = r.Value,
                                            Address = r.Value.OutPut.ScriptHash,
                                            Value = r.Value.OutPut.Value.GetInternalValue(),
                                            TxId = r.Key.TxId,
                                            N = r.Key.N
                                        });
                                    }
                                    if (feeutxos.SortSearch(Fixed8.D + betFee.GetInternalValue(), excludedUtxoKeys, out AssetTrustUTXO[] selectedUtxosFee, out long remainderFee))
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
                                        waitSpents.AddRange(selectedUtxosFee);
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
                                        foreach (var u in waitSpents)
                                        {
                                            u.AssetTrustOutput.SpentIndex = Blockchain.Singleton.HeaderHeight;
                                        }
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

        }

    }
}
