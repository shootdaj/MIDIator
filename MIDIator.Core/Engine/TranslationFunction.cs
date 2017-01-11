using TypeLite;

namespace MIDIator.Engine
{
	[TsEnum(Module = "")]
	public enum TranslationFunction
	{
		DirectTranslation,
		ChangeNote,
		PCToNote,
		KeepData2,
		ToggleData2,
		InvertData2
	}
}
