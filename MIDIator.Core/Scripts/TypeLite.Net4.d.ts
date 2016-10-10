
 
 
 

 

/// <reference path="Enums.ts" />

declare module MIDIator {
	interface IMIDIInputDevice {
		DeviceID: number;
		DriverVersion: number;
		IsRecording: boolean;
		MID: number;
		Name: string;
		PID: number;
		Support: number;
		TranslationMap: MIDIator.ITranslationMap;
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
		InputMatchFunction: MIDIator.InputMatchFunction;
		InputMessageMatchTarget: Sanford.Multimedia.Midi.ShortMessage;
		OutputMessageTemplate: Sanford.Multimedia.Midi.ShortMessage;
		TranslationFunction: MIDIator.TranslationFunction;
	}
	interface ITranslationMap {
		Translations: MIDIator.ITranslation[];
	}
	interface MIDIInputDevice {
		DeviceID: number;
		DriverVersion: number;
		IsRecording: boolean;
		MID: number;
		Name: string;
		PID: number;
		Support: number;
		TranslationMap: MIDIator.ITranslationMap;
	}
	interface MIDIOutputDevice {
		DeviceID: number;
		DriverVersion: number;
		MID: number;
		Name: string;
		PID: number;
		Support: number;
	}
	interface Profile {
		Name: string;
		Transformations: MIDIator.Transformation[];
		VirtualOutputDevices: MIDIator.VirtualOutputDevice[];
	}
	interface Transformation {
		InputDevice: MIDIator.IMIDIInputDevice;
		Name: string;
		OutputDevice: MIDIator.IMIDIOutputDevice;
		TranslationMap: MIDIator.ITranslationMap;
	}
	interface Translation extends MIDIator.ITranslation {
		InputMatchFunction: MIDIator.InputMatchFunction;
		InputMessageMatchTarget: Sanford.Multimedia.Midi.ShortMessage;
		OutputMessageTemplate: Sanford.Multimedia.Midi.ShortMessage;
		TranslationFunction: MIDIator.TranslationFunction;
	}
	interface VirtualOutputDevice {
		Name: string;
	}
}
declare module Sanford.Multimedia.Midi {
	interface ChannelMessage extends Sanford.Multimedia.Midi.ShortMessage {
		Command: Sanford.Multimedia.Midi.ChannelCommand;
		Data1: number;
		Data2: number;
		MessageType: Sanford.Multimedia.Midi.MessageType;
		MidiChannel: number;
	}
	interface ShortMessage {
		Message: number;
		MessageType: Sanford.Multimedia.Midi.MessageType;
		Status: number;
	}
}


