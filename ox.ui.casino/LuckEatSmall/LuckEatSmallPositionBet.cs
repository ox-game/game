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
    public partial class LuckEatSmallPositionBet : DarkForm, IBetWatch
    {
        Module Module;
        INotecase Operater;
        MixRoom Room;
        uint PeroidBlocks;
        uint MinBetHeight;
        Fixed8 MinBet;
        uint initIndex;
        byte Position;
        Dictionary<string, SignatureValidator<SeasonPassport>> passports = new Dictionary<string, SignatureValidator<SeasonPassport>>();
        public LuckEatSmallPositionBet(Module module, INotecase operater, MixRoom room, Fixed8 minBet, uint index, byte position)
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
            var fee = Fixed8.One + this.Room.GetPrivateRoomBetFee();
            this.Text = UIHelper.LocalString($"大吃小下注  {this.Position}号位      单注费 {fee} OXC", $"EatSmall Bet in Position {this.Position}       bet fee {fee} OXC");
            this.lb_roomId.Text = UIHelper.LocalString($"房间号:    {this.Room.RoomId}", $"Room Id:    {this.Room.RoomId}");
            this.lb_Index.Text = UIHelper.LocalString($"下注高度:", $"Bet Height:");
            this.lb_balance.Text = UIHelper.LocalString("账户余额:", "Balance:");
            this.lb_betamt.Text = UIHelper.LocalString("下注额:", "Bet Amount:");
            this.lb_from.Text = UIHelper.LocalString("下注账户:", "Bet Account:");
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
            var ali = this.cbAccounts.SelectedItem as AccountListItem;
            this.tb_balance.Text = ali.IsNull() ? "0" : this.Operater.Wallet.GeAccountAvailable(ali.Account.Address.ToScriptHash(), this.Room.Request.AssetId).ToString();

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
                if (index % PeroidBlocks != GameHelper.ReviseIndex(this.Room))
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
                    DarkMessageBox.ShowInformation(UIHelper.LocalString($"最低投注额为{MinBet} ", $"the least bet amount {MinBet} "), "");
                    return;
                }
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
                        var spc = this.cb_specialchar.Text;
                        if (spc.IsNullOrEmpty()) return;
                        var spcc = spc.ToCharArray()[0];
                        if (!GuessKey.SpecialChars.Contains(spcc)) return;
                        BetRequest request = new BetRequest() { BetPoint = $"{this.Position.ToString()}|{spcc}", From = from.Account.ScriptHash, Index = index, BetAddress = this.Room.BetAddress, Passport = default };
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
