using System;
using MIDIator.VirtualMIDI;
using Refigure;
using TypeLite;

namespace MIDIator.Engine
{
	[TsClass(Module = "")]
	public class VirtualDevice : IDisposable
	{
		protected VirtualDevice(string name, ref Guid manufacturerID, ref Guid productID, VirtualDeviceType virtualDeviceType, bool prefixName = true)
		{
			Name = name;
			TeVirtualMIDIDevice = new TeVirtualMIDI(prefixName ? $"{Config.Get("Core.VirtualOutputDevicePrefix")}{name}" : name,
				65535,
				virtualDeviceType == VirtualDeviceType.Input
					? TeVirtualMIDI.TE_VM_FLAGS_INSTANTIATE_TX_ONLY
					: TeVirtualMIDI.TE_VM_FLAGS_INSTANTIATE_RX_ONLY,
				ref manufacturerID, ref productID);
		}

		private TeVirtualMIDI TeVirtualMIDIDevice { get; set; }

		public string Name { get; private set; }
		public void Dispose()
		{
			TeVirtualMIDIDevice.shutdown();
			TeVirtualMIDIDevice = null;
		}
	}

	public enum VirtualDeviceType
	{
		Input,
		Output
	}
}