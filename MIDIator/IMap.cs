using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIator
{
	public interface IMap
	{
		MIDIInputDevice InputDevice { get; set; }
		MIDIOutputDevice OutputDevice { get; set; }
		IList<ITranslation> Translations { get; set; }
	}
}
