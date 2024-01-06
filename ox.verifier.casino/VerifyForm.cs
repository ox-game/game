using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OX;
using OX.IO;

namespace ox.verifier.casino
{
    public partial class VerifyForm : Form
    {
        public VerifyForm()
        {
            InitializeComponent();
        }
        public static bool IsChina()
        {
            return System.Globalization.CultureInfo.InstalledUICulture.Name.ToLower() == "zh-cn";
        }
        public static string LocalString(string ChinaString, string EnglishString)
        {
            return IsChina() ? ChinaString : EnglishString;
        }

        private void VerifyForm_Load(object sender, EventArgs e)
        {
            this.Text = LocalString("谜底验证", "Verify Riddles");
            this.button1.Text = LocalString("验证", "Verify");
            this.button2.Text = LocalString("关闭", "Clos色");
            this.label1.Text = LocalString("谜底数据", "Riddles Data");
            this.label2.Text = LocalString("区块随机数", "Block Nonce");
            this.label3.Text = LocalString("谜底哈希", "Riddles Hash");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.richTextBox2.Clear();
            var proof = this.richTextBox1.Text;
            if (!string.IsNullOrEmpty(proof) && ulong.TryParse(this.textBox1.Text, out ulong nonce))
            {
                try
                {
                    var riddles = proof.HexToBytes().AsSerializable<Riddles>();
                    if (riddles.Hash.ToString() == this.textBox2.Text.Trim())
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var g in riddles.GuessKeys)
                        {
                            var riddlesName = LocalString(g.RiddlesKind.StringValue(), g.RiddlesKind.EngStringValue());
                            sb.AppendLine($"{riddlesName} : {g.SpecialPosition}/{g.SpecialChar}/{g.ReRandomSanGongOrLottoInnerString(riddles.Index, nonce)} \n");
                        }
                        this.richTextBox2.Text = sb.ToString();
                        MessageBox.Show(LocalString("验证成功", "Validation  Success"));
                    }
                    else
                    {
                        MessageBox.Show(LocalString("验证失败", "Validation  Failed"));
                    }
                }
                catch (Exception) { MessageBox.Show(LocalString("验证失败", "Validation  Failed")); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
