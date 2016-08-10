using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public class MIDIChannelMessage : MIDIMessage
	{
		public ChannelMessage ChannelMessage => (ChannelMessage) ShortMessage;

		public int Data1 => ChannelMessage.Data1;

		public int Data2 => ChannelMessage.Data2;

		public int Channel => ChannelMessage.MidiChannel;

		public ChannelCommand Command => ChannelMessage.Command;

		public int Status => ChannelMessage.Status;
	}
}