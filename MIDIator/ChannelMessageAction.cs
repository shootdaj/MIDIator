using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public class ChannelMessageAction
	{
		public ChannelMessageAction(ChannelMessageMatch match, Action<ChannelMessage> action)
		{
			Match = match;
			Action = action;
		}

		public ChannelMessageMatch Match { get; private set; }
			
		public Action<ChannelMessage> Action { get; private set; }
	}
}
