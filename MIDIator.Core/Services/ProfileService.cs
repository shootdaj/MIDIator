﻿using System;
using System.Collections.Generic;
using System.Linq;
using MIDIator.Engine;
using MIDIator.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MIDIator.Services
{
    public class ProfileService
    {
        public MIDIDeviceService MIDIDeviceService { get; set; }
        public VirtualMIDIManager VirtualMIDIManager { get; set; }

        public ProfileService(MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager)
        {
            MIDIDeviceService = midiDeviceService;
            VirtualMIDIManager = virtualMIDIManager;
        }

        public Profile CreateFromJSON(JsonReader reader, JsonSerializer serializer)
        {
            var profileDTO = JObject.Load(reader);
            var profile = new Profile { Name = profileDTO["name"].ToString() };

            LoadVirtualLoopbackDevices(profileDTO, profile);
            LoadTransformations(serializer, profileDTO, profile);

            return profile;
        }

        public void LoadTransformations(JsonSerializer serializer, JObject profileDTO, Profile profile)
        {
            var transformationNames = new List<string>();
            foreach (var transformationDTO in profileDTO["transformations"])
            {
                transformationNames.Add(transformationDTO["name"].ToString());

                var matchedTransformations = profile.Transformations.Where(t => t.Name == transformationDTO["name"].ToString());

                //TODO: check which transformations are different - currently updating all transformations
                var matchedTransformationsList = matchedTransformations.ToList();
                if (matchedTransformationsList.Any())
                {
                    IMIDIInputDevice inputDevice;
                    bool linkedOutputVirtualDevice;
                    IMIDIOutputDevice outputDevice;
                    TranslationMap translationMap;
	                bool enabled;
	                bool collapsed;
	                bool translationsCollapsed;
                    var matchedTransformation = matchedTransformationsList.Single();

                    GetTransformationProperties(serializer, transformationDTO, profile, out inputDevice, out linkedOutputVirtualDevice, out outputDevice, out translationMap, out enabled, out collapsed, out translationsCollapsed);

					matchedTransformation.Enabled = enabled;
                    matchedTransformation.InputDevice = inputDevice;
                    matchedTransformation.OutputDevice = outputDevice;
                    matchedTransformation.TranslationMap = translationMap;
                    matchedTransformation.LinkedOutputVirtualDevice = linkedOutputVirtualDevice;
                    matchedTransformation.Collapsed = collapsed;
                    matchedTransformation.TranslationsCollapsed = translationsCollapsed;

	                if (!matchedTransformation.Enabled && matchedTransformation.InputDevice.IsRecording)
	                {
		                matchedTransformation.InputDevice.Stop();
	                }

					if (matchedTransformation.Enabled && !matchedTransformation.InputDevice.IsRecording)
					{
						matchedTransformation.InputDevice.Start();
					}

				}
                else
                {
                    //TODO: Test
                    //else create new transformations	
                    IMIDIInputDevice inputDevice;
                    bool linkedOutputVirtualDevice;
                    IMIDIOutputDevice outputDevice;
                    TranslationMap translationMap;
                    bool collapsed;
                    bool translationsCollapsed;
                    bool enabled;

                    GetTransformationProperties(serializer, transformationDTO, profile, out inputDevice, out linkedOutputVirtualDevice, out outputDevice, out translationMap, out enabled, out collapsed, out translationsCollapsed);

                    var transformation = new Transformation(transformationDTO["name"].ToString(),
                        inputDevice, outputDevice, translationMap, linkedOutputVirtualDevice, enabled, collapsed, translationsCollapsed);

                    profile.Transformations.Add(transformation);
                }
            }

            profile.Transformations.RemoveAll(t => !transformationNames.Contains(t.Name));
        }

        private void GetTransformationProperties(JsonSerializer serializer,
            JToken transformationDTO, Profile profile, out IMIDIInputDevice inputDevice, out bool linkedOutputVirtualDevice,
            out IMIDIOutputDevice outputDevice, out TranslationMap translationMap, out bool enabled, out bool collapsed, out bool translationsCollapsed)
        {
            var inputDeviceName = transformationDTO["inputDevice"]["name"].ToString();
            var outputDeviceName = transformationDTO["outputDevice"]["name"].ToString();

            inputDevice = MIDIDeviceService.GetInputDevice(inputDeviceName, failSilently: true);
            linkedOutputVirtualDevice = transformationDTO["linkedOutputVirtualDevice"].ToObject<bool>(serializer);
            collapsed = transformationDTO["collapsed"]?.ToObject<bool>(serializer) ?? false;
            translationsCollapsed = transformationDTO["translationsCollapsed"]?.ToObject<bool>(serializer) ?? false;

            if (linkedOutputVirtualDevice)
            {
                MIDIDeviceService.CreateVirtualOutputDeviceForInputDevice(inputDevice, VirtualMIDIManager, profile);
                outputDevice = MIDIDeviceService.GetOutputDevice(Extensions.GetVirtualDeviceName(inputDeviceName));
            }
            else
            {
                outputDevice = MIDIDeviceService.GetOutputDevice(outputDeviceName, failSilently: true);
            }

            translationMap = transformationDTO["translationMap"].ToObject<TranslationMap>(serializer);
	        enabled = transformationDTO["enabled"].ToObject<bool>(serializer);
        }

        public void LoadVirtualLoopbackDevices(JObject profileDTO, Profile profile)
        {
            //load each virtualdevice in dto
            if (profileDTO.Children().Select(x => x.Path).Contains("virtualLoopbackDevices"))
                foreach (var virtualLoopbackDeviceDTO in profileDTO["virtualLoopbackDevices"])
                {
                    var name = virtualLoopbackDeviceDTO["name"].ToString();
                    if (
                        !VirtualMIDIManager.DoesDeviceExist(name))
                    {
                        var virtualDevice =
                            VirtualMIDIManager.CreateVirtualDevice(name,
                                Guid.Parse(virtualLoopbackDeviceDTO["manufacturerID"].ToString()),
                                Guid.Parse(virtualLoopbackDeviceDTO["productID"].ToString()), VirtualDeviceType.Loopback);

                        profile.VirtualLoopbackDevices.Add((VirtualLoopbackDevice)virtualDevice);
                    }
                    else if (!profile.VirtualLoopbackDevices.Select(x => x.Name).Contains(name))
                    {
                        profile.VirtualLoopbackDevices.Add((VirtualLoopbackDevice)VirtualMIDIManager.VirtualDevices[name]);
                    }
                }
        }
    }
}