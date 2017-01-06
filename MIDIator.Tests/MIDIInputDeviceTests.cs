using System;
using System.Collections.Generic;
using System.Threading;
using MIDIator.Engine;
using MIDIator.Interfaces;
using MIDIator.Services;
using NUnit.Framework;
using Sanford.Multimedia.Midi;
// ReSharper disable UnusedVariable

namespace MIDIator.Tests
{
	[Category("LocalOnly")]
	public class MIDIInputDeviceTests
	{
		private static int Timeout => 1000;

		[Test]
		public void MIDIInputDevice_Constructor_And_Dispose_Work()
		{
			var testDeviceName = "TestDevice" + new Random().Next(1, 100);
			var virtualMIDIManager = new VirtualMIDIManager();
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input);

			var midiInputDevice = new MIDIInputDevice(0, new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunction.NoteMatch,
					TranslationFunction.DirectTranslation)
			}));

			//cleanup
			midiInputDevice.Dispose();
			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();
		}

		[Test, RunInApplicationDomain]
		public void MIDIInputDevice_DirectTranslation_Works()
		{
			var virtualMIDIManager = new VirtualMIDIManager();
			var midiDeviceService = new MIDIDeviceService();

			var testDeviceName = "TestDevice" + new Random().Next(1, 100);
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input);
			
			var midiInputDevice = midiDeviceService.GetInputDevice(testDeviceName, new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 49, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunction.NoteMatch,
					TranslationFunction.DirectTranslation)
			}));

			midiInputDevice.AddChannelMessageAction(new ChannelMessageAction(message => true,
				message => Console.WriteLine($"Data1:{message.Data1} | Command:{message.Command.ToString()}")));

			midiInputDevice.Start();
			Thread.Sleep(Timeout);
			midiInputDevice.Stop();

			//cleanup
			midiDeviceService.RemoveInputDevice(midiInputDevice);
			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();
		}


		/// <summary>
		/// Need virtualMidi C# library to create virtual MIDI inputs and send commands to it
		/// </summary>
		[Test, RunInApplicationDomain]
		public void MIDIInputDevice_ChangeNote_Works()
		{
			var virtualMIDIManager = new VirtualMIDIManager();
			var midiDeviceService = new MIDIDeviceService();

			var testDeviceName = "TestDevice" + new Random().Next(1, 100);
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input);

			var midiInputDevice = midiDeviceService.GetInputDevice(testDeviceName, new TranslationMap(new List<ITranslation>()
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

			//cleanup
			midiDeviceService.RemoveInputDevice(midiInputDevice);
			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();
		}
	}
}
