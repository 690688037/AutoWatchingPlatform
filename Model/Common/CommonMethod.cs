using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace AutoWatchingPlatform.Model
{
    public class CommonMethod
    {
        //常量
        public const int quicklyIntervalTime = 1000;//快速间隔时间（1s）
        public const int resendIntervalTime = 10000;//重发间隔时间（10s）

        //判断是否存在中文
        public static bool HashChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        //Http请求基类
        public static string getData(string url, string contentType = "application/json")
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = contentType; //"application/json";
                request.Method = "GET";

                var response = (HttpWebResponse)request.GetResponse();

                string result = string.Empty;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                return result;
            }
            catch
            {
                return null;
            }
        }
        public static string postData(string url, string param_json)
        {
            try
            {
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent strcontent = new StringContent(param_json, Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync(url, strcontent).Result.Content.ReadAsStringAsync().Result;

                return result;
            }
            catch
            {
                return null;
            }
        }

        //获取ip地址
        public static string IPV4()
        {
            string ipv4 = GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            // 如果不是无线网卡，则获取有线网卡的地址
            if (ipv4 == "")
            {
                ipv4 = GetLocalIPv4(NetworkInterfaceType.Ethernet);
                // 如果有线网卡也没有获取到数据，则使用最开始可能包含虚拟网卡的方法来获取IP
                if (ipv4 == "")
                {
                    ipv4 = GetLoacalIPMaybeVirtualNetwork();
                }
            }
            return ipv4;
        }
        public static string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                // 网络类型是所规定的并且网络再运行状态
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
        public static string GetLoacalIPMaybeVirtualNetwork()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return null;
        }
    }
}
