﻿using System;
using Microsoft.Owin.Hosting;
using MIDIator.Engine;
using MIDIator.Web;
using MIDIator.WebClient;
using Refigure;

namespace MIDIator.Manager
{
	public class Manager : IDisposable
	{
		private WebAPIManager WebAPIManager = new WebAPIManager();
		
		private WebClientManager WebClientManager = new WebClientManager();

		private VirtualMIDIManager VirtualMIDIManager { get; set; } = new VirtualMIDIManager();

		private IDisposable WebAPIProcess { get; set; }

		public bool Running { get; private set; } = false;

		public void Start(Action<Exception> errorAction, Action onShutdown)
		{
		    try
		    {
		        StartWebAPI(onShutdown);
		        StartWebClient();
		        Running = true;
		    }
		    catch (Exception ex)
		    {
		        errorAction(ex);
		    }
		}

		public void Stop()
		{
			StopWebClient();
			StopWebAPI();
			Running = false;
		}

		private void StartWebAPI(Action onShutdown)
		{
			string baseAddress = Config.Get("WebAPI.BaseAddress");
			WebAPIProcess = WebApp.Start<Startup>(baseAddress);
			WebAPIManager.InitializeWebAPI(VirtualMIDIManager, onShutdown);
		}

		private void StopWebAPI()
		{
			WebAPIManager.DisposeWebAPI();
			WebAPIProcess.Dispose();
		}



		private void StartWebClient()
		{
			string baseAddress = Config.Get("WebClient.BaseAddress");
			WebClientManager.InitializeWebClient(baseAddress);
		}

		private void StopWebClient()
		{
			WebClientManager.DisposeWebClient();
		}

	    private void StartExitListener()
	    {
	        
	    }
		public void Dispose()
		{
			VirtualMIDIManager.Dispose();
		}
	}
}
