
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIator
{
	public class Map : IMap
	{
		public MIDIInputDevice InputDevice { get; set; }
		public MIDIOutputDevice OutputDevice { get; set; }
		public IList<ITranslation> Translations { get; set; }
	}
}
