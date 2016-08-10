using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace MIDIator.Tests
{
	public class TranslationTests
	{
		public void Translation_Serialize()
		{
			var translation = new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1),
				new ChannelMessage(ChannelCommand.ProgramChange, 1, 2));


		}
	}
}
