import { DecimalPipe } from '@angular/common';
import { Component, QueryList, ViewChildren } from '@angular/core';
import { Observable } from 'rxjs';

import { Technique } from '../interfaces/technique';
import { TechniqueService } from '../services/techniques.service';
import { NgbdSortableHeader, SortEvent } from '../../directives/sortable.directive';

@Component({
  selector: 'techniques-list',
  templateUrl: './techniques-list.component.html',
  providers: [TechniqueService, DecimalPipe]
})
export class TechniquesListComponent {
  techniques$: Observable<Technique[]>;
  total$: Observable<number>;

  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;

  constructor(public service: TechniqueService) {
    this.techniques$ = service.techniques$;
    this.total$ = service.total$;
  }

  onSort({ column, direction }: SortEvent) {
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }

  DeleteTechnique(item: Technique) {
    alert("Calling API to delete Technique with ID " + item.name);
  }
}
