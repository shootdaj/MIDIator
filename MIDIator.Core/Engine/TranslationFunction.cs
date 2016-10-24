using TypeLite;

namespace MIDIator.Engine
{
	[TsEnum(Module = "MIDIator.UI")]
	public enum TranslationFunction
	{
		DirectTranslation,
		ChangeNote,
		PCToNote,
	}
}
