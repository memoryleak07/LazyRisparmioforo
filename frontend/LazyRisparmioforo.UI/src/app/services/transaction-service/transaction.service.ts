import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs';
import {
  Transaction,
  TransactionGetCommand,
  TransactionSearchCommand,
  TransactionCreateCommand,
  TransactionUpdateCommand,
  TransactionRemoveCommand,
} from './transaction.model';
import {Pagination} from '../../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/transactions`;

  get(request: TransactionGetCommand): Observable<Transaction> {
    return this.http.get<Transaction>(`${this.apiUrl}/${request.id}`);
  }

  search(request: TransactionSearchCommand): Observable<Pagination<Transaction>> {
    let url = `${this.apiUrl}/search?PageIndex=${request.pageIndex}&PageSize=${request.pageSize}`;
    if (request.flow != undefined) url = url + `&Flow=${request.flow}`;
    if (request.query != undefined) url = url + `&Query=${request.query}`;
    if (request.fromDate != undefined) url = url + `&FromDate=${request.fromDate}`;
    if (request.toDate != undefined) url = url + `&ToDate=${request.toDate}`;
    return this.http.get<Pagination<Transaction>>(url);
  }

  create(request: TransactionCreateCommand): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/create`, request);
  }

  update(request: TransactionUpdateCommand): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/update`, request);
  }

  delete(request: TransactionRemoveCommand): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/remove?Id=${request.id}`);
  }
}
