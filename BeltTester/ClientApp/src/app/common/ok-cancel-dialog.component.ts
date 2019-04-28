import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'ok-cancel-dialog',
  templateUrl: './ok-cancel-dialog.component.html'
})
export class OkCancelDialogComponent implements OnInit {
  @Input() title: string;
  @Input() message: string;
  @Output() getDialogResult: EventEmitter<string> = new EventEmitter();

  constructor(public activeModal: NgbActiveModal) {
  }

  ngOnInit() {
  }

  Ok() {
    this.getDialogResult.emit("ok");
  }

  Cancel() {
    this.getDialogResult.emit("cancel");
  }
}
