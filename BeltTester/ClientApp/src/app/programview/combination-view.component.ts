import { Component, Input, OnInit } from '@angular/core';
import { Combination } from '../_models/program';

@Component({
  selector: 'combination-view',
  template: `
<span>{{combination.sequenceNumber}}.</span>
<span *ngFor="let motion of combination.motions as motions; let i = index">
  <motion-view [motion]="motions[i]" *ngIf="i == 0"></motion-view>
  <motion-view [motion]="motions[i]" [precedingMotion]="motions[i-1]" *ngIf="i > 0"></motion-view>
</span>
`
})
export class CombinationViewComponent implements OnInit {
  @Input() combination: Combination;

  ngOnInit() {
  }
}
