import { Component, Input, Output, EventEmitter, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, ReactiveFormsModule, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, TranslationMap } from '../../models/domainModel';

@Component({
    selector: 'mdl-dropdown',
    templateUrl: './mdl-dropdown.component.html'
})
export class DropdownComponent implements OnInit, AfterViewInit {

    @Input() options: IDropdownOption[];

    @Input() dropdownLabel: string;
    @Input() placeholder: string;
    @Input() id: string;

    @Input() control: FormControl;

	set selectedValue(inValue: string) {

		console.log("setting control value to:");
		console.log(inValue);

		if (this.options == null || inValue == null)
			return;

		//this.control.setValue(inValue);
		this.control.setValue(this.options.filter(x => x.value === inValue)[0]);
	}

	get selectedValue() : string {
		if (this.control != null) {
			var returnValue = this.control.value.label;
			return returnValue;
		} else
			return null;
	}


    constructor() {
    }

	ngOnInit(): void {
		console.log("options:");
		console.log(this.options);
	}

	ngAfterViewInit(): void {
		console.log("ngAfterViewInit options:");
		console.log(this.options);
	}
}

export class DropdownOption implements IDropdownOption {
	constructor(public value: string, public label: string) {}
}