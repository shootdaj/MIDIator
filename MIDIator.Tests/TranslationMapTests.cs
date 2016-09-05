using System.Linq;
using Anshul.Utilities;
using MIDIator.JsonConverters;
using Newtonsoft.Json;
using NUnit.Framework;
using Sanford.Multimedia.Midi;

namespace MIDIator.Tests
{
	public class TranslationMapTests
	{
		[Test]
		public void TranslationMap_Serialize()
		{
			var expectedSerialization = "{\r\n  \"$type\": \"MIDIator.TranslationMap, MIDIator\",\r\n  \"Translations\": {\r\n    \"$type\": \"System.Collections.Generic.List`1[[MIDIator.ITranslation, MIDIator]], mscorlib\",\r\n    \"$values\": [\r\n      {\r\n        \"$type\": \"MIDIator.Translation, MIDIator\",\r\n        \"InputMessageMatchTarget\": {\r\n          \"$type\": \"Sanford.Multimedia.Midi.ChannelMessage, Sanford.Multimedia.Midi\",\r\n          \"msg\": 193\r\n        },\r\n        \"OutputMessageTemplate\": {\r\n          \"$type\": \"Sanford.Multimedia.Midi.ChannelMessage, Sanford.Multimedia.Midi\",\r\n          \"msg\": 401\r\n        },\r\n        \"TranslationFunction\": \"PCToNote\",\r\n        \"InputMatchFunction\": \"Data1Match\"\r\n      }\r\n    ]\r\n  }\r\n}";

			var map = new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.ProgramChange, 1, 0),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 1), InputMatchFunction.Data1Match,
					TranslationFunction.PCToNote).Listify<ITranslation>());

			var serializedMap = JsonConvert.SerializeObject(map, SerializerSettings.Indented);

			Assert.AreEqual(expectedSerialization, serializedMap);
		}

		[Test]
		public void TranslationMap_Deserialize()
		{
			var serializedMap = "{\r\n  \"$type\": \"MIDIator.TranslationMap, MIDIator\",\r\n  \"Translations\": {\r\n    \"$type\": \"System.Collections.Generic.List`1[[MIDIator.ITranslation, MIDIator]], mscorlib\",\r\n    \"$values\": [\r\n      {\r\n        \"$type\": \"MIDIator.Translation, MIDIator\",\r\n        \"InputMessageMatchTarget\": {\r\n          \"$type\": \"Sanford.Multimedia.Midi.ChannelMessage, Sanford.Multimedia.Midi\",\r\n          \"msg\": 193\r\n        },\r\n        \"OutputMessageTemplate\": {\r\n          \"$type\": \"Sanford.Multimedia.Midi.ChannelMessage, Sanford.Multimedia.Midi\",\r\n          \"msg\": 401\r\n        },\r\n        \"TranslationFunction\": \"PCToNote\",\r\n        \"InputMatchFunction\": \"Data1Match\"\r\n      }\r\n    ]\r\n  }\r\n}";

			//left here for reference - this is the object used to create serializedTranslation by serializing it
			//var map = new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.ProgramChange, 1, 0),
			//		new ChannelMessage(ChannelCommand.NoteOn, 1, 1), InputMatchFunction.Data1Match,
			//		TranslationFunction.PCToNote).Listify<ITranslation>());

			var map = JsonConvert.DeserializeObject<TranslationMap>(serializedMap, SerializerSettings.Indented);

			Assert.AreEqual(map.Translations.Count, 1);
			Assert.AreEqual(map.Translations.First().InputMessageMatchTarget.Message, 193);
			Assert.AreEqual(map.Translations.First().OutputMessageTemplate.Message, 401);
			Assert.AreEqual(map.Translations.First().InputMatchFunction, InputMatchFunction.Data1Match);
			Assert.AreEqual(map.Translations.First().TranslationFunction, TranslationFunction.PCToNote);
		}
	}
}
