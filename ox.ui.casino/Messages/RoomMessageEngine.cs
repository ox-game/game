using OX.Wallets.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OX.UI.Messages
{
    public class RoomMessageEngine
    {
        public const int MESSAGEQUEUEMAX = 1000;
        static Dictionary<UInt160, RoomMessageEngine> engines = new Dictionary<UInt160, RoomMessageEngine>();
        static RoomMessageEngine()
        {
            StateDispatcher.Instance.ServerStateNotice += Instance_ServerStateNotice;
        }

        private static void Instance_ServerStateNotice(IServerStateMessage message)
        {
            if (message is RoomChatMessage chatMessage)
            {
                GetRoomMessageEngine(chatMessage.BetAddress).Push(chatMessage);
            }
        }

        public static RoomMessageEngine GetRoomMessageEngine(UInt160 betAddress)
        {
            if (!engines.TryGetValue(betAddress, out RoomMessageEngine roomMessageEngine))
            {
                roomMessageEngine = new RoomMessageEngine(betAddress);
                engines[betAddress] = roomMessageEngine;
            }
            return roomMessageEngine;
        }


        public event Action<RoomChatMessage> MessageNotice;
        public UInt160 BetAdress { get; private set; }
        public Queue<RoomChatMessage> MessageQueue { get; protected set; } = new Queue<RoomChatMessage>();
        public int Count
        {
            get
            {
                if (MessageQueue.IsNullOrEmpty()) return 0;
                return MessageQueue.Count;
            }
        }
        public RoomChatMessage[] Messages
        {
            get
            {
                return MessageQueue.OrderBy(m => m.TimeStamp).ToArray();
            }
        }
        public RoomMessageEngine(UInt160 betAddress)
        {
            this.BetAdress = betAddress;
        }
        public void Push(RoomChatMessage message)
        {
            if (Count >= MESSAGEQUEUEMAX)
            {
                this.MessageQueue.Dequeue();
            }
            this.MessageQueue.Enqueue(message);
            MessageNotice?.Invoke(message);
        }
    }
}
