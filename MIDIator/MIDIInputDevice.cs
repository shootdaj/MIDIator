using System;
using System.Collections.Generic;
using System.Linq;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

namespace MIDIator
{
	public class MIDIInputDevice : InputDevice
	{
		public MIDIInputDevice(int deviceID, ITranslationMap translationMap = null) : base(deviceID)
		{
			TranslationMap = translationMap;
			ChannelMessageReceived += MIDIInputDevice_ChannelMessageReceived;
		}
		
		private List<ChannelMessageAction> ChannelMessageActions { get; } = new List<ChannelMessageAction>();
		
		public ITranslationMap TranslationMap { get; private set; }

		public bool IsRecording { get; protected set; }

		public string Name => GetDeviceCapabilities(DeviceID).name;

		public int DriverVersion => GetDeviceCapabilities(DeviceID).driverVersion;

		public short MID => GetDeviceCapabilities(DeviceID).mid;

		public short PID => GetDeviceCapabilities(DeviceID).pid;

		public int Support => GetDeviceCapabilities(DeviceID).support;

		private void MIDIInputDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
		{
			ExecuteChannelMessageAction(e);
		}

		private void ExecuteChannelMessageAction(ChannelMessageEventArgs channelMessageEventArgs)
		{
			var incomingMessage = channelMessageEventArgs.Message;
			var translations = TranslationMap?.Translations.Where(t => t.InputMatchFunction(incomingMessage, t.InputMessageMatchTarget)).ToList();
			if (translations != null && translations.Any())
			{
				translations
					.ToList()
					.ForEach(translation =>
					{
						var translatedMessage = translation.TranslationFunction(incomingMessage, translation.OutputMessageTemplate);
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
			StartRecording();
			IsRecording = true;
		}

		public void Stop()
		{
			StopRecording();
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
			SysCommonMessageReceived += action;

			if (wasRecording)
				Start();
		}

		public void AddSysExMessageAction(EventHandler<SysExMessageEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			SysExMessageReceived += action;

			if (wasRecording)
				Start();
		}

		public void AddSysRealtimeMessageAction(EventHandler<SysRealtimeMessageEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			SysRealtimeMessageReceived += action;

			if (wasRecording)
				Start();
		}

		public void AddErrorAction(EventHandler<ErrorEventArgs> action)
		{
			var wasRecording = IsRecording;
			if (IsRecording)
				Stop();
			Error += action;

			if (wasRecording)
				Start();
		}

		public new void Dispose()
		{
			Reset();
			base.Dispose();
		}
	}
}