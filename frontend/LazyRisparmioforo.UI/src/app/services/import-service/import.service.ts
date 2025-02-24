import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpEvent, HttpHeaders} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ImportService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/import`;

  csv(file: File): Observable<HttpEvent<void>> {
    const formData = new FormData();
    formData.append('formFile', file, file.name);
    return this.http.post<void>(
      `${this.apiUrl}/csv`,
      formData,
      {
        headers: new HttpHeaders({Accept: 'application/json',}),
        reportProgress: true,
        observe: 'events',
      });
  }
}
