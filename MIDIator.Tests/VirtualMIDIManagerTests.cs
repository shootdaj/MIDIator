using System;
using System.Linq;
using System.Threading;
using MIDIator.Engine;
using NUnit.Framework;

namespace MIDIator.Tests
{
	public class VirtualMIDIManagerTests
	{
		private MIDIManager MIDIManager { get; set; }

		[SetUp]
		public void Setup()
		{
			MIDIManager = new MIDIManager();
		}

		[TearDown]
		public void TearDown()
		{
			MIDIManager = null;
		}

		[Test]
		public void CreateVirtualDevice_CanBeSeenByMIDIManager()
		{
			var virtualMIDIManager = new VirtualMIDIManager();

			var testDeviceName = "TestMIDIatorDevice";
			var testDevice = virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);

			Thread.Sleep(1000);

			Assert.That(virtualMIDIManager.VirtualDevices.Contains(testDevice));
			Assert.That(MIDIManager.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));

			//cleanup
			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();

			Thread.Sleep(1000);
		}

		[Test]
		public void RemoveVirtualMIDIDevice_CannotBeSeenByMIDIManager()
		{
			var virtualMIDIManager = new VirtualMIDIManager();

			var testDeviceName = "TestMIDIatorDevice";
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input, false);

			Thread.Sleep(1000);

			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			
			Assert.That(!MIDIManager.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));

			Thread.Sleep(1000);
		}
	}
}
