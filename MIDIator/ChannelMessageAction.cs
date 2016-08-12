using System;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public class ChannelMessageAction
	{
		public ChannelMessageAction(Func<ChannelMessage, bool> matchFunction, Action<ChannelMessage> action)
		{
			MatchFunction = matchFunction;
			Action = action;
		}

		public Func<ChannelMessage, bool> MatchFunction { get; private set; }

		public Action<ChannelMessage> Action { get; private set; }
	}
}
