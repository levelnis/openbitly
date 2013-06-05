using System;
using System.Collections.Generic;
using System.IO;
using Hammock;
using Hammock.Serialization;
using Newtonsoft.Json;
using NewtonsoftSerializer = Newtonsoft.Json.JsonSerializer;

namespace OpenBitly.Serialization
{
    public abstract class SerializerBase : Utf8Serializer, ISerializer, IDeserializer
    {
        private readonly NewtonsoftSerializer serializer;

        protected SerializerBase()
            : this(new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                ContractResolver = new JsonConventionResolver(),
                Converters = new List<JsonConverter>()
            })
        {
            
        }

        protected SerializerBase(JsonSerializerSettings settings)
        {
            serializer = new NewtonsoftSerializer
            {
                ConstructorHandling = settings.ConstructorHandling,
                ContractResolver = settings.ContractResolver,
                ObjectCreationHandling = settings.ObjectCreationHandling,
                MissingMemberHandling = settings.MissingMemberHandling,
                DefaultValueHandling = settings.DefaultValueHandling,
                NullValueHandling = settings.NullValueHandling
            };

            foreach (var converter in settings.Converters)
            {
                serializer.Converters.Add(converter);
            }
        }

        public virtual object DeserializeJson(string content, Type type)
        {
            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return serializer.Deserialize(jsonTextReader, type);
                }
            }
        }

        public virtual T DeserializeJson<T>(string content)
        {
            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        public virtual string SerializeJson(object instance, Type type)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    serializer.Serialize(jsonTextWriter, instance);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }

        public abstract string Serialize(object instance, Type type);

        public abstract string ContentType { get; }

        public abstract object Deserialize(RestResponseBase response, Type type);

        public abstract T Deserialize<T>(RestResponseBase response);

        public dynamic DeserializeDynamic(RestResponseBase response)
        {
            throw new NotSupportedException();
        }
    }
}