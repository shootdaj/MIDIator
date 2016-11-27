using System;
using System.Collections.Generic;
using System.Threading;
using MIDIator.Engine;
using MIDIator.Interfaces;
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
			var testDeviceName = "TestMIDIatorDevice_MIDIInputDevice_Constructor_And_Dispose_Work";
			var virtualMIDIManager = new VirtualMIDIManager();
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);

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

			//midiInputDevice = null;

			//Thread.Sleep(1000);
			//GC.Collect();
			//GC.WaitForPendingFinalizers();
		}

		[Test, RunInApplicationDomain]
		public void MIDIInputDevice_DirectTranslation_Works()
		{
			var virtualMIDIManager = new VirtualMIDIManager();
			var midiDeviceService = new MIDIDeviceService();

			var testDeviceName = "TestMIDIatorDevice_MIDIInputDevice_DirectTranslation_Works";
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);
			
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
			//midiInputDevice = null;

			//Thread.Sleep(1000);

			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();

			//virtualMIDIManager = null;

			//Thread.Sleep(1000);

			//GC.Collect();
			//GC.WaitForPendingFinalizers();

			//Thread.Sleep(1000);
		}


		/// <summary>
		/// Need virtualMidi C# library to create virtual MIDI inputs and send commands to it
		/// </summary>
		[Test, RunInApplicationDomain]
		public void MIDIInputDevice_ChangeNote_Works()
		{
			var virtualMIDIManager = new VirtualMIDIManager();
			var midiDeviceService = new MIDIDeviceService();

			var testDeviceName = "TestMIDIatorDevice_MIDIInputDevice_ChangeNote_Works";
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);

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

			midiInputDevice = null;

			Thread.Sleep(1000);

			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();

			virtualMIDIManager = null;

			Thread.Sleep(1000);

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}
