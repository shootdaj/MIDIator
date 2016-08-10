using Sanford.Multimedia.Midi;

namespace MIDIator
{
	/// <summary>
	/// Represents a MIDI Message
	/// </summary>
	public abstract class MIDIMessage : IMIDIMessage
	{
		public ShortMessage ShortMessage { get; protected set; }
	}
}