using MIDIator.Engine;
using MIDIator.Json;
using Newtonsoft.Json;
using NUnit.Framework;
using Sanford.Multimedia.Midi;

namespace MIDIator.Tests
{
	public class TranslationTests
	{
        [Test]
        public void Translation_Serialize()
        {
	        var expectedSerialization =
				"{\r\n  \"inputMessageMatchTarget\": {\r\n    \"$type\": \"ChannelMessage\",\r\n    \"command\": \"NoteOn\",\r\n    \"midiChannel\": 1,\r\n    \"data1\": 1,\r\n    \"data2\": 0\r\n  },\r\n  \"outputMessageTemplate\": {\r\n    \"$type\": \"ChannelMessage\",\r\n    \"command\": \"ProgramChange\",\r\n    \"midiChannel\": 1,\r\n    \"data1\": 2,\r\n    \"data2\": 0\r\n  },\r\n  \"translationFunction\": \"DirectTranslation\",\r\n  \"inputMatchFunction\": \"CatchAll\",\r\n  \"enabled\": true\r\n}";

            var translation = new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1),
                new ChannelMessage(ChannelCommand.ProgramChange, 1, 2),
                InputMatchFunction.CatchAll, TranslationFunction.DirectTranslation);

            var serializedTranslation = JsonConvert.SerializeObject(translation, SerializerSettings.DefaultSettings);
            
            Assert.AreEqual(expectedSerialization, serializedTranslation);
        }

		[Test]
		public void Translation_Deserialize()
		{
			var	serializedTranslation =
				"{\r\n  \"inputMessageMatchTarget\": {\r\n    \"$type\": \"ChannelMessage\",\r\n    \"command\": \"NoteOn\",\r\n    \"midiChannel\": 1,\r\n    \"data1\": 1,\r\n    \"data2\": 0\r\n  },\r\n  \"outputMessageTemplate\": {\r\n    \"$type\": \"ChannelMessage\",\r\n    \"command\": \"ProgramChange\",\r\n    \"midiChannel\": 1,\r\n    \"data1\": 2,\r\n    \"data2\": 0\r\n  },\r\n  \"translationFunction\": \"DirectTranslation\",\r\n  \"inputMatchFunction\": \"CatchAll\"\r\n}";

			//left here for reference - this is the object used to create serializedTranslation by serializing it
			//var translation = new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1),
			//	new ChannelMessage(ChannelCommand.ProgramChange, 1, 2),
			//	InputMatchFunction.CatchAll, TranslationFunction.DirectTranslation);

			var translation = JsonConvert.DeserializeObject<Translation>(serializedTranslation, SerializerSettings.DefaultSettings);

			Assert.AreEqual(translation.InputMessageMatchTarget.Message, 400);
			Assert.AreEqual(translation.OutputMessageTemplate.Message, 704);
			Assert.AreEqual(translation.InputMatchFunction, InputMatchFunction.CatchAll);
			Assert.AreEqual(translation.TranslationFunction, TranslationFunction.DirectTranslation);
		}
    }
}
