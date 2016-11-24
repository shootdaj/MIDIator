using System;
using MIDIator.Interfaces;
using Sanford.Multimedia.Midi;
using TypeLite;

namespace MIDIator.Engine
{
	[TsClass(Module = "")]
	[UIDropdownOption("DeviceID")]
	public class MIDIOutputDevice : IDisposable, IMIDIOutputDevice, IDropdownOption
	{
        public MIDIOutputDevice(int deviceID)
        {
			OutputDevice = new OutputDevice(deviceID);
		}

        private OutputDevice OutputDevice { get; }

        public string Name => OutputDeviceBase.GetDeviceCapabilities(DeviceID).name;

	    public int DriverVersion => OutputDeviceBase.GetDeviceCapabilities(DeviceID).driverVersion;

	    public short MID => OutputDeviceBase.GetDeviceCapabilities(DeviceID).mid;

	    public short PID => OutputDeviceBase.GetDeviceCapabilities(DeviceID).pid;
		
		public int Support => OutputDeviceBase.GetDeviceCapabilities(DeviceID).support;

		public int DeviceID => OutputDevice.DeviceID;

	    public void Send(ShortMessage midiMessage)
	    {
			OutputDevice.Send((ChannelMessage)midiMessage);
		}

        public void Dispose()
        {
            OutputDevice.Reset();
            OutputDevice.Dispose();
        }

		[TsIgnore]
		public string Value => Name;

		[TsIgnore]
		public string Label => Name;
	}
}