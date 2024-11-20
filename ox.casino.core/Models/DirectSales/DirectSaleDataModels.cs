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
using OX.Network.P2P.Payloads;
using OX.Cryptography.ECC;
using static Akka.Actor.ProviderSelection;
using OX.Wallets;
using System.Reflection.Metadata.Ecma335;

namespace OX.DirectSales
{
    public class DirectSaleRequestData : ISerializable
    {
        public UInt256 AssetId;
        public ECPoint Buyer;
        public ECPoint Seller;
        public uint TimeStamp;
        public virtual int Size => AssetId.Size + Buyer.Size + Seller.Size + sizeof(uint);
        public DirectSaleRequestData()
        {

        }
        public DirectSaleRequestData(UInt256 assetId, ECPoint seller, ECPoint buyer) : this()
        {
            this.AssetId = assetId;
            this.Seller = seller;
            this.Buyer = buyer;
            this.TimeStamp = System.DateTime.UtcNow.ToTimestamp();
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Buyer);
            writer.Write(Seller);
            writer.Write(TimeStamp);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            Buyer = reader.ReadSerializable<ECPoint>();
            Seller = reader.ReadSerializable<ECPoint>();
            TimeStamp = reader.ReadUInt32();
        }
    }
    public class DirectSaleRequest : ISerializable
    {
        public DirectSaleRequestData Request;
        public byte[] Signature;
        public virtual int Size => Request.Size + Signature.GetVarSize();
        public DirectSaleRequest()
        {

        }
        public DirectSaleRequest(UInt256 assetId, ECPoint seller, KeyPair localKey) : this()
        {
            this.Request = new DirectSaleRequestData(assetId, seller, localKey.PublicKey);
            this.Signature = Crypto.Default.Sign(this.Request.ToArray(), localKey.PrivateKey, localKey.PublicKey.EncodePoint(false).Skip(1).ToArray());
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Request);
            writer.WriteVarBytes(Signature);
        }
        public void Deserialize(BinaryReader reader)
        {
            Request = reader.ReadSerializable<DirectSaleRequestData>();
            Signature = reader.ReadVarBytes();
        }
        public static bool TryParse(ECPoint myPublicKey, string hex, out DirectSaleRequestData data)
        {
            data = default;
            try
            {
                if (!hex.StartsWith("??")) return false;
                hex = hex.Substring(2);
                var bs = hex.HexToBytes();
                if (bs.TryAsSerializable<DirectSaleRequest>(out var request))
                {
                    if (Crypto.Default.VerifySignature(request.Request.ToArray(), request.Signature, request.Request.Buyer.EncodePoint(false).Skip(1).ToArray()) && myPublicKey.Equals(request.Request.Seller))
                    {
                        data = request.Request;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }

    public class DirectSaleApproveContent : ISerializable
    {
        public UInt256 MLSTHash;
        public UInt256 ApproveCode;
        public virtual int Size => MLSTHash.Size + ApproveCode.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MLSTHash);
            writer.Write(ApproveCode);
        }
        public void Deserialize(BinaryReader reader)
        {
            MLSTHash = reader.ReadSerializable<UInt256>();
            ApproveCode = reader.ReadSerializable<UInt256>();
        }
    }
    public class DirectSaleApprove : ISerializable
    {
        public ECPoint Buyer;
        public ECPoint Seller;
        public uint TimeStamp;
        public byte[] CipherData;
        public virtual int Size => Buyer.Size + Seller.Size + sizeof(uint) + CipherData.GetVarSize();
        public DirectSaleApprove()
        {

        }
        public DirectSaleApprove(ECPoint buyer, KeyPair localKey, DirectSaleApproveContent content) : this()
        {
            this.Seller = localKey.PublicKey;
            this.Buyer = buyer;
            this.TimeStamp = System.DateTime.Now.ToTimestamp();
            this.CipherData = content.ToArray().Encrypt(localKey, buyer, BitConverter.GetBytes(this.TimeStamp));
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Buyer);
            writer.Write(Seller);
            writer.Write(TimeStamp);
            writer.WriteVarBytes(CipherData);
        }
        public void Deserialize(BinaryReader reader)
        {
            Buyer = reader.ReadSerializable<ECPoint>();
            Seller = reader.ReadSerializable<ECPoint>();
            TimeStamp = reader.ReadUInt32();
            CipherData = reader.ReadVarBytes();
        }
        static bool TryParse(string hex, out DirectSaleApprove reply)
        {
            reply = default;
            try
            {
                reply = hex.HexToBytes().AsSerializable<DirectSaleApprove>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool BuyerVerify(KeyPair buyerKey, out DirectSaleApproveContent content)
        {
            content = default;
            var plaintext = this.CipherData.Decrypt(buyerKey, this.Seller, BitConverter.GetBytes(this.TimeStamp));
            if (plaintext.IsNullOrEmpty()) return false;
            try
            {
                content = plaintext.AsSerializable<DirectSaleApproveContent>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool BuyerTryParser(KeyPair buyerKey, string hex, out DirectSaleApprove reply, out DirectSaleApproveContent content)
        {
            reply = default;
            content = default;
            if (!hex.StartsWith("%%")) return false;
            hex = hex.Substring(2);
            if (TryParse(hex, out reply))
            {
                return reply.BuyerVerify(buyerKey, out content);
            }
            return false;
        }
    }
    public class DirectSaleReplyContent : ISerializable
    {
        public UInt256 AssetId;
        public Fixed8 Amount;
        public UInt256 ApproveSource;
        public string Msg;

        public virtual int Size => AssetId.Size + Amount.Size + ApproveSource.Size + Msg.GetVarSize();

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Amount);
            writer.Write(ApproveSource);
            writer.WriteVarString(Msg);
        }
        public void Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            Amount = reader.ReadSerializable<Fixed8>();
            ApproveSource = reader.ReadSerializable<UInt256>();
            Msg = reader.ReadVarString();
        }
    }
    public class DirectSaleReply : ISerializable
    {
        public ECPoint Buyer;
        public ECPoint Seller;
        public uint TimeStamp;
        public byte[] CipherData;
        public virtual int Size => Buyer.Size + Seller.Size + sizeof(uint) + CipherData.GetVarSize();
        public DirectSaleReply()
        {

        }
        public DirectSaleReply(ECPoint seller, KeyPair localKey, DirectSaleReplyContent content) : this()
        {
            this.Seller = seller;
            this.Buyer = localKey.PublicKey;
            this.TimeStamp = System.DateTime.Now.ToTimestamp();
            this.CipherData = content.ToArray().Encrypt(localKey, seller, BitConverter.GetBytes(this.TimeStamp));
        }
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Buyer);
            writer.Write(Seller);
            writer.Write(TimeStamp);
            writer.WriteVarBytes(CipherData);
        }
        public void Deserialize(BinaryReader reader)
        {
            Buyer = reader.ReadSerializable<ECPoint>();
            Seller = reader.ReadSerializable<ECPoint>();
            TimeStamp = reader.ReadUInt32();
            CipherData = reader.ReadVarBytes();
        }
        static bool TryParse(string hex, out DirectSaleReply reply)
        {
            reply = default;
            try
            {
                reply = hex.HexToBytes().AsSerializable<DirectSaleReply>();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SellerVerify(KeyPair sellerKey, out DirectSaleReplyContent content)
        {
            content = default;
            var plaintext = this.CipherData.Decrypt(sellerKey, this.Buyer, BitConverter.GetBytes(this.TimeStamp));
            if (plaintext.IsNullOrEmpty()) return false;
            try
            {
                content = plaintext.AsSerializable<DirectSaleReplyContent>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SellerTryParser(KeyPair sellerKey, string hex, out DirectSaleReply reply, out DirectSaleReplyContent content)
        {
            reply = default;
            content = default;
            if (!hex.StartsWith("??")) return false;
            hex = hex.Substring(2);
            if (TryParse(hex, out reply))
            {
                return reply.SellerVerify(sellerKey, out content);
            }
            return false;
        }

    }
    public class MutualLockSellerTransactionKey : ISerializable
    {
        public UInt160 Holder;
        public bool IsSeller;
        public uint BlockIndex;
        public uint TimeStamp;
        public UInt256 TxHash;
        public virtual int Size => Holder.Size + sizeof(bool) + sizeof(uint) + sizeof(uint) + TxHash.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Holder);
            writer.Write(IsSeller);
            writer.Write(BlockIndex);
            writer.Write(TimeStamp);
            writer.Write(TxHash);
        }
        public void Deserialize(BinaryReader reader)
        {
            Holder = reader.ReadSerializable<UInt160>();
            IsSeller = reader.ReadBoolean();
            BlockIndex = reader.ReadUInt32();
            TimeStamp = reader.ReadUInt32();
            TxHash = reader.ReadSerializable<UInt256>();
        }
    }
    public class MutualLockSellerTransactionValue : ISerializable
    {
        public MutualLockSellerTransaction MLST;
        public MutualLockBuyerTransaction MLBT;
        public virtual int Size => MLST.Size + sizeof(uint) + (MLBT.IsNotNull() ? MLBT.Size : 0);

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MLST);
            uint length = MLBT.IsNotNull() ? (uint)MLBT.Size : 0;
            writer.Write(length);
            if (MLBT.IsNotNull())
            {
                writer.Write(MLBT);
            }
        }
        public void Deserialize(BinaryReader reader)
        {
            MLST = reader.ReadSerializable<MutualLockSellerTransaction>();
            var length = reader.ReadUInt32();
            if (length > 0)
            {
                MLBT = reader.ReadSerializable<MutualLockBuyerTransaction>();
            }
        }
    }
    public class MutualLockSellerTransactionMerge : ISerializable
    {
        public MutualLockSellerTransactionKey Key;
        public MutualLockSellerTransactionValue Value;
        public virtual int Size => Key.Size + Value.Size;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Key);
            writer.Write(Value);
        }
        public void Deserialize(BinaryReader reader)
        {
            Key = reader.ReadSerializable<MutualLockSellerTransactionKey>();
            Value = reader.ReadSerializable<MutualLockSellerTransactionValue>();
        }
    }
}
