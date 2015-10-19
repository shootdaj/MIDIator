using System.Threading;
using Xunit;

namespace MIDIator.Tests
{
	public class MIDIManagerTests
	{
		[Fact]
		public void Devices_ReturnsSomeDevices()
		{
			var devices = MIDIManager.FreeDevices;
		}

		[Fact]
		public void AddChannelMessageAction_Works()
		{
			var device = MIDIManager.CreateDevice(3);
			device.AddChannelMessageAction((sender, args) =>
			{
				Assert.True(true);
			});
			device.StartRecording();
			Thread.Sleep(15000);
			device.StopRecording();
		}

		[Fact]
		public void AddChannelMessageAction_Works_AfterStartRecording()
		{
			var device = MIDIManager.CreateDevice(3);
			device.StartRecording();
			device.AddChannelMessageAction((sender, args) =>
			{
				Assert.True(true);
			});
			Thread.Sleep(15000);
			device.StopRecording();
		}
	}
}
