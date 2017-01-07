using TypeLite;

namespace MIDIator.Engine
{
	[TsEnum(Module = "")]
	public enum InputMatchFunction
	{
		Data1Match,
		NoteMatch,
		CatchAll,
		CCData1Match,
		NoteOffData1Match,
		NoteOnData1Match,
		NoteData1Match
	}
}
