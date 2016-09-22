import { Component } from '@angular/core';
import {Router, Routes, ROUTER_DIRECTIVES } from '@angular/router';

@Component({
    templateUrl: 'app/home/welcome.component.html'
})
export class WelcomeComponent {
    constructor(private _router: Router) {

    }


    public pageTitle: string = 'Welcome';
}
