import { Component, OnChanges, Input, trigger, state, animate, transition, style } from '@angular/core';
//, transform: 'scaleY(1.0)', 'transform-origin': 'top'
//, transform: 'scaleY(0.0)', 'transform-origin': 'top'

@Component({
  selector : 'expander',
  animations: [
    trigger('isVisibleChanged', [
          state('true', style({ display: 'block' })), 
          state('false', style({ display: 'none' })),
      transition('1 => 0', animate('10ms')),
      transition('0 => 1', animate('10ms'))
    ])
  ],
  template: `
    <div [@isVisibleChanged]="!collapsed" >
      <ng-content></ng-content>
    </div>
  `
})
export class ExpanderComponent {
  @Input() collapsed : boolean = false;
}