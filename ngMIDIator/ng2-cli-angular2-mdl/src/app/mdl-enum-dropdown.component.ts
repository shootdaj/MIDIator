import { Component, Input, Output, EventEmitter, DoCheck, Pipe } from '@angular/core';
import { EnumValues } from 'enum-values';

@Component({
    selector: 'mdl-enum-dropdown',
    templateUrl: './mdl-enum-dropdown.component.html'
})
export class DropdownComponent implements DoCheck {
    @Input() options: any;
    @Input() dropdownLabel: string;
    @Input() placeholder: string;
    @Input() id: string;
    @Output() selectedOptionChange: EventEmitter<any>;
    @Input() selectedOption: any;
	

    constructor() {
    }

	ngDoCheck(): void {
		this.selectedOptionChange.next(this.selectedOption);
	}
}

export interface DropdownOption {
    value: string;
    label: string;
}

@Pipe({ name: 'keys' })
export class KeysPipe implements PipeTransform {
	transform(value, args: string[]): any {
		let keys = [];
		for (var enumMember in value) {
			var isValueProperty = parseInt(enumMember, 10) >= 0
			if (isValueProperty) {
				keys.push({ key: enumMember, value: value[enumMember] });
				// Uncomment if you want log
				// console.log("enum member: ", value[enumMember]);
			}
		}
		return keys;
	}
}
