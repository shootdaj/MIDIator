import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Profile, MIDIInputDevice, MIDIOutputDevice,
	ChannelCommand, InputMatchFunction, TranslationFunction } from './base';

@Component({
	selector: 'profile',
	templateUrl: './profile.component.html'
})

export class ProfileComponent {
	@Input() profile: Profile;

	@Input() availableInputDevices: MIDIInputDevice[];
	@Input() availableOutputDevices: MIDIOutputDevice[];
	@Input() availableChannelCommands: ChannelCommand[];
	@Input() availableMIDIChannels: number[];
	@Input() availableInputMatchFunctions: InputMatchFunction[];
	@Input() availableTranslationFunctions: TranslationFunction[];
}