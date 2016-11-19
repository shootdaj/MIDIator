import { Component, Input, Output, EventEmitter, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, ReactiveFormsModule, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, TranslationMap } from '../../models/domainModel';
import { HelperService } from '../../services/helperService'
import { DropdownOption } from '../../components/mdl-dropdown/dropdownOption';

@Component({
    selector: 'mdl-dropdown',
    templateUrl: './mdl-dropdown.component.html',
	providers: [HelperService]
})

export class DropdownComponent {

    @Input() options: IDropdownOption[];

    @Input() dropdownLabel: string;
    @Input() placeholder: string;
    @Input() id: string;

    @Input() control: FormControl;

	constructor(private helperService: HelperService) {
		
	}

	set selectedValue(inValue: string) {
		if (this.options == null || inValue == null)
			return;

		this.control.setValue(this.options.filter(x => this.getDropdownValue(x.value) === inValue)[0]);
	}

	get selectedValue(): string {
		if (this.control != null) {
			var returnValue = this.getDropdownValue(this.control.value);
			return returnValue;
		} else
			return null;
	}

	getDropdownLabel(input: any): string {
		return this.helperService.getDropdownOption(input).label;
	}

	getDropdownValue(input: any): string {
		return this.helperService.getDropdownOption(input).value;
	}
}