using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using OX.Wallets.UI.Forms;
using OX.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OX.SmartContract;
using OX.Casino;

namespace OX.UI.Casino
{
    public partial class MorePrize : DarkDialog
    {
        public MorePrize(Module module, Wallet wallet, MixRoom room, uint index, string dibsaccount)
        {
            InitializeComponent();
            this.Text = UIHelper.LocalString($"{room.RoomId}房间{index}局", $"RoomId:{room.RoomId},Round:{index}");
            this.btnOk.Text = UIHelper.LocalString("关闭", "Close");
            var plugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (plugin.IsNotNull())
            {
                var results = plugin.GetRoundClearResults(room.BetAddress, index);
                if (results != default)
                {
                    foreach (var result in results)
                    {
                        var tx = Blockchain.Singleton.GetTransaction(result.Value.TxHash);
                        if (tx is ReplyTransaction rt)
                        {
                            var bizshs = module.Bapp.ValidBizScriptHashs.Select(m => Contract.CreateSignatureRedeemScript(m).ToScriptHash()).ToArray();

                            if (rt.GetDataModel<RoundClear>(bizshs, (byte)CasinoType.RoundClear, out RoundClear roundClear))
                            {
                                if (roundClear.BetAddress == room.BetAddress && roundClear.Index == index)
                                {
                                    foreach (var output in tx.Outputs)
                                    {
                                        if (output.AssetId == Blockchain.OXC)
                                        {
                                            var pb = new PrizeButton(output.ScriptHash.ToAddress(), output.Value, wallet, room, dibsaccount);
                                            this.container.Controls.Add(pb);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
