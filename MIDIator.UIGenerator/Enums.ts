module MIDIator.Engine {
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
}
module Sanford.Multimedia.Midi {
	export const enum ChannelCommand {
		NoteOff = 128,
		NoteOn = 144,
		PolyPressure = 160,
		Controller = 176,
		ProgramChange = 192,
		ChannelPressure = 208,
		PitchWheel = 224
	}
	export const enum MessageType {
		Channel = 0,
		SystemExclusive = 1,
		SystemCommon = 2,
		SystemRealtime = 3,
		Meta = 4
	}
}

