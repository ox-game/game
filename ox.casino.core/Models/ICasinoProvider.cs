using OX.Bapps;
using OX.Cryptography.ECC;
using OX.Network.P2P.Payloads;
using OX.Wallets;
using System.Collections.Generic;
using OX.IO.Data.LevelDB;
using OX.Casino;

namespace OX
{
    public interface ICasinoProvider : IBappProvider
    {
        DB Db { get; }
        public Dictionary<UInt160, MixRoom> MixRooms { get; }
        MixRoom[] AllRooms { get; }
        uint BuryNumber { get; }
        //RoomKey[] AllRoomKeys { get; }
        T GetGameProvider<T>() where T : class, IGameProvider;
        IEnumerable<KeyValuePair<byte[], CasinoSettingRecord>> GetAllCasinoSettings();
        IEnumerable<KeyValuePair<byte[], RoomDestroyRecord>> GetAllRoomDestroies();
        RoomDestroyRecord GetRoomDestory(uint roomId);
        MixRoom[] GetWalletRooms();
        IEnumerable<KeyValuePair<UInt160, MixRoom>> GetRooms();
        MixRoom GetRoom(uint roomId);
        MixRoom GetRoom(UInt160 betAddress);
        //IEnumerable<KeyValuePair<uint, RoomStateRequest>> GetRoomStates();
        //IEnumerable<KeyValuePair<uint, uint>> GetRoomSplits();
        //RoomStateRequest GetRoomState(uint roomId);
        //uint GetRoomSplit(uint roomId);
        Riddles GetRiddles(uint index);
        RiddlesHash GetRiddlesHash(uint index);
        IEnumerable<KeyValuePair<UInt160, uint>> GetAllRoomIds();
        IEnumerable<KeyValuePair<BetKey, Betting>> GetBettings(UInt160 betAddress, uint? index = null);
        Betting GetBetting(BetKey key);
        IEnumerable<KeyValuePair<RoundClearKey, RoundClearResult>> GetRoundClearResults(UInt160 betAddress, uint? index = null, uint? sno = null);
        IEnumerable<KeyValuePair<IndexRangeKey, RiddlesHash>> GetRangeRiddlesHash(uint indexrange);
        IEnumerable<KeyValuePair<IndexRangeKey, Riddles>> GetRangeRiddles(uint indexrange);
        bool GetWatchBalance(UInt160 scriptHash, out Fixed8 amount);
        IEnumerable<RoomAdminKey> GetRoomAdminsInWallet();
        bool ExistRoomAdmin(RoomAdminKey rak);
        //IEnumerable<KeyValuePair<UInt160, ValetRegisterKey>> GetAllValetRegisters();
        //IEnumerable<KeyValuePair<ValetUtxoKey, ValetOutputState>> GetValetUtxos(UInt160 valet);
        //IEnumerable<ValetUtxoMerge> RefreshValetUtxoState(UInt160 valet, uint BlockChainIndex);
        //IEnumerable<KeyValuePair<ValetOXSClaimKey, ValetOXSClaimRecord>> GetValetOXSClaim(UInt160 valet);
        //IEnumerable<KeyValuePair<UInt160, RoomPledgeAccountReply>> GetAllRoomPledges();
        uint GetBuryNumber(UInt160 betAddress);
        BuryRecord GetBury(UInt160 betAddress, uint number);
        BuryMergeTx GetRoomReplyBury(UInt256 txid);
        uint GetRoomCodeCount(BuryCodeKey codekey);
        IEnumerable<KeyValuePair<BuryCodeKey, uint>> GetRoomCodeCount(UInt160 betAddress);
        IEnumerable<BuryRecord> GetMyBuryRecords(UInt160 betAddress, UInt160 player);
        IEnumerable<BuryMergeTx> GetMyHitBuryRecords(UInt160 betAddress, UInt160 player);
        IEnumerable<BuryMergeTx> GetEthMapHitBuryRecords(UInt160 betAddress, UInt160 player);
        IEnumerable<KeyValuePair<RoomPartnerLockRecord, LockAssetTransaction>> GetRoomPartnerLockRecords(UInt160 betAddress);
    }
    public interface IGameProvider
    {
        ICasinoProvider ParentProvider { get; set; }
        void OnBlock(Block block, WriteBatch batch);
        void OnRebuild(Wallet wallet = null);
        void OnBappEvent(BappEvent bappEvent);
        void OnCrossBappMessage(CrossBappMessage message);
    }
}
