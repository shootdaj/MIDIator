using TypeLite;

namespace MIDIator.Engine
{
	[TsEnum(Module = "MIDIator.UI")]
	public enum InputMatchFunction
	{
		Data1Match,
		NoteMatch,
		CatchAll
	}
}
