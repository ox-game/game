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
using System.Reflection;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Asn1.X509;
using System.Web;

namespace OX.BMS
{
    public static class BitSixAgentHelper
    {
        public const int MAXBITMAXSIXORDERITEMS = 100;


        public static MarkSixBetPlainOrderTempMerge CombinePlainOrders(UInt160 player, MarkTerm term, MarkInboundOrder[] agentOrders, bool autoCute = true)
        {
            if (agentOrders.IsNullOrEmpty()) return default;
            var firstOrder = agentOrders.FirstOrDefault();
            MarkPlainBetOrder neworder = new MarkPlainBetOrder { Player = player, Term = term, ChannelRound = firstOrder.OrderHead.ChannelRound };
            //merge
            Dictionary<BitMarkSixBetTarget, uint> dic = new Dictionary<BitMarkSixBetTarget, uint>();
            foreach (var order in agentOrders)
            {
                foreach (var item in order.OrderBody.BetItems)
                {
                    var amt = item.Amount;
                    if (dic.TryGetValue(item.BetTarget, out uint v))
                    {
                        amt += v;
                    }
                    dic[item.BetTarget] = amt;
                }
            }
            var totlal_origin = (uint)dic.Sum(m => m.Value);
            //cut
            List<BitMarkSixBet> list = new List<BitMarkSixBet>();
            foreach (var d in dic)
            {
                BitMarkSixBet bet = new BitMarkSixBet { BetTarget = d.Key, Amount = d.Value };
                list.Add(bet);
            }
            uint totalCut = 0;
            if (autoCute)
            {
                foreach (var g in list.GroupBy(m => m.BetTarget.Method))
                {
                    if (firstOrder.OrderHead.ChannelRound.Channel == BetChannel.MarkOne)
                    {
                        MarkOneBetMethod method = (MarkOneBetMethod)g.Key;
                        var methodSetting = method.GetMethodSetting();
                        var cutAmt = methodSetting.Combine(g.ToArray());
                        totalCut += cutAmt;
                    }
                    else if (firstOrder.OrderHead.ChannelRound.Channel == BetChannel.MarkSix)
                    {
                        MarkSixBetMethod method = (MarkSixBetMethod)g.Key;
                        var methodSetting = method.GetMethodSetting();
                        var cutAmt = methodSetting.Combine(g.ToArray());
                        totalCut += cutAmt;
                    }
                }
                list = list.Where(m => m.Amount > 0).ToList();
            }
            neworder.BetItems = list.ToArray();
            MarkSixBetPlainOrderTempMerge orderMerge = new MarkSixBetPlainOrderTempMerge { Order = neworder, OriginAmount = totlal_origin, CutAmount = totalCut };
            return orderMerge;
        }
        public static MarkPlainBetOrder[] PlainShard(this MarkPlainBetOrder order)
        {
            var count = order.BetItems.Count();
            var remainder = count % MAXBITMAXSIXORDERITEMS;
            var groupCount = count / MAXBITMAXSIXORDERITEMS;
            if (remainder > 0)
                groupCount++;

            Dictionary<int, List<BitMarkSixBet>> dic = new Dictionary<int, List<BitMarkSixBet>>();
            for (int i = 0; i < groupCount; i++)
            {
                dic[i] = new List<BitMarkSixBet>();
            }
            int flag = 0;
            foreach (var item in order.BetItems.OrderByDescending(m => m.Amount))
            {
                dic[flag].Add(item);
                flag++;
                flag = flag % groupCount;
            }
            List<MarkPlainBetOrder> orders = new List<MarkPlainBetOrder>();
            foreach (var d in dic)
            {
                MarkPlainBetOrder rd = new MarkPlainBetOrder { Player = order.Player, Term = order.Term, ChannelRound = order.ChannelRound, BetItems = d.Value.ToArray() };
                orders.Add(rd);
            }
            return orders.ToArray();
        }

        public static MarkSixBetCipherOrderTempMerge CombineCipherOrders(UInt160 player,UInt160 portHolder, MarkTerm term, MarkInboundOrder[] agentOrders, bool autoCute = true)
        {
            if (agentOrders.IsNullOrEmpty()) return default;
            var firstOrder = agentOrders.FirstOrDefault();
            MarkCipherBetOrder neworder = new MarkCipherBetOrder { Player = player, PortHolder=portHolder, Term = term, ChannelRound = firstOrder.OrderHead.ChannelRound };
            //merge
            Dictionary<BitMarkSixBetTarget, uint> dic = new Dictionary<BitMarkSixBetTarget, uint>();
            foreach (var order in agentOrders)
            {
                foreach (var item in order.OrderBody.BetItems)
                {
                    var amt = item.Amount;
                    if (dic.TryGetValue(item.BetTarget, out uint v))
                    {
                        amt += v;
                    }
                    dic[item.BetTarget] = amt;
                }
            }
            var totlal_origin = (uint)dic.Sum(m => m.Value);
            //cut
            List<BitMarkSixBet> list = new List<BitMarkSixBet>();
            foreach (var d in dic)
            {
                BitMarkSixBet bet = new BitMarkSixBet { BetTarget = d.Key, Amount = d.Value };
                list.Add(bet);
            }
            uint totalCut = 0;
            if (autoCute)
            {
                foreach (var g in list.GroupBy(m => m.BetTarget.Method))
                {
                    if (firstOrder.OrderHead.ChannelRound.Channel == BetChannel.MarkOne)
                    {
                        MarkOneBetMethod method = (MarkOneBetMethod)g.Key;
                        var methodSetting = method.GetMethodSetting();
                        var cutAmt = methodSetting.Combine(g.ToArray());
                        totalCut += cutAmt;
                    }
                    else if (firstOrder.OrderHead.ChannelRound.Channel == BetChannel.MarkSix)
                    {
                        MarkSixBetMethod method = (MarkSixBetMethod)g.Key;
                        var methodSetting = method.GetMethodSetting();
                        var cutAmt = methodSetting.Combine(g.ToArray());
                        totalCut += cutAmt;
                    }
                }
                list = list.Where(m => m.Amount > 0).ToList();
            }
            neworder.BetItems = list.ToArray();
            MarkSixBetCipherOrderTempMerge orderMerge = new MarkSixBetCipherOrderTempMerge { Order = neworder, OriginAmount = totlal_origin, CutAmount = totalCut };
            return orderMerge;
        }
        public static MarkCipherBetOrder[] CipherShard(this MarkCipherBetOrder order)
        {
            var count = order.BetItems.Count();
            var remainder = count % MAXBITMAXSIXORDERITEMS;
            var groupCount = count / MAXBITMAXSIXORDERITEMS;
            if (remainder > 0)
                groupCount++;

            Dictionary<int, List<BitMarkSixBet>> dic = new Dictionary<int, List<BitMarkSixBet>>();
            for (int i = 0; i < groupCount; i++)
            {
                dic[i] = new List<BitMarkSixBet>();
            }
            int flag = 0;
            foreach (var item in order.BetItems.OrderByDescending(m => m.Amount))
            {
                dic[flag].Add(item);
                flag++;
                flag = flag % groupCount;
            }
            List<MarkCipherBetOrder> orders = new List<MarkCipherBetOrder>();
            foreach (var d in dic)
            {
                MarkCipherBetOrder rd = new MarkCipherBetOrder { Player = order.Player, PortHolder=order.PortHolder, Term = order.Term, ChannelRound = order.ChannelRound, BetItems = d.Value.ToArray() };
                orders.Add(rd);
            }
            return orders.ToArray();
        }
        //public static List<MarkPlainBetOrder>[] ShardForBanker(this MarkPlainBetOrder[] orders, int bankerNumber)
        //{
        //    Dictionary<int, List<MarkPlainBetOrder>> dic = new Dictionary<int, List<MarkPlainBetOrder>>();
        //    for (int i = 0; i < bankerNumber; i++)
        //    {
        //        dic[i] = new List<MarkPlainBetOrder>();
        //    }
        //    int flag = 0;
        //    foreach (var order in orders)
        //    {
        //        dic[flag].Add(order);
        //        flag++;
        //        flag = flag % bankerNumber;
        //    }
        //    return dic.Values.ToArray();
        //}
    }
}
