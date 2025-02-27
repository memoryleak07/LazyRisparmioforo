import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs';
import {StatRequest, StatSpentPerCategoryResponse} from './statistics.models';
import {Flow} from '../../constants/enums';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/statistics`;

  totalAmount(request: StatRequest): Observable<number> {
    return this.http.get<number>(
      `${this.apiUrl}/total-amount?FromDate=${request.fromDate}&ToDate=${request.toDate}&Flow=${request.flow}`);
  }

  // totalEarned(request: StatRequest): Observable<number> {
  //   return this.http.get<number>(
  //     `${this.apiUrl}/total-spent?FromDate=${request.fromDate}&ToDate=${request.toDate}&Flow=${Flow.Income}`);
  // }

  spentPerCategory(request: StatRequest): Observable<StatSpentPerCategoryResponse[]> {
    return this.http.get<StatSpentPerCategoryResponse[]>(
      `${this.apiUrl}/spent-per-category?FromDate=${request.fromDate}&ToDate=${request.toDate}&Flow=${Flow.Expense}`);
  }
}
