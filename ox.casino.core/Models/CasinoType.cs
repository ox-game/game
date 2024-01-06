
using OX.Casino;
using OX.Cryptography.ECC;
using OX.IO;
using OX.Ledger;
using OX.Network.P2P;
using OX.Network.P2P.Payloads;
using OX.SmartContract;
using System.Collections.Generic;
using System.Linq;

namespace OX
{
    public enum CasinoType : byte
    {
        RiddlesAndHash = 0x57,
        //RiddlesAndHash = 0x58,
        RoomSplitRequest = 0x59,
        BankerWithdraw = 0x5A,
        ValetRegister = 0x5B,
        BetReturn = 0x5C,
        CreatePledgeAccountRequest = 0x5D,
        CreateRoomGuaranteeRequest = 0x60,
        RoomGuaranteeReply = 0x61,
        RoomPledgeAccountReply = 0x62,
        ReplyBury = 0x65,
        CreateRoomRequest = 0x66,
        RoundClear = 0x67,
        RoomStateRequest = 0x68,
        Bet = 0x69,
        Bury = 0x6A,
        PrivateRoomMemberSetting = 0x6B
    }

    public static class CasinoHelper
    {
        public static bool VerifyBetRequest(this BetRequest request, AskTransaction at, Dictionary<UInt160, MixRoom> Rooms, out ushort? n)
        {
            if (at.Outputs.IsNotNullAndEmpty())
            {
                for (ushort k = 0; k < at.Outputs.Length; k++)
                {
                    var output = at.Outputs[k];

                    if (Rooms.TryGetValue(request.BetAddress, out MixRoom Room))
                    {
                        if (output.ScriptHash == Room.BetAddress && output.AssetId == Room.Request.AssetId && Room.VerifyPrivateRoomBetFee(at))
                        {
                            //if (request.Passport.IsNotNull())
                            //{

                            //    if (request.Passport.Target.BetAddress == request.BetAddress && (request.Passport.Target.PublicKey.Equals(pubKey) || rsr.IsNotNull() && rsr.Admins.Contains(request.Passport.Target.PublicKey)) && request.Passport.Target.Gambler.Equals(request.From))
                            //    {
                            //        var ok = request.Passport.Verify();
                            //        if (ok)
                            //        {
                            //            n = k;
                            //            return true;
                            //        }
                            //        else
                            //        {
                            //            n = default;
                            //            return false;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        n = default;
                            //        return false;
                            //    }
                            //}
                            //else
                            //{
                            n = k;
                            return true;
                            //}
                        }
                    }
                }
            }
            n = default;
            return false;
        }
        public static bool TryGetBetRequest(this RangeTransaction rgt, out BetRequest request, out TransactionOutput txOutput, out ushort? n, out UInt160 fromSh, out ECPoint fromPubKey)
        {
            if (rgt.Outputs.IsNotNullAndEmpty())
            {
                for (ushort k = 0; k < rgt.Outputs.Length; k++)
                {
                    var output = rgt.Outputs[k];
                    var attr = rgt.Attributes.Where(m => m.Usage == TransactionAttributeUsage.RelatedData).FirstOrDefault();
                    var attr2 = rgt.Attributes.Where(m => m.Usage == TransactionAttributeUsage.RelatedScriptHash).FirstOrDefault();
                    var attr3 = rgt.Attributes.Where(m => m.Usage == TransactionAttributeUsage.RelatedPublicKey).FirstOrDefault();
                    if (attr.IsNotNull() && attr2.IsNotNull() && attr3.IsNotNull())
                    {
                        try
                        {
                            request = attr.Data.AsSerializable<BetRequest>();
                            var sh = new UInt160(attr2.Data);
                            var pubkey = ECPoint.DecodePoint(attr3.Data, ECCurve.Secp256r1);
                            if (sh.Equals(request.From) && request.Index == rgt.MaxIndex)
                            {
                                txOutput = output;
                                n = k;
                                fromSh = sh;
                                fromPubKey = pubkey;
                                return true;
                            }
                        }
                        catch
                        {
                            request = default;
                            txOutput = default;
                            n = default;
                            fromSh = default;
                            fromPubKey = default;
                            return false;
                        }
                    }
                }
            }
            request = default;
            txOutput = default;
            n = default;
            fromSh = default;
            fromPubKey = default;
            return false;
        }
        public static bool VerifyBetRequest(this BetRequest request, RangeTransaction rgt, TransactionOutput output, Dictionary<UInt160, MixRoom> Rooms)
        {
            if (Rooms.TryGetValue(request.BetAddress, out MixRoom Room))
            {
                if (output.ScriptHash == Room.BetAddress && output.AssetId == Room.Request.AssetId && Room.VerifyPrivateRoomBetFee(rgt))
                {
                    //if (request.Passport.IsNotNull())
                    //{
                    //    if (request.Passport.Target.BetAddress == request.BetAddress && (request.Passport.Target.PublicKey.Equals(pubKey) || rsr.IsNotNull() && rsr.Admins.Contains(request.Passport.Target.PublicKey)) && request.Passport.Target.Gambler.Equals(request.From))
                    //    {
                    //        var ok = request.Passport.Verify();
                    //        if (ok)
                    //        {
                    //            return true;
                    //        }
                    //        else
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return false;
                    //    }
                    //}
                    //else
                    //{
                    return true;
                    //}
                }
            }
            return false;
        }
        public static bool VerifyRoomStateRequest(this SignatureValidator<RoomStateRequest> request, AskTransaction at, Dictionary<uint, RoomRecord> Rooms, Dictionary<uint, ECPoint> RoomHolders)
        {
            if (Rooms.TryGetValue(request.Target.RoomId, out RoomRecord Room) && RoomHolders.TryGetValue(request.Target.RoomId, out ECPoint pubKey))
            {
                if (pubKey.Equals(request.Target.PublicKey) && request.Verify()) return true;
            }
            return false;
        }
        public static bool VerifyRoomSplitRequest(this SignatureValidator<RoomSplitRequest> request, AskTransaction at, Dictionary<uint, RoomRecord> Rooms, Dictionary<uint, ECPoint> RoomHolders)
        {
            if (Rooms.TryGetValue(request.Target.RoomId, out RoomRecord Room) && RoomHolders.TryGetValue(request.Target.RoomId, out ECPoint pubKey))
            {
                if (pubKey.Equals(request.Target.PublicKey) && request.Verify()) return true;
            }
            return false;
        }
        public static bool VerifyBuryRequest(this BuryRequest request, AskTransaction at, out ushort? n)
        {
            if (at.Outputs.IsNotNullAndEmpty())
            {
                for (ushort k = 0; k < at.Outputs.Length; k++)
                {
                    var output = at.Outputs[k];
                    if(request.VerifyBuryRequest(output))
                    {
                        n = k;
                        return true;
                    }                    
                }
            }
            n = default;
            return false;
        }
        public static bool TryGetBuryRequest(this RangeTransaction rgt, out BuryRequest request, out TransactionOutput txOutput, out ushort? n, out UInt160 fromSh, out ECPoint fromPubKey)
        {
            if (rgt.Outputs.IsNotNullAndEmpty())
            {
                for (ushort k = 0; k < rgt.Outputs.Length; k++)
                {
                    var output = rgt.Outputs[k];
                    if (output.AssetId.Equals(Blockchain.OXC))
                    {
                        var attr = rgt.Attributes.Where(m => m.Usage == TransactionAttributeUsage.RelatedData).FirstOrDefault();
                        var attr2 = rgt.Attributes.Where(m => m.Usage == TransactionAttributeUsage.RelatedScriptHash).FirstOrDefault();
                        var attr3 = rgt.Attributes.Where(m => m.Usage == TransactionAttributeUsage.Tip5).FirstOrDefault();
                        if (attr.IsNotNull() && attr2.IsNotNull() && attr3.IsNotNull())
                        {
                            try
                            {
                                request = attr.Data.AsSerializable<BuryRequest>();
                                var sh = new UInt160(attr2.Data);
                                var pubkey = ECPoint.DecodePoint(attr3.Data, ECCurve.Secp256r1);
                                if (sh.Equals(request.From))
                                {
                                    txOutput = output;
                                    n = k;
                                    fromSh = sh;
                                    fromPubKey = pubkey;
                                    return true;
                                }
                            }
                            catch
                            {
                                request = default;
                                txOutput = default;
                                n = default;
                                fromSh = default;
                                fromPubKey = default;
                                return false;
                            }
                        }
                    }
                }
            }
            request = default;
            txOutput = default;
            n = default;
            fromSh = default;
            fromPubKey = default;
            return false;
        }
        public static bool VerifyBuryRequest(this BuryRequest request, TransactionOutput output)
        {
            if (output.AssetId == Blockchain.OXC && output.ScriptHash == request.BetAddress && output.ScriptHash == casino.BuryBetAddress) return true;
            return false;
        }
    }

}
