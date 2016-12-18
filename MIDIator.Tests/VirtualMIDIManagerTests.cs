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

			var testDeviceName = "TestDevice" + new Random().Next(1, 100);
			var testDevice = virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input);

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

			var testDeviceName = "TestDevice" + new Random().Next(1, 100);
			virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input);
			Assert.That(midiDeviceService.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));

			new Thread(() => virtualMIDIManager.RemoveVirtualDevice(testDeviceName)).Start();

			Thread.Sleep(50);

			Assert.That(midiDeviceService.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName), Is.False);

			virtualMIDIManager.Dispose();

			//virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			//Assert.That(midiDeviceService.AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName), Is.False);

			//virtualMIDIManager.Dispose();
		}

		public void CreateVirtualDevice_DeviceCreatedWhenDeviceNameUpTo31Characters()
		{
			var testDeviceName = "TestDevice111111111111111111111";
			var virtualMIDIManager = new VirtualMIDIManager();

			var testDevice = virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Input);

			Assert.That(virtualMIDIManager.VirtualDevices.Contains(testDevice));
			Assert.That(new MIDIDeviceService().AvailableInputDevices.Select(x => x.Name).Contains(testDeviceName));

			//cleanup
			virtualMIDIManager.RemoveVirtualDevice(testDeviceName);
			virtualMIDIManager.Dispose();
		}

		public void CreateVirtualDevice_ExceptionWhenDeviceNameGreaterThan31Characters()
		{
			var testDeviceName = "TestDevice1111111111111111111111";
			var virtualMIDIManager = new VirtualMIDIManager();

			Assert.Throws<ArgumentException>(() =>
			{
				var testDevice = virtualMIDIManager.CreateVirtualDevice(testDeviceName, Guid.NewGuid(), Guid.NewGuid(),
					VirtualDeviceType.Input);
			});
			
			//cleanup
			virtualMIDIManager.Dispose();
		}

	}
}
