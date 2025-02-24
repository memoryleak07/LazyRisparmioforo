import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs';
import {StatRequest, StatSpentPerCategoryResponse} from './statistics.models';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/statistics`;

  totalSpent(request: StatRequest): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-spent?FromDate=${request.fromDate}&ToDate=${request.toDate}`);
  }

  spentPerCategory(request: StatRequest): Observable<StatSpentPerCategoryResponse[]> {
    console.log(request);
    return this.http.get<StatSpentPerCategoryResponse[]>(`${this.apiUrl}/spent-per-category?FromDate=${request.fromDate}&ToDate=${request.toDate}`);
  }
}
