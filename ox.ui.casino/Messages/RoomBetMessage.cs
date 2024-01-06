using OX.Wallets.States;

namespace OX.UI.Messages
{
    public class RoomBetMessage : ServerStateMessage
    {
        public override ServerStateMessageKind StateMessageKind => ServerStateMessageKind.ApplicationMessage;
        public BetRequest BetRequest { get; set; }

    }
}
