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
	//[Ignore("Manual Test with Numark Orbit -- need to be changed to a virtualized MIDI device using VirtualMIDI.")]
	public class MIDIInputDeviceTests
	{
		private MIDIManager MIDIManager { get; set; }

		[SetUp]
		public void Setup()
		{
			MIDIManager = new MIDIManager();
		}

		[TearDown]
		public void TearDown()
		{
			MIDIManager = null;
		}

		private static int Timeout => 1000;

		[Test]
		public void MIDIInputDevice_Constructor_And_Dispose_Work()
		{
			//var testDeviceName = "TestMIDIatorDevice";
			//VirtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);

			var midiInputDevice = new MIDIInputDevice(0, new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 1, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunction.NoteMatch,
					TranslationFunction.DirectTranslation)
			}));

			//cleanup
			midiInputDevice.Dispose();
			//VirtualMIDIManager.RemoveVirtualDevice(testDeviceName);

			midiInputDevice = null;

			Thread.Sleep(1000);
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		[Test]
		public void MIDIInputDevice_DirectTranslation_Works()
		{
			var virtualMIDIManager = new VirtualMIDIManager();

			var testDeviceName = "TestMIDIatorDevice";
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);

			Thread.Sleep(1000);

			var midiInputDevice = MIDIManager.GetInputDevice(testDeviceName, new TranslationMap(new List<ITranslation>()
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
			MIDIManager.RemoveInputDevice(midiInputDevice);

			midiInputDevice = null;

			Thread.Sleep(1000);

			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();

			virtualMIDIManager = null;

			Thread.Sleep(1000);

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Thread.Sleep(1000);
		}


		/// <summary>
		/// Need virtualMidi C# library to create virtual MIDI inputs and send commands to it
		/// </summary>
		[Test]
		public void MIDIInputDevice_ChangeNote_Works()
		{
			var virtualMIDIManager = new VirtualMIDIManager();

			var testDeviceName = "TestMIDIatorDevice";
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);

			var midiInputDevice = MIDIManager.GetInputDevice(testDeviceName, new TranslationMap(new List<ITranslation>()
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
			MIDIManager.RemoveInputDevice(midiInputDevice);

			midiInputDevice = null;

			Thread.Sleep(1000);

			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);

			Thread.Sleep(1000);

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}
