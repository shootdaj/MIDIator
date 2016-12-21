import { Component, Input, Output, EventEmitter, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, ReactiveFormsModule, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, TranslationMap } from '../../models/domainModel';

@Component({
    selector: 'mdl-textinput',
    templateUrl: './mdl-textinput.component.html'
})
export class TextInputComponent {

    @Input() textInputLabel: string;
    @Input() id: string;

    @Input() control: FormControl;

	set selectedValue(inValue: any) {
		this.control.setValue(inValue);
	}

	get selectedValue(): any {
		if (this.control == null)
			return null;
		else {
			let returnValue = this.control.value;

			return returnValue;
		}
	}


	constructor() {
    }
}