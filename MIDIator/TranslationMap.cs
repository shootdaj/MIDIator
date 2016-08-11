
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIator
{
	public class TranslationMap : ITranslationMap
	{
		public TranslationMap(List<ITranslation> translations)
		{
			Translations = translations;
		}
	//	public MIDIInputDevice InputDevice { get; set; }
	//	public MIDIOutputDevice OutputDevice { get; set; }
		public List<ITranslation> Translations { get; set; }
	}
}
