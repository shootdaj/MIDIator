using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Anshul.Utilities;
using MIDIator.VirtualMIDI;
using Refigure;

namespace MIDIator.Engine
{
	public class VirtualMIDIManager : IDisposable
	{
		public int DeviceNameMaxLength { get; } = 31;

		public bool? TeVirtualMidiLogging = Config.GetAsBoolSilent("Core.TeVirtualMidiLogging");

        public BetterList<VirtualDevice> VirtualDevices { get; private set; } = new BetterList<VirtualDevice>();

        public Subject<VirtualDevice> VirtualDeviceAdd { get; private set; } = new Subject<VirtualDevice>();
        public Subject<string> VirtualDeviceRemove { get; private set; } = new Subject<string>();

		public VirtualDevice CreateVirtualDevice(string name, Guid manufacturerID, Guid productID, VirtualDeviceType virtualDeviceType, bool truncateName = true)
		{
			if (truncateName && name.Length > DeviceNameMaxLength)
				name = name.Substring(0, DeviceNameMaxLength);

			if (name.Length > DeviceNameMaxLength)
				throw new ArgumentException($"Device name cannot be longer than {DeviceNameMaxLength} characters.");

			if (TeVirtualMidiLogging ?? true)
				TeVirtualMIDI.logging(TeVirtualMIDI.TE_VM_LOGGING_MISC | TeVirtualMIDI.TE_VM_LOGGING_RX | TeVirtualMIDI.TE_VM_LOGGING_TX);

			VirtualDevice virtualDevice;

			if (virtualDeviceType == VirtualDeviceType.Input)
				virtualDevice = new VirtualInputDevice(name,
					ref manufacturerID, ref productID);
			else if (virtualDeviceType == VirtualDeviceType.Output)
				virtualDevice = new VirtualOutputDevice(name,
					ref manufacturerID, ref productID);
			else
			{
				virtualDevice = new VirtualLoopbackDevice(name,
					ref manufacturerID, ref productID);
				((VirtualLoopbackDevice)virtualDevice).Start();
			}

			VirtualDevices.Add(virtualDevice);
            VirtualDeviceAdd.OnNext(virtualDevice);
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
                VirtualDeviceRemove.OnNext(name);
            }
			else
				throw new Exception($"No device with name {name}.");
		}

		public bool DoesDeviceExist(string name)
		{
			var returnValue = VirtualDevices[name] != null;
			return returnValue;
		}

		public void Dispose()
		{
			VirtualDevices.ForEach(x => x.Dispose());
			VirtualDevices = null;
			TeVirtualMidiLogging = null;
		}
	}
}
