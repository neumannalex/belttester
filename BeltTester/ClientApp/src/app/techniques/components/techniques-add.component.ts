import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router'
import { FormControl, FormGroup } from '@angular/forms'
import { Technique } from '../interfaces/technique';
import { TechniqueService } from '../services/techniques.service';

@Component({
  selector: 'techniques-add',
  templateUrl: './techniques-add.component.html',
  providers: [TechniqueService]
})
export class TechniquesAddComponent implements OnInit {
  techniqueForm: FormGroup;
  name: FormControl = new FormControl();
  level: FormControl = new FormControl();
  weapon: FormControl = new FormControl();
  purpose: FormControl = new FormControl();


  //, private techniqueService: TechniqueService
  constructor(private router: Router, private service: TechniqueService) { console.log("TechniquesAddComponent ctor") }

  ngOnInit() {
    console.log("TechniquesAddComponent starting ngOnInit")
    this.techniqueForm = new FormGroup({
      name: this.name,
      level: this.level,
      weapon: this.weapon,
      purpose: this.purpose
    });
    console.log("TechniquesAddComponent finishing ngOnInit")
  }

  saveTechnique(event) {
    //this.eventService.saveEvent(event)
    console.log(event);
    this.router.navigate(['/techniques']);
  }

  cancel(form) {
    this.router.navigate(['/techniques']);
  }
}
