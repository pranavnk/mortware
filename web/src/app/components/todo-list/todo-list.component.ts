import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { catchError, map, Observable, throwError } from 'rxjs';
import { TodoService } from 'src/app/core/services/todo.service';
import { BaseComponent } from '../base.component';
import { faCheck, faL } from '@fortawesome/free-solid-svg-icons';
import {MatButtonToggleModule} from '@angular/material/button-toggle';


@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent extends BaseComponent {

  vm$: Observable<any>;
  form: FormGroup;
  faCheck = faCheck;
  submissionInProgress:boolean;
  showCompletedItems:boolean;

  constructor(
    private todoService: TodoService,
    private formBuilder: FormBuilder) {
    super();

    this.form = this.formBuilder.group({
      text: new FormControl(null)
    })
    this.vm$ = this.getItemsList();

    this.submissionInProgress = false;
    this.showCompletedItems = false;
  }

   private getItemsList() : Observable<any> {
    var vm = this.todoService.list$.pipe(
      map(items => {
        return {
          items
        };
      }),
      catchError(err => {
        this.handleError('Failed to fetch items', err.message);
        return throwError(() => err);
      })
    );
    return vm;
  }

  submit() {
    this.submissionInProgress = true;
    this.todoService.create(this.form.get('text')?.value)
      .subscribe(() => {
        this.form.reset();
        this.submissionInProgress = false;
      });
  }

  complete(id: string) {
    this.submissionInProgress = true;
    this.todoService.complete(id)
      .subscribe(() => {
        this.form.reset();
        this.submissionInProgress = false;
      });
  }
  toggleCompletedVisibility() {
    this.showCompletedItems = !this.showCompletedItems;
    this.todoService.list(this.showCompletedItems);
    this.vm$ = this.getItemsList();
  }
}
