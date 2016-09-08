using System;
using System.Collections.Generic;
using System.Linq;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public class MIDIInputDevice : IDisposable, IMIDIInputDevice
	{
		private InputDevice InputDevice { get; }

		public MIDIInputDevice(int deviceID, ITranslationMap translationMap = null)
		{
			InputDevice = new InputDevice(deviceID);
			TranslationMap = translationMap;
			InputDevice.ChannelMessageReceived += MIDIInputDevice_ChannelMessageReceived;
		}

		public int DeviceID => InputDevice.DeviceID;
		
		private List<ChannelMessageAction> ChannelMessageActions { get; } = new List<ChannelMessageAction>();
		
		public ITranslationMap TranslationMap { get; private set; }

		public bool IsRecording { get; protected set; }

		public string Name => InputDevice.GetDeviceCapabilities(DeviceID).name;

		public int DriverVersion => InputDevice.GetDeviceCapabilities(DeviceID).driverVersion;

		public short MID => InputDevice.GetDeviceCapabilities(DeviceID).mid;

		public short PID => InputDevice.GetDeviceCapabilities(DeviceID).pid;

		public int Support => InputDevice.GetDeviceCapabilities(DeviceID).support;

		private void MIDIInputDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
		{
			ExecuteChannelMessageAction(e);
		}

		private void ExecuteChannelMessageAction(ChannelMessageEventArgs channelMessageEventArgs)
		{
			var incomingMessage = channelMessageEventArgs.Message;
			var translations = TranslationMap?.Translations.Where(t => InputMatchFunctions.Get(t.InputMatchFunction)(incomingMessage, t.InputMessageMatchTarget)).ToList();
			if (translations != null && translations.Any())
			{
				translations
					.ToList()
					.ForEach(translation =>
					{
						var translatedMessage = TranslationFunctions.Get(translation.TranslationFunction)(incomingMessage, translation.OutputMessageTemplate);
						ChannelMessageActions.Where(channelMessageAction => channelMessageAction.MatchFunction(translatedMessage.ToChannelMessage()))
							.ToList()
							.ForEach(c =>
							{
								c.Action(translatedMessage.ToChannelMessage());
							});
					});
			}
			else
			{
				ChannelMessageActions.Where(channelMessageAction => channelMessageAction.MatchFunction(incomingMessage))
					.ToList()
					.ForEach(c =>
					{
						c.Action(incomingMessage.ToChannelMessage());
					});
			}
		}

		public void Start()
		{
			InputDevice.StartRecording();
			IsRecording = true;
		}

		public void Stop()
		{
			InputDevice.StopRecording();
			IsRecording = false;
		}

		public void AddChannelMessageAction(ChannelMessageAction channelMessageAction)
		{
			ChannelMessageActions.Add(channelMessageAction);
		}

		public void AddSysCommonMessageAction(EventHandler<SysCommonMessageEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			InputDevice.SysCommonMessageReceived += action;

			if (wasRecording)
				Start();
		}

		public void AddSysExMessageAction(EventHandler<SysExMessageEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			InputDevice.SysExMessageReceived += action;

			if (wasRecording)
				Start();
		}

		public void AddSysRealtimeMessageAction(EventHandler<SysRealtimeMessageEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			InputDevice.SysRealtimeMessageReceived += action;

			if (wasRecording)
				Start();
		}

		public void AddErrorAction(EventHandler<ErrorEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			InputDevice.Error += action;

			if (wasRecording)
				Start();
		}
		
		public void Dispose()
		{
			InputDevice.Dispose();
		}
	}
}