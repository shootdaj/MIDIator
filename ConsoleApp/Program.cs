using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MIDIator;
using Sanford.Multimedia.Midi;
using TobiasErichsen.teVirtualMIDI;

namespace ConsoleApp
{
	class Program
	{
		private static TeVirtualMIDI OrbitReader { get; set; }
		private static MIDIInputDevice Orbit { get; set; }

		static void Main(string[] args)
		{
			Orbit = MIDIManager.GetInputDevice("Numark ORBIT", new TranslationMap(new List<ITranslation>()
			{
				new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 49, 1),
					new ChannelMessage(ChannelCommand.NoteOn, 1, 2, 1), InputMatchFunctions.NoteMatch,
					TranslationFunctions.DirectTranslation)
			}));

			TeVirtualMIDI.logging(TeVirtualMIDI.TE_VM_LOGGING_MISC | TeVirtualMIDI.TE_VM_LOGGING_RX | TeVirtualMIDI.TE_VM_LOGGING_TX);

			var manufacturer = new Guid("aa4e075f-3504-4aab-9b06-9a4104a91cf0");
			var product = new Guid("bb4e075f-3504-4aab-9b06-9a4104a91cf0");

			OrbitReader = new TeVirtualMIDI("MIDIator - OrbitReader", 65535, TeVirtualMIDI.TE_VM_FLAGS_PARSE_RX, ref manufacturer, ref product);

			Orbit.AddChannelMessageAction(new ChannelMessageAction(message => true,
				message =>
				{
					Debug.WriteLine($"Data1:{message.Data1} | Command:{message.Command.ToString()}");
					OrbitReader.sendCommand(BitConverter.GetBytes(message.msg));
				}));

			Orbit.StartRecording();

			Thread.Sleep(300000);
		}
	}
}
