using TypeLite;

namespace MIDIator.Engine
{
	[TsEnum(Module = "")]
	public enum InputMatchFunction
	{
		Data1Match,
		NoteMatch,
		CatchAll
	}
}
