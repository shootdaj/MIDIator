using System;
using System.Linq;
using System.Threading;
using MIDIator.Engine;
using NUnit.Framework;

namespace MIDIator.Tests
{
	public class VirtualMIDIManagerTests
	{
		//private MIDIManager MIDIManager { get; set; }

		//[SetUp]
		//public void Setup()
		//{
		//	MIDIManager = new MIDIManager(new MIDIDeviceService());
		//}

		//[TearDown]
		//public void TearDown()
		//{
		//	MIDIManager = null;
		//}

		[Test, RunInApplicationDomain]
		public void CreateVirtualDevice_CanBeSeenByMIDIManager()
		{
			var virtualMIDIManager = new VirtualMIDIManager();

			var testDeviceName = "TestMIDIatorDevice";
			var testDevice = virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);

			Assert.That(virtualMIDIManager.VirtualDevices.Contains(testDevice));
			Assert.That(new MIDIDeviceService().AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));

			//cleanup
			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();
		}

		[Test, RunInApplicationDomain]
		public void RemoveVirtualMIDIDevice_CannotBeSeenByMIDIManager()
		{
			var virtualMIDIManager = new VirtualMIDIManager();
			var midiDeviceService = new MIDIDeviceService();

			var testDeviceName = "TestMIDIatorDevice";
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);
			Assert.That(midiDeviceService.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));

			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			Assert.That(midiDeviceService.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName), Is.False);

			virtualMIDIManager.Dispose();
		}
	}
}
