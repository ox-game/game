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
using OX.BMS;
using OX.Wallets;
using System.Collections.Generic;
using OX.IO.Wrappers;

namespace OX.Casino
{

    public static partial class CasinoPersistenceHelper
    {
        public static void Save_Web3Node(this WriteBatch batch, CasinoProvider provider, Block block, ReplyTransaction rt, Web3NodeSet web3NodeSet)
        {
            if (web3NodeSet.IsNotNull() && web3NodeSet.Nodes.IsNotNullAndEmpty())
            {
                foreach (var node in web3NodeSet.Nodes)
                {
                    var ts = (UInt32Wrapper)block.Timestamp;
                    batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.Casino_Web3Node_Publish).Add(node), SliceBuilder.Begin().Add(ts.ToArray()));
                    if (!provider.Web3Nodes.TryGetValue(node.Catalog, out var dic))
                    {
                        dic = new Dictionary<string, uint>();
                        provider.Web3Nodes[node.Catalog] = dic;
                    }
                    dic[$"{node.NodeAddress}:{node.Port}"] = block.Timestamp;
                }
            }
        }
        //public static void Save_GuessAnswer(this WriteBatch batch, CasinoProvider provider, GuessAnswerReply guessAnswerReply)
        //{
        //    foreach (var answer in guessAnswerReply.Answers)
        //    {
        //        batch.Put(SliceBuilder.Begin(CasinoBizPersistencePrefixes.BMS_GuessAnswer).Add(answer.Key), SliceBuilder.Begin().Add(answer.Value));
        //        var record = new GuessAnswer { Key = answer.Key, Value = answer.Value };
        //        provider.GuessAnswers[answer.Key.ToString()] = record;
        //        if (!provider.LatestGuessAnswer.TryGetValue(record.Key.ChannelRound, out var v) || record.Key.Term.ToDateTime() > v.Key.Term.ToDateTime())
        //        {
        //            provider.LatestGuessAnswer[record.Key.ChannelRound] = record;
        //        }
        //    }
        //}
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
