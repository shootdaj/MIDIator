using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MIDIator.Tests
{
	public class MIDIManagerTests
	{
		[Fact]
		public void Devices_ReturnsSomeDevices()
		{
			var devices = MIDIManager.AvailableDevices;
		}

		[Fact]
		public void AddChannelRecievedAction_Works()
		{
			MIDIManager.AddChannelRecievedAction(3, (sender, args) =>
			{
				Assert.True(true);
			});

			MIDIManager.StartRecording(3);
			Thread.Sleep(15000);
		}
    }
}
