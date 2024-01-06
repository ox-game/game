using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX.Wallets;
using OX.Wallets.UI.Forms;
using OX.Bapps;
using OX.IO.Data.LevelDB;
using Akka.Pattern;
using OX.Casino;

namespace OX.UI.Casino.Bury
{
    public partial class MyBuryHitQuery : DarkForm
    {
        public INotecase Notecase;
        public UInt160 BetAddress;
        public MyBuryHitQuery(UInt160 betAddress, INotecase notcase)
        {
            this.Notecase = notcase;
            this.BetAddress = betAddress;
            InitializeComponent();
            this.Text = UIHelper.LocalString($"我的爆雷命中记录", $"My  hit records in bury");
            this.bt_close.Text = UIHelper.LocalString("关闭", "Close");
        }



        private void bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MyBuryQuery_Load(object sender, EventArgs e)
        {
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin != default)
            {
                List<Tuple<BuryMergeTx, UInt160>> list = new List<Tuple<BuryMergeTx, UInt160>>();
                foreach (var act in this.Notecase.Wallet.GetHeldAccounts())
                {
                    list.AddRange(bizPlugin.GetMyHitBuryRecords(this.BetAddress, act.ScriptHash).Select(m => new Tuple<BuryMergeTx, UInt160>(m, act.ScriptHash)));
                }
                foreach (var br in list.OrderByDescending(m => m.Item1.BlockIndex))
                {
                    this.flowLayoutPanel1.Controls.Add(new MyBuryHitRecordButton(br.Item1, br.Item2));
                }
            }
        }
    }
}
