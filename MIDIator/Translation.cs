using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	[Serializable]
	[JsonConverter(typeof(TranslationConverter))]
	public class Translation : ITranslation
	{
		public Translation(ShortMessage inputMessageMatchTarget, ShortMessage outputMessageTemplate, Func<ShortMessage, ShortMessage, bool> inputMatchFunction, Func<ShortMessage, ShortMessage, ShortMessage> translationFunction)
		{
			InputMessageMatchTarget = inputMessageMatchTarget;
			OutputMessageTemplate = outputMessageTemplate;
			InputMatchFunction = inputMatchFunction;
			TranslationFunction = translationFunction;
		}

		public ShortMessage InputMessageMatchTarget { get; }

		public ShortMessage OutputMessageTemplate { get; }

		public Func<ShortMessage, ShortMessage, ShortMessage> TranslationFunction { get; }

		public Func<ShortMessage, ShortMessage, bool> InputMatchFunction { get; }
	}
}
