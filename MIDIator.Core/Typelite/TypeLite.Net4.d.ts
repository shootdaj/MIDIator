
 
 
 

 

/// <reference path="Enums.ts" />

declare module MIDIator.Interfaces {
	interface IMIDIInputDevice {
		DeviceID: number;
		DriverVersion: number;
		IsRecording: boolean;
		MID: number;
		Name: string;
		PID: number;
		Support: number;
		TranslationMap: MIDIator.Interfaces.ITranslationMap;
	}
	interface IMIDIOutputDevice {
		DeviceID: number;
		DriverVersion: number;
		MID: number;
		Name: string;
		PID: number;
		Support: number;
	}
	interface ITranslation {
		InputMatchFunction: MIDIator.Engine.InputMatchFunction;
		InputMessageMatchTarget: MIDIator.UI.ShortMessage;
		OutputMessageTemplate: MIDIator.UI.ShortMessage;
		TranslationFunction: MIDIator.Engine.TranslationFunction;
	}
	interface ITranslationMap {
		Translations: MIDIator.Interfaces.ITranslation[];
	}
}
declare module MIDIator.UI {
	interface ChannelMessage extends MIDIator.UI.ShortMessage {
		Command: Sanford.Multimedia.Midi.ChannelCommand;
		Data1: number;
		Data2: number;
		MessageType: Sanford.Multimedia.Midi.MessageType;
		MidiChannel: number;
	}
	interface MIDIInputDevice extends MIDIator.Interfaces.IMIDIInputDevice {
		DeviceID: number;
		DriverVersion: number;
		IsRecording: boolean;
		MID: number;
		Name: string;
		PID: number;
		Support: number;
		TranslationMap: MIDIator.Interfaces.ITranslationMap;
	}
	interface MIDIOutputDevice extends MIDIator.Interfaces.IMIDIOutputDevice {
		DeviceID: number;
		DriverVersion: number;
		MID: number;
		Name: string;
		PID: number;
		Support: number;
	}
	interface Profile {
		Name: string;
		Transformations: MIDIator.UI.Transformation[];
		VirtualOutputDevices: MIDIator.UI.VirtualOutputDevice[];
	}
	interface ShortMessage {
		Message: number;
		MessageType: Sanford.Multimedia.Midi.MessageType;
		Status: number;
	}
	interface Transformation {
		InputDevice: MIDIator.Interfaces.IMIDIInputDevice;
		Name: string;
		OutputDevice: MIDIator.Interfaces.IMIDIOutputDevice;
		TranslationMap: MIDIator.Interfaces.ITranslationMap;
	}
	interface Translation extends MIDIator.Interfaces.ITranslation {
		InputMatchFunction: MIDIator.Engine.InputMatchFunction;
		InputMessageMatchTarget: MIDIator.UI.ShortMessage;
		OutputMessageTemplate: MIDIator.UI.ShortMessage;
		TranslationFunction: MIDIator.Engine.TranslationFunction;
	}
	interface VirtualDevice {
		Name: string;
	}
	interface VirtualOutputDevice extends MIDIator.UI.VirtualDevice {
	}
}


