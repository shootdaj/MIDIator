using Sanford.Multimedia.Midi;
using TypeLite;

namespace MIDIator.Interfaces
{
	[TsInterface(Module = "MIDIator.UI")]
	public interface IMIDIOutputDevice
	{
		string Name { get; }
		int DriverVersion { get; }
		short MID { get; }
		short PID { get; }
		int Support { get; }
		int DeviceID { get; }
		void Send(ShortMessage midiMessage);
	}
}