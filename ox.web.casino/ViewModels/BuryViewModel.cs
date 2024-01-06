using Akka.Util.Internal;
using AntDesign;
using System;
using System.Collections.Generic;

namespace OX.Web.ViewModels
{
    public class BuryMergeModel
    {
        public BuryRecord BuryRecord { get; set; }
        public BuryMergeTx BuryMergeTx { get; set; }
        public int index { get; set; }
    }
    public class BuryViewModel
    {
        public BuryViewModel()
        {
            Random rd = new Random();
            this.PlainCode = (byte)rd.Next(byte.MinValue, byte.MaxValue + 1);
            this.SecretCode = (byte)rd.Next(byte.MinValue, byte.MaxValue + 1);
        }
        public uint Amount { get; set; }
        public byte PlainCode
        {
            get; set;
        }
        public byte SecretCode
        {
            get; set;
        }
    }
}
