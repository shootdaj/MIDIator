using System;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
    public class MIDIOutputDevice : IDisposable, IMIDIOutputDevice
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

		public int Support => InputDevice.GetDeviceCapabilities(DeviceID).support;

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
    }
}