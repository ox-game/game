using OX.IO.Caching;

namespace OX
{
    //不能与其他BizModelType的定义冲突
    public enum CasinoBizModelType : byte
    {
        [ReflectionCache(typeof(CasinoSettingRecord))]
        CasinoSetting = 0x10,
        //[ReflectionCache(typeof(RoomStateRecord_V1))]
        //RoomState_V1 = 0xc1,
        //[ReflectionCache(typeof(RiddlesRecord))]
        //Riddles = 0xc2,
        //[ReflectionCache(typeof(RiddlesHashRecord))]
        //RiddlesHash = 0xc3,
        //[ReflectionCache(typeof(RoomRecord_V1))]
        //Room_V1 = 0xc4,

        [ReflectionCache(typeof(GameMiningTask))]
        GameMiningTask = 0xD1,
        [ReflectionCache(typeof(RoomRecord))]
        Room = 0xD2,
        //[ReflectionCache(typeof(SangongRoomBankerSeatRecord))]
        //SangongRoomBankerSeat = 0xD3,
        [ReflectionCache(typeof(RoomDestroyRecord))]
        RoomDestroy = 0xD4,
    }
}
