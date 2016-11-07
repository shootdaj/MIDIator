import { Component, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { FormGroup, FormBuilder, ReactiveFormsModule, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';

@Component({
    selector: 'mdl-dropdown',
    templateUrl: './mdl-dropdown.component.html'
})
export class DropdownComponent {

    @Input() options: IDropdownOption[];

    @Input() dropdownLabel: string;
    @Input() placeholder: string;
    @Input() id: string;

    //@Input() form: FormGroup;
    @Input() control: FormControl;
	//@Input() formControlName: string;

    constructor() {
    }
}

export class DropdownOption implements IDropdownOption {
	constructor(public value: string, public label: string) {}
}