//import { Component, Input, Output, EventEmitter, DoCheck } from '@angular/core';
//import { EnumValues } from 'enum-values';

//@Component({
//    selector: 'mdl-enum-dropdown',
//    templateUrl: './mdl-enum-dropdown.component.html'
//})
//export class DropdownComponent implements DoCheck {
//    @Input() options: any;
//    @Input() dropdownLabel: string;
//    @Input() placeholder: string;
//    @Input() id: string;
//    @Output() selectedOptionChange: EventEmitter<any>; //this can be fed by parent to do something when dropdown changes
//    @Input() selectedOption: DropdownOption;
	

//    constructor() {
//    }

//    //onOptionSelected(value) {
//    //    this.selectedOption = value;
//    //    this.selectedOptionChange.emit(value);
//    //}

//	ngDoCheck(): void {
//		this.selectedOptionChange.next(this.selectedOption);
//	}
//}

//export interface DropdownOption {
//    value: string;
//    label: string;
//}


