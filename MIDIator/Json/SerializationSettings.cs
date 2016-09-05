using Newtonsoft.Json;

namespace MIDIator.Json
{
	public static class SerializerSettings
	{
		public static JsonSerializerSettings DefaultSettings => new JsonSerializerSettings()
		{
			TypeNameHandling = TypeNameHandling.None,
			Binder = new DisplayNameSerializationBinder(),
			Formatting = Formatting.Indented,
			
		};
	}
}
