using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public class ChannelMessageMatch
	{
		public ChannelMessageMatch(Func<ChannelMessage, bool> matchFunction)
		{
			MatchFunction = matchFunction;
		}


		public Func<ChannelMessage, bool> MatchFunction { get; private set; }
	}
}
