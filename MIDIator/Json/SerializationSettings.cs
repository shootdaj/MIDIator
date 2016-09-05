using Newtonsoft.Json;

namespace MIDIator.JsonConverters
{
	public static class SerializerSettings
	{
		public static JsonSerializerSettings DefaultSettings => new JsonSerializerSettings()
		{
			TypeNameHandling = TypeNameHandling.All
		};

		public static JsonSerializerSettings Indented
		{
			get
			{
				var settings = DefaultSettings;
				settings.Formatting = Formatting.Indented;
				return settings;
			}
		}
	}
}
