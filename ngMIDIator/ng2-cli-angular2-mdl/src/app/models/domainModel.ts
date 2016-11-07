export class ShortMessage {
    message: number;
    messageType: MessageType;
    status: number;
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
export enum InputMatchFunction {
    Data1Match = 0,
    NoteMatch = 1,
    CatchAll = 2
}
export enum MessageType {
    Channel = 0,
    SystemExclusive = 1,
    SystemCommon = 2,
    SystemRealtime = 3,
    Meta = 4
}
export enum TranslationFunction {
    DirectTranslation = 0,
    ChangeNote = 1,
    PCToNote = 2
}
export class ChannelMessage extends ShortMessage {
    command: ChannelCommand;
    data1: number;
    data2: number;
    messageType: MessageType;
    midiChannel: number;
}
export interface IDropdownOption {
    label: string;
    value: string;
}
export interface IMIDIInputDevice {
    deviceID: number;
    driverVersion: number;
    isRecording: boolean;
    mid: number;
    name: string;
    pid: number;
    support: number;
    translationMap: ITranslationMap;
}
export interface IMIDIOutputDevice {
    deviceID: number;
    driverVersion: number;
    mid: number;
    name: string;
    pid: number;
    support: number;
}
export interface ITranslation {
    inputMatchFunction: InputMatchFunction;
    inputMessageMatchTarget: ShortMessage;
    outputMessageTemplate: ShortMessage;
    translationFunction: TranslationFunction;
}
export interface ITranslationMap {
    translations: ITranslation[];
}
export class MIDIInputDevice implements IMIDIInputDevice, IDropdownOption {
    deviceID: number;
    driverVersion: number;
    isRecording: boolean;
    mid: number;
    name: string;
    pid: number;
    support: number;
    translationMap: ITranslationMap;
    get value(): string {
        return this.deviceID.toString();
    }
    get label(): string {
        return this.name.toString();
    }
}
export class MIDIOutputDevice implements IMIDIOutputDevice, IDropdownOption {
    deviceID: number;
    driverVersion: number;
    mid: number;
    name: string;
    pid: number;
    support: number;
    get value(): string {
        return this.deviceID.toString();
    }
    get label(): string {
        return this.name.toString();
    }
}
export class Profile {
    name: string;
    transformations: Transformation[];
    virtualOutputDevices: VirtualOutputDevice[];
}
export class Transformation {
    inputDevice: IMIDIInputDevice;
    name: string;
    outputDevice: IMIDIOutputDevice;
    translationMap: ITranslationMap;
}
export class Translation implements ITranslation {
    inputMatchFunction: InputMatchFunction;
    inputMessageMatchTarget: ShortMessage;
    outputMessageTemplate: ShortMessage;
    translationFunction: TranslationFunction;
}
export class VirtualDevice {
    name: string;
}
export class VirtualOutputDevice extends VirtualDevice {
}