using System;
using Anshul.Utilities;
using MIDIator.VirtualMIDI;
using Refigure;
using TypeLite;

namespace MIDIator.Engine
{
	[TsClass(Module = "")]
	public class VirtualDevice : IDisposable, IBetterListType
	{
		protected VirtualDevice(string name, ref Guid manufacturerID, ref Guid productID, VirtualDeviceType virtualDeviceType)
		{
			Name = name;
			TeVirtualMIDIDevice = new TeVirtualMIDI(name,
				65535,
				virtualDeviceType == VirtualDeviceType.Input
					? TeVirtualMIDI.TE_VM_FLAGS_INSTANTIATE_TX_ONLY :
					virtualDeviceType  == VirtualDeviceType.Output ?
					TeVirtualMIDI.TE_VM_FLAGS_INSTANTIATE_RX_ONLY : TeVirtualMIDI.TE_VM_FLAGS_INSTANTIATE_BOTH ,
				ref manufacturerID, ref productID);
		}

		protected TeVirtualMIDI TeVirtualMIDIDevice { get; set; }

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
		Output,
		Loopback
	}
}