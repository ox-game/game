using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.Cryptography.ECC;
using OX.Wallets;
using OX.IO;
using OX.Network.P2P;
using OX.BMS;
using static OX.BMS.MarkPortPlayerTermKey;
using System.IO;

namespace OX
{
    public class Web3NodeSet : ISerializable
    {
        public Web3Node[] Nodes;
        public virtual int Size => Nodes.GetVarSize();
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Nodes);
        }
        public void Deserialize(BinaryReader reader)
        {
            Nodes=reader.ReadSerializableArray<Web3Node>();
        }
    }
    public class Web3Node : ISerializable
    {
        /// <summary>
        /// 0:web3 node address
        /// 1:mark result address
        /// </summary>
        public ushort Catalog;
        public string NodeAddress;
        public ushort Port;
     
        public virtual int Size => sizeof(ushort) + NodeAddress.GetVarSize()+sizeof(ushort);
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Catalog);
            writer.WriteVarString(NodeAddress);
            writer.Write(Port);
        }
        public void Deserialize(BinaryReader reader)
        {
            Catalog = reader.ReadUInt16();
            NodeAddress =reader.ReadVarString();
            Port = reader.ReadUInt16();
        }
        public override string ToString()
        {
            return $"{Catalog}|{NodeAddress}|{Port}";
        }
    }
}
