import { Component, ViewChild, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { Program, Combination, Motion } from '../_models/program';
import { retry } from 'rxjs/operators';

@Component({
  selector: 'motion-view',
  template: `
<span class=\"motion\" *ngIf="motion.stance.symbol != precedingMotion?.stance.symbol">{{motion.stance.symbol}}</span>
<span class=\"motion\">{{motion.move.symbol}}</span>
<span class=\"motion\">{{techniqueWithAnnotation}}</span>
<span class=\"annotation\" *ngIf="motion.annotation != ''">({{motion.annotation}})</span>
`
})
export class MotionViewComponent implements OnInit {
  @Input() precedingMotion: Motion;
  @Input() motion: Motion;

  ngOnInit() {
  }

  get techniqueWithAnnotation(): string {
    let techniqueString: string = this.motion.technique.name;

    if (this.motion.technique.level.toLowerCase() != 'none')
      techniqueString += ' ' + this.motion.technique.level;

    return techniqueString;
  }
}
