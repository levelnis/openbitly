using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Hammock;
using Newtonsoft.Json;
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
                if (content.StartsWith("{\"errors\":["))
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


            var wantsCollection = typeof(IEnumerable).IsAssignableFrom(type);

            var deserialized = wantsCollection
                                   ? DeserializeCollection(content, type)
                                   : DeserializeSingle(content, type);

            return deserialized;
        }

        private object DeserializeSingle(string content, Type type)
        {
            var deserialized = DeserializeJson(content, type);
            if (typeof(IBitlyEntity).IsAssignableFrom(type))
            {
                ((IBitlyEntity)deserialized).RawSource = content;
            }

            return deserialized;
        }

        private object DeserializeCollection(string content, Type type)
        {
            if (type == typeof (byte[]))
            {
                var binary = (IEnumerable) Encoding.UTF8.GetBytes(content);
                var deserialized = binary;
                return deserialized;
            }

            IList collection;
            var collectionType = ConstructCollection(out collection, type);

            Type cursor = null;
            var generics = type.GetGenericArguments();
            if (generics.Length > 0)
            {
                var inner = generics[0];
                cursor = typeof (BitlyList<>).MakeGenericType(inner);
            }
            try
            {
                JArray array = null;
                JObject instance = null;

                instance = ParseInnerContent("trends", content, type, cursor, instance, ref array);
                instance = ParseInnerContent("users", content, type, cursor, instance, ref array);
                instance = ParseInnerContent("lists", content, type, cursor, instance, ref array);
                instance = ParseInnerContent("ids", content, type, cursor, instance, ref array);
                instance = ParseInnerContent("result", content, type, cursor, instance, ref array);

                if (array == null)
                {
                    array = JArray.Parse(content);
                }

                var model = typeof (IBitlyEntity).IsAssignableFrom(collectionType);
                var items = array.Select(item => item.ToString());
                if (model)
                {
                    foreach (var c in items)
                    {
                        AddDeserializedItem(c, collectionType, collection);
                    }
                }
                else
                {
                    foreach (var c in items)
                    {
                        AddDeserializedItemWithoutRawSource(c, collectionType, collection);
                    }
                }

                return collection;
            }
            catch (JsonReaderException readerException) // <-- Likely 502 
            {
                TraceException(readerException, collectionType, content);

                AddEmptyItem(content, collectionType, collection);

                var deserialized = collection;

                return deserialized;
            }
            catch (Exception ex) // <-- Likely entity mismatch (error)
            {
                TraceException(ex, collectionType, content);

                AddDeserializedItem(content, collectionType, collection);

                var deserialized = collection;

                return deserialized;
            }
        }

        private static JObject ParseInnerContent(string entity, string content, Type outer, Type cursor, JObject instance, ref JArray array)
        {
            if (!content.Contains(string.Format("\"{0}\"", entity)))
            {
                return instance;
            }
            instance = JObject.Parse(content);
            array = ParseFromCursorListOrObject(entity, content, outer, cursor, instance);
            return instance;
        }

        private static JArray ParseFromCursorListOrObject(string type, string content, Type outer, Type cursor, JObject instance)
        {
            JArray array;
            if (cursor != null && outer == cursor)
            {
                array = instance != null
                            ? ((JArray)instance[type] ?? JArray.Parse(content))
                            : JArray.Parse(content);
            }
            else
            {
                // [DC]: We need to go one level deeper than "result" in the case of places
                if (type.Equals("result"))
                {
                    instance = JObject.Parse(content);
                    var inner = instance["result"]["places"].ToString();
                    array = JArray.Parse(inner);
                }
                else
                {
                    array = JArray.Parse(content);
                }
            }
            return array;
        }

        private static void TraceException(Exception ex, Type type, string content)
        {
            Trace.TraceError(string.Concat("TweetSharp: Could not parse content into 'IEnumerable<", type.Name, ">' : '", content));
            Trace.TraceError(ex.Message);
            Trace.TraceError(ex.StackTrace);
        }

        private void AddDeserializedItem(string c, Type type, IList collection)
        {
            var d = Deserialize(c, type);
            ((IBitlyEntity)d).RawSource = c;
            collection.Add(d);
        }

        private void AddDeserializedItemWithoutRawSource(string c, Type type, IList collection)
        {
            var d = Deserialize(c, type);
            collection.Add(d);
        }

        private static void AddEmptyItem(string c, Type type, IList collection)
        {
            var d = Activator.CreateInstance(type);
            ((IBitlyEntity)d).RawSource = c;
            collection.Add(d);
        }

        private static Type ConstructCollection(out IList collection, Type type)
        {
            type = type.GetGenericArguments()[0];
            var collectionType = typeof(List<>).MakeGenericType(type);
            collection = (IList)Activator.CreateInstance(collectionType);
            return type;
        }
    }
}