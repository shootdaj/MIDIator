using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace MIDIator.Tests
{
	public static class InputMatchFunctions
	{
		public static Func<ChannelMessage, MatchArgs, bool> NoteMatch =
			(message, matchArgs) => message.Data1.Equals(((NoteMatchArgs) matchArgs).Note);
	}

	public class MatchArgs
	{
		
	}

	public class NoteMatchArgs : MatchArgs
	{
		public int Note;
	}
}
