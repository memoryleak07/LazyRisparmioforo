import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs';
import {Category, CategoryPredictRequest, CategoryPredictResponse} from './category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/categories`;

  all(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/all`);
  }

  predict(request: CategoryPredictRequest): Observable<CategoryPredictResponse> {
    return this.http.post<CategoryPredictResponse>(`${this.apiUrl}/predict?Input=${request.input}`, null);
  }
}
