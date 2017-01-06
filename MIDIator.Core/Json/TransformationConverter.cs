﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using MIDIator.Engine;
using MIDIator.Interfaces;
using MIDIator.UIGenerator.Consumables;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MIDIator.Json
{
    public class ProfileConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Profile));
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
	        var virtualLoopbackDevices = jo["virtualLoopbackDevices"];

			LoadVirtualLoopbackDevices(virtualLoopbackDevices);
	        
			//jo.SelectToken("asdf").Mo

            var translationMap = jo["translationMap"].ToObject<TranslationMap>(serializer);
            var transformation = new Transformation(jo["name"].ToString(), 
                jo["inputDevice"]["name"].ToString(), 
                jo["outputDevice"]["name"].ToString(), 
                translationMap,
                jo["linkedOutputVirtualDevice"].ToObject<bool>(serializer),
                MIDIManager.Instance.MIDIDeviceService, 
                MIDIManager.Instance.VirtualMIDIManager);

            return transformation;
        }

	    private static void LoadVirtualLoopbackDevices(JToken virtualLoopbackDevices)
	    {
		    foreach (var virtualLoopbackDevice in virtualLoopbackDevices)
		    {
			    MIDIManager.Instance.VirtualMIDIManager.CreateVirtualDevice(virtualLoopbackDevice["name"].ToString(),
				    Guid.Parse(virtualLoopbackDevice["manufacturerID"].ToString()),
				    Guid.Parse(virtualLoopbackDevice["productID"].ToString()), VirtualDeviceType.Loopback);
		    }
	    }

	    public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
