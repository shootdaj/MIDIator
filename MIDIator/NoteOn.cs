using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIator
{
	public class NoteOn : IMIDIMessage
	{
		public MIDIMessageType MessageType { get; set; }
		public string Data1 { get; set; }
		public string Data2 { get; set; }
	}
}
