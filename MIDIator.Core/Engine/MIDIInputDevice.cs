using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using MIDIator.Interfaces;
using NLog;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using TypeLite;

namespace MIDIator.Engine
{
	[TsClass(Module = "")]
	[UIDropdownOption("DeviceID")]
	public class MIDIInputDevice : IDisposable, IMIDIInputDevice, IDropdownOption
	{
		private InputDevice InputDevice { get; }

		public MIDIInputDevice(int deviceID, ITranslationMap translationMap = null)
		{
			try
			{
				InputDevice = new InputDevice(deviceID);
                InputDevice.ChannelMessageReceived += MIDIInputDevice_ChannelMessageReceived;
            }
			catch (InputDeviceException ex)
			{
				if (ex.ErrorCode == DeviceException.MMSYSERR_NOMEM)
				{
					throw new ArgumentException($"Device with ID {deviceID} is in use by another application.");
				}
			}
			TranslationMap = translationMap;
		}
		
		public int DeviceID => InputDevice.DeviceID;
		
		private List<ChannelMessageAction> ChannelMessageActions { get; } = new List<ChannelMessageAction>();

		public int ChannelMessageActionCount => ChannelMessageActions.Count;

		public ITranslationMap TranslationMap { get; set; }

		public bool IsRecording { get; protected set; }

		public string Name => InputDevice.GetDeviceCapabilities(DeviceID).name;

		public int DriverVersion => InputDevice.GetDeviceCapabilities(DeviceID).driverVersion;

		public short MID => InputDevice.GetDeviceCapabilities(DeviceID).mid;

		public short PID => InputDevice.GetDeviceCapabilities(DeviceID).pid;

		public int Support => InputDevice.GetDeviceCapabilities(DeviceID).support;

		private bool MIDIReaderMode { get; set; } = false;

		private Action<ChannelMessageEventArgs> MIDIReaderMessageAction { get; set; }

		private void MIDIInputDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
		{
			if (MIDIReaderMode)
				MIDIReaderMessageAction(e);
			else
				ExecuteChannelMessageAction(e);
		}

		private void ExecuteChannelMessageAction(ChannelMessageEventArgs channelMessageEventArgs)
		{
			var incomingMessage = channelMessageEventArgs.Message;
			var translations = TranslationMap?.Translations.Where(t => t.Enabled).Where(t => InputMatchFunctions.Get(t.InputMatchFunction)(incomingMessage, t.InputMessageMatchTarget)).ToList();
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
								var channelMessage = translatedMessage.ToChannelMessage();
								LogManager.GetCurrentClassLogger()
									.Info(
										$"{Name}: Translating {{{incomingMessage.Command},{incomingMessage.MidiChannel},{incomingMessage.Data1},{incomingMessage.Data2}}} -> " +
										$"{{{channelMessage.Command},{channelMessage.MidiChannel},{channelMessage.Data1},{channelMessage.Data2}}} using Translation {translation.Name}");
								c.Action(channelMessage);
							});
					});
			}
			else
			{
				ChannelMessageActions.Where(channelMessageAction => channelMessageAction.MatchFunction(incomingMessage))
					.ToList()
					.ForEach(c =>
					{
						var channelMessage = incomingMessage.ToChannelMessage();
						LogManager.GetCurrentClassLogger()
									.Info(
										$"{Name}: Forwarding {{{incomingMessage.Command},{incomingMessage.MidiChannel},{incomingMessage.Data1},{incomingMessage.Data2}}}");
						c.Action(channelMessage);
					});
			}
		}

		public void Start()
		{
			LogManager.GetCurrentClassLogger().Info("Starting MIDI Input Device: " + Name);
			InputDevice.StartRecording();
			IsRecording = true;
			LogManager.GetCurrentClassLogger().Info("Started MIDI Input Device: " + Name);
		}

		public void Stop()
		{
			LogManager.GetCurrentClassLogger().Info("Stopping MIDI Input Device: " + Name);
			InputDevice.StopRecording();
			IsRecording = false;
			LogManager.GetCurrentClassLogger().Info("Stopped MIDI Input Device: " + Name);
		}

		public void StartMIDIReader(Action<ChannelMessageEventArgs> messageAction)
		{
			MIDIReaderMessageAction = messageAction;
			MIDIReaderMode = true;
		}

		public void StopMIDIReader()
		{
			MIDIReaderMode = false;
			MIDIReaderMessageAction = null;
		}

		public void AddChannelMessageAction(ChannelMessageAction channelMessageAction)
		{
			ChannelMessageActions.Add(channelMessageAction);
		}

		/// <summary>
		/// Removes all channel message actions that have the given name.
		/// </summary>
		/// <param name="name"></param>
		public void RemoveChannelMessageAction(string name)
		{
			ChannelMessageActions.RemoveAll(action => action.Name == name);
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

		[TsIgnore]
		//[DataMember]
		public string Value => Name;

		[TsIgnore]
		//[DataMember]
		public string Label => Name;
	}
}