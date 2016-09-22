import {Component} from '@angular/core';
import {AlertComponent } from 'ng2-bootstrap/ng2-bootstrap';
import {NgModel} from '@angular/forms';

@Component({
	selector: 'input-devices',
	templateUrl: 'app/input-devices.component.html'
})
export class InputDevicesComponent {
	public inputDevices: any[];

	constructor() {
		inputDevices	
	}
};
