using System;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class InputMatchFunctions
	{
		public static Func<ShortMessage, ShortMessage, bool> CatchAll =
			(incomingMessage, inputMessageMatchTarget) =>
				true;

		public static Func<ShortMessage, ShortMessage, bool> NoteMatch =
			(incomingMessage, inputMessageMatchTarget) =>
				incomingMessage.ToChannelMessage().Data1.Equals(inputMessageMatchTarget.ToChannelMessage().Data1);


	}
}
