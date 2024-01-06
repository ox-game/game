using OX.Bapps;
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
using OX.Cryptography.AES;
using OX.Cryptography;
using OX.Casino;

namespace OX.UI.Casino
{
    public partial class TeamKillBet : DarkForm, IBetWatch
    {
        public string[] Keys = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        Module Module;
        INotecase Operater;
        MixRoom Room;
        uint PeroidBlocks;
        uint MinBetHeight;
        Fixed8 MinBet;
        uint initIndex;
        Dictionary<string, SignatureValidator<SeasonPassport>> passports = new Dictionary<string, SignatureValidator<SeasonPassport>>();
        public TeamKillBet(Module module, INotecase operater, MixRoom room, Fixed8 minBet, uint index)
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
            var fee = Fixed8.One + this.Room.GetPrivateRoomBetFee();
            this.Text = UIHelper.LocalString($"团杀下注     单注费 {fee} OXC", $"TeamKill Bet      bet fee {fee} OXC");
            this.lb_roomId.Text = UIHelper.LocalString($"房间号:    {this.Room.RoomId}", $"Room Id:    {this.Room.RoomId}");
            this.lb_Index.Text = UIHelper.LocalString($"下注高度:", $"Bet Height:");
            this.lb_balance.Text = UIHelper.LocalString("账户余额:", "Balance:");
            this.lb_betamt.Text = UIHelper.LocalString("下注额:", "Bet Amount:");
            this.lb_from.Text = UIHelper.LocalString("下注账户:", "Bet Account:");
            this.bt_bet.Text = UIHelper.LocalString("跟团下注", "Follow Team Bet");
            this.bt_betself.Text = UIHelper.LocalString("开团下注", "Create Team Bet");
            this.bt_verify.Text = UIHelper.LocalString("验证授权", "Verify Authorize");
            this.lb_markproof.Text = UIHelper.LocalString("通用授权:", "Common Authorize:");

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
            bt_betself.Enabled = false;
        }
        void EnableBet()
        {
            bt_bet.Enabled = true;
            bt_betself.Enabled = true;
        }
        private void darkComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ali = this.cbAccounts.SelectedItem as AccountListItem;
            this.tb_balance.Text = ali.IsNull() ? "0" : this.Operater.Wallet.GeAccountAvailable(ali.Account.Address.ToScriptHash(), this.Room.Request.AssetId).ToString();
        }
        private void bt_NewRoom_Click(object sender, EventArgs e)
        {
            Bet(false);
        }
        private void Bet(bool self)
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
                if (!uint.TryParse(this.tb_amount.Text, out uint amtNum)) return;
                Fixed8 amt = Fixed8.One * amtNum;
                var from = this.cbAccounts.SelectedItem as AccountListItem;
                var address = from?.Account.Address;
                if (address.IsNullOrEmpty()) return;
                if (!Fixed8.TryParse(this.tb_balance.Text, out Fixed8 balanc)) return;
                if (amt > balanc) return;
                if (amt < MinBet)
                {
                    DarkMessageBox.ShowInformation(UIHelper.LocalString($"最低投注额为{MinBet}", $"the least bet amount {MinBet}"), "");
                    return;
                }
                SignatureValidator<CommonAuthorizeMark> validator = default;
                if (self)
                {
                    var fromkey = from.Account.GetKey();
                    CommonAuthorizeMark mark = new CommonAuthorizeMark()
                    {
                        Gambler = from.Account.ScriptHash,
                        PublicKey = fromkey.PublicKey
                    };
                    validator = new SignatureValidator<CommonAuthorizeMark>() { Target = mark, Signature = mark.Sign(fromkey) };

                }
                else
                {
                    var proofStr = this.tb_markproof.Text;
                    if (proofStr.IsNullOrEmpty()) return;
                    try
                    {
                        var bs = proofStr.HexToBytes();
                        validator = bs.AsSerializable<SignatureValidator<CommonAuthorizeMark>>();
                        if (!validator.Verify() || !validator.Target.Gambler.Equals(from.Account.ScriptHash))
                        {
                            string msg = UIHelper.LocalString($"跟团证无效", $"Team Passport Invalid");
                            Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                            DarkMessageBox.ShowInformation(msg, "");
                            return;
                        }
                    }
                    catch
                    {
                        string msg = UIHelper.LocalString($"跟团证数据错误", $"Team Passport Data Error");
                        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                        DarkMessageBox.ShowInformation(msg, "");
                        return;
                    }

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
                        var pubkey = Bapp.GetBapp<CasinoBapp>().ValidBizScriptHashs.FirstOrDefault();
                        var sh = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash();
                        BetRequest request = new BetRequest() { BetPoint = "a", From = from.Account.ScriptHash, Index = index, BetAddress = this.Room.BetAddress, Passport = default, Mark = validator };
                        SingleAskTransactionWrapper stw = new SingleAskTransactionWrapper(from.Account, outputs.ToArray());
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



        private void darkButton1_Click_1(object sender, EventArgs e)
        {
            Bet(true);
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {

        }

        private void darkButton1_Click_2(object sender, EventArgs e)
        {
            var proofStr = this.tb_markproof.Text;
            if (proofStr.IsNullOrEmpty()) return;
            try
            {
                var bs = proofStr.HexToBytes();
                var validator = bs.AsSerializable<SignatureValidator<CommonAuthorizeMark>>();
                if (validator.Verify())
                {
                    var sh = Contract.CreateSignatureRedeemScript(validator.Target.PublicKey).ToScriptHash();
                    string msg = UIHelper.LocalString($"{sh.ToAddress()}授权{validator.Target.Gambler.ToAddress()}", $"{sh.ToAddress()}   authorize   {validator.Target.Gambler.ToAddress()}");
                    Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                    DarkMessageBox.ShowInformation(msg, "");
                    return;
                }
            }
            catch
            {
                string msg = UIHelper.LocalString($"跟团证数据错误", $"Team Passport Data Error");
                Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                DarkMessageBox.ShowInformation(msg, "");
                return;
            }
        }
    }
}
