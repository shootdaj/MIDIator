﻿using System;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class InputMatchFunctions
	{
		public static Func<ShortMessage, ShortMessage, bool> CatchAll =
			(incomingMessage, inputMessageMatchTarget) =>
				true;

		public static Func<ShortMessage, ShortMessage, bool> NoteMatch => Data1Match;

		public static Func<ShortMessage, ShortMessage, bool> Data1Match =
			(incomingMessage, inputMessageMatchTarget) =>
				incomingMessage.ToChannelMessage().Data1.Equals(inputMessageMatchTarget.ToChannelMessage().Data1);
	}
}
