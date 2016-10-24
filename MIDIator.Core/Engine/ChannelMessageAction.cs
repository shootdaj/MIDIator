using System;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
	public class ChannelMessageAction
	{
		public string Name { get; private set; }

		public ChannelMessageAction(Func<ChannelMessage, bool> matchFunction, Action<ChannelMessage> action, string name = null)
		{
			MatchFunction = matchFunction;
			Action = action;
			Name = name;
		}

		public Func<ChannelMessage, bool> MatchFunction { get; private set; }

		public Action<ChannelMessage> Action { get; private set; }
	}
}
