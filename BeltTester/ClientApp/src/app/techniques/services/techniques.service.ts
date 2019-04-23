import { Inject, Injectable, PipeTransform } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {BehaviorSubject, Observable, of, Subject} from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap, retry } from 'rxjs/operators';
import {SortDirection} from '../../directives/sortable.directive';
import { Technique } from '../interfaces/technique';


interface SearchResult {
  techniques: Technique[];
  total: number;
}

interface State {
  page: number;
  pageSize: number;
  searchTerm: string;
  sortColumn: string;
  sortDirection: SortDirection;
}

function compare(v1, v2) {
  return v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
}

function sort(techniques: Technique[], column: string, direction: string): Technique[] {
  //console.log("Sorting column " + column);

  if (direction === '') {
    //console.log("SortDirection is empty.");
    return techniques;
  } else {
    //console.log("SortDirection is " + direction + ".");

    return [...techniques].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(technique: Technique, term: string, pipe: PipeTransform) {
  return technique.name.toLowerCase().indexOf(term) > -1;
}

//@Injectable({providedIn: 'root'})
@Injectable()
export class TechniqueService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _techniques$ = new BehaviorSubject<Technique[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private _data: Technique[] = [];

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    sortColumn: '',
    sortDirection: ''
  };

  constructor(private pipe: DecimalPipe, private _http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) {
    //console.log("TechniqueService ctor");

    this.GetTechniquesFromAPI().subscribe(result => {
      //console.log("Data is now:");
      //console.log(result);
      this._data = result;

      this._search$.pipe(
        tap(() => this._loading$.next(true)),
        debounceTime(200),
        switchMap(() => this._search()),
        //delay(200),
        tap(() => this._loading$.next(false))
      ).subscribe(result => {
        //console.log("Got result from _search");
        //console.log(result);
        this._techniques$.next(result.techniques);
        this._total$.next(result.total);
      });

      this._search$.next();
    });
  }

  get techniques$() { return this._techniques$.asObservable(); }
  get total$() { return this._total$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }
  get page() { return this._state.page; }
  get pageSize() { return this._state.pageSize; }
  get searchTerm() { return this._state.searchTerm; }

  set page(page: number) { this._set({page}); }
  set pageSize(pageSize: number) { this._set({pageSize}); }
  set searchTerm(searchTerm: string) { this._set({searchTerm}); }
  set sortColumn(sortColumn: string) { this._set({sortColumn}); }
  set sortDirection(sortDirection: SortDirection) { this._set({sortDirection}); }

  private _set(patch: Partial<State>) {
    if(patch.page != undefined) this._state.page = patch.page;
    if (patch.pageSize != undefined) this._state.pageSize = patch.pageSize;
    if (patch.searchTerm != undefined) this._state.searchTerm = patch.searchTerm;
    if (patch.sortColumn != undefined) this._state.sortColumn = patch.sortColumn;
    if (patch.sortDirection != undefined) this._state.sortDirection = patch.sortDirection;

    this._search$.next();
  }

  private GetTechniquesFromAPI(): Observable<Technique[]> {
    return this._http.get<Technique[]>(this._baseUrl + 'api/techniques/all');
  }

  private _search(): Observable<SearchResult> {
    //console.log("Starting search");

    const {sortColumn, sortDirection, pageSize, page, searchTerm} = this._state;

    //console.log("Searching in data:");
    //console.log(this._data);
    //console.log("With searchTerm: " + searchTerm);

    // 1. sort
    let techniques = sort(this._data, sortColumn, sortDirection).map((item, i) => ({ index: i + 1, ...item }));

    // 2. filter
    techniques = techniques.filter(item => matches(item, searchTerm, this.pipe));
    const total = techniques.length;

    // 3. paginate
    techniques = techniques.slice((page - 1) * pageSize, (page - 1) * pageSize + pageSize);
    return of({ techniques, total });
  }
}
