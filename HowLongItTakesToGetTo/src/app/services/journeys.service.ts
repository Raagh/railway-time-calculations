import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Journey } from '../model/journey.model';

@Injectable({
  providedIn: 'root'
})
export class JourneysService {

  constructor(private http: HttpClient) { }

  getJourneyTime(departFrom: string, arriveAt:string): Observable<Journey> {
    const url = `https://localhost:44325/v1/journeys/${departFrom}/${arriveAt}`;

    return this.http.get<Journey>(url);
  }
}
