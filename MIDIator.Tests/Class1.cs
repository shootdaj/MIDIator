using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MIDIator.Tests
{
	public class MIDIManagerTests
	{
		[Fact]
		public void Devices_ReturnsSomeDevices()
		{
			var devices = MIDIManager.Devices;
		}
	}
}
