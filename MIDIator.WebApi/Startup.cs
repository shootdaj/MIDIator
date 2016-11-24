﻿using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Web.Http;
using Anshul.Utilities;
using Microsoft.Owin.Cors;
using MIDIator.Engine;
using MIDIator.Json;
using Newtonsoft.Json;
using Owin;
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

			config.Routes.IgnoreRoute("Glimpse", "{resource}.axd/{*pathInfo}");
			config.Formatters.JsonFormatter.SerializerSettings = SerializerSettings.DefaultSettings;

			app.MapSignalR();
			app.UseCors(CorsOptions.AllowAll);
			app.UseWebApi(config);

			JsonConvert.DefaultSettings = () => SerializerSettings.DefaultSettings;

			//initialize midi manager
			var inputDeviceName = "TouchOSC Bridge";
			var outputDeviceName = //"TouchOSC Bridge";
				"Microsoft GS Wavetable Synth";

			MIDIManager.Instantiate(new MIDIDeviceService());

			MIDIManager.Instance.SetProfile(new Profile()
			{
				Name = "DefaultProfile",
				Transformations = new List<Transformation>()
				{
					new Transformation("TouchOSCXForm",
						MIDIManager.Instance.MIDIDeviceService.GetInputDevice(inputDeviceName),
						MIDIManager.Instance.MIDIDeviceService.GetOutputDevice(outputDeviceName),
						new TranslationMap(new Translation(new ChannelMessage(ChannelCommand.NoteOn, 1, 66),
								new ChannelMessage(ChannelCommand.ProgramChange, 1, 23),
								InputMatchFunction.NoteMatch, TranslationFunction.DirectTranslation).Listify()
						))
				}
			});

			//MIDIManager.Instance.CurrentProfile.Update();
		}
	}
}
