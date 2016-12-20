using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using MIDIator.Engine;
using MIDIator.Web;
using Refigure;

namespace MIDIator.Manager
{
	public class Manager : IDisposable
	{
		public WebAPIManager WebAPIManager = new WebAPIManager();
		
		private Process WebClientProcess { get; set; } = new Process();

		private VirtualMIDIManager VirtualMIDIManager { get; set; } = new VirtualMIDIManager();

		private IDisposable WebAPIProcess { get; set; }

		public bool Running { get; private set; } = false;

		public void Start()
		{
			StartWebAPI();
			StartWebClient();
			Running = true;
		}

		public void Stop()
		{
			StopWebClient();
			StopWebAPI();
			Running = false;
		}

		private void StartWebAPI()
		{
			string baseAddress = Config.Get("WebApi.BaseAddress");
			WebAPIProcess = WebApp.Start<Startup>(baseAddress);
			WebAPIManager.InitializeWebAPI(VirtualMIDIManager);
		}

		private void StopWebAPI()
		{
			WebAPIManager.DisposeWebAPI();
			WebAPIProcess.Dispose();
		}

		private void StartWebClient()
		{

		}

		private void StopWebClient()
		{

		}


		public void Dispose()
		{
			VirtualMIDIManager.Dispose();
		}
	}
}
