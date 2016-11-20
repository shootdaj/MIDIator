import { Component, Input, Output, EventEmitter, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, ReactiveFormsModule, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, TranslationMap } from '../../models/domainModel';

@Component({
    selector: 'mdl-dropdown',
    templateUrl: './mdl-dropdown.component.html'
})
export class DropdownComponent {

    @Input() options: IDropdownOption[];

    @Input() dropdownLabel: string;
    @Input() placeholder: string;
    @Input() id: string;

    @Input() control: FormControl;
	@Input() valueSetFunction: Function;
	@Input() valueGetFunction: Function;

	set selectedValue(inValue: any) {
	
		if (this.options == null || inValue == null)
			return;
			
		this.valueSetFunction(inValue, this.options, this.control);
	}

	get selectedValue() : any {
		if (this.control != null) {
			var returnValue = this.valueGetFunction(this.control, this.options);
			return returnValue;
		} else
			return null;
	}


    constructor() {
    }
}

export class DropdownOption implements IDropdownOption {
	constructor(public value: string, public label: string) {}
}