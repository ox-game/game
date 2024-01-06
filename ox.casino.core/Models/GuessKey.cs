using OX.Cryptography;
using OX.IO;
using OX.Network.P2P;
using System;
using System.IO;
using System.Linq;

namespace OX
{
    public class GuessKey : ISerializable
    {
        [Flags]
        public enum BetAtom : int
        {
            P0 = 1,
            P1 = 2,
            P2 = 4,
            P3 = 8,
            P4 = 16,
            P5 = 32,
            P6 = 64,
            P7 = 128,
            P8 = 256,
            P9 = 512
        }
        public static readonly char[] SpecialChars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public RiddlesKind RiddlesKind;
        public byte SpecialPosition;
        public char SpecialChar;
        public string Keys;
        private UInt256 _hash = null;
        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    _hash = new UInt256(Crypto.Default.Hash256(this.GetHashData()));
                }
                return _hash;
            }
        }
        public int Size => sizeof(RiddlesKind) + sizeof(byte) + sizeof(char) + Keys.GetVarSize();
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)RiddlesKind);
            writer.Write(SpecialPosition);
            writer.Write(SpecialChar);
            writer.WriteVarString(Keys);
        }
        public void Deserialize(BinaryReader reader)
        {
            RiddlesKind = (RiddlesKind)reader.ReadByte();
            SpecialPosition = reader.ReadByte();
            SpecialChar = reader.ReadChar();
            Keys = reader.ReadVarString();
        }
        public static ulong BuildRiddlesSeed(uint index, ulong seed)
        {
            return seed % 100000;
        }
        public char[] ReRandomSanGongOrLottoInner(uint index, ulong seed)
        {
            if (this.RiddlesKind == RiddlesKind.EatSmall || this.RiddlesKind == RiddlesKind.Lotto)
            {
                var cs = this.Keys.ToCharArray().Select(m => int.Parse(m.ToString()));

                Random rd = new Random((int)BuildRiddlesSeed(index, seed));
                return string.Join(string.Empty, cs.OrderBy(m => rd.Next())).ToCharArray();
            }
            return this.Keys.ToCharArray();
        }
        public char[] ReRandomSanGongOrLotto(ulong seed, uint currentIndex)
        {

            return ReRandomSanGongOrLottoInner(currentIndex, seed);
        }
        public string ReRandomSanGongOrLottoInnerString(uint index, ulong seed)
        {
            return string.Concat(ReRandomSanGongOrLottoInner(index, seed));
        }
        public string ReRandomSanGongOrLottoString(ulong seed, uint currentIndex)
        {

            return string.Concat(ReRandomSanGongOrLotto(seed, currentIndex));
        }
    }
}
