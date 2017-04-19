using System;
using System.Diagnostics;
using MIDIator.Interfaces;
using NLog;
using Sanford.Multimedia.Midi;
using TypeLite;

namespace MIDIator.Engine
{
    [TsClass(Module = "")]
    [UIDropdownOption("DeviceID")]
    public class MIDIOutputDevice : IDisposable, IMIDIOutputDevice, IDropdownOption
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

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
            var channelMessage = (ChannelMessage)midiMessage;
            Log.Info(
                $"{Name}: Receving {{{channelMessage.Command},{channelMessage.MidiChannel},{channelMessage.Data1},{channelMessage.Data2}}}");
            OutputDevice.Send(channelMessage);
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