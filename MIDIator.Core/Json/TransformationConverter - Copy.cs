using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using MIDIator.Engine;
using MIDIator.Interfaces;
using MIDIator.UIGenerator.Consumables;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MIDIator.Json
{
    public class TransformationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Transformation));
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var translationMap = jo["translationMap"].ToObject<TranslationMap>(serializer);
            var transformation = new Transformation(jo["name"].ToString(), 
                jo["inputDevice"]["name"].ToString(), 
                jo["outputDevice"]["name"].ToString(), 
                translationMap,
                jo["linkedOutputVirtualDevice"].ToObject<bool>(serializer),
                MIDIManager.Instance.MIDIDeviceService, 
                MIDIManager.Instance.VirtualMIDIManager);

            return transformation;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
