import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';

import { routing } from './app.routing';
import { AppComponent }  from './app.component';
import { WelcomeComponent } from './home/welcome.component';
import { Ng2BootstrapModule } from 'ng2-bootstrap/ng2-bootstrap';

/* Feature Modules */
import { ProductModule } from './products/product.module';

@NgModule({
    imports: [
        BrowserModule,
        HttpModule,
        routing,
        ProductModule,
        Ng2BootstrapModule,
        FormsModule
    ],
    declarations: [
        AppComponent,
        WelcomeComponent
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
