using System;
using System.Linq;
using System.Reflection;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
	public static class InputMatchFunctions
	{
		public static Func<ShortMessage, ShortMessage, bool> CatchAll =>
			(incomingMessage, inputMessageMatchTarget) =>
				true;

		public static Func<ShortMessage, ShortMessage, bool> NoteMatch => Data1Match;

		/// <summary>
		/// Matches if the incomingMessage's Data1 matches inputMessageMatchTarget's Data1 and incomingMessage's channel command is NoteOn.
		/// </summary>
		public static Func<ShortMessage, ShortMessage, bool> NoteOnData1Match =>
			(incomingMessage, inputMessageMatchTarget) =>
				incomingMessage.ToChannelMessage().Data1.Equals(inputMessageMatchTarget.ToChannelMessage().Data1) &&
				incomingMessage.ToChannelMessage().Command == ChannelCommand.NoteOn;

		/// <summary>
		/// Matches if the incomingMessage's Data1 matches inputMessageMatchTarget's Data1 and incomingMessage's channel command is NoteOn or NoteOff.
		/// </summary>
		public static Func<ShortMessage, ShortMessage, bool> NoteData1Match =>
			(incomingMessage, inputMessageMatchTarget) =>
				incomingMessage.ToChannelMessage().Data1.Equals(inputMessageMatchTarget.ToChannelMessage().Data1) &&
				(incomingMessage.ToChannelMessage().Command == ChannelCommand.NoteOn ||
				 incomingMessage.ToChannelMessage().Command == ChannelCommand.NoteOff);

		/// <summary>
		/// Matches if the incomingMessage's Data1 matches inputMessageMatchTarget's Data1 and incomingMessage's channel command is NoteOff.
		/// </summary>
		public static Func<ShortMessage, ShortMessage, bool> NoteOffData1Match =>
			(incomingMessage, inputMessageMatchTarget) =>
				incomingMessage.ToChannelMessage().Data1.Equals(inputMessageMatchTarget.ToChannelMessage().Data1) &&
				incomingMessage.ToChannelMessage().Command == ChannelCommand.NoteOff;

		/// <summary>
		/// Matches if the incoming value's Data1 matches inputMessageMatchTarget's Data1
		/// </summary>
		public static Func<ShortMessage, ShortMessage, bool> Data1Match =>
			(incomingMessage, inputMessageMatchTarget) =>
				incomingMessage.ToChannelMessage().Data1.Equals(inputMessageMatchTarget.ToChannelMessage().Data1);

		/// <summary>
		/// Matches if the incoming value is a CC and matches inputMessageMatchTemplate's Data1.
		/// </summary>
		public static Func<ShortMessage, ShortMessage, bool> CCData1Match =>
			(incomingMessage, inputMessageMatchTarget) =>
				incomingMessage.ToChannelMessage().Data1.Equals(inputMessageMatchTarget.ToChannelMessage().Data1) && 
				incomingMessage.ToChannelMessage().Command == ChannelCommand.Controller;


		public static object Parse(string functionName)
		{
			//TODO: possible place for bug
			return
				typeof(InputMatchFunctions).GetProperties(BindingFlags.Public | BindingFlags.Static)
					.First(p => p.Name == functionName);
		}

		public static Func<ShortMessage, ShortMessage, bool> Get(InputMatchFunction func)
		{
			//TODO: possible place for bug
			var returnValue = (Func<ShortMessage, ShortMessage, bool>)typeof(InputMatchFunctions).GetProperties(BindingFlags.Public | BindingFlags.Static)
				.First(p => p.Name == Enum.GetName(typeof(InputMatchFunction), func)).GetValue(null);
			return
				returnValue;
		}

		/// <summary>
		/// TODO: Finish him
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static Func<ShortMessage, ShortMessage, bool> GetReasonableFunction(ShortMessage message)
	    {
	        throw new NotImplementedException("This function should determine the most reasonable " +
	                                          "input match function for the given message type and return it.");
	    }
	}
}
