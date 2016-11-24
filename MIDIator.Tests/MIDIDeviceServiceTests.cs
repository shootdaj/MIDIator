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
			MIDIDeviceService = null;
		}

		[Test]
		public void Devices_ReturnsSomeDevices()
		{
			var devices = MIDIDeviceService.AvailableInputDevices;
		}
	}
}
