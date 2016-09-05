using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Sanford.Multimedia.Midi;
// ReSharper disable UnusedVariable

namespace MIDIator.Tests
{
	[Ignore("Manual Test with Numark Orbit -- need to be changed to a virtualized MIDI device using VirtualMIDI.")]
	public class MIDIInputDeviceTests
	{
		private static int Timeout => 10000;

		[Test]
		public void MIDIInputDevice_Constructor_And_Dispose_Work()
		{
			var midiInputDevice = new MIDIInputDevice(0, new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunction.NoteMatch,
					TranslationFunction.DirectTranslation)
			}));

			midiInputDevice.Dispose();
		}

		[Test]
		public void MIDIInputDevice_DirectTranslation_Works()
		{
			var midiInputDevice = MIDIManager.GetInputDevice("Numark ORBIT", new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 49, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunction.NoteMatch,
					TranslationFunction.DirectTranslation)
			}));

			midiInputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true,
				message => Console.WriteLine($"Data1:{message.Data1} | Command:{message.Command.ToString()}")));

			midiInputDevice.Start();

			Thread.Sleep(Timeout);

			midiInputDevice.Stop	();
			MIDIManager.RemoveDevice(midiInputDevice);
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
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunction.CatchAll,
					TranslationFunction.ChangeNote)
			}));

			midiInputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true,
				message => Console.WriteLine($"Data1:{message.Data1} | Command:{message.Command.ToString()}")));

			midiInputDevice.Start();

			Thread.Sleep(Timeout);

			midiInputDevice.Stop();
			MIDIManager.RemoveDevice(midiInputDevice);
		}
	}
}
