using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Hammock;
using Hammock.Serialization;
using Hammock.Web;
using OpenBitly.Serialization;

namespace OpenBitly
{
    public partial class BitlyService
    {
        private string consumerKey;
        private string consumerSecret;
        private string token;
        private string tokenSecret;
        private string username;
        private string password;
        private readonly BitlyClientInfo info;
        private readonly RestClient client;
        private readonly JsonSerializer json;

        internal string FormatAsString { get; private set; }

        public bool TraceEnabled { get; set; }
        public virtual BitlyResponse Response { get; private set; }

        public BitlyService(string consumerKey, string consumerSecret)
            : this()
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }

        public BitlyService(string consumerKey, string consumerSecret, string username, string password)
            : this()
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.username = username;
            this.password = password;
        }

        public BitlyService()
        {
            json = new JsonSerializer();
            FormatAsString = "";

            oauth = new RestClient
            {
                Authority = Globals.Authority,
                UserAgent = "OpenBitly",
                DecompressionMethods = DecompressionMethods.GZip,
            };

            client = new RestClient
            {
                Authority = Globals.Authority,
                QueryHandling = QueryHandling.AppendToParameters,
                VersionPath = "v3",
                Serializer = json,
                Deserializer = json,
                DecompressionMethods = DecompressionMethods.GZip,
                UserAgent = "OpenBitly",
                FollowRedirects = true,
            };
        }

        private readonly Func<RestRequest> noAuthQuery
            = () =>
            {
                var request = new RestRequest();
                return request;
            };

        private RestRequest PrepareHammockQuery(string path)
        {
            RestRequest request;
            if (string.IsNullOrEmpty(token))
            {
                request = noAuthQuery.Invoke();
            }
            else
            {
                var args = new FunctionArguments
                {
                    ConsumerKey = consumerKey,
                    ConsumerSecret = consumerSecret,
                    Token = token,
                    Username = username,
                    Password = password
                };
                request = protectedResourceQuery.Invoke(args);
            }
            request.Path = path;

            request.TraceEnabled = TraceEnabled;
            return request;
        }

        private string ResolveUrlSegments(string path, List<object> segments)
        {
            if (segments == null) throw new ArgumentNullException("segments");

            var cleansed = new List<object>();
            for (var i = 0; i < segments.Count; i++)
            {
                if (i == 0)
                {
                    cleansed.Add(segments[i]);
                }
                if (i > 0 && i % 2 == 0)
                {
                    var key = segments[i - 1];
                    var value = segments[i];
                    if (value != null)
                    {
                        if (cleansed.Count == 1 && key is string)
                        {
                            var keyString = key.ToString();
                            if (keyString.StartsWith("&"))
                            {
                                key = "?" + keyString.Substring(1);
                            }
                        }
                        cleansed.Add(key);
                        cleansed.Add(value);
                    }
                }
            }
            segments = cleansed;

            for (var i = 0; i < segments.Count; i++)
            {
                if (segments[i] is DateTime)
                {
                    segments[i] = ((DateTime)segments[i]).ToString("yyyy-MM-dd");
                }

                if (segments[i] is bool)
                {
                    segments[i] = ((bool)segments[i]).ToString().ToLowerInvariant();
                }

                if (segments[i] is double)
                {
                    segments[i] = ((double)segments[i]).ToString(CultureInfo.InvariantCulture);
                }

                if (segments[i] is decimal)
                {
                    segments[i] = ((decimal)segments[i]).ToString(CultureInfo.InvariantCulture);
                }

                if (segments[i] is float)
                {
                    segments[i] = ((float)segments[i]).ToString(CultureInfo.InvariantCulture);
                }

                if (segments[i] is IEnumerable && !(segments[i] is string))
                {
                    ResolveEnumerableUrlSegments(segments, i);
                }
            }

            path = PathHelpers.ReplaceUriTemplateTokens(segments, path);

            PathHelpers.EscapeDataContainingUrlSegments(segments);

            segments.Insert(0, path);

            return string.Concat(segments.ToArray()).ToString(CultureInfo.InvariantCulture);
        }

        private static void ResolveEnumerableUrlSegments(IList<object> segments, int i)
        {
            // [DC] Enumerable segments will be typed, but we only care about string values
            var collection = (from object item in (IEnumerable)segments[i] select item.ToString()).ToList();
            var total = collection.Count();
            var sb = new StringBuilder();
            var count = 0;
            foreach (var item in collection)
            {
                sb.Append(item);
                if (count < total - 1)
                {
                    sb.Append(",");
                }
                count++;
            }
            segments[i] = sb.ToString();
        }

        private T WithHammock<T>(string path)
        {
            var request = PrepareHammockQuery(path);

            return WithHammockImpl<T>(request);
        }

        private T WithHammock<T>(string path, params object[] segments)
        {
            var url = ResolveUrlSegments(path, segments.ToList());
            return WithHammock<T>(url);
        }

        private T WithHammock<T>(WebMethod method, string path)
        {
            var request = PrepareHammockQuery(path);
            request.Method = method;

            return WithHammockImpl<T>(request);
        }

        private T WithHammock<T>(WebMethod method, string path, params object[] segments)
        {
            var url = ResolveUrlSegments(path, segments.ToList());

            return WithHammock<T>(method, url);
        }

        private T WithHammockImpl<T>(RestRequest request)
        {
            var response = client.Request<T>(request);

            SetResponse(response);

            var entity = client.Deserializer.Deserialize<T>(response);
            return entity;
        }

        private void SetResponse(RestResponseBase response)
        {
            Response = new BitlyResponse(response);
        }
    }
}