﻿using NUnit.Framework;

namespace MIDIator.Tests
{
	public class MIDIManagerTests
	{
		[Test]
		public void Devices_ReturnsSomeDevices()
		{
			var devices = MIDIManager.AvailableInputDevices;
		}

		//[Test]
		//public void AddChannelMessageAction_Works()
		//{
		//	var device = MIDIManager.CreateInputDevice(3);
		//	device.AddChannelMessageAction((sender, args) =>
		//	{
		//		Assert.True(true);
		//	});
		//	device.Start();
		//	Thread.Sleep(15000);
		//	device.Stop();
		//}

		//[Test]
		//public void AddChannelMessageAction_Works_AfterStartRecording()
		//{
		//	var device = MIDIManager.CreateInputDevice(3);
		//	device.Start();
		//	device.AddChannelMessageAction((sender, args) =>
		//	{
		//		Assert.True(true);
		//	});
		//	Thread.Sleep(15000);
		//	device.Stop();
		//}
	}
}
