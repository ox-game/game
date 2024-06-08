using OX.Bapps;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using OX.Wallets.UI.Controls;
using OX.Wallets.UI.Docking;
using System.Collections.Generic;
using System.Linq;

namespace OX.UI.Casino
{
    public partial class CasinoRule : DarkToolWindow, INotecaseTrigger, IModuleComponent
    {
        public Module Module { get; set; }
        private INotecase Operater;
        #region Constructor Region

        public CasinoRule()
        {
            InitializeComponent();
            var bizPlugin = Bapp.GetBappProvider<CasinoBapp, ICasinoProvider>();
            if (bizPlugin != default)
            {
                var settings = bizPlugin.GetAllCasinoSettings();
                //this.Clear();
                AddSetting(settings, CasinoSettingTypes.CasinoState, UIHelper.LocalString("娱乐系统状态", "Casino System State"), "");
                AddSetting(settings, CasinoSettingTypes.PublicRoomFee, UIHelper.LocalString("公众娱乐房注册费", "Public Casino Room Register Fee"), "OXC");
                AddSetting(settings, CasinoSettingTypes.PublicRoomOXSLockVolume, UIHelper.LocalString("公众娱乐房OXS锁仓入伙总量", "Public Casino Room Partner Total Lock OXS Volume"), "OXS");
                AddSetting(settings, CasinoSettingTypes.PrivateRoomFee, UIHelper.LocalString("私有娱乐房注册费", "Private Casino Room Register Fee"), "OXC");
                AddSetting(settings, CasinoSettingTypes.PrivateRoomOXSLockVolume, UIHelper.LocalString("私有娱乐房OXS锁仓入伙总量", "Private Casino Room Partner Total Lock OXS Volume"), "OXS");
                AddSetting(settings, CasinoSettingTypes.PublicRoomPledgePeriod, UIHelper.LocalString("公众房间OXS入伙锁仓周期", "Public Room Partner Lock OXS Period"), "blocks");
                AddSetting(settings, CasinoSettingTypes.PrivateRoomPledgePeriod, UIHelper.LocalString("私有房间OXS入伙锁仓周期", "Private Room Partner Lock OXS Period"), "blocks");
                AddSetting(settings, CasinoSettingTypes.RoomOXSMinLock, UIHelper.LocalString("房间OXS入伙最低单笔锁仓量", "Room Partner Min Single Lock OXS Volume"), "OXS");
                AddSetting(settings, CasinoSettingTypes.UnitFee, UIHelper.LocalString("每10注系统佣金", "System Commission Per 10 Bets"), "");
                AddSetting(settings, CasinoSettingTypes.EatSmallMinBet, UIHelper.LocalString("大吃小最低投注额", "Eat Small Min Bet"), "");
                AddSetting(settings, CasinoSettingTypes.LottoMinBet, UIHelper.LocalString("乐透最低投注额", "Lotto Min Bet"), "");
                AddSetting(settings, CasinoSettingTypes.MarkSixMinBet, UIHelper.LocalString("六合最低投注额", "MarkSix Min Bet"), "");
                AddSetting(settings, CasinoSettingTypes.TeamKillMinBet, UIHelper.LocalString("团杀最低投注额", "TeamKill Min Bet"), "");
                AddSetting(settings, CasinoSettingTypes.BuryMinBet, UIHelper.LocalString("埋雷最低投注额", "Bury Min Bet"), "");
                AddSetting(settings, CasinoSettingTypes.RoomFeeMinBet, UIHelper.LocalString("计提房费最低投注门槛", "Min Bet of Accrual Room Fee"), "");
                AddSetting(settings, CasinoSettingTypes.PoolBonusMinBet, UIHelper.LocalString("计提奖池最低投注门槛", "Min Bet of Accrual Pool Bonus"), "");

                AddSetting(settings, CasinoSettingTypes.CasinoBonusToken, UIHelper.LocalString("投注奖励资产Id", "Bet Bonus Asset Id"), "");
            }
        }
        void AddSetting(IEnumerable<KeyValuePair<byte[], CasinoSettingRecord>> settings, byte settingKey, string name, string suffix)
        {
            var setting = settings.FirstOrDefault(m => Enumerable.SequenceEqual(m.Key, new[] { settingKey }));
            if (setting.Equals(new KeyValuePair<byte[], CasinoSettingRecord>()))
                return;
            string text = $"{name}  :  {setting.Value.Value}  {suffix}";
            DarkListItem node = new DarkListItem(text);
            this.lstConsole.Items.Add(node);
        }
        public void Clear()
        {
            this.lstConsole.Items.Clear();
        }
        #endregion
        #region IBlockChainTrigger
        public void OnBappEvent(BappEvent be) { }

        public void OnCrossBappMessage(CrossBappMessage message)
        {
        }
        public void HeartBeat(HeartBeatContext context)
        {

        }
        public void OnFlashMessage(FlashMessage flashMessage)
        {

        }
        public void OnBlock(Block block)
        {
        }
        public void BeforeOnBlock(Block block) { }
        public void AfterOnBlock(Block block) { }
        public void ChangeWallet(INotecase operater)
        {
            this.Operater = operater;
        }
        public void OnRebuild() { }

        #endregion
    }
}
