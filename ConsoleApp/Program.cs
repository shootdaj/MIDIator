using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MIDIator;
using Sanford.Multimedia.Midi;
using TobiasErichsen.teVirtualMIDI;

namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{ 
			GuitarWingMap();
		}

		static void CreateTranslationAndSaveIt()
		{
			
		}


		static void GuitarWingMap()
		{
			var channel = 1;

			var guitarWing = MIDIManager.GetInputDevice("Livid Guitar Wing", new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.ProgramChange, channel, 0),
					new ChannelMessage(ChannelCommand.NoteOn, channel, 1), InputMatchFunction.Data1Match,
					TranslationFunction.PCToNote)
			}));

			TeVirtualMIDI.logging(TeVirtualMIDI.TE_VM_LOGGING_MISC | TeVirtualMIDI.TE_VM_LOGGING_RX | TeVirtualMIDI.TE_VM_LOGGING_TX);

			var manufacturer = new Guid("aa4e075f-3504-4aab-9b06-9a4104a91cf0");
			var product = new Guid("bb4e075f-3504-4aab-9b06-9a4104a91cf0");

			var guitarWingReader = new TeVirtualMIDI("MIDIator - GuitarWingReader", 65535, TeVirtualMIDI.TE_VM_FLAGS_PARSE_RX, ref manufacturer, ref product);

			guitarWing.AddChannelMessageAction(new ChannelMessageAction(message => true,
				message =>
				{
					Debug.WriteLine($"Command:{message.Command.ToString()} | Data1:{message.Data1.ToString()} | Data2:{message.Data2.ToString()} | Ch:{message.MidiChannel} | MsgType:{message.MessageType}");
					//OrbitReader.sendCommand(BitConverter.GetBytes(message.msg));
				}));

			guitarWing.Start();

			Thread.Sleep(300000);

			MIDIManager.RemoveDevice(guitarWing);
		}

		static void OrbitMap()
		{
			var orbit = MIDIManager.GetInputDevice("Numark ORBIT", new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 49, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunction.NoteMatch,
					TranslationFunction.DirectTranslation)
			}));

			TeVirtualMIDI.logging(TeVirtualMIDI.TE_VM_LOGGING_MISC | TeVirtualMIDI.TE_VM_LOGGING_RX | TeVirtualMIDI.TE_VM_LOGGING_TX);

			var manufacturer = new Guid("aa4e075f-3504-4aab-9b06-9a4104a91cf0");
			var product = new Guid("bb4e075f-3504-4aab-9b06-9a4104a91cf0");

			var orbitReader = new TeVirtualMIDI("MIDIator - OrbitReader", 65535, TeVirtualMIDI.TE_VM_FLAGS_PARSE_RX, ref manufacturer, ref product);

			orbit.AddChannelMessageAction(new ChannelMessageAction(message => true,
				message =>
				{
					Debug.WriteLine($"Data1:{message.Data1} | Command:{message.Command.ToString()}");
					orbitReader.sendCommand(BitConverter.GetBytes(message.Message));
				}));

			orbit.Start();

			Thread.Sleep(300000);

			MIDIManager.RemoveDevice(orbit);
		}
	}
}
