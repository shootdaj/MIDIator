import { Component, ViewChild, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'mdl-dropdown',
    templateUrl: './mdl-dropdown.component.html'
})
export class DropdownComponent {
    @Input() options: DropdownOption[];
    @Input() dropdownLabel: string;
    @Input() placeholder: string;
    @Input() id: string;
    @Output() select: EventEmitter<any>; //this can be fed by parent to do something when dropdown changes
    @Input() selectedOption: DropdownOption;

    constructor() {
        this.select = new EventEmitter();
    }

    onOptionSelected(value) {
        this.selectedOption = value;
        this.select.emit(value);
    }
}

export class DropdownOption {
    value: string;
    label: string;

    constructor(value: string, label: string) {
        this.value = value;
        this.label = label;
    }
}
