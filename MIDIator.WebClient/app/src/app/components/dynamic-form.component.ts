import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { UIControlBase } from '../models/ui-control-base';

@Component({
    selector: 'dfc',
    templateUrl: 'dynamic-form-control.component.html'
})
export class DynamicFormControlComponent {
    @Input() control: UIControlBase<any>;
    @Input() form: FormGroup;
    get isValid() { return this.form.controls[this.control.key].valid; }
}