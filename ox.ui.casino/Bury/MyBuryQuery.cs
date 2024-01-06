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
    public partial class MyBuryQuery : DarkForm
    {
        public INotecase Notecase;
        public UInt160 BetAddress;
        public MyBuryQuery(UInt160 betAddress, INotecase notcase)
        {
            this.BetAddress = betAddress;
            this.Notecase = notcase;
            InitializeComponent();
            this.Text = UIHelper.LocalString($"我的埋雷记录", $"My bury records");
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
                List<BuryRecord> list = new List<BuryRecord>();
                foreach (var act in this.Notecase.Wallet.GetHeldAccounts())
                {
                    list.AddRange(bizPlugin.GetMyBuryRecords(this.BetAddress, act.ScriptHash));
                }
                foreach (var br in list.OrderByDescending(m => m.BlockIndex))
                {
                    this.flowLayoutPanel1.Controls.Add(new MyBuryRecordButton(br));
                }
            }
        }
    }
}
