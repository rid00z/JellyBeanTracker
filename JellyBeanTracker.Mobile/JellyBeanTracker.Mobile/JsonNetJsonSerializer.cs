using System;
using Xamarin.Forms.Labs.Services.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace JellyBeanTracker.Mobile
{
    public class JsonNetJsonSerializer : IJsonSerializer
    {
        public JsonNetJsonSerializer()
        {
        }

        #region ISerializer implementation

        public void Flush()
        {

        }

        public SerializationFormat Format
        {
            get
            {
                return SerializationFormat.Json;
            }
        }

        #endregion

        #region IStringSerializer implementation

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject (obj);
        }

        public T Deserialize<T>(string data)
        {
            JsonSerializer _jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            return (T)_jsonSerializer.Deserialize(new StringReader(data), typeof(T));
        }

        #endregion

        #region IStreamSerializer implementation

        public void Serialize<T>(T obj, System.IO.Stream stream)
        {
            JsonSerializer _jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            _jsonSerializer.Serialize(new StreamWriter(stream), typeof(T));
        }

        public T Deserialize<T>(System.IO.Stream stream)
        {
            JsonSerializer _jsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            return (T)_jsonSerializer.Deserialize(new StreamReader(stream), typeof(T));
        }

        #endregion

        #region IByteSerializer implementation

        public byte[] SerializeToBytes<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(byte[] data)
        {
            return default(T);
        }

        #endregion
    }
}

