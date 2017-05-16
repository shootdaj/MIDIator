export class ShortMessage {
  constructor() {

  }

  $type: string;
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
  Data1Match,
  NoteMatch,
  CatchAll,
  CCData1Match,
  NoteOffData1Match,
  NoteOnData1Match,
  NoteData1Match
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
  PCToNote = 2,
  KeepData2 = 3,
  ToggleData2 = 4,
  InvertData2 = 5
}
export class ChannelMessage extends ShortMessage {
  constructor() {
    super();
    this.$type = "ChannelMessage";
    this.messageType = MessageType.Channel;
  }

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
  mid: number;
  name: string;
  pid: number;
  support: number;
  translationMap: TranslationMap;
}
export interface IMIDIOutputDevice {
  deviceID: number;
  driverVersion: number;
  mid: number;
  name: string;
  pid: number;
  support: number;
}
export class TranslationMap {
  constructor() {
    this.translations = <Translation[]>[];
  }
  translations: Translation[];
}
export class MIDIInputDevice implements IMIDIInputDevice, IDropdownOption {
  //constructor() {
  //	this.value = this.name;
  //	this.label = this.name;
  //}

  deviceID: number;
  driverVersion: number;
  mid: number;
  name: string;
  pid: number;
  support: number;
  translationMap: TranslationMap;
  get value(): string {
    return this.name.toString();
  }
  get label(): string {
    return this.name.toString();
  }
}
export class MIDIOutputDevice implements IMIDIOutputDevice, IDropdownOption {

  //constructor() {
  //	this.value = this.name;
  //	this.label = this.name;
  //}

  deviceID: number;
  driverVersion: number;
  mid: number;
  name: string;
  pid: number;
  support: number;
  get value(): string {
    return this.name.toString();
  }
  get label(): string {
    return this.name.toString();
  }
  //value: string;
  //label:string;

}
export class Profile {
  constructor() {
    this.name = "";
    this.transformations = <Transformation[]>[];
  }
  name: string;
  transformations: Transformation[];
  virtualOutputDevices: VirtualOutputDevice[];
  collapsed = false;
  get label() {
    return this.name;
  }
}
export class Transformation {
  constructor() {
    this.inputDevice = new MIDIInputDevice();
    this.outputDevice = new MIDIOutputDevice();
    this.translationMap = new TranslationMap();
  }
  id: string;
  inputDevice: MIDIInputDevice;
  name: string;
  outputDevice: MIDIOutputDevice;
  translationMap: TranslationMap;
  linkedOutputVirtualDevice: boolean;
  enabled = true;
  collapsed = false;
  translationsCollapsed = false;
}
export class Translation {
  constructor() {
    this.inputMessageMatchTarget = new ShortMessage();
    this.outputMessageTemplate = new ShortMessage();
  }

  inputMatchFunction: InputMatchFunction;
  inputMessageMatchTarget: ShortMessage;
  outputMessageTemplate: ShortMessage;
  translationFunction: TranslationFunction;
  enabled = true;
  collapsed = false;
  name: string;
  description: string;
  id: string;
}
export class VirtualDevice {
  name: string;
}
export class VirtualOutputDevice extends VirtualDevice {
}