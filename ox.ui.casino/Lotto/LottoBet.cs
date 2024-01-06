using OX.Bapps;
using OX.Casino;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
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
    public partial class LottoBet : DarkForm, IBetWatch
    {
        Module Module;
        INotecase Operater;
        MixRoom Room;
        uint PeroidBlocks;
        uint MinBetHeight;
        Fixed8 MinBet;
        uint initIndex;
        BetPack BetPack = default;
        public LottoBet(Module module, INotecase operater, MixRoom room, Fixed8 minBet, uint index)
        {
            this.Module = module;
            this.Operater = operater;
            this.Room = room;
            this.MinBet = minBet;
            this.initIndex = index;
            InitializeComponent();

            PeroidBlocks = Game.PeroidBlocks(room.Request);
            UpdateMinBetHeight();
            foreach (var act in operater.Wallet.GetHeldAccounts())
            {
                if (room.Request.Permission == RoomPermission.Public || (room.Request.Permission == RoomPermission.Private
                    && room.RoomMemberSetting.IsNotNull()
                    && room.RoomMemberSetting.Members.IsNotNullAndEmpty()
                    && room.RoomMemberSetting.Members.Contains(act.ScriptHash)))
                    this.cbAccounts.Items.Add(new AccountListItem(act));
            }
        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
           var fee=Fixed8.One+ this.Room.GetPrivateRoomBetFee();
            this.Text = UIHelper.LocalString($"乐透下注   单注费 {fee} OXC", $"Lotto Bet    bet fee {fee} OXC");
            this.lb_roomId.Text = UIHelper.LocalString($"房间号:    {this.Room.RoomId}", $"Room Id:    {this.Room.RoomId}");
            this.lb_Index.Text = UIHelper.LocalString($"下注高度:", $"Bet Height:");
            this.lb_balance.Text = UIHelper.LocalString("账户余额:", "Balance:");
            this.lb_betamt.Text = UIHelper.LocalString("下注额:", "Bet Amount:");
            this.lb_from.Text = UIHelper.LocalString("下注账户:", "Bet Account:");
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
            var ali = this.cbAccounts.SelectedItem as AccountListItem;
            this.tb_balance.Text = ali.IsNull() ? "0" : this.Operater.Wallet.GeAccountAvailable(ali.Account.Address.ToScriptHash(), Room.Request.AssetId).ToString();

        }

        private void bt_NewRoom_Click(object sender, EventArgs e)
        {
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
                var from = this.cbAccounts.SelectedItem as AccountListItem;
                var address = from?.Account.Address;
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
                    if (Room.ValidPrivateRoom(from.Account.ScriptHash))
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
                        BetRequest request = new BetRequest() { BetPoint = betpoint, From = from.Account.ScriptHash, Index = index, BetAddress = this.Room.BetAddress, Passport = default };
                        SingleAskTransactionWrapper stw = new SingleAskTransactionWrapper(from.Account, outputs.ToArray());
                        var pubkey = Bapp.GetBapp<CasinoBapp>().ValidBizScriptHashs.FirstOrDefault();
                        var sh = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash();
                        var tx = this.Operater.Wallet.MakeSingleAskTransaction(stw, sh, (byte)CasinoType.Bet, request, index);
                        if (tx.IsNotNull() && index > Blockchain.Singleton.HeaderHeight)
                        {
                            this.Operater.SignAndSendTx(tx);
                            if (this.Operater != default)
                            {
                                string msg = $"{UIHelper.LocalString($"{Room.RoomId}房间下注区块高度 {index} 交易已广播", $"Room:{Room.RoomId} bet height {index} relay betting transaction")}   {tx.Hash}";
                                Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                                //DarkMessageBox.ShowInformation(msg, "");
                                this.Close();
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
