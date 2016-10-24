using System;
using System.Linq;
using System.Reflection;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
	public static class TranslationFunctions
	{
		/// <summary>
		/// Outputs the output message template without any modifications or data from the incoming message.
		/// </summary>
		public static Func<ShortMessage, ShortMessage, ShortMessage> DirectTranslation =>
			(incomingMessage, outputMessageTemplate) => outputMessageTemplate;

		/// <summary>
		/// Takes the incoming message, changes the note to the one contained in the output message template, and outputs it.
		/// </summary>
		public static Func<ShortMessage, ShortMessage, ShortMessage> ChangeNote => (incomingMessage, outputMessageTemplate) =>
		{
			var translatedMessage = new ChannelMessage(incomingMessage.ToChannelMessage().Command,
				incomingMessage.ToChannelMessage().MidiChannel, outputMessageTemplate.ToChannelMessage().Data1,
				incomingMessage.ToChannelMessage().Data2);

			return translatedMessage;
		};

		public static Func<ShortMessage, ShortMessage, ShortMessage> PCToNote => (incomingMessage, outputMessageTemplate) =>
		{
			if (incomingMessage.ToChannelMessage().Command != ChannelCommand.ProgramChange)
			{
				throw new Exception("inputMessage must be a ProgramChange.");
			}

			if (outputMessageTemplate.ToChannelMessage().Command != ChannelCommand.NoteOff && outputMessageTemplate.ToChannelMessage().Command != ChannelCommand.NoteOn)
			{
				throw new Exception("outputMessageTemplate must be a NoteOn or NoteOff.");
			}

			var translatedMessage = new ChannelMessage(outputMessageTemplate.ToChannelMessage().Command,
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
	    public static object Parse(string functionName)
	    {
            //TODO: possible place for bug
	        return
	            typeof(TranslationFunctions).GetProperties(BindingFlags.Public | BindingFlags.Static)
	                .First(p => p.Name == functionName);
	    }

		public static Func<ShortMessage, ShortMessage, ShortMessage> Get(TranslationFunction func)
		{
			//TODO: possible place for bug
			return
				(Func<ShortMessage, ShortMessage, ShortMessage>)typeof(TranslationFunctions).GetProperties(BindingFlags.Public | BindingFlags.Static)
					.First(p => p.Name == Enum.GetName(typeof(TranslationFunction), func)).GetValue(null);
		}

		/// <summary>
		/// TODO: Finish him
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static Func<ShortMessage, ShortMessage, bool> GetReasonableFunction(ShortMessage message)
        {
            throw new NotImplementedException("This function should determine the most reasonable " +
                                              "translation function for the given message type and return it.");
        }
    }
}
