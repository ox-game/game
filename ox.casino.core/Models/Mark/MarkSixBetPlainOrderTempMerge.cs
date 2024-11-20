using Org.BouncyCastle.Utilities;
using System.Linq;
using OX.IO;
using OX.Ledger;
using System.Collections.ObjectModel;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OX.Cryptography;
using OX.Network.P2P;
using System;
using System.Drawing;

namespace OX.BMS
{
   

    public class MarkSixBetPlainOrderTempMerge : ISerializable
    {
        public MarkPlainBetOrder Order;
        public uint OriginAmount;
        public uint CutAmount;
        public virtual int Size => Order.Size + sizeof(uint) + sizeof(uint);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Order);
            writer.Write(OriginAmount);
            writer.Write(CutAmount);
        }
        public void Deserialize(BinaryReader reader)
        {
            Order = reader.ReadSerializable<MarkPlainBetOrder>();
            OriginAmount = reader.ReadUInt32();
            CutAmount = reader.ReadUInt32();
        }
    }
    public class MarkSixBetCipherOrderTempMerge : ISerializable
    {
        public MarkCipherBetOrder Order;
        public uint OriginAmount;
        public uint CutAmount;
        public virtual int Size => Order.Size + sizeof(uint) + sizeof(uint);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Order);
            writer.Write(OriginAmount);
            writer.Write(CutAmount);
        }
        public void Deserialize(BinaryReader reader)
        {
            Order = reader.ReadSerializable<MarkCipherBetOrder>();
            OriginAmount = reader.ReadUInt32();
            CutAmount = reader.ReadUInt32();
        }
    }
}
