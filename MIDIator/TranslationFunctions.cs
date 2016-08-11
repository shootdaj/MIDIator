using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class TranslationFunctions
	{
		public static Func<ChannelMessage, C DirectTranslation(ChannelMessage inputMessage, ChannelMessage outputMessage)
		{
			return outputMessage;
		}

		public static ChannelMessage TranslateNotePreseveVelocity(ChannelMessage inputMessage, ChannelMessage outputMessage)
		{
			if ((outputMessage.Command != ChannelCommand.NoteOff && outputMessage.Command != ChannelCommand.NoteOn) ||
				(inputMessage.Command != ChannelCommand.NoteOff && inputMessage.Command != ChannelCommand.NoteOn))
				throw new Exception("inputMessage and outputMessage must be a NoteOff or NoteOn message.");

			var returnValue = new ChannelMessage(outputMessage.Command, outputMessage.MidiChannel, outputMessage.Data1, inputMessage.Data2);
			return returnValue;
		}

		//public static ChannelMessage NoteToPC(ChannelMessage inputMessage, ChannelMessage outputMessage)
		//{
		//	if (outputMessage.Command != ChannelCommand.ProgramChange ||
		//		(inputMessage.Command != ChannelCommand.NoteOff && inputMessage.Command != ChannelCommand.NoteOn))
		//		throw new Exception("outputMessage must be a ProgramChange message and inputMessage must be a NoteOn or NoteOff message.");

		//	var returnValue = new ChannelMessage(outputMessage.);
		//}
	}
}
