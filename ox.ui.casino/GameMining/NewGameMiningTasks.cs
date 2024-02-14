using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.Wallets;
using OX.Wallets.Models;
using OX.Wallets.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OX.UI.GameMining
{
    public partial class NewGameMiningTasks : DarkDialog
    {
        public class GameMiningTaskInfo
        {
            public GameMiningTaskKey Key;
            public GameMiningTask Value;
            public override string ToString()
            {
                return $"{UIHelper.LocalString(Key.Kind.StringValue(), Key.Kind.EngStringValue())}/{Key.BetIndex}/{Value.BetLockExpiration}/{Value.AirDropAmount.ToString()}/{Value.AirdropLockRange}";
            }
        }
        INotecase Operater;
        public NewGameMiningTasks(INotecase notecase)
        {
            this.Operater = notecase;
            InitializeComponent();
        }


        private void ClaimForm_Load(object sender, EventArgs e)
        {
            this.Text = UIHelper.LocalString("创建竞争挖矿任务", "New Game Mining Task");
            this.btnOk.Text = UIHelper.LocalString("确定", "OK");
            this.bt_add.Text = UIHelper.LocalString("增加", "Add");
            this.lb_gmKind.Text = UIHelper.LocalString("挖矿类型:", "Mine Kind:");
            this.rb_eatsmall.Text = UIHelper.LocalString("原位", "Fixed");
            this.rb_looting.Text = UIHelper.LocalString("本位", "Floating");
            this.lb_betindex.Text = UIHelper.LocalString("下注高度:", "Bet Height:");
            this.lb_betlockexpration.Text = UIHelper.LocalString("下注锁仓到期:", "Bet Lock Expire:");
            this.lb_bonusLockRange.Text = UIHelper.LocalString("空投锁期:", "Airdrop Lock:");
            this.lb_bonusAmount.Text = UIHelper.LocalString("空投额:", "Airdrop Amount:");
        }

        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var items = this.darkListView1.Items;
            if (items.IsNotNullAndEmpty())
            {
                var sh = casino.BizAddresses.First().Key;
                List<Record> records = new List<Record>();
                foreach (var item in items)
                {
                    GameMiningTaskInfo info = item.Tag as GameMiningTaskInfo;
                    Record r = RecordHelper<CasinoBizModelType>.BuildRecord(info.Value, sh, info.Key.ToArray());
                    records.Add(r);
                }
                Random rand = new Random();
                var tx = new BillTransaction()
                {
                    Nonce = (uint)rand.Next(),
                    Records = records.ToArray(),
                    BizScriptHash = sh,
                    BizNo = 0
                };
                tx = this.Operater.Wallet.MakeTransaction(tx, sh, sh);
                if (tx.IsNotNull())
                {
                    this.Operater.SignAndSendTx(tx);
                    if (this.Operater != default)
                    {
                        string msg = $"{UIHelper.LocalString("广播发行竞争挖矿任务资产交易", "Relay issue game mining task transaction")}   {tx.Hash}";
                        DarkMessageBox.ShowInformation(msg, "");
                    }
                    this.Close();
                }
            }
        }

        private void bt_add_Click(object sender, EventArgs e)
        {
            if (!uint.TryParse(this.tb_betindex.Text, out uint betindex) || betindex < Blockchain.Singleton.HeaderHeight) return;
            if (!uint.TryParse(this.tb_betlockexpration.Text, out uint betLockExpire) || betLockExpire < betindex + 1000) return;
            if (!uint.TryParse(this.tb_bonusLockRange.Text, out uint bonusLockRange)) return;
            if (!Fixed8.TryParse(this.tb_bonusAmount.Text, out Fixed8 amount)) return;
            GameMiningTaskKey key = new GameMiningTaskKey { Kind = this.rb_eatsmall.Checked ? GameMiningKind.Fixed : GameMiningKind.Floating, BetIndex = betindex };
            GameMiningTask value = new GameMiningTask { AssetId = casino.GamblerLuckBonusAsset, AirDropAmount = amount, AirdropLockRange = bonusLockRange, BetLockExpiration = betLockExpire };
            GameMiningTaskInfo info = new GameMiningTaskInfo { Value = value, Key = key };
            this.darkListView1.Items.Add(new Wallets.UI.Controls.DarkListItem { Text = info.ToString(), Tag = info });

        }
    }
}
