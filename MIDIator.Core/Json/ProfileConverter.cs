using System;
using MIDIator.Engine;
using Newtonsoft.Json;

namespace MIDIator.Json
{
    public class ProfileConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Profile);
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return MIDIManager.Instance.ProfileService.CreateFromJSON(reader, serializer);
        }
        
	    public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
