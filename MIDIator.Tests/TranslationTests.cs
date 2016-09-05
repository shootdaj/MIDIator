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
				"{\r\n  \"InputMessageMatchTarget\": {\r\n    \"$type\": \"ChannelMessage\",\r\n    \"Command\": 144,\r\n    \"MidiChannel\": 1,\r\n    \"Data1\": 1,\r\n    \"Data2\": 0\r\n  },\r\n  \"OutputMessageTemplate\": {\r\n    \"$type\": \"ChannelMessage\",\r\n    \"Command\": 192,\r\n    \"MidiChannel\": 1,\r\n    \"Data1\": 2,\r\n    \"Data2\": 0\r\n  },\r\n  \"TranslationFunction\": \"DirectTranslation\",\r\n  \"InputMatchFunction\": \"CatchAll\"\r\n}";

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
				"{\r\n  \"InputMessageMatchTarget\": {\r\n    \"$type\": \"ChannelMessage\",\r\n    \"Command\": 144,\r\n    \"MidiChannel\": 1,\r\n    \"Data1\": 1,\r\n    \"Data2\": 0\r\n  },\r\n  \"OutputMessageTemplate\": {\r\n    \"$type\": \"ChannelMessage\",\r\n    \"Command\": 192,\r\n    \"MidiChannel\": 1,\r\n    \"Data1\": 2,\r\n    \"Data2\": 0\r\n  },\r\n  \"TranslationFunction\": \"DirectTranslation\",\r\n  \"InputMatchFunction\": \"CatchAll\"\r\n}";

			//left here for reference - this is the object used to create serializedTranslation by serializing it
			//var translation = new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1),
			//	new ChannelMessage(ChannelCommand.ProgramChange, 1, 2),
			//	InputMatchFunction.CatchAll, TranslationFunction.DirectTranslation);

			var translation = JsonConvert.DeserializeObject<Translation>(serializedTranslation, SerializerSettings.DefaultSettings);

			Assert.AreEqual(translation.InputMessageMatchTarget.Message, 401);
			Assert.AreEqual(translation.OutputMessageTemplate.Message, 705);
			Assert.AreEqual(translation.InputMatchFunction, InputMatchFunction.CatchAll);
			Assert.AreEqual(translation.TranslationFunction, TranslationFunction.DirectTranslation);
		}
    }
}
