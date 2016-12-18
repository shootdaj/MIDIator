import { Component, Input, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectionStrategy, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, ReactiveFormsModule, FormControl, ControlValueAccessor } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { IMIDIInputDevice, ShortMessage, IMIDIOutputDevice, Transformation, Profile, VirtualOutputDevice, VirtualDevice, MIDIOutputDevice, IDropdownOption, MIDIInputDevice, Translation, ChannelMessage, MessageType, TranslationFunction, InputMatchFunction, ChannelCommand, TranslationMap } from '../../models/domainModel';
declare var componentHandler;

@Component({
    selector: 'mdl-dropdown',
    templateUrl: './mdl-dropdown.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DropdownComponent implements AfterViewInit  {

    @Input() options: IDropdownOption[];

    @Input() dropdownLabel: string;
    @Input() placeholder: string;
    @Input() id: string;

    @Input() control: FormControl;
    @Input() valueSetFunction: Function;
    @Input() valueGetFunction: Function;

    set selectedValue(inValue: any) {

        if (inValue === this.selectedValue)
            return;

        if (this.options == null || inValue == null)
            return;

        if (this.valueSetFunction != null)
            this.valueSetFunction(inValue, this.options, this.control);
        else {
            this.control.setValue(inValue);
        }
    }

    get selectedValue(): any {
        if (this.control == null)
            return null;
        else {
            let returnValue;
            if (this.valueGetFunction != null) {
                returnValue = this.valueGetFunction(this.control, this.options);
            } else {
                returnValue = this.control.value;
            }
            return returnValue;
        }
    }
    
    constructor(private elementRef: ElementRef) {
    }

    ngAfterViewInit(): void {
		setTimeout(() => {
			//componentHandler.downgradeElements(this.elementRef.nativeElement.firstChild, 'MaterialSelectfield');
			componentHandler.upgradeElement(this.elementRef.nativeElement.firstChild, 'MaterialSelectfield');
			//console.log(`upgrading element ${this.elementRef.nativeElement.firstChild.id}`);

		});
    }
}