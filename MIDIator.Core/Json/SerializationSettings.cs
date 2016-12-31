using System.Collections.Generic;
using MIDIator.Engine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MIDIator.Json
{
	public static class SerializerSettings
	{
		public static JsonSerializerSettings DefaultSettings => new JsonSerializerSettings()
		{
			TypeNameHandling = TypeNameHandling.None,
			Binder = new DisplayNameSerializationBinder(),
			Formatting = Formatting.Indented,
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = new List<JsonConverter> { new CamelCaseToPascalCaseExpandoObjectConverter(), new TransformationConverter() }
		};
	}
}
