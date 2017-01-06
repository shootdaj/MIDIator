using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using MIDIator.Engine;
using MIDIator.Json;
using MIDIator.Services;
using Newtonsoft.Json;
using Refigure;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
    public static class Extensions
    {
        public static ChannelMessage ToChannelMessage(this ShortMessage shortMessage)
        {
            return (ChannelMessage)shortMessage;
        }

        public static T ConvertAsJsonTo<T>(this ExpandoObject inObject)
        {
            var serializedTranslationMap = JsonConvert.SerializeObject(inObject);
            var deserializedObject = JsonConvert.DeserializeObject<T>(serializedTranslationMap, SerializerSettings.DefaultSettings);
            return deserializedObject;
        }

        public static string GetVirtualDeviceName(string deviceName)
        {
            var returnValue = $"{GetVirtualDeviceNamePrefix()}{deviceName}";
            if (returnValue.Length > VirtualMIDIManager.DeviceNameMaxLength)
            {
                returnValue = returnValue.Substring(0, VirtualMIDIManager.DeviceNameMaxLength);
            }
            return returnValue;
        }

        private static string GetVirtualDeviceNamePrefix()
        {
            var configPrefix = Config.Get("Core.VirtualOutputDevicePrefix");
            return string.IsNullOrEmpty(configPrefix) ? "M-" : configPrefix;
        }
    }
}
