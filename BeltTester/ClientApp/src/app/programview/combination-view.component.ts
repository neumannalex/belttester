import { Component, Input, OnInit } from '@angular/core';
import { Combination, Motion } from '../_models/program';
import { retry } from 'rxjs/operators';

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
    if (this.combination) {
      this.combination.motions.sort((a: Motion, b: Motion) => {
        if (a.sequenceNumber < b.sequenceNumber) {
          return -1;
        }
        if (a.sequenceNumber > b.sequenceNumber) {
          return 1;
        }
        return 0;
      });
    }
  }
}
