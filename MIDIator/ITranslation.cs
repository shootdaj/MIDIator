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
		ShortMessage InputMessage { get; }
		ShortMessage OutputMessage { get; }
		//Func<ShortMessage, ShortMessage> TranslationFunction { get; }
	}
}
