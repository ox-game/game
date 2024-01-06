using OX.Bapps;
using OX.Ledger;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Forms;
using OX.SmartContract;
using OX.Wallets.UI.Config;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using OX.UI.Casino;
using OX.Cryptography.ECC;
using OX.IO;

namespace OX.UI.GameMining
{
    public class SangongGM : GM
    {
        protected GMViewHead ViewHead;
        protected GMSangongTable ViewBody;
        public SangongGM(INotecase notecase, GameMiningKind gameMiningKind) : base(notecase, gameMiningKind)
        {

        }
        protected override void InitUI()
        {
            this.SuspendLayout();
            this.ViewHead = new GMViewHead(this, OnViewHeadPaint);
            this.panel1.Controls.Add(this.ViewHead);
            this.ViewHead.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ViewHead.Top = 0;
            this.ViewHead.Left = 0;
            this.ViewHead.Width = ClientSize.Width;
            this.ViewHead.Height = 250;

            List<GMSangongData> l = new List<GMSangongData>();
            for (byte i = 0; i < 10; i++)
            {
                l.Add(new GMSangongData() { Position = i });
            }
            this.ViewBody = new GMSangongTable(this, l.ToArray(), OnViewBodyPaint, bet, moreBet);
            this.panel1.Controls.Add(this.ViewBody);
            this.ViewBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.ViewBody.Top = 250;
            this.ViewBody.Left = 0;
            this.ViewBody.Width = ClientSize.Width;
            this.ViewBody.Height = ClientSize.Height - 250;
            this.ResumeLayout();
        }





        protected override void RebuildUI()
        {
            if (this.CurrentTaskKey.IsNotNull())
            {
                this.ViewHead.betIndexLab.Text = this.CurrentTaskKey.BetIndex.ToString();
            }
            this.ViewHead.Invalidate();
            for (byte i = 0; i < 10; i++)
            {

                if (this.ViewBody.Cells.TryGetValue(i, out GMSangongCell cell))
                {
                    List<GMBetting> bettings = new List<GMBetting>();
                    if (this.Seeds != default)
                    {
                        var bs = this.Seeds.Where(m =>
                        {
                            var cs = m.Key.Seed.Position.ToString();
                            return cs == i.ToString();
                        });
                        if (bs.IsNotNullAndEmpty())
                        {
                            foreach (var b in bs)
                            {
                                bettings.Add(new GMBetting { Position = b.Key.Seed.Position, Amount = b.Value.Amount, SH = b.Value.Player });
                            }
                        }
                    }
                    cell.Bettings = bettings.ToArray();
                    cell.FillBettingPanel();
                }
            }
            this.ViewHead.Panel.Controls.Clear();
            if (this.Airdrops.IsNotNullAndEmpty())
            {
                foreach (var airdrop in this.Airdrops)
                {
                    var pb = new AirdropButton(airdrop.Value.Winner.ToAddress(), airdrop.Value.Amount, this.Operater.Wallet);
                    this.ViewHead.Panel.Controls.Add(pb);
                }
            }
        }
        public void OnViewBodyPaint(GMSangongCell cell, PaintEventArgs e)
        {
            //写方位投注合计及开奖结果
            var g = e.Graphics;
            var total = cell.Bettings.Sum(m => m.Amount);
            using (var b = new SolidBrush(Colors.LightText))
            {
                using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                {
                    g.DrawString(total.ToString(), bigFont, b, new PointF(100, 5));
                }
            }
            if (this.Riddles.IsNotNull() && this.MineNonce > 0)
            {
                var guessKey = this.Riddles.GetGuessKey(GameKind.EatSmall);
                if (guessKey.IsNotNull())
                {
                    char[] keys = guessKey.ReRandomSanGongOrLotto(this.MineNonce, this.CurrentTaskKey.BetIndex);
                    var c = keys[cell.Data.Position].ToString();
                    using (var b = new SolidBrush(Color.Yellow))
                    {
                        using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                        {
                            g.DrawString(c, bigFont, b, new PointF(33, 5));
                        }
                    }
                }
            }
        }
        public void OnViewHeadPaint(PaintEventArgs e)
        {
            if (this.CurrentTask.IsNotNull())
            {
                var g = e.Graphics;
                using (var b = new SolidBrush(Color.Yellow))
                {
                    using (Font bigFont = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular))
                    {
                        g.DrawString(UIHelper.LocalString(this.GameMiningKind.StringValue(), this.GameMiningKind.EngStringValue()), bigFont, b, new PointF(5, 15));
                        g.DrawString(UIHelper.LocalString($"种子锁定:{this.CurrentTask.BetLockExpiration}", $"Seed Lock:{this.CurrentTask.BetLockExpiration}"), bigFont, b, new PointF(5, 65));
                        g.DrawString(UIHelper.LocalString($"空投额:{this.CurrentTask.AirDropAmount}", $"Airdrop Amount:{this.CurrentTask.AirDropAmount}"), bigFont, b, new PointF(5, 115));
                        var range = this.CurrentTask.AirdropLockRange;
                        if (range == 0)
                            range = 1000;
                        g.DrawString(UIHelper.LocalString($"空投锁定区块数:{range}", $"Airdrop Lock Blocks:{range}"), bigFont, b, new PointF(5, 165));
                    }
                }
            }
        }
        void bet(byte position, bool isTrust)
        {
            if (this.CurrentTaskKey.IsNull()) return;
            if (this.CurrentTaskKey.BetIndex <= Blockchain.Singleton.HeaderHeight || this.CurrentTaskKey.BetIndex - Blockchain.Singleton.HeaderHeight >= GameMiningTask.MAXBETRANGE) return;

            var assetId = this.GameMiningKind == GameMiningKind.Fixed ? Blockchain.OXC : casino.GamblerLuckBonusAsset;

            using (var dialog = new SeedSelfForm(this.Operater, this.CurrentTaskKey.BetIndex, position, assetId, GameMiningTask.MINSEED, this.CurrentTask.BetLockExpiration))
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                var output = dialog.GetOutput(out ECPoint ecp, out uint expiration);
                LockAssetTransaction lat = new LockAssetTransaction
                {
                    LockContract = Blockchain.LockAssetContractScriptHash,
                    IsTimeLock = false,
                    LockExpiration = expiration,
                    Recipient = ecp
                };
                GameMiningSeed seed = new GameMiningSeed { Kind = this.GameMiningKind, BetIndex = this.CurrentTaskKey.BetIndex, Position = position };
                TransactionAttribute attr = new TransactionAttribute { Usage = TransactionAttributeUsage.Tip5, Data = seed.ToArray() };
                var from = Contract.CreateSignatureRedeemScript(ecp).ToScriptHash();
                output.ScriptHash = lat.GetContract().ScriptHash;
                lat.Outputs = new TransactionOutput[] { output };
                lat.Attributes = new TransactionAttribute[] { attr };
                lat = this.Operater.Wallet.MakeTransaction(lat, from, from);
                if (lat != null)
                {
                    if (lat.Inputs.Count() > 20)
                    {
                        string msg = $"{UIHelper.LocalString("交易输入项太多,请分为多次转账", "There are too many transaction input. Please transfer multiple times")}";
                        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                        DarkMessageBox.ShowInformation(msg, "");
                        return;
                    }
                    this.Operater.SignAndSendTx(lat);
                    if (this.Operater != default)
                    {
                        string msg = $"{UIHelper.LocalString("交易已广播", "Relay transaction completed")}   {lat.Hash}";
                        Bapp.PushCrossBappMessage(new CrossBappMessage() { Content = msg, From = this.Module.Bapp });
                        DarkMessageBox.ShowInformation(msg, "");
                    }
                }
            }

        }
        void moreBet(byte position)
        {
            if (this.CurrentTaskKey.IsNotNull())
                new GMSangongMoreBettings(this, position, this.CurrentTaskKey.BetIndex).ShowDialog();
        }
    }
}
