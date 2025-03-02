import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs';
import {StatRequest, CategoryAmountResponse, StatResponse} from './statistics.models';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/statistics`;

  summary(request: StatRequest): Observable<StatResponse> {
    return this.http.get<StatResponse>(
      `${this.apiUrl}/summary?FromDate=${request.fromDate}&ToDate=${request.toDate}`);
  }

  spentPerCategory(request: StatRequest): Observable<CategoryAmountResponse[]> {
    return this.http.get<CategoryAmountResponse[]>(
      `${this.apiUrl}/spent-per-category?FromDate=${request.fromDate}&ToDate=${request.toDate}`);
  }
}
