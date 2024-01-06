using OX.Wallets.States;

namespace OX.UI.Messages
{
    public class RoomPrizeMessage : ServerStateMessage
    {
        public override ServerStateMessageKind StateMessageKind => ServerStateMessageKind.ApplicationMessage;
        public RoundClear RoundClear { get; set; }

    }
}
