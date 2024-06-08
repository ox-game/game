using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.Cryptography.ECC;
using OX.SmartContract;
using OX.Wallets;
using OX.Ledger;
using OX.Casino;

namespace OX
{

    public class casino
    {
        public const string Name = "casino";
        public const string OfficalEventBoardId = "106374-1";
        public static string[] PubKeys = new string[] {
        "02e673665c8a62067bc2764fa4fa75328494bf74546dbf60faf04dd8d21f25e324",
        "02a13213153cdcf622c66327f66b14ea01107488a2011ee2d33146f4354c9db018",
        "023cdfbb5649406ed507f78830d3ad6b2c47cfd0dbfb07d1d0488573f1039da2cf",
        "034549c02abf70ab5309edf7d3d1447be41845ef91971da8a7015fd15817867b00",
        "0331134ec39559d6a0844756fb5b12d35e59eaf6f8fc8406cbf119174c822b2838",
        "02a3147c018322228c648a9d5f95f8f73eafb932f790d307388c0918921fca73f0",
        "03a3fa832226ea01d7233cbe624887c166158acab7daf477b155fcb2bfb0b38a32"
        };
        public static ECPoint[] BizPublicKeys { get; private set; }
        public static Dictionary<UInt160, ECPoint> BizAddresses { get; private set; }
        public static string[] SettingAccounts = new string[] {
        "AZ1YUPWmvHumN6fYhKW3wxNd1hEcAmx83F",
        "AJ7CZZob7GAxfBJkttDJ1QpsmVH7ZVLmf9",
        "AMgt5HE1aLLrP57oa4g25gVvXwYtdg89bf",
        "AFs3vAEWfi5vjHFhEC9M6N8QysGKxvT1ic",
        "AV9sUha8ZjDVouqTTXnacUNFVThMXdiisn",
        "AafxG4CuzCcEWfNFNuxmcY6fePYzoeNj6B",
        "AXJLqzr2kDvXYTf9yC5tzqg77wKUy7w73d"
        };
      
        public static UInt256 GamblerLuckBonusAsset { get; private set; } = UInt256.Parse("0xf986af708e70b54dfecfedb18bd03f4530698da48bb7c47794c2c2b4f7bacd37");
       
        public static ECPoint CasinoMasterAccountPubKey = ECPoint.DecodePoint(PubKeys[0].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 CasinoMasterAccountAddress = Contract.CreateSignatureRedeemScript(CasinoMasterAccountPubKey).ToScriptHash();

        //public static ECPoint CasinoSettleAccountPubKey = ECPoint.DecodePoint(PubKeys[2].HexToBytes(), ECCurve.Secp256r1);
        //public static UInt160 CasinoSettleAccountAddress = Contract.CreateSignatureRedeemScript(CasinoSettleAccountPubKey).ToScriptHash();

        //public static ECPoint TeamSettleAccountPubKey = ECPoint.DecodePoint(PubKeys[4].HexToBytes(), ECCurve.Secp256r1);
        //public static UInt160 TeamSettleAccountAddress = Contract.CreateSignatureRedeemScript(TeamSettleAccountPubKey).ToScriptHash();

        public static ECPoint CasinoWitnessAccountPubKey = ECPoint.DecodePoint(PubKeys[5].HexToBytes(), ECCurve.Secp256r1);
        public static UInt160 CasinoWitnessAccountAddress = Contract.CreateSignatureRedeemScript(CasinoWitnessAccountPubKey).ToScriptHash();

        public static UInt160 BuryBetAddress = BuryHelper.GetBuryAddress();
        public const string GamblerLuckBonusAssetDexPool = "AVJJpNAvzgPz3FCEFMhgriQdX16eVUumXn";

        static casino()
        {
            BizPublicKeys = PubKeys.Select(p => ECPoint.DecodePoint(p.HexToBytes(), ECCurve.Secp256r1)).ToArray();
            BizAddresses = BizPublicKeys.ToDictionary(n => Contract.CreateSignatureRedeemScript(n).ToScriptHash());
        }
    }
}
