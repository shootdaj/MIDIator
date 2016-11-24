using System.Collections.Generic;
using System.Dynamic;
using TypeLite;

namespace MIDIator.Interfaces
{
	[TsInterface(Module = "")]
	public interface ITranslationMap
	{
		List<ITranslation> Translations { get; set; }
		void Update(dynamic translationMap);
	}
}
