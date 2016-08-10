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
		public Translation(ShortMessage inputMessage, ShortMessage outputMessage)
		{
			InputMessage = inputMessage;
			OutputMessage = outputMessage;
		}

		//public Func<ShortMessage, ShortMessage> TranslationFunction { get; private set; }
		public ShortMessage InputMessage { get; }
		public ShortMessage OutputMessage { get; }
	}
}
