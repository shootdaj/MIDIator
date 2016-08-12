using System;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class TranslationFunctions
	{
		/// <summary>
		/// Outputs the output message template without any modifications or data from the incoming message.
		/// </summary>
		public static Func<ShortMessage, ShortMessage, ShortMessage> DirectTranslation =
			(incomingMessage, outputMessageTemplate) => outputMessageTemplate;

		/// <summary>
		/// Takes the incoming message, changes the note to the one contained in the output message template, and outputs it.
		/// </summary>
		public static Func<ShortMessage, ShortMessage, ShortMessage> ChangeNote = (incomingMessage, outputMessageTemplate) =>
		{
			var translatedMessage = new ChannelMessage(incomingMessage.ToChannelMessage().Command,
				incomingMessage.ToChannelMessage().MidiChannel, outputMessageTemplate.ToChannelMessage().Data1,
				incomingMessage.ToChannelMessage().Data2);

			return translatedMessage;
		};

		//public static ChannelMessage NoteToPC(ChannelMessage inputMessage, ChannelMessage outputMessage)
		//{
		//	if (outputMessage.Command != ChannelCommand.ProgramChange ||
		//		(inputMessage.Command != ChannelCommand.NoteOff && inputMessage.Command != ChannelCommand.NoteOn))
		//		throw new Exception("outputMessage must be a ProgramChange message and inputMessage must be a NoteOn or NoteOff message.");

		//	var returnValue = new ChannelMessage(outputMessage.);
		//}
	}
}
