using Akka.Util.Internal;
using System.Collections.Generic;

namespace OX.Web.ViewModels
{
    public class EatSmallBetViewModel
    {
        public byte Position { get; set; }
        public uint BetIndex { get; set; }
        public uint Amount { get; set; }
        public string SpecialCode { get; set; }
    }
    public class SpecialCode
    {
        public string Code { get; set; }
        public static IEnumerable<SpecialCode> All()
        {
            foreach (var r in GuessKey.SpecialChars)
                yield return new SpecialCode { Code = r.ToString() };
        }
    }
    public class SpecialPosition
    {
        public string Position { get; set; }
        public static IEnumerable<SpecialPosition> All()
        {
            for (byte i = 0; i < 10; i++)
                yield return new SpecialPosition { Position = i.ToString() };
        }
    }
}
