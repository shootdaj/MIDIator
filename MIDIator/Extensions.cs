using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class Extensions
	{
		public static ChannelMessage ToChannelMessage(this ShortMessage shortMessage)
		{
			return (ChannelMessage) shortMessage;
		}
	}
}
