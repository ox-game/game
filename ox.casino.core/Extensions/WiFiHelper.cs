using OX.Ledger;
using OX.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OX.IO;
using OX.Cryptography.ECC;
using System.Security.Policy;
using System.IO;
using OX.Network.P2P;
using System.Runtime;
using System.Reflection;
using System.Diagnostics;

namespace OX.Casino
{
    public struct WiFiInfo
    {
        public string SSID { get; set; }
        public string Password { get; set; }
    }

    public static class WiFiHelper
    {
        private static string RunNetsh(string args)
        {
            Process processWifi = new();
            processWifi.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processWifi.StartInfo.FileName = "netsh";
            processWifi.StartInfo.Arguments = args;

            processWifi.StartInfo.UseShellExecute = false;
            processWifi.StartInfo.RedirectStandardError = true;
            processWifi.StartInfo.RedirectStandardInput = true;
            processWifi.StartInfo.RedirectStandardOutput = true;
            processWifi.StartInfo.CreateNoWindow = true;
            processWifi.Start();
            string output = processWifi.StandardOutput.ReadToEnd();
            string _ = processWifi.StandardError.ReadToEnd();

            processWifi.WaitForExit();
            return output;
        }
        private static string GetWifiNetworks() =>
            RunNetsh("wlan show profiles");
        private static string GetConnectedWifiNetworks() =>
          RunNetsh("wlan show interfaces");
        private static string ReadPassword(string wifiName) =>
            RunNetsh($"wlan show profile name=\"{wifiName}\" key=clear");

        public static string GetWifiPassword(string wifiName, bool IsChina)
        {
            var password = ReadPassword(wifiName);

            using var reader = new StringReader(password);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                Regex regex = default;
                if (IsChina)
                {
                    regex = new(@"关键内容 * : (?<after>.*)");
                }
                else
                {
                    regex = new(@"Key Content * : (?<after>.*)");
                }
                Match match = regex.Match(line);

                if (match.Success)
                {
                    var currentPassword = match.Groups["after"].Value;
                    return currentPassword;
                }
            }
            return "unknown characters";
        }

        public static List<WiFiInfo> GetWiFiInfos(bool IsChina)
        {
            List<WiFiInfo> wifiInfos = [];
            string wifiNetworks = GetWifiNetworks();

            using var reader = new StringReader(wifiNetworks);

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                Regex regex = default;
                if (IsChina)
                {
                    regex = new(@"所有用户配置文件 * : (?<after>.*)");
                }
                else
                {
                    regex = new(@"All User Profile * : (?<after>.*)");
                }
                Match match = regex.Match(line);

                if (match.Success)
                {
                    string wifiName = match.Groups["after"].Value;
                    string currentPassword = GetWifiPassword(wifiName, IsChina);

                    wifiInfos.Add(new WiFiInfo { SSID = wifiName, Password = currentPassword });
                }

            }
            return wifiInfos;
        }
        public static string GetConnectedWifi(bool IsChina)
        {
            string wifiNetworks = GetConnectedWifiNetworks();

            using var reader = new StringReader(wifiNetworks);

            string line;

            bool Connected = false;
            string ssid = string.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                Regex regex = default;
                Regex regexssid = default;
                if (IsChina)
                {
                    regex = new(@" 状态 * : 已连接");
                    regexssid = new(@" 配置文件 * : (?<after>.*)");
                }
                else
                {
                    regex = new(@" State * : connected");
                    regexssid = new(@" Profile * : (?<after>.*)");
                }
                Match match = regex.Match(line);
                if (match.Success)
                    Connected = match.Success;
                Match matchssid = regexssid.Match(line);
                if (matchssid.Success)
                {
                    ssid = matchssid.Groups["after"].Value;
                }
            }
            if (Connected) return ssid.Trim();
            return string.Empty;
        }
        public static string GetQRString(string ssid, string pwd)
        {
            return $"WIFI:T:WPA;S:{ssid};P:{pwd};;";
        }
    }
}
