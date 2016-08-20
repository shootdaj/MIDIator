using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MIDIator
{
	public class TranslationConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var map = (TranslationMap)value;
			serializer.Serialize(writer, map);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var result = new TranslationMap();
			reader.Read();

			while (reader.TokenType == JsonToken.PropertyName)
			{
				var propertyName = (string)reader.Value;
				reader.Read();

				var value = propertyName == "" ? Convert.ToInt32(reader.Value) : serializer.Deserialize(reader);
				result.Add(propertyName, value);
				reader.Read();
			}

			return result;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(TranslationMap);
		}

		public override bool CanWrite => true;
	}
}
