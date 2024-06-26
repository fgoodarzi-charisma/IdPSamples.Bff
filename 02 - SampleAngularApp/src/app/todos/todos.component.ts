import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, catchError, filter, Observable, of, Subject, switchMap, tap } from 'rxjs';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-todos',
  templateUrl: './todos.component.html',
})
export class TodosComponent implements OnInit {
  publicData : string = '';

  private readonly todos = new BehaviorSubject<Todo[]>([]);
  public readonly todos$: Observable<Todo[]> = this.todos;

  private readonly errors = new BehaviorSubject<string>('');
  public readonly error$: Observable<string> = this.errors;

  public authenticated$ = this.auth.getIsAuthenticated();
  public anonymous$ = this.auth.getIsAnonymous();

  public date = (new Date()).toISOString().split('T')[0];
  public name = "";

  public constructor(
    private http: HttpClient,
    private auth: AuthenticationService) { }

  public ngOnInit(): void {
    this.fetchPublicData();
    this.authenticated$
      .pipe(
        filter(isAuthenticated => isAuthenticated),
        tap(() => {
          this.fetchTodos();
     
        })
    ).subscribe();
  }

  public createTodo(): void {
    this.http
      .post<Todo>('plans/todos', {
        name: this.name,
        date: this.date,
      })
      .pipe(catchError(this.showError))
      .subscribe((todo) => {
        const todos = [...this.todos.getValue(), todo];
        this.todos.next(todos);
      });
  }

  public deleteTodo(id: number): void {
    this.http.delete(`plans/todos/${id}`)
      .pipe(catchError(this.showError))
      .subscribe(() => {
        const todos = this.todos.getValue().filter((x) => x.id !== id);
        this.todos.next(todos);
      });
  }

  private fetchTodos(): void {
    this.http
      .get<Todo[]>('plans/todos')
      .pipe(catchError(this.showError))
      .subscribe((todos) => {
        this.todos.next(todos);
      });
  }

  private fetchPublicData(): void {
    this.http
      .get<any>('pub/public')
      .pipe(catchError(this.showError))
      .subscribe((data) => {
        const name = `${data.firstName} ${data.lastName}`;
        console.log(name);
        this.publicData = name;
      });
  }

  private readonly showError = (err: HttpErrorResponse) => {
    if (err.status !== 401) {
      this.errors.next(err.message);
    }
    throw err;
  }
}

interface Todo {
  id: number;
  name: string;
  date: string;
  user: string;
}
