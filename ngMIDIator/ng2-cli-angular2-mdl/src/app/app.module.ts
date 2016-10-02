import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MdlModule } from 'angular2-mdl';
//import { Observable } from 'rxjs/Observable';
import { AppComponent } from './app.component';
import { DropdownComponent } from './mdl-dropdown.component';

@NgModule({
    declarations: [
        AppComponent,
        DropdownComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        MdlModule
        //,Observable
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
