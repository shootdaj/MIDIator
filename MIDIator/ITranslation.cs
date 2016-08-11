using System;
using System.Collections.Generic;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	/// <summary>
	/// Abstracts out the information needed to translate one MIDI signal into another.
	/// </summary>
	public interface ITranslation
	{
		/// <summary>
		/// InputMessageMatchTarget's properties are used to match an incoming message based on InputMatchFunction.
		/// </summary>
		ShortMessage InputMessageMatchTarget { get; }

		/// <summary>
		/// Template used to design the output message, which is ultimately built by TranslationFunction.
		/// </summary>
		ShortMessage OutputMessageTemplate { get; }

		/// <summary>
		/// Used to convert an incoming message that to an outgoing message. The injected translation function
		/// may or may not adhere to using the InputMessageMatchTarget and/or OutputMessageTemplate. But the suggestion
		/// is to use them to extract and build the output message from the input message.
		/// </summary>
		Func<ShortMessage, ShortMessage, ShortMessage> TranslationFunction { get; }

		/// <summary>
		/// Used to match an incoming message to a Translation.
		/// </summary>
		Func<ShortMessage, bool> InputMatchFunction { get; }
	}
}
