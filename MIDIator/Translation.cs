using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public class Translation : ITranslation
	{
		public Translation(ShortMessage inputMessageMatchTarget, ShortMessage outputMessageTemplate, Func<ShortMessage, bool> inputMatchFunction, Func<ShortMessage, ShortMessage, ShortMessage> translationFunction)
		{
			InputMessageMatchTarget = inputMessageMatchTarget;
			OutputMessageTemplate = outputMessageTemplate;
			InputMatchFunction = inputMatchFunction;
			TranslationFunction = translationFunction;
		}

		public ShortMessage InputMessageMatchTarget { get; }
		public ShortMessage OutputMessageTemplate { get; }
		public Func<ShortMessage, ShortMessage, ShortMessage> TranslationFunction { get; }

		public Func<ShortMessage, bool> InputMatchFunction { get; private set; }
	}
}
