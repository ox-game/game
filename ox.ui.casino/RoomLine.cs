using System.Collections.Generic;

namespace OX.UI.Casino
{
    public class RoomLine
    {
        public uint RoomId { get; set; }
        public Dictionary<uint, Round> Rounds = new Dictionary<uint, Round>();
    }
}
