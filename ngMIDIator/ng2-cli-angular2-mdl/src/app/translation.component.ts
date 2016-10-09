import { Component, Input } from '@angular/core';
import { Translation, MIDIInputDevice, MIDIOutputDevice, ChannelCommand,  } from './base';
import { Observable } from 'rxjs/Observable';
import { IDropdownOption, DropdownComponent } from './mdl-dropdown.component';

@Component({
	selector: 'translation',
	templateUrl: './translation.component.html'
})

export class TranslationComponent {
	@Input() translation: Translation;
	@Input() availableInputDevices: Observable<Array<MIDIInputDevice>>;
	@Input() availableOutputDevices: Observable<Array<MIDIOutputDevice>>;
	@Input() availableChannelCommands: Observable<Array<ChannelCommand>>;
	@Input() availableMIDIChannels: Observable<Array<number>>;
}