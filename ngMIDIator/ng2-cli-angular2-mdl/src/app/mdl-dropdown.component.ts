import { Component, Input, Output, EventEmitter } from '@angular/core';


// @Component({
//     selector: 'mdl-dropdown',
//     templateUrl: './mdl-dropdown.component.html'
// })

// export class MdlDropdownComponent {

// }



export class DropdownOption {
    value: string;
    label: string;

    constructor(value: string, label: string) {
        this.value = value;
        this.label = label;
    }
}

@Component({
    selector: 'mdl-dropdown',
    templateUrl: './mdl-dropdown.component.html'
})
export class DropdownComponent {
    @Input() options: DropdownOption[];
    @Input() dropdownLabel: string;
    @Input() placeholder: string;
    @Input() id: string;
    @Output() select: EventEmitter<any>;

    constructor() {
        this.select = new EventEmitter();
    }

    selectItem(value) {
        this.select.emit(value);
    }
}
