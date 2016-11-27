using System;
using System.Linq;
using System.Threading;
using MIDIator.Engine;
using NUnit.Framework;

namespace MIDIator.Tests
{
	[Category("LocalOnly")]
	public class VirtualMIDIManagerTests
	{
		[Test, RunInApplicationDomain]
		public void CreateVirtualDevice_CanBeSeenByMIDIManager()
		{
			var virtualMIDIManager = new VirtualMIDIManager();

			var testDeviceName = "TestMIDIatorDevice_CreateVirtualDevice_CanBeSeenByMIDIManager";
			var testDevice = virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);

			Assert.That(virtualMIDIManager.VirtualDevices.Contains(testDevice));
			Assert.That(new MIDIDeviceService().AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));

			//cleanup
			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();
		}

		[Test, RunInApplicationDomain, Ignore("Fix")]
		public void RemoveVirtualMIDIDevice_CannotBeSeenByMIDIManager()
		{
			var virtualMIDIManager = new VirtualMIDIManager();
			var midiDeviceService = new MIDIDeviceService();

			var testDeviceName = "TestMIDIatorDevice_RemoveVirtualMIDIDevice_CannotBeSeenByMIDIManager";
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);
			Assert.That(midiDeviceService.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));

			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			Assert.That(midiDeviceService.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName), Is.False);

			virtualMIDIManager.Dispose();
		}
	}
}
