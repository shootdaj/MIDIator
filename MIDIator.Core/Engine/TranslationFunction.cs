using TypeLite;

namespace MIDIator.Engine
{
	[TsEnum(Module = "")]
	public enum TranslationFunction
	{
		DirectTranslation,
		ChangeNote,
		PCToNote,
	}
}
