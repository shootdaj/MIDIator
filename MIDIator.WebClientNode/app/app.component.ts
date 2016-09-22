import {Component} from '@angular/core';
import {AlertComponent } from 'ng2-bootstrap/ng2-bootstrap';
import {NgModel} from '@angular/forms';

@Component({
  selector: 'my-app',
  templateUrl: 'app/app.component.html'
  
  //`
  //  <alert type="info">ng2-bootstrap hello world!</alert>
  //    <pre>Selected date is: <em *ngIf="dt">{{ getDate() | date:'fullDate'}}</em></pre>
  //    <h4>Inline</h4>
  //    <div style="display:inline-block; min-height:290px;">
  //      <datepicker [(ngModel)]="dt" [minDate]="minDate" [showWeeks]="true"></datepicker>
  //    </div>
  //`
})
export class AppComponent {
  public dt:Date = new Date();
  private minDate:Date = null;
  private events:Array<any>;
  private tomorrow:Date;
  private afterTomorrow:Date;
  private formats:Array<string> = ['DD-MM-YYYY', 'YYYY/MM/DD', 'DD.MM.YYYY', 'shortDate'];
  private format = this.formats[0];
  private dateOptions:any = {
    formatYear: 'YY',
    startingDay: 1
  };
  private opened:boolean = false;

  public getDate():number {
    return this.dt && this.dt.getTime() || new Date().getTime();
  }
}