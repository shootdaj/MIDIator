using System;
using System.Collections.Generic;
using System.Linq;
using MIDIator.VirtualMIDI;
using Refigure;

namespace MIDIator.Engine
{
	public class VirtualMIDIManager : IDisposable
	{
		public bool? TeVirtualMidiLogging = Config.GetAsBoolSilent("Core.TeVirtualMidiLogging");

		public List<VirtualDevice> VirtualDevices { get; private set; } = new List<VirtualDevice>();

		public VirtualDevice CreateVirtualDevice(string name, Guid manufacturerID, Guid productID, VirtualDeviceType virtualDeviceType, bool prefixName = true)
		{
			if (TeVirtualMidiLogging ?? true)
				TeVirtualMIDI.logging(TeVirtualMIDI.TE_VM_LOGGING_MISC | TeVirtualMIDI.TE_VM_LOGGING_RX | TeVirtualMIDI.TE_VM_LOGGING_TX);

			var virtualDevice = virtualDeviceType == VirtualDeviceType.Input ?
				(VirtualDevice)new VirtualInputDevice(prefixName ? $"{Config.Get("Core.VirtualDevicePrefix")}{name}" : name,
				ref manufacturerID, ref productID)
				:
				(VirtualDevice)new VirtualOutputDevice(prefixName ? $"{Config.Get("Core.VirtualDevicePrefix")}{name}" : name,
				ref manufacturerID, ref productID);

			VirtualDevices.Add(virtualDevice);

			return virtualDevice;
		}

		public void RemoveVirtualDevice(string name)
		{
			var matchedDevices = VirtualDevices.Where(x => x.Name == name).ToList();
			if (matchedDevices.Any())
			{
				var virtualDevice = matchedDevices.ToList().First();

				virtualDevice.Dispose();
				VirtualDevices.Remove(virtualDevice);
			}
			else
				throw new Exception($"No device with name {name}.");
		}

		public void Dispose()
		{
			VirtualDevices.ForEach(x => x.Dispose());
			VirtualDevices = null;
			TeVirtualMidiLogging = null;
		}
	}
}
