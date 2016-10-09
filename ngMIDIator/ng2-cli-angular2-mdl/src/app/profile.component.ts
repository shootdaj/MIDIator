import { Component, Input } from '@angular/core';
import { Profile } from './base';

@Component({
	selector: 'profile',
	templateUrl: './profile.component.html'
})

export class ProfileComponent {
	@Input() profile: Profile;
}