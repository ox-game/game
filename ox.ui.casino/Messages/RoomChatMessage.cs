using OX.Wallets.States;

namespace OX.UI.Messages
{
    public class RoomChatMessage : ServerStateMessage
    {
        public override ServerStateMessageKind StateMessageKind => ServerStateMessageKind.ApplicationMessage;
        public UInt160 BetAddress { get; set; }
        public uint AddressId { get; set; }
        public string Message { get; set; }
        public uint TimeStamp { get; set; }

    }
}
