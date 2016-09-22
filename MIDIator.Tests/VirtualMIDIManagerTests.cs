using System;
using System.Linq;
using NUnit.Framework;

namespace MIDIator.Tests
{
	public class VirtualMIDIManagerTests
	{
		[Test]
		public void CreateVirtualDevice_CanBeSeenByMIDIManager()
		{
			var testDeviceName = "TestMIDIatorDevice";
			var testDevice = VirtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), false);

			Assert.That(VirtualMIDIManager.VirtualDevices.Contains(testDevice));
			Assert.That(MIDIManager.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));
		}

		[Test]
		public void RemoveVirtualMIDIDevice_CannotBeSeenByMIDIManager()
		{
			var testDeviceName = "TestMIDIatorDevice";
			VirtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), false);
			VirtualMIDIManager.RemoveVirtualDevice(testDeviceName);

			Assert.That(!MIDIManager.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));
		}
	}
}
