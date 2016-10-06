export interface Translation {
	inputMatchFunction: InputMatchFunction;
	inputMessageMatchTarget: ShortMessage;
	outputMessageTemplate: ShortMessage;
	translationFunction: TranslationFunction;
}

export interface ShortMessage {
	message: number;
	messageType: MessageType;
	status: number;
}

export const enum MessageType {
	Channel = 0,
	SystemExclusive = 1,
	SystemCommon = 2,
	SystemRealtime = 3,
	Meta = 4
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