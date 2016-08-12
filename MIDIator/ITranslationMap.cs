using System.Collections.Generic;

namespace MIDIator
{
	public interface ITranslationMap
	{
		//MIDIInputDevice InputDevice { get; set; }
		//MIDIOutputDevice OutputDevice { get; set; }
		List<ITranslation> Translations { get; set; }
	}
}
