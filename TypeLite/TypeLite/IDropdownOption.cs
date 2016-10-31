namespace TypeLite
{
	[TsInterface(Module = "")]
	public interface IDropdownOption
	{
		string Value { get; }
		string Label { get; }
	}
}