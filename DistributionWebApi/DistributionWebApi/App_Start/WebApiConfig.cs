using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Converters;
using System.Web;
using System.Text;
using System.Xml.Serialization;
using NLog.Targets;

namespace DistributionWebApi
{
    public static class WebApiConfig
    {
        public class CustomJsonFormatter : JsonMediaTypeFormatter
        {
            public CustomJsonFormatter()
            {
                this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            }

            public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
        }

        public class CsvMediaTypeFormatter : MediaTypeFormatter
        {
            public CsvMediaTypeFormatter()
            {
                this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
                //SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                //SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
            }

            public CsvMediaTypeFormatter(MediaTypeMapping mediaTypeMapping) : this()
            {
                MediaTypeMappings.Add(mediaTypeMapping);
            }

            public CsvMediaTypeFormatter(IEnumerable<MediaTypeMapping> mediaTypeMappings) : this()
            {
                foreach (var mediaTypeMapping in mediaTypeMappings)
                {
                    MediaTypeMappings.Add(mediaTypeMapping);
                }
            }

            public override bool CanWriteType(Type type)
            {
                if (type == null)
                    throw new ArgumentNullException("type");

                return isTypeOfIEnumerable(type);
            }

            private bool isTypeOfIEnumerable(Type type)
            {
                foreach (Type interfaceType in type.GetInterfaces())
                {
                    if (interfaceType == typeof(IEnumerable))
                        return true;
                }

                return false;
            }

            public override bool CanReadType(Type type)
            {
                return false;
            }

            public override void SetDefaultContentHeaders
            (Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.Add("Content-Disposition", "attachment; filename = " + "TLGX_API_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".csv");

            }

            public override Task WriteToStreamAsync(Type type, object value,
            Stream stream, HttpContent content, TransportContext transportContext)
            {
                writeStream(type, value, stream, content);
                var tcs = new TaskCompletionSource<int>();
                tcs.SetResult(0);
                return tcs.Task;
            }

            private void writeStream(Type type, object value, Stream stream, HttpContent contentHeaders)
            {
                //NOTE: We have check the type inside CanWriteType method
                //If request comes this far, the type is IEnumerable. We are safe.

                Type itemType = type.GetGenericArguments()[0];

                StringWriter _stringWriter = new StringWriter();

                _stringWriter.WriteLine(
                    string.Join<string>(
                        ",", itemType.GetProperties().Select(x => x.Name)
                    )
                );

                foreach (var obj in (IEnumerable<object>)value)
                {

                    var vals = obj.GetType().GetProperties().Select(
                        pi => new
                        {
                            Value = pi.GetValue(obj, null)
                        }
                    );

                    string _valueLine = string.Empty;

                    foreach (var val in vals)
                    {
                        if (val.Value != null)
                        {
                            var _val = val.Value.ToString();

                            //Check if the value contains a comma and place it in quotes if so
                            if (_val.Contains(","))
                                _val = string.Concat("\"", _val, "\"");

                            //Replace any \r or \n special characters from a new line with a space
                            if (_val.Contains("\r"))
                                _val = _val.Replace("\r", " ");
                            if (_val.Contains("\n"))
                                _val = _val.Replace("\n", " ");

                            _valueLine = string.Concat(_valueLine, _val, ",");
                        }
                        else
                        {
                            _valueLine = string.Concat(string.Empty, ",");
                        }
                    }

                    _stringWriter.WriteLine(_valueLine.TrimEnd(','));
                }

                //var streamWriter = new StreamWriter(stream, effectiveEncoding));
                var streamWriter = new StreamWriter(stream);
                streamWriter.Write(_stringWriter.ToString());
                streamWriter.Flush();
                streamWriter.Close();
            }

        }

        public class JsonpFormatter : JsonMediaTypeFormatter
        {
            public JsonpFormatter()
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));

                JsonpParameterName = "callback";
            }

            /// <summary>
            ///  Name of the query string parameter to look for
            ///  the jsonp function name
            /// </summary>
            public string JsonpParameterName { get; set; }

            /// <summary>
            /// Captured name of the Jsonp function that the JSON call
            /// is wrapped in. Set in GetPerRequestFormatter Instance
            /// </summary>
            private string JsonpCallbackFunction;
            
            public override bool CanWriteType(Type type)
            {
                return true;
            }

            /// <summary>
            /// Override this method to capture the Request object
            /// </summary>
            /// <param name="type"></param>
            /// <param name="request"></param>
            /// <param name="mediaType"></param>
            /// <returns></returns>
            public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, System.Net.Http.HttpRequestMessage request, MediaTypeHeaderValue mediaType)
            {
                var formatter = new JsonpFormatter()
                {
                    JsonpCallbackFunction = GetJsonCallbackFunction(request)
                };

                // this doesn't work unfortunately
                //formatter.SerializerSettings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;

                // You have to reapply any JSON.NET default serializer Customizations here    
                formatter.SerializerSettings.Converters.Add(new StringEnumConverter());
                formatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

                return formatter;
            }

            public override Task WriteToStreamAsync(Type type, object value,
                                            Stream stream,
                                            HttpContent content,
                                            TransportContext transportContext)
            {
                if (string.IsNullOrEmpty(JsonpCallbackFunction))
                    return base.WriteToStreamAsync(type, value, stream, content, transportContext);

                StreamWriter writer = null;

                // write the pre-amble
                try
                {
                    writer = new StreamWriter(stream);
                    writer.Write(JsonpCallbackFunction + "(");
                    writer.Flush();
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (writer != null)
                            writer.Dispose();
                    }
                    catch { }

                    var tcs = new TaskCompletionSource<object>();
                    tcs.SetException(ex);
                    return tcs.Task;
                }

                return base.WriteToStreamAsync(type, value, stream, content, transportContext)
                           .ContinueWith(innerTask =>
                           {
                               if (innerTask.Status == TaskStatus.RanToCompletion)
                               {
                                   writer.Write(")");
                                   writer.Flush();
                               }

                           }, TaskContinuationOptions.ExecuteSynchronously)
                            .ContinueWith(innerTask =>
                            {
                                writer.Dispose();
                                return innerTask;

                            }, TaskContinuationOptions.ExecuteSynchronously)
                            .Unwrap();
            }

            /// <summary>
            /// Retrieves the Jsonp Callback function
            /// from the query string
            /// </summary>
            /// <returns></returns>
            private string GetJsonCallbackFunction(HttpRequestMessage request)
            {
                if (request.Method != HttpMethod.Get)
                    return null;

                var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
                var queryVal = query[this.JsonpParameterName];

                if (string.IsNullOrEmpty(queryVal))
                    return null;

                return queryVal;
            }

        }

        public class JsonDownLoadFormatter : JsonMediaTypeFormatter
        {
            public JsonDownLoadFormatter()
            {
                this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                //SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                //SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
            }

            public JsonDownLoadFormatter(MediaTypeMapping mediaTypeMapping) : this()
            {
                MediaTypeMappings.Add(mediaTypeMapping);
            }

            public JsonDownLoadFormatter(IEnumerable<MediaTypeMapping> mediaTypeMappings) : this()
            {
                foreach (var mediaTypeMapping in mediaTypeMappings)
                {
                    MediaTypeMappings.Add(mediaTypeMapping);
                }
            }

            public override bool CanWriteType(Type type)
            {
                if (type == null)
                    throw new ArgumentNullException("type");

                return isTypeOfIEnumerable(type);
            }

            private bool isTypeOfIEnumerable(Type type)
            {
                foreach (Type interfaceType in type.GetInterfaces())
                {
                    if (interfaceType == typeof(IEnumerable))
                        return true;
                }

                return false;
            }

            public override bool CanReadType(Type type)
            {
                return false;
            }

            public override void SetDefaultContentHeaders
            (Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.Add("Content-Disposition", "attachment; filename = " + "TLGX_API_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".json");

            }

            public override Task WriteToStreamAsync(Type type, object value,
            Stream stream, HttpContent content, TransportContext transportContext)
            {
                writeStream(type, value, stream, content);
                var tcs = new TaskCompletionSource<int>();
                tcs.SetResult(0);
                return tcs.Task;
            }

            private void writeStream(Type type, object value, Stream stream, HttpContent contentHeaders)
            {
                //NOTE: We have check the type inside CanWriteType method
                //If request comes this far, the type is IEnumerable. We are safe.

                StringWriter _stringWriter = new StringWriter();
                _stringWriter.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(value));

                //var streamWriter = new StreamWriter(stream, effectiveEncoding));
                var streamWriter = new StreamWriter(stream);
                streamWriter.Write(_stringWriter.ToString());
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        public class XmlDownLoadFormatter : XmlMediaTypeFormatter
        {
            public XmlDownLoadFormatter()
            {
                this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
                //SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                //SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
            }

            public XmlDownLoadFormatter(MediaTypeMapping mediaTypeMapping) : this()
            {
                MediaTypeMappings.Add(mediaTypeMapping);
            }

            public XmlDownLoadFormatter(IEnumerable<MediaTypeMapping> mediaTypeMappings) : this()
            {
                foreach (var mediaTypeMapping in mediaTypeMappings)
                {
                    MediaTypeMappings.Add(mediaTypeMapping);
                }
            }

            public override bool CanWriteType(Type type)
            {
                if (type == null)
                    throw new ArgumentNullException("type");

                return isTypeOfIEnumerable(type);
            }

            private bool isTypeOfIEnumerable(Type type)
            {
                foreach (Type interfaceType in type.GetInterfaces())
                {
                    if (interfaceType == typeof(IEnumerable))
                        return true;
                }

                return false;
            }

            public override bool CanReadType(Type type)
            {
                return false;
            }

            public override void SetDefaultContentHeaders
            (Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.Add("Content-Disposition", "attachment; filename = " + "TLGX_API_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xml");

            }

            public override Task WriteToStreamAsync(Type type, object value,
            Stream stream, HttpContent content, TransportContext transportContext)
            {
                writeStream(type, value, stream, content);
                var tcs = new TaskCompletionSource<int>();
                tcs.SetResult(0);
                return tcs.Task;
            }

            private void writeStream(Type type, object value, Stream stream, HttpContent contentHeaders)
            {
                //NOTE: We have check the type inside CanWriteType method
                //If request comes this far, the type is IEnumerable. We are safe.
                StringWriter _stringWriter = new StringWriter();

                var serializer = new XmlSerializer(type);
                serializer.Serialize(_stringWriter, value);
                
                //var streamWriter = new StreamWriter(stream, effectiveEncoding));
                var streamWriter = new StreamWriter(stream);
                streamWriter.Write(_stringWriter.ToString());
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new LoggingFilterAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "TlgxDistributionDefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "TlgxDistributionActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Add(new CustomJsonFormatter());
            config.Formatters.Add(new JsonpFormatter());

            //Preety RAW Json response
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            //Key Names are camel cased
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //Remove Xml Formatter (Response will only be in Json format)
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Remove Json Formatter (Response will only be in Xml format)
            //config.Formatters.Remove(config.Formatters.JsonFormatter);

            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            config.Formatters.XmlFormatter.UseXmlSerializer = true;

            Target.Register("ELK", typeof(DistributionWebApi.App_Start.NLogELKTargetWithProxy));
        }
    }
}
