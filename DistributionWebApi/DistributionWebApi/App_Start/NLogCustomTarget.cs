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
    /// <summary>
    /// This is a custom NLogger class which logs the message to a network address.
    /// </summary>
    [Target("ELK")]
    public sealed class NLogELKTargetWithProxy : TargetWithLayout
    {
        /// <summary>
        /// Target URL
        /// </summary>
        [RequiredParameter]
        public string Host { get; set; }
        
        /// <summary>
        /// Constructor ElkTargetNlog
        /// </summary>
        public NLogELKTargetWithProxy()
        {
            Host = System.Configuration.ConfigurationManager.AppSettings["elklogindex"];
        }

        /// <summary>
        /// write method to send the message to network
        /// </summary>
        /// <param name="logEvent"></param>
        protected async override void Write(LogEventInfo logEvent)
        {
            await SendTheMessageToRemoteHost(Host, logEvent.Message);
        }

        async Task SendTheMessageToRemoteHost(string host, string message)
        {
            try
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
                await client.PutAsync(new Uri(host), json);

                json.Dispose();
                client.Dispose();
            }
            catch (Exception ex)
            {
                NLogHelper.Nlogger_LogError.LogError(ex, this.GetType().FullName, "ELK - TraceLog", host);
                NLogHelper.Nlogger_LogTrace.LogTrace(message);
            }
        }
    }
}