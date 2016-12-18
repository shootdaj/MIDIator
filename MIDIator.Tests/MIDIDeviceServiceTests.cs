using System.Linq;
using Anshul.Utilities;
using MIDIator.Engine;
using NUnit.Framework;

namespace MIDIator.Tests
{
	public class MIDIDeviceServiceTests
	{
		private MIDIDeviceService MIDIDeviceService { get; set; }

		[SetUp]
		public void Setup()
		{
			MIDIDeviceService = new MIDIDeviceService();
		}

		[TearDown]
		public void TearDown()
		{
			MIDIDeviceService.Dispose();
			MIDIDeviceService = null;
		}

		[Test]
		public void Devices_ReturnsSomeDevices()
		{
			var devices = MIDIDeviceService.AvailableInputDevices;
		}

		[Test]
		[Category("LocalOnly")]
		[Ignore("This logic was moved to Tranformation.InitFromServices. Put the test around that logic instead")]
		public void GetInputDevice_WithVirtualMIDIManager_VerifyExistenceOfCorrespondingVirtualOutputDevice()
		{
			var virtualMIDIManager = new VirtualMIDIManager();
			var inputDevice = MIDIDeviceService.GetInputDevice(0);//, virtualMIDIManager: virtualMIDIManager);
			Assert.DoesNotThrow(() => MIDIDeviceService.GetOutputDevice(
				Extensions.GetVirtualDeviceName(inputDevice.Name)));

			virtualMIDIManager.Dispose();

		}

		[Test, RunInApplicationDomain]
		[Category("LocalOnly")]
		public void GetInputDevice_WithoutVirtualMIDIManager_VerifyAbsenceOfCorrespondingVirtualOutputDevice()
		{
			var inputDevice = MIDIDeviceService.GetInputDevice(0);
			var virtualOutputDeviceName = Extensions.GetVirtualDeviceName(inputDevice.Name);
			Assert.That(() => MIDIDeviceService.AvailableOutputDevices.Select(x => x.Name).Contains(virtualOutputDeviceName).Not());
		}

		[Test, RunInApplicationDomain]
		[Category("LocalOnly")]
		[Ignore("This logic was moved to Tranformation.InitFromServices. Put the test around that logic instead")]
		public void RemoveInputDevice_WithVirtualMIDIManager_VerifyAbsenceOfCorrespondingVirtualOutputDevice()
		{
			//arrange
			var virtualMIDIManager = new VirtualMIDIManager();
			var inputDevice = MIDIDeviceService.GetInputDevice(0);//, virtualMIDIManager: virtualMIDIManager);
			var virtualOutputDeviceName = Extensions.GetVirtualDeviceName(inputDevice.Name);

			Assert.DoesNotThrow(() => MIDIDeviceService.GetOutputDevice(virtualOutputDeviceName));

			//act
			MIDIDeviceService.RemoveInputDevice(inputDevice, virtualMIDIManager);

			//assert
			Assert.That(() => MIDIDeviceService.AvailableOutputDevices.Select(x => x.Name).Contains(virtualOutputDeviceName).Not());

			//cleanup
			virtualMIDIManager.Dispose();
		}
	}
}
