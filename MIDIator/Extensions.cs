using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class Extensions
	{
		//public static ChannelMessage ToSanfordChannelMessage(this IMIDIMessage inputMessage)
		//{
		//	return new ChannelMessage(inputMessage.ShortMessage);
		//}

		public static ChannelMessage ToChannelMessage(this ShortMessage shortMessage)
		{
			return (ChannelMessage) shortMessage;
		}
	}
}
