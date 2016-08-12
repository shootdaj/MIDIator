using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using Sanford.Multimedia.Midi;

namespace MIDIator.Tests
{
	public class MIDIInputDeviceTests
	{
		[Test]
		public void MIDIInputDevice_Constructor_Works()
		{
			var midiInputDevice = new MIDIInputDevice(0, new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunctions.NoteMatch,
					TranslationFunctions.DirectTranslation)
			}));
		}

		[Test]
		public void MIDIInputDevice_DirectTranslation_Works()
		{
			var midiInputDevice = MIDIManager.GetInputDevice("Numark ORBIT", new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 49, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunctions.NoteMatch,
					TranslationFunctions.DirectTranslation)
			}));

			midiInputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true,
				message => Debug.WriteLine($"Data1:{message.Data1} | Command:{message.Command.ToString()}")));

			midiInputDevice.StartRecording();

			Thread.Sleep(300000);
		}


		/// <summary>
		/// Need virtualMidi C# library to create virtual MIDI inputs and send commands to it
		/// </summary>
		[Test]
		public void MIDIInputDevice_ChangeNote_Works()
		{
			var midiInputDevice = MIDIManager.GetInputDevice("Numark ORBIT", new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 49, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunctions.CatchAll,
					TranslationFunctions.ChangeNote)
			}));

			midiInputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true,
				message => Debug.WriteLine($"Data1:{message.Data1} | Command:{message.Command.ToString()}")));

			midiInputDevice.StartRecording();

			Thread.Sleep(300000);
		}
	}
}
