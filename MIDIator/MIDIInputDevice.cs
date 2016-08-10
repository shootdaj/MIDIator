using System;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
    public class MIDIInputDevice : InputDevice
    {
        public MIDIInputDevice(int deviceID) : base(deviceID)
        {
            
        }

	    public bool IsRecording { get; protected set; }
	    public string Name => GetDeviceCapabilities(DeviceID).name;
        public int DriverVersion => GetDeviceCapabilities(DeviceID).driverVersion;
		public short MID => GetDeviceCapabilities(DeviceID).mid;
		public short PID => GetDeviceCapabilities(DeviceID).pid;
		public int Support => GetDeviceCapabilities(DeviceID).support;

		public void Start()
        {
            StartRecording();
			IsRecording = true;
        }

        public void Stop()
        {
            StopRecording();
            IsRecording = false;
        }

        public void AddChannelMessageAction(EventHandler<ChannelMessageEventArgs> action)
        {
            var wasRecording = IsRecording;
            if (IsRecording)
                Stop();
            ChannelMessageReceived += action;

            if (wasRecording)
                Start();
        }

        public void AddSysCommonMessageAction(EventHandler<SysCommonMessageEventArgs> action)
        {
            var wasRecording = IsRecording;
            if (IsRecording)
                Stop();
            SysCommonMessageReceived += action;

			if (wasRecording)
                Start();
        }

        public void AddSysExMessageAction(EventHandler<SysExMessageEventArgs> action)
        {
            var wasRecording = IsRecording;
            if (IsRecording)
                Stop();
            SysExMessageReceived += action;

            if (wasRecording)
                Start();
        }

        public void AddSysRealtimeMessageAction(EventHandler<SysRealtimeMessageEventArgs> action)
        {
            var wasRecording = IsRecording;
            if (IsRecording)
                Stop();
            SysRealtimeMessageReceived += action;

            if (wasRecording)
                Start();
        }

        public void AddErrorAction(EventHandler<ErrorEventArgs> action)
        {
            var wasRecording = IsRecording;
            if (IsRecording)
                Stop();
            Error += action;

            if (wasRecording)
                Start();
        }

        public new void Dispose()
        {
            Reset();
            base.Dispose();
        }
    }
}