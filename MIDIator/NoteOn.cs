namespace MIDIator
{
	public class NoteOn : MIDIMessage
	{
		public MIDIMessageType MessageType { get; set; }
		public string Data1 { get; set; }
		public string Data2 { get; set; }
	}
}
