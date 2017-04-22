using System;
using System.Linq;
using Anshul.Utilities;
using MIDIator.Engine;
using MIDIator.Interfaces;
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
			    "{\r\n  \"translations\": [\r\n    {\r\n      \"name\": \"TestName\",\r\n      \"description\": \"This is a test translation\",\r\n      \"id\": \"b93497c9-a854-47a4-9c4e-a3a188bf0d97\",\r\n      \"inputMessageMatchTarget\": {\r\n        \"$type\": \"ChannelMessage\",\r\n        \"command\": \"NoteOn\",\r\n        \"midiChannel\": 1,\r\n        \"data1\": 1,\r\n        \"data2\": 0\r\n      },\r\n      \"outputMessageTemplate\": {\r\n        \"$type\": \"ChannelMessage\",\r\n        \"command\": \"ProgramChange\",\r\n        \"midiChannel\": 1,\r\n        \"data1\": 2,\r\n        \"data2\": 0\r\n      },\r\n      \"translationFunction\": \"DirectTranslation\",\r\n      \"inputMatchFunction\": \"CatchAll\",\r\n      \"enabled\": true,\r\n      \"collapsed\": false\r\n    }\r\n  ]\r\n}";

		    var map = new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1),
		        new ChannelMessage(ChannelCommand.ProgramChange, 1, 2),
		        InputMatchFunction.CatchAll, TranslationFunction.DirectTranslation, "TestName", "This is a test translation",
		        Guid.Parse("b93497c9-a854-47a4-9c4e-a3a188bf0d97")).Listify<ITranslation>());


            var serializedMap = JsonConvert.SerializeObject(map, SerializerSettings.DefaultSettings);

			Assert.AreEqual(expectedSerialization, serializedMap);
		}

		[Test]
		public void TranslationMap_Deserialize()
		{
			var serializedMap =
			    "{\r\n  \"translations\": [\r\n    {\r\n      \"name\": \"TestName\",\r\n      \"description\": \"This is a test translation\",\r\n      \"id\": \"b93497c9-a854-47a4-9c4e-a3a188bf0d97\",\r\n      \"inputMessageMatchTarget\": {\r\n        \"$type\": \"ChannelMessage\",\r\n        \"command\": \"NoteOn\",\r\n        \"midiChannel\": 1,\r\n        \"data1\": 1,\r\n        \"data2\": 0\r\n      },\r\n      \"outputMessageTemplate\": {\r\n        \"$type\": \"ChannelMessage\",\r\n        \"command\": \"ProgramChange\",\r\n        \"midiChannel\": 1,\r\n        \"data1\": 2,\r\n        \"data2\": 0\r\n      },\r\n      \"translationFunction\": \"DirectTranslation\",\r\n      \"inputMatchFunction\": \"CatchAll\",\r\n      \"enabled\": true,\r\n      \"collapsed\": false\r\n    }\r\n  ]\r\n}";

            //left here for reference - this is the object used to create serializedTranslation by serializing map
            //var map = new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1),
            //    new ChannelMessage(ChannelCommand.ProgramChange, 1, 2),
            //    InputMatchFunction.CatchAll, TranslationFunction.DirectTranslation, "TestName", "This is a test translation").Listify<ITranslation>());

            var map = JsonConvert.DeserializeObject<TranslationMap>(serializedMap, SerializerSettings.DefaultSettings);

			Assert.AreEqual(map.Translations.Count, 1);
			Assert.AreEqual(map.Translations.First().InputMessageMatchTarget.Message, 400);
			Assert.AreEqual(map.Translations.First().OutputMessageTemplate.Message, 704);
			Assert.AreEqual(map.Translations.First().InputMatchFunction, InputMatchFunction.CatchAll);
			Assert.AreEqual(map.Translations.First().TranslationFunction, TranslationFunction.DirectTranslation);
			Assert.AreEqual(map.Translations.First().Name, "TestName");
			Assert.AreEqual(map.Translations.First().Description, "This is a test translation");
			Assert.AreEqual(map.Translations.First().ID, Guid.Parse("b93497c9-a854-47a4-9c4e-a3a188bf0d97"));
        }
	}
}
