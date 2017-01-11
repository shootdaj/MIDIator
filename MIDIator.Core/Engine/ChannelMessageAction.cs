using System;
using MIDIator.Interfaces;
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

		public static ChannelMessageAction SendToOutput(IMIDIOutputDevice outputDevice)
		{
			return new ChannelMessageAction(message => true, message =>
			{
				if (message != null)
					outputDevice.Send(message);
			}, "SendToOutput");
		}
	}
}
