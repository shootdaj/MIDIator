using Sanford.Multimedia.Midi;
using TypeLite;

namespace MIDIator.Interfaces
{
	[TsInterface(Module = "")]
	public interface IMIDIOutputDevice
	{
		string Name { get; }
		int DeviceID { get; }
		int DriverVersion { get; }
		short MID { get; }
		short PID { get; }
		int Support { get; }
		void Send(ShortMessage midiMessage);
	}
}