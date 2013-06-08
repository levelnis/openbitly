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

            var deserialized = DeserializeJson(content, type);
            if (typeof(IBitlyEntity).IsAssignableFrom(type))
            {
                ((IBitlyEntity)deserialized).RawSource = content;
            }

            return deserialized;
        }
    }
}