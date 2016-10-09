import { Component, Input, Output, DoCheck, EventEmitter } from '@angular/core';
import { Transformation, MIDIInputDevice, MIDIOutputDevice, ChannelCommand,  } from './base';
import { Observable } from 'rxjs/Observable';
import { IDropdownOption, DropdownComponent } from './mdl-dropdown.component';

@Component({
	selector: 'transformation',
	templateUrl: './transformation.component.html'
})

export class TransformationComponent {
	@Input() transformation: Transformation;
	@Input() availableInputDevices: Observable<Array<MIDIInputDevice>>;
	@Input() availableOutputDevices: Observable<Array<MIDIOutputDevice>>;
	@Input() availableChannelCommands: Observable<Array<ChannelCommand>>;
	@Input() availableMIDIChannels: Observable<Array<number>>;

	@Input() inputDevice: MIDIInputDevice;
	@Output() data1Change: EventEmitter<any> = new EventEmitter();
}