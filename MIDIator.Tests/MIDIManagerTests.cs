using MIDIator.Engine;
using MIDIator.Services;
using NUnit.Framework;

namespace MIDIator.Tests
{
	public class MIDIManagerTests
	{
		private MIDIManager MIDIManager { get; set; }

		[SetUp]
		public void Setup()
		{
		    var midiDeviceService = new MIDIDeviceService();
		    var virtualMIDIManager = new VirtualMIDIManager();
		    MIDIManager = new MIDIManager(midiDeviceService, new ProfileService(midiDeviceService, virtualMIDIManager), virtualMIDIManager);
		}

		[TearDown]
		public void TearDown()
		{
			MIDIManager = null;
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
