using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using NLog.Common;
using System.Threading.Tasks;

namespace DistributionWebApi.App_Start
{
    [Target("ELK")]
    public sealed class NLogELKTargetWithProxy : TargetWithLayout
    {
        [RequiredParameter]
        public string Host { get; set; }
        public NLogELKTargetWithProxy()
        {
            Host = System.Configuration.ConfigurationManager.AppSettings["elklogindex"];
        }
        protected async override void Write(LogEventInfo logEvent)
        {
            await SendTheMessageToRemoteHost(Host, logEvent.Message);
        }

        async Task SendTheMessageToRemoteHost(string host, string message)
        {
            string proxy = System.Configuration.ConfigurationManager.AppSettings["ProxyUri"];
            HttpClient client;
            HttpClientHandler httpClientHandler;
            if (!string.IsNullOrWhiteSpace(proxy))
            {
                httpClientHandler = new HttpClientHandler
                {
                    Proxy = new WebProxy(proxy, false),
                    UseProxy = true
                };
                client = new HttpClient(httpClientHandler);
            }
            else
            {
                client = new HttpClient();
            }
            
            StringContent json = new StringContent(message);
            await client.PostAsync(new Uri(host), json);

            json.Dispose();
            client.Dispose();
        }
    }
}