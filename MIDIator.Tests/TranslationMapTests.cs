using System.Linq;
using Anshul.Utilities;
using MIDIator.Json;
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
			var expectedSerialization =
				"{\r\n  \"Translations\": [\r\n    {\r\n      \"InputMessageMatchTarget\": {\r\n        \"$type\": \"ChannelMessage\",\r\n        \"Command\": 192,\r\n        \"MidiChannel\": 1,\r\n        \"Data1\": 0,\r\n        \"Data2\": 0\r\n      },\r\n      \"OutputMessageTemplate\": {\r\n        \"$type\": \"ChannelMessage\",\r\n        \"Command\": 144,\r\n        \"MidiChannel\": 1,\r\n        \"Data1\": 1,\r\n        \"Data2\": 0\r\n      },\r\n      \"TranslationFunction\": \"PCToNote\",\r\n      \"InputMatchFunction\": \"Data1Match\"\r\n    }\r\n  ]\r\n}";

			var map = new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.ProgramChange, 1, 0),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 1), InputMatchFunction.Data1Match,
					TranslationFunction.PCToNote).Listify<ITranslation>());

			var serializedMap = JsonConvert.SerializeObject(map, SerializerSettings.DefaultSettings);

			Assert.AreEqual(expectedSerialization, serializedMap);
		}

		[Test]
		public void TranslationMap_Deserialize()
		{
			var serializedMap =
				"{\r\n  \"Translations\": [\r\n    {\r\n      \"InputMessageMatchTarget\": {\r\n        \"$type\": \"ChannelMessage\",\r\n        \"Command\": 192,\r\n        \"MidiChannel\": 1,\r\n        \"Data1\": 0,\r\n        \"Data2\": 0\r\n      },\r\n      \"OutputMessageTemplate\": {\r\n        \"$type\": \"ChannelMessage\",\r\n        \"Command\": 144,\r\n        \"MidiChannel\": 1,\r\n        \"Data1\": 1,\r\n        \"Data2\": 0\r\n      },\r\n      \"TranslationFunction\": \"PCToNote\",\r\n      \"InputMatchFunction\": \"Data1Match\"\r\n    }\r\n  ]\r\n}";

			//left here for reference - this is the object used to create serializedTranslation by serializing map
			//var map = new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.ProgramChange, 1, 0),
			//		new ChannelMessage(ChannelCommand.NoteOn, 1, 1), InputMatchFunction.Data1Match,
			//		TranslationFunction.PCToNote).Listify<ITranslation>());

			var map = JsonConvert.DeserializeObject<TranslationMap>(serializedMap, SerializerSettings.DefaultSettings);

			Assert.AreEqual(map.Translations.Count, 1);
			Assert.AreEqual(map.Translations.First().InputMessageMatchTarget.Message, 193);
			Assert.AreEqual(map.Translations.First().OutputMessageTemplate.Message, 401);
			Assert.AreEqual(map.Translations.First().InputMatchFunction, InputMatchFunction.Data1Match);
			Assert.AreEqual(map.Translations.First().TranslationFunction, TranslationFunction.PCToNote);
		}
	}
}
