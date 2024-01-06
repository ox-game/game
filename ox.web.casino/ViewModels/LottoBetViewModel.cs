using Akka.Util.Internal;
using AntDesign;
using System.Collections.Generic;

namespace OX.Web.ViewModels
{
    public class LottoBetViewModel
    {
        public uint BetIndex { get; set; }
        public uint Amount { get; set; }
        public string SpecialCode { get; set; }
        public string SpecialPosition { get; set; }
        public CkeckData[] CheckDatas { get; set; }
        public BetPack BetPack { get; set; }
    }
    public class CkeckData
    {
        public uint N { get; set; }
        public CheckboxOption[] Options { get; set; }
        public string[] Result { get; set; }

    }
}
