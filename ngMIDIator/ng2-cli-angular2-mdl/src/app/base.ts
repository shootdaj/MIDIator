import { IDropdownOption } from './mdl-dropdown.component'

export interface TranslationMap {
	translations: Translation[];
}

export abstract class MIDIInputDevice implements IDropdownOption {
	deviceId: number;
	driverVersion: number;
	isRecording: boolean;
	mid: number;
	name: string;
	pid: number;
	support: number;
	translationMap: TranslationMap;
	get value(): string {
        return this.deviceId.toString();
    }
	get label(): string {
        return this.name.toString();
    }
}

export abstract class MIDIOutputDevice implements IDropdownOption {
	deviceId: number;
	driverVersion: number;
	mid: number;
	name: string;
	pid: number;
	support: number;
	get value(): string {
        return this.deviceId.toString();
    }
	get label(): string {
        return this.name.toString();
    }
}

export interface Transformation {
	inputDevice: MIDIInputDevice;
	name: string;
	outputDevice: MIDIOutputDevice;
	translationMap: TranslationMap;
}

export interface Translation {
	inputMatchFunction: InputMatchFunction;
	inputMessageMatchTarget: ShortMessage;
	outputMessageTemplate: ShortMessage;
	translationFunction: TranslationFunction;
}

interface VirtualOutputDevice {
	name: string;
}

export interface Profile {
	name: string;
	transformations: Transformation[];
	virtualOutputDevices: VirtualOutputDevice[];
}

export interface ShortMessage {
	messageType: MessageType;
}

export enum InputMatchFunction {
	Data1Match = 0,
	NoteMatch = 1,
	CatchAll = 2
}

export enum ChannelCommand {
	NoteOff = 128,
	NoteOn = 144,
	PolyPressure = 160,
	Controller = 176,
	ProgramChange = 192,
	ChannelPressure = 208,
	PitchWheel = 224
}

export enum TranslationFunction {
	DirectTranslation = 0,
	ChangeNote = 1,
	PCToNote = 2
}

export const enum MessageType {
	Channel = 0,
	SystemExclusive = 1,
	SystemCommon = 2,
	SystemRealtime = 3,
	Meta = 4
}

export abstract class ChannelMessage implements ShortMessage {
	channelCommand: ChannelCommand;
	data1: number;
	data2: number;
	get messageType(): MessageType {
		return MessageType.Channel;
	}
	midiChannel: number;
}
