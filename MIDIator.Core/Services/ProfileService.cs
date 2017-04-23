using System;
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
		private Action<IBroadcastPayload, string> BroadcastAction { get; set; }

        public ProfileService(MIDIDeviceService midiDeviceService, VirtualMIDIManager virtualMIDIManager, Action<IBroadcastPayload, string> broadcastAction)
        {
            MIDIDeviceService = midiDeviceService;
            VirtualMIDIManager = virtualMIDIManager;
	        BroadcastAction = broadcastAction;
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
            var transformationIDs = new List<Guid>();
            foreach (var transformationDTO in profileDTO["transformations"])
            {
	            var id = Guid.Parse(transformationDTO["id"].ToString());
	            transformationIDs.Add(id);

	            var matchedTransformations =
		            profile.Transformations.Where(t => t.ID.Equals(id));

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
	                string name;

                    GetTransformationProperties(serializer, transformationDTO, profile, out inputDevice, 
						out linkedOutputVirtualDevice, out outputDevice, out translationMap, out enabled, 
						out collapsed, out translationsCollapsed, out name, BroadcastAction);

					matchedTransformation.Name = name;
					matchedTransformation.Enabled = enabled;
                    matchedTransformation.InputDevice = inputDevice;
                    matchedTransformation.OutputDevice = outputDevice;
                    matchedTransformation.TranslationMap = translationMap;
                    matchedTransformation.LinkedOutputVirtualDevice = linkedOutputVirtualDevice;
                    matchedTransformation.Collapsed = collapsed;
                    matchedTransformation.TranslationsCollapsed = translationsCollapsed;

                    matchedTransformation.InputDevice.RemoveChannelMessageAction("SendToOutput");
                    matchedTransformation.InputDevice.AddChannelMessageAction(ChannelMessageAction.SendToOutput(matchedTransformation.OutputDevice));

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
	                string name;

                    GetTransformationProperties(serializer, transformationDTO, profile, 
						out inputDevice, out linkedOutputVirtualDevice, out outputDevice, 
						out translationMap, out enabled, out collapsed, out translationsCollapsed, 
						out name, BroadcastAction);

                    var transformation = new Transformation(name, inputDevice, outputDevice, 
						translationMap, linkedOutputVirtualDevice, 
						enabled, collapsed, translationsCollapsed, id);

                    profile.Transformations.Add(transformation);
                }
            }

            profile.Transformations.RemoveAll(t => !transformationIDs.Contains(t.ID));
        }

        private void GetTransformationProperties(JsonSerializer serializer,
            JToken transformationDTO, Profile profile, out IMIDIInputDevice inputDevice, out bool linkedOutputVirtualDevice,
            out IMIDIOutputDevice outputDevice, out TranslationMap translationMap, out bool enabled, out bool collapsed, out bool translationsCollapsed, out string name, Action<IBroadcastPayload, string> broadcastAction = null)
        {
            var inputDeviceName = transformationDTO["inputDevice"]["name"].ToString();
            var outputDeviceName = transformationDTO["outputDevice"]["name"].ToString();

			name = transformationDTO["name"]?.ToObject<string>(serializer);
			inputDevice = MIDIDeviceService.GetInputDevice(inputDeviceName, failSilently: true, broadcastAction: broadcastAction) ??
			              MIDIDeviceService.GetInputDevice(0, failSilently: true, broadcastAction: broadcastAction);

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