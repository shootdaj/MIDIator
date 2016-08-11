using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIator
{
	public interface ITranslationMap
	{
		//MIDIInputDevice InputDevice { get; set; }
		//MIDIOutputDevice OutputDevice { get; set; }
		List<ITranslation> Translations { get; set; }
	}
}
