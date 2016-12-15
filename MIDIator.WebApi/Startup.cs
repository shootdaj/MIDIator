﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Formatting;
using System.Web.Http;
using Anshul.Utilities;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Cors;
using MIDIator.Engine;
using MIDIator.Json;
using Newtonsoft.Json;
using Owin;
using Refigure;
using Sanford.Multimedia.Midi;

namespace MIDIator.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			HttpConfiguration config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{action}",
				defaults: new { controller = "MIDIManager", action = "Index" }

			);

			//ignore glimpse route
			config.Routes.IgnoreRoute("Glimpse", "{resource}.axd/{*pathInfo}");

			//always return json
			config.Formatters.Clear();
			config.Formatters.Add(new JsonMediaTypeFormatter());

			//set default serialization settings
			config.Formatters.JsonFormatter.SerializerSettings = SerializerSettings.DefaultSettings;
			JsonConvert.DefaultSettings = () => SerializerSettings.DefaultSettings;

			//add api services
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
			app.UseWebApi(config);

			var inputDeviceName = "TouchOSC Bridge";// "Launchpad";
			var outputDeviceName = string.Empty;

			//initialize midi manager
			MIDIManager.Instantiate(new MIDIDeviceService(), new VirtualMIDIManager());

			//create translation map - this should be loaded from a service or from disk or something
			var translationMap = new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 66),
					new ChannelMessage(ChannelCommand.ProgramChange, 1, 23),
					InputMatchFunction.NoteMatch, TranslationFunction.DirectTranslation).Listify()
			);

			//set initial profile - this should be loaded from a service or from disk or something
			MIDIManager.Instance.SetProfile(new Profile()
			{
				Name = "DefaultProfile",
				Transformations = new List<Transformation>()
				{
					new Transformation("TouchOSCXForm", inputDeviceName, string.Empty, translationMap, 
					true, MIDIManager.Instance.MIDIDeviceService, MIDIManager.Instance.VirtualMIDIManager)
				}
				//,
				//VirtualOutputDevices = new List<VirtualOutputDevice>()
				//{
				//	(VirtualOutputDevice)MIDIManager.Instance.VirtualMIDIManager.CreateVirtualDevice("TestVirtualDevice", Guid.NewGuid(), Guid.NewGuid(), VirtualDeviceType.Output)
				//}
			});
        }
    }
}
