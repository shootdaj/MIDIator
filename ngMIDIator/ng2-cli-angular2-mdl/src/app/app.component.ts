import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
    FormGroup,
    FormControl,
    Validators,
    FormBuilder
} from '@angular/forms';
import {Injectable} from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import './rxjs-operators';


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {

    constructor(private http: Http) {
        this.getAvailableInputDevices();
		this.getAvailableOutputDevices();
    }

    public inputDevice = new FormControl('');

	public availableInputDevices: Observable<Array<any>>;
	public availableOutputDevices: Observable<Array<any>>;
	
    public getAvailableInputDevices() {
	    this.http.get('http://localhost:9000/midi/AvailableInputDevices')
		    .map(response => response.json())
		    .subscribe(data => this.availableInputDevices = data,
			    err => console.log(err));
    }

	public getAvailableOutputDevices() {
		this.http.get('http://localhost:9000/midi/AvailableOutputDevices')
			.map(response => response.json())
			.subscribe(data => this.availableOutputDevices = data,
				err => console.log(err));
	}
    
}

