using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OX.IO;
using OX.Cryptography.ECC;
using System.Security.Policy;
using System.IO;
using OX.Network.P2P;
using System.Runtime;
using OX.BMS;
using OX.Wallets;
using OX.Bapps;
using OX.SmartContract;
using OX.Cryptography;
using NBitcoin.Secp256k1;

namespace OX.Casino
{
    public static class BuryHelper
    {
        public static UInt160 GetBuryAddress()
        {
            return GetBuryMetaSideTransaction().GetContract().ScriptHash;
        }
        public static SlotSideTransaction GetBuryMetaSideTransaction()
        {
            return new SlotSideTransaction()
            {
                Slot = casino.CasinoMasterAccountPubKey,
                Channel = 0x01,
                SideType = SideType.PublicKey,
                Data = casino.CasinoMasterAccountPubKey.ToArray(),
                Flag = 0,
                AuthContract = Blockchain.SideAssetContractScriptHash,
                Attributes = new TransactionAttribute[0],
                Outputs = new TransactionOutput[0],
                Inputs = new CoinReference[0]
            };
        }


    }
    public class MarkBetAddressHelper
    {
        public const string MarkAdminPublicKeyString = "024ff1a511a316820500518678ff3ccfc87d69ea3a8c72e6d342db531d99dd767f";
        static MarkBetAddressHelper _instance;
        public static MarkBetAddressHelper Instance
        {
            get
            {
                if (_instance.IsNull())
                    _instance = new MarkBetAddressHelper();
                return _instance;
            }
        }
        MarkBetAddressHelper()
        {
            _markAdminPublicKey = ECPoint.DecodePoint(MarkAdminPublicKeyString.HexToBytes(), ECCurve.Secp256r1);
            _markAdmin = Contract.CreateSignatureRedeemScript(_markAdminPublicKey).ToScriptHash();
            _noticeShareKey = _markAdminPublicKey.ToArray();
            _noticeTalkLine = new UInt256(Crypto.Default.Hash256(Crypto.Default.Hash256(NoticeShareKey)));

        }
        ECPoint _markAdminPublicKey = default;
        public ECPoint MarkAdminPublicKey
        {
            get { return _markAdminPublicKey; }
        }
        public UInt160 _markAdmin = default;

        public UInt160 MarkAdmin
        {
            get
            {
                return _markAdmin;
            }
        }
        UInt160 _markUnionBetAddress = default;
        byte[] _noticeShareKey;
        public byte[] NoticeShareKey
        {
            get
            {
                return _noticeShareKey;
            }
        }
        UInt256 _noticeTalkLine;
        public UInt256 NoticeTalkLine
        {
            get { return _noticeTalkLine; }
        }
        public UInt160 GetMarkBetAddress(MarkChannelRound channelRound)
        {
            if (channelRound.Channel == BetChannel.MarkSix)
            {
                if (channelRound.Round == (byte)MarkSixRound.MarkUnion)
                    return GetMarkSixSideTransaction(0).GetContract().ScriptHash;
            }
            else if (channelRound.Channel == BetChannel.MarkOne)
            {
                return GetMarkSixSideTransaction(channelRound.Round).GetContract().ScriptHash;
            }
            return UInt160.Zero;
        }
        public UInt160 GetMarkUnionBetAddress()
        {
            if (_markUnionBetAddress == default)
            {
                _markUnionBetAddress = GetMarkBetAddress(new MarkChannelRound(BetChannel.MarkSix, (byte)MarkSixRound.MarkUnion));
            }
            return _markUnionBetAddress;
        }
        public SlotSideTransaction GetMarkUnionSlotSideTransaction()
        {
            return GetMarkSixSideTransaction(0);
        }
        public SlotSideTransaction GetMarkSixSideTransaction(byte flag)
        {
            return new SlotSideTransaction()
            {
                Slot = casino.CasinoMasterAccountPubKey,
                Channel = 0x03,
                SideType = SideType.PublicKey,
                Data = casino.CasinoMasterAccountPubKey.ToArray(),
                Flag = 0,
                AuthContract = Blockchain.SideAssetContractScriptHash,
                Attributes = new TransactionAttribute[0],
                Outputs = new TransactionOutput[0],
                Inputs = new CoinReference[0]
            };

        }
        public UInt160 GetMemberDepositAddress(ECPoint memberPublicKey, out AssetTrustTransaction att)
        {
            att = new AssetTrustTransaction
            {
                TrustContract = Blockchain.TrustAssetContractScriptHash,
                IsMustRelateTruster = true,
                Truster = memberPublicKey,
                Trustee = MarkAdminPublicKey,
                Targets = [MarkAdmin],
                SideScopes = new UInt160[0]
            };
            return att.GetContract().ScriptHash;
        }
    }
}
