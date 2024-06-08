using OX.IO;
using OX.IO.Data.LevelDB;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using OX;
using OX.VM;
using OX.Ledger;
using System.Linq;
using System;
using System.Runtime.CompilerServices;

namespace OX.Casino
{
    public static partial class CasinoPersistenceHelper
    {
        public static void Save_MixRoom(this WriteBatch batch, CasinoProvider provider, MixRoom room)
        {
            batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Room).Add(room.BetAddress), SliceBuilder.Begin().Add(room));
            provider.MixRooms[room.BetAddress] = room;

        }
        public static void Save_LastRoomId(this WriteBatch batch, LastRoomId lastRoomId)
        {
            batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Last_RoomId).Add(casino.CasinoMasterAccountAddress), SliceBuilder.Begin().Add(lastRoomId));
        }
        public static void Save_RoomPartnerLockRecord(this WriteBatch batch, CasinoProvider miningProvider, Block block, ushort txIndex, MixRoom Room, LockAssetTransaction lat, TransactionOutput output, UInt160 recipient)
        {
            if (lat.IsNotNull())
            {
                RoomPartnerLockRecord key = new RoomPartnerLockRecord { BetAddress = Room.BetAddress, Partner = recipient, Amount = output.Value, LockAddress = output.ScriptHash, StartIndex = block.Index, EndIndex = lat.LockExpiration, Timestamp = block.Timestamp, TxIndex = txIndex };
                batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_RoomPartnerLock_Record).Add(key), SliceBuilder.Begin().Add(lat));
            }
        }

    }
}
