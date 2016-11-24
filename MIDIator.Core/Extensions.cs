using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using MIDIator.Engine;
using MIDIator.Json;
using Newtonsoft.Json;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class Extensions
	{
		public static ChannelMessage ToChannelMessage(this ShortMessage shortMessage)
		{
			return (ChannelMessage) shortMessage;
		}

		public static T ConvertTo<T>(this ExpandoObject inObject)
		{
			var serializedTranslationMap = JsonConvert.SerializeObject(inObject);
			var deserializedObject = JsonConvert.DeserializeObject<T>(serializedTranslationMap, SerializerSettings.DefaultSettings);
			return deserializedObject;
		}
	}
}
