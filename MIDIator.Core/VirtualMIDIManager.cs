using System;
using System.Collections.Generic;
using System.Linq;
using Refigure;
using TobiasErichsen.teVirtualMIDI;

namespace MIDIator
{
	public static class VirtualMIDIManager
	{
		public static bool? TeVirtualMidiLogging = Config.GetAsBoolSilent("Core.TeVirtualMidiLogging");

		public static List<VirtualOutputDevice> VirtualDevices { get; } = new List<VirtualOutputDevice>();

		public static VirtualOutputDevice CreateVirtualDevice(string name, Guid manufacturerID, Guid productID, bool prefixName = true)
		{
			if (TeVirtualMidiLogging ?? true)
				TeVirtualMIDI.logging(TeVirtualMIDI.TE_VM_LOGGING_MISC | TeVirtualMIDI.TE_VM_LOGGING_RX | TeVirtualMIDI.TE_VM_LOGGING_TX);

			var virtualDevice = new VirtualOutputDevice(prefixName ? $"{Config.Get("Core.VirtualDevicePrefix")}{name}" : name,
				ref manufacturerID, ref productID);

			VirtualDevices.Add(virtualDevice);

			return virtualDevice;
		}

		public static void RemoveVirtualDevice(string name)
		{
			var matchedDevices = VirtualDevices.Where(x => x.Name == name).ToList();
			if (matchedDevices.Any())
			{
				matchedDevices.First().Dispose();
			}
			else
				throw new Exception($"No device with name {name}.");
		}
	}
}
