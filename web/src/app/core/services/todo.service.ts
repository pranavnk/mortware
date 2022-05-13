import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { TodoItem } from '../models';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  static readonly BasePath = 'https://localhost:7050/todo';

  private refreshList$ = new BehaviorSubject<boolean>(true);
  private showCompletedItems : boolean;

  list$: Observable<TodoItem[]>;

  constructor(private http: HttpClient) {
    this.showCompletedItems = false;
    this.list$ = this.list(this.showCompletedItems);

  }

  public list(showCompletedItems: boolean): Observable<TodoItem[]> {
    this.showCompletedItems = showCompletedItems;
    return this.refreshList$.pipe(switchMap(
      _ => this.http.get<TodoItem[]>(TodoService.BasePath + '/list/' + this.showCompletedItems.toString())
    ))
  }

  create(text: string): Observable<string> {
    return this.http.post<string>(TodoService.BasePath + '/create', { text }).pipe(
      tap(_ => {
        this.refreshList$.next(false);
      })
    );
  }

  complete(id: string): Observable<string> {
    return this.http.patch<string>(TodoService.BasePath + '/complete', { id }).pipe(
      tap(_ => {
        this.refreshList$.next(false);
      })
    );
  }
}
