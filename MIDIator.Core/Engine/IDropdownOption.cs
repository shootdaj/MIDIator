using TypeLite;

namespace MIDIator.Engine
{
	[TsInterface(Module = "MIDIator.UI")]
	public interface IDropdownOption
	{
		string Value { get; }
		string Label { get; }
	}
}