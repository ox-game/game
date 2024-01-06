using System;
using System.Collections.Generic;
using System.Linq;

namespace OX
{
    public class BetPack
    {
        public byte SpecialPosition;
        public char SpecialChar;
        internal GuessKey.BetAtom[] Positions = new GuessKey.BetAtom[10];
        public BetPackHitResult HitResult { get; private set; } = new BetPackHitResult();
        public bool Valid
        {
            get
            {
                bool ok = true;
                foreach (var position in Positions)
                    if (position == 0) return false;
                if (SpecialPosition > 9) return false;
                if (SpecialChar == default || !GuessKey.SpecialChars.Contains(SpecialChar)) return false;
                return ok;
            }
        }
        public void BetPostion(uint position, GuessKey.BetAtom atom)
        {
            if (position < 10)
            {
                Positions[position] = Positions[position] | atom;
            }
        }
        public bool TryMerge(out string betpoint)
        {
            if (!this.Valid)
            {
                betpoint = default;
                return false;
            }
            betpoint = string.Join("|", this.Positions.Select(m => m.Value().ToString())) + "|" + SpecialPosition.ToString() + "|" + SpecialChar;
            return true;
        }
        public Fixed8 TotalAmount()
        {
            if (!Valid) return Fixed8.Zero;
            int k = 1;
            for (int i = 0; i < this.Positions.Length; i++)
            {
                var p = this.Positions[i];
                k *= p.Parse().Length;
            }
            return Fixed8.FromDecimal(k);
        }
        public void Compare(uint index, GuessKey guessKey, ulong mineNonce)
        {
            BetPackHitResult result = new BetPackHitResult();
            if (this.Valid)
            {
                char[] keys = guessKey.ReRandomSanGongOrLottoInner(index, mineNonce);
                byte c = 0;
                for (int i = 0; i < this.Positions.Length; i++)
                {
                    var atom = this.Positions[i];
                    var kv = byte.Parse(keys[i].ToString());
                    if (BetPack.GetBitPlaces(atom).Contains(kv))
                    {
                        c++;
                        if (i == guessKey.SpecialPosition)
                        {
                            result.IsLuck = true;
                        }
                    }
                }
                result.HitCount = c;
                if (guessKey.SpecialPosition != this.SpecialPosition)
                    result.IsLuck = false;
                if (guessKey.SpecialChar != this.SpecialChar)
                    result.IsLuck = false;
            }
            this.HitResult = result;
        }
        public static string ShowBetPoint(string betpoint, bool special = true)
        {
            var ss = betpoint.Split('|', StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length != 12)
            {
                return string.Empty;
            }
            var SpecialChar = ss[11].ToCharArray()[0];
            if (!byte.TryParse(ss[10], out byte SpecialPosition) || SpecialPosition > 9)
            {
                return string.Empty;
            }
            var totalvalue = EnumHelper.All<GuessKey.BetAtom>().Sum(m => m.Value());
            List<string> list = new List<string>();
            for (uint i = 0; i < 10; i++)
            {
                if (!uint.TryParse(ss[i], out uint v) || v > totalvalue)
                {
                    return string.Empty;
                }
                int atomvalue = (int)v;
                var atoms = atomvalue.MergeParse<GuessKey.BetAtom>();
                var s = string.Join(',', GetBitPlaces(atoms).Select(m => m.ToString()));
                list.Add(s);
            }
            if (special)
                return SpecialChar + "-" + SpecialPosition.ToString() + "|" + string.Join('|', list);
            else
                return string.Join('|', list);
        }
        public static GuessKey.BetAtom GetAtomFromBitPlace(byte bitPlace)
        {
            if (bitPlace > 9 || bitPlace < 0) return (GuessKey.BetAtom)0;
            if (bitPlace == 0) return GuessKey.BetAtom.P0;
            else if (bitPlace == 1) return GuessKey.BetAtom.P1;
            else if (bitPlace == 2) return GuessKey.BetAtom.P2;
            else if (bitPlace == 3) return GuessKey.BetAtom.P3;
            else if (bitPlace == 4) return GuessKey.BetAtom.P4;
            else if (bitPlace == 5) return GuessKey.BetAtom.P5;
            else if (bitPlace == 6) return GuessKey.BetAtom.P6;
            else if (bitPlace == 7) return GuessKey.BetAtom.P7;
            else if (bitPlace == 8) return GuessKey.BetAtom.P8;
            else if (bitPlace == 9) return GuessKey.BetAtom.P9;
            return (GuessKey.BetAtom)0;
        }
        public static List<byte> GetBitPlaces(GuessKey.BetAtom atom)
        {
            List<byte> bs = new List<byte>();
            foreach (var at in atom.Parse())
            {
                switch (at)
                {
                    case GuessKey.BetAtom.P0:
                        bs.Add(0);
                        break;
                    case GuessKey.BetAtom.P1:
                        bs.Add(1);
                        break;
                    case GuessKey.BetAtom.P2:
                        bs.Add(2);
                        break;
                    case GuessKey.BetAtom.P3:
                        bs.Add(3);
                        break;
                    case GuessKey.BetAtom.P4:
                        bs.Add(4);
                        break;
                    case GuessKey.BetAtom.P5:
                        bs.Add(5);
                        break;
                    case GuessKey.BetAtom.P6:
                        bs.Add(6);
                        break;
                    case GuessKey.BetAtom.P7:
                        bs.Add(7);
                        break;
                    case GuessKey.BetAtom.P8:
                        bs.Add(8);
                        break;
                    case GuessKey.BetAtom.P9:
                        bs.Add(9);
                        break;
                }
            }
            return bs;
        }
        public static BetPack Parse(string betpoint)
        {
            if (TryParse(betpoint, out BetPack betPack))
                return betPack;
            return default;
        }
        public static bool TryParse(string betpoint, out BetPack betPack)
        {
            var ss = betpoint.Split('|', StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length != 12)
            {
                betPack = default;
                return false;
            }
            BetPack pack = new BetPack();

            var SpecialChar = ss[11].ToCharArray()[0];
            if (!GuessKey.SpecialChars.Contains(SpecialChar))
            {
                betPack = default;
                return false;
            }
            pack.SpecialChar = SpecialChar;

            if (!byte.TryParse(ss[10], out byte SpecialPosition) || SpecialPosition > 9)
            {
                betPack = default;
                return false;
            }
            pack.SpecialPosition = SpecialPosition;

            var totalvalue = EnumHelper.All<GuessKey.BetAtom>().Sum(m => m.Value());
            for (uint i = 0; i < 10; i++)
            {
                if (!uint.TryParse(ss[i], out uint v) || v > totalvalue)
                {
                    betPack = default;
                    return false;
                }
                int atomvalue = (int)v;
                pack.Positions[i] = atomvalue.MergeParse<GuessKey.BetAtom>();
            }

            betPack = pack;
            return true;
        }
    }
    public class BetPackHitResult
    {
        public byte HitCount { get; set; } = 0;
        public bool IsLuck { get; set; } = false;
    }
}
