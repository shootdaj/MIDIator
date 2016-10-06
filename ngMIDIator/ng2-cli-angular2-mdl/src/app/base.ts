export interface ITranslation {
	inputMatchFunction: InputMatchFunction;
	inputMessageMatchTarget: ShortMessage;
	outputMessageTemplate: ShortMessage;
	translationFunction: TranslationFunction;
}

export interface ITranslationMap {
	translations: ITranslation[];
}

export interface MIDIInputDevice {
	deviceId: number;
	driverVersion: number;
	isRecording: boolean;
	mid: number;
	name: string;
	pid: number;
	support: number;
	translationMap: ITranslationMap;
}

export interface MIDIOutputDevice {
	deviceId: number;
	driverVersion: number;
	mid: number;
	name: string;
	pid: number;
	support: number;
}

export interface Transformation {
	inputDevice: MIDIInputDevice;
	name: string;
	outputDevice: MIDIOutputDevice;
	translationMap: ITranslationMap;
}

export interface Translation extends ITranslation {
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
	message: number;
	messageType: MessageType;
	status: number;
}

export const enum InputMatchFunction {
	Data1Match = 0,
	NoteMatch = 1,
	CatchAll = 2
}

export const enum TranslationFunction {
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