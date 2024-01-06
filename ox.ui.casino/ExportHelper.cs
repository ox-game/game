using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OX.Bapps;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX.IO.Data.LevelDB;
using OX.Wallets;
using OX.Wallets.Models;
using OX.Wallets.NEP6;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using Akka.Util;

namespace OX.UI.Casino
{
    public static class ExportHelper
    {
        public static List<Riddles> ExportRiddles(uint[] Ranges = default)
        {
            var provider = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (provider.IsNotNull())
            {
                if (Ranges == default)
                {
                    var rem = Blockchain.Singleton.HeaderHeight % 1000;
                    var indexrange = Blockchain.Singleton.HeaderHeight - rem;
                    List<Riddles> riddles = new List<Riddles>();
                    while (true)
                    {
                        foreach (var r in provider.GetRangeRiddles(indexrange))
                        {
                            riddles.Add(r.Value);
                        }
                        indexrange -= 1000;
                        if (indexrange < 0) break;
                    }
                    return riddles;
                }
                else
                {
                    List<Riddles> riddles = new List<Riddles>();
                    foreach (var rindex in Ranges)
                    {
                        foreach (var r in provider.GetRangeRiddles(rindex))
                        {
                            riddles.Add(r.Value);
                        }
                    }
                    return riddles;
                }

            }
            return default;
        }
        public static string BuildString(this IEnumerable<Riddles> riddles)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(UIHelper.LocalString($"谜底列表  {riddles.Count()}条记录:", $"Riddles list {riddles.Count()} records:"));
            sb.AppendLine("-------------------------------------------------------------");
            foreach (var r in riddles)
            {
                sb.AppendLine(UIHelper.LocalString($"区块高度:{r.Index}", $"Blockchain Height{r.Index}"));
                foreach (var g in r.GuessKeys)
                {
                    var riddlesName = UIHelper.LocalString(g.RiddlesKind.StringValue(), g.RiddlesKind.EngStringValue());
                    sb.AppendLine($"{riddlesName} : {g.SpecialPosition}/{g.SpecialChar}/{g.ReRandomSanGongOrLottoString(BlockHelper.GetMineNonce(r.Index), r.Index)}");
                }
                sb.AppendLine("-------------------------------------------------------------");
            }
            return sb.ToString();
        }
        public static void CopyFile(string filename, string text)
        {
            var path = @"c:\ox_export";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var filepath = $"{path}\\{filename}";
            //if (!File.Exists(filepath))
            StreamWriter streamWriter = new StreamWriter(filepath);
            streamWriter.Write(text);
            streamWriter.Close();
            System.Collections.Specialized.StringCollection files = new System.Collections.Specialized.StringCollection();
            files.Add(filepath);
            Clipboard.SetFileDropList(files);
        }
    }
}
