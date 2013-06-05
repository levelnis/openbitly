using System;
using Hammock;
using Newtonsoft.Json.Linq;

namespace OpenBitly.Serialization
{
    public class JsonSerializer : SerializerBase
    {
        public override string Serialize(object instance, Type type)
        {
            return SerializeJson(instance, type);
        }

        public override string ContentType
        {
            get { return "application/json"; }
        }

        public override object Deserialize(RestResponseBase response, Type type)
        {
            return DeserializeJson(response.Content, type);
        }

        public object Deserialize(string content, Type type)
        {
            return DeserializeJson(content, type);
        }

        public override T Deserialize<T>(RestResponseBase response)
        {
            if (response == null)
            {
                return default(T);
            }

            if ((int)response.StatusCode >= 500)
            {
                return default(T);
            }

            var content = response.Content;
            return (T)DeserializeContent(content, typeof(T));
        }

        public override object DeserializeJson(string content, Type type)
        {
            return type == typeof(BitlyError) ? DeserializeContent(content, type) : base.DeserializeJson(content, type);
        }

        public override T DeserializeJson<T>(string content)
        {
            return (T)DeserializeContent(content, typeof(T));
        }

        internal object DeserializeContent(string content, Type type)
        {
            if (string.IsNullOrEmpty(content) || content.Trim().Length == 0)
            {
                return null;
            }

            if (type == typeof (BitlyError))
            {
                // {"errors":[{"message":"Bad Authentication data","code":215}]}
                content = content.Trim('\n');
                if (content.StartsWith("{\"status_code\": 50"))
                {
                    var errors = (JArray)JObject.Parse(content)["errors"];
                    if (errors != null)
                    {
                        var result = new BitlyError { RawSource = content };
                        var error = errors.First;
                        result.Message = error["message"].ToString();
                        result.Code = int.Parse(error["code"].ToString());
                        return result;
                    }
                }
                else
                {
                    var unknown = new BitlyError { RawSource = content };
                    return unknown;
                }
            }

            var deserialized = DeserializeJson(content, type);
            if (typeof(IBitlyEntity).IsAssignableFrom(type))
            {
                ((IBitlyEntity)deserialized).RawSource = content;
            }

            return deserialized;
        }
    }
}