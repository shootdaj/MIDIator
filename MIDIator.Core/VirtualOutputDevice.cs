using System;
using Refigure;
using TobiasErichsen.teVirtualMIDI;

namespace MIDIator
{	
	public class VirtualOutputDevice : IDisposable
	{
		public VirtualOutputDevice(string name, ref Guid manufacturerID, ref Guid productID, bool prefixName = true)
		{
			Name = name;
			TeVirtualMIDIDevice = new TeVirtualMIDI(prefixName ? $"{Config.Get("Core.VirtualOutputDevicePrefix")}{name}" : name, 65535, TeVirtualMIDI.TE_VM_FLAGS_PARSE_RX,
				ref manufacturerID, ref productID);
		}

		private TeVirtualMIDI TeVirtualMIDIDevice { get; set; }

		public string Name { get; private set; }
		public void Dispose()
		{
			TeVirtualMIDIDevice.shutdown();
		}
	}
}