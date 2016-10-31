using System.Collections.Generic;
using TypeLite;

namespace MIDIator.Interfaces
{
	[TsInterface(Module = "")]
	public interface ITranslationMap
	{
		//MIDIInputDevice InputDevice { get; set; }
		//MIDIOutputDevice OutputDevice { get; set; }
		List<ITranslation> Translations { get; set; }
	}
}
