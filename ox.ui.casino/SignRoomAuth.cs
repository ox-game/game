using OX.IO;
using OX.Ledger;
using OX.Network.P2P;
using OX.SmartContract;
using OX.Wallets;
using OX.Bapps;
using OX.Wallets.UI.Forms;
using System;
using System.Linq;
using System.Windows.Forms;
using OX.Casino;
using Akka.Actor.Dsl;
using OX.Network.P2P.Payloads;
using System.Collections.Generic;

namespace OX.UI.Casino
{
    public partial class SignRoomAuth : DarkDialog
    {
        INotecase Operater;
        MixRoom Room;
        WalletAccount Holder;
        Module Module;
        public SignRoomAuth(INotecase notecase, Module module, MixRoom roomInfo)
        {
            InitializeComponent();
            this.Operater = notecase;
            this.Room = roomInfo;
            this.Module = module;
        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.Text = UIHelper.LocalString($"管理 {this.Room.RoomId} 房间成员", $"Room {this.Room.RoomId}  members manage");
            this.btnOk.Text = UIHelper.LocalString("设定", "Apply");
            this.btnCancel.Text = UIHelper.LocalString("取消", "Cancel");
            this.lb_memberAddr.Text = UIHelper.LocalString("成员地址:", "Member Address:");
            this.bt_add_addr.Text = UIHelper.LocalString("增加", "Add");
            this.bt_clear.Text = UIHelper.LocalString("清空", "Clear");
            this.lb_betFee.Text = UIHelper.LocalString("单注费:", "Bet Fee:");
            int seletedIndex = 0;
            if (this.Room.RoomMemberSetting.IsNotNull() && this.Room.RoomMemberSetting.Members.IsNotNullAndEmpty())
            {
                seletedIndex = this.Room.RoomMemberSetting.Flag;
                foreach (var sh in this.Room.RoomMemberSetting.Members)
                {
                    this.lv_members.Items.Add(new Wallets.UI.Controls.DarkListItem { Tag = sh, Text = sh.ToAddress() });
                }
            }
            Holder = this.Operater.Wallet.GetAccount(Room.Holder);
            for (int i = 0; i < 100; i++)
            {
                this.cb_betFee.Items.Add(i.ToString());
            }
            this.cb_betFee.SelectedIndex = seletedIndex;

        }

        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }



        private void bt_clear_Click(object sender, EventArgs e)
        {
            this.lv_members.Items.Clear();
        }

        private void bt_add_addr_Click(object sender, EventArgs e)
        {
            var s = this.tb_sh_to_add.Text;
            if (s.IsNotNullAndEmpty())
            {
                try
                {
                    var sh = s.ToScriptHash();
                    bool find = false;
                    foreach (var item in this.lv_members.Items)
                    {
                        var shItem = item.Tag as UInt160;
                        if (shItem.Equals(sh)) find = true;
                    }
                    if (!find)
                    {
                        this.lv_members.Items.Add(new Wallets.UI.Controls.DarkListItem { Tag = sh, Text = sh.ToAddress() });
                        if (this.lv_members.Items.Count >= 100) this.bt_add_addr.Enabled = false;
                    }
                    this.tb_sh_to_add.Clear();
                }
                catch
                {
                    this.tb_sh_to_add.Clear();
                    DarkMessageBox.ShowError(UIHelper.LocalString("地址重复或者错误", "Duplicate or incorrect address"), "");
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.lv_members.Items.IsNotNullAndEmpty() && this.Holder.IsNotNull() && this.Operater.Wallet.ContainsAndHeld(this.Holder.ScriptHash))
            {
                List<UInt160> list = new List<UInt160>();
                foreach (var item in this.lv_members.Items)
                {
                    list.Add(item.Tag as UInt160);
                }
                byte betFee = byte.Parse(this.cb_betFee.SelectedItem as string);
                PrivateRoomMemberSettingRequest request = new PrivateRoomMemberSettingRequest
                {
                    BetAddress = this.Room.BetAddress,
                    RoomMemberSetting = new RoomMemberSetting
                    {
                        Flag = betFee,
                        Members = list.ToArray()
                    }
                };

                SingleAskTransactionWrapper stw = new SingleAskTransactionWrapper(this.Holder);
                var pubkey = Bapp.GetBapp<CasinoBapp>().ValidBizScriptHashs.FirstOrDefault();
                var sh = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash();
                var tx = this.Operater.Wallet.MakeSingleAskTransaction(stw, sh, (byte)CasinoType.PrivateRoomMemberSetting, request);

                if (tx.IsNotNull())
                {
                    this.Operater.SignAndSendTx(tx);
                    if (this.Operater != default)
                    {
                        string msg = $"{UIHelper.LocalString("广播设定房间成员交易", "Relay set room members transaction")}   {tx.Hash}";
                        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });

                        DarkMessageBox.ShowInformation(msg, "");
                    }
                    this.Close();
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
