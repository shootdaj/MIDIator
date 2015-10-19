using System;
using System.Collections.Generic;
using System.Dynamic;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public static class MIDIManager
	{
		public class MIDIDevice : IDisposable
		{
			public MIDIDevice(int deviceID)
			{
				DeviceID = deviceID;
				Name = InputDevice.GetDeviceCapabilities(deviceID).name;
				DriverVersion = InputDevice.GetDeviceCapabilities(deviceID).driverVersion;
				MID = InputDevice.GetDeviceCapabilities(deviceID).mid;
				PID = InputDevice.GetDeviceCapabilities(deviceID).pid;
				Support = InputDevice.GetDeviceCapabilities(deviceID).support;
				InputDevice = new InputDevice(deviceID);
			}

			private InputDevice InputDevice { get; }
			public bool IsRecording { get; set; }
			public string Name { get; }
			public int DriverVersion { get; }
			public short MID { get; }
			public short PID { get; }
			public int Support { get; }
			public int DeviceID { get; }

			public void StartRecording()
			{
				InputDevice.StartRecording();
				IsRecording = true;
			}

			public void StopRecording()
			{
				InputDevice.StopRecording();
				IsRecording = false;
			}

			public void AddChannelMessageAction(EventHandler<ChannelMessageEventArgs> action)
			{
				var wasRecording = IsRecording;
				if (IsRecording)
					StopRecording();
				InputDevice.ChannelMessageReceived += action;

				if (wasRecording)
					StartRecording();
			}

			public void AddSysCommonMessageAction(EventHandler<SysCommonMessageEventArgs> action)
			{
				var wasRecording = IsRecording;
				if (IsRecording)
					StopRecording();
				InputDevice.SysCommonMessageReceived += action;

				if (wasRecording)
					StartRecording();
			}

			public void AddSysExMessageAction(EventHandler<SysExMessageEventArgs> action)
			{
				var wasRecording = IsRecording;
				if (IsRecording)
					StopRecording();
				InputDevice.SysExMessageReceived += action;

				if (wasRecording)
					StartRecording();
			}

			public void AddSysRealtimeMessageAction(EventHandler<SysRealtimeMessageEventArgs> action)
			{
				var wasRecording = IsRecording;
				if (IsRecording)
					StopRecording();
				InputDevice.SysRealtimeMessageReceived += action;

				if (wasRecording)
					StartRecording();
			}

			public void AddErrorAction(EventHandler<ErrorEventArgs> action)
			{
				var wasRecording = IsRecording;
				if (IsRecording)
					StopRecording();
				InputDevice.Error += action;

				if (wasRecording)
					StartRecording();
			}

			public void Dispose()
			{
				InputDevice.Dispose();
			}
		}

		public static int DeviceCount => InputDevice.DeviceCount;

		public static IEnumerable<MIDIDevice> FreeDevices
		{
			get
			{
				for (int i = 0; i < InputDevice.DeviceCount; i++)
				{
					dynamic returnValue = new ExpandoObject();
					returnValue.Name = InputDevice.GetDeviceCapabilities(i).name;
					returnValue.DriverVersion = InputDevice.GetDeviceCapabilities(i).driverVersion;
					returnValue.MID = InputDevice.GetDeviceCapabilities(i).mid;
					returnValue.PID = InputDevice.GetDeviceCapabilities(i).pid;
					returnValue.Support = InputDevice.GetDeviceCapabilities(i).support;
					returnValue.DeviceID = i;
					yield return returnValue;
				}
			}
		}

		public static IList<MIDIDevice> DevicesInUse { get; } = new List<MIDIDevice>();

		public static MIDIDevice CreateDevice(int deviceID)
		{
			var device = new MIDIDevice(deviceID);
			DevicesInUse.Add(device);
			return device;
		}
	}
}
