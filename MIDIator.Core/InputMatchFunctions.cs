using System;
using System.Linq;
using System.Reflection;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class InputMatchFunctions
	{
		public static Func<ShortMessage, ShortMessage, bool> CatchAll =>
			(incomingMessage, inputMessageMatchTarget) =>
				true;

		public static Func<ShortMessage, ShortMessage, bool> NoteMatch => Data1Match;

		public static Func<ShortMessage, ShortMessage, bool> Data1Match =>
			(incomingMessage, inputMessageMatchTarget) =>
				incomingMessage.ToChannelMessage().Data1.Equals(inputMessageMatchTarget.ToChannelMessage().Data1);


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
			return
				(Func<ShortMessage, ShortMessage, bool>)typeof(InputMatchFunctions).GetProperties(BindingFlags.Public | BindingFlags.Static)
					.First(p => p.Name == Enum.GetName(typeof(InputMatchFunction), func)).GetValue(null);
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
