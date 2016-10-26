import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Profile, MIDIInputDevice, MIDIOutputDevice,
	ChannelCommand, InputMatchFunction, TranslationFunction } from './base';
import { TransformationComponent } from './transformation.component'

@Component({
	selector: 'profile',
	templateUrl: './profile.component.html'
})

export class ProfileComponent {
	@Input() profile: Observable<Profile>;
}