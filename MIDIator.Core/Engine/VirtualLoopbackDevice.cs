using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MIDIator.VirtualMIDI;
using Sanford.Multimedia.Midi;

namespace MIDIator.Engine
{
	public class VirtualLoopbackDevice : VirtualDevice
	{

		private readonly Thread _loopbackThread;

		public VirtualLoopbackDevice(string name, ref Guid manufacturerID, ref Guid productID)
			: base(name, ref manufacturerID, ref productID, VirtualDeviceType.Loopback)
		{
			_loopbackThread = new Thread(() =>
			{
				//Running = true;
				while (true) //technically this will wait for one final command before quitting
								//but it will be shutdown during the final dispose
				{
					var command = TeVirtualMIDIDevice.getCommand();
					TeVirtualMIDIDevice.sendCommand(command);
				}

			});
		}

		public void Start()
		{
			_loopbackThread.Start();
		}



		public void Stop()
		{
			_loopbackThread.Abort();
		}

		public override void Dispose()
		{
			Stop();
			base.Dispose();
		}
	}
}
