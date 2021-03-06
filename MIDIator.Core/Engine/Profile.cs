﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Anshul.Utilities;
using MIDIator.Interfaces;
using MIDIator.Json;
using MIDIator.Services;
using MIDIator.UIGeneration;
using MIDIator.UIGenerator.Consumables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TypeLite;

namespace MIDIator.Engine
{
	/// <summary>
	/// Defines a profile that can be loaded from a persistent store.
	/// </summary>
	[TsClass(Module = "")]
	[Ng2Component(typeof(ProfileView), componentCodeTemplate: typeof(ProfileComponentCode))]//, new[] {typeof(Transformation), typeof(VirtualOutputDevice)})]
	public class Profile : IProfile
	{
		public string Name { get; set; }

		public List<Transformation> Transformations { get; set; } = new List<Transformation>();

        public BetterList<VirtualLoopbackDevice> VirtualLoopbackDevices { get; set; } = new BetterList<VirtualLoopbackDevice>();

        public bool Collapsed { get; set; } = false;

        //[JsonProperty(nameof(VirtualLoopbackDevices))]
        //private BetterList<VirtualLoopbackDevice> VirtualLoopbackDevicesJson => VirtualLoopbackDevices;

        public void Update(JObject profileDTO, MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager)
        {
            var serializer = JsonSerializer.Create(SerializerSettings.DefaultSettings);

            Name = profileDTO["name"].ToString();
	        Collapsed = profileDTO["collapsed"].ToObject<bool>(serializer);
            MIDIManager.Instance.ProfileService.LoadVirtualLoopbackDevices(profileDTO, this);
            MIDIManager.Instance.ProfileService.LoadTransformations(serializer, profileDTO, this);
		}
	}
}

