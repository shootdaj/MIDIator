//import { Component, Input, Output, EventEmitter, DoCheck } from '@angular/core';
//import { IMIDIInputDevice, ITranslationMap, ITranslation, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand } from '../../models/domainModel';

//@Component({
//    selector: 'mdl-dropdown',
//    templateUrl: './mdl-dropdown.component.html'
//})
//export class DropdownComponent {//implements DoCheck {
//    @Input() options: IDropdownOption[];
//    @Input() dropdownLabel: string;
//    @Input() placeholder: string;
//    @Input() id: string;
//    @Output() selectedOptionChange: EventEmitter<any>; //this can be fed by parent to do something when dropdown changes
//    @Input() selectedOption: IDropdownOption;

    

//    constructor() {
//    }

//    //onOptionSelected(value) {
//    //    this.selectedOption = value;
//    //    this.selectedOptionChange.emit(value);
//    //}

//	//ngDoCheck(): void {
//	//	this.selectedOptionChange.next(this.selectedOption);
//	//}}
//}

//export class DropdownOption implements IDropdownOption {
//	constructor(public value: string, public label: string) {}
//}