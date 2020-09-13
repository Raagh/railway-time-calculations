import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Journey } from '../model/journey.model';
import { CONFIG } from '../../assets/config/api.config';

@Injectable({
  providedIn: 'root'
})
export class JourneysService {

  constructor(private http: HttpClient) { }

  getJourneyTime(departFrom: string, arriveAt:string): Observable<Journey> {
    const url = `${CONFIG.API}${departFrom}/${arriveAt}`;

    return this.http.get<Journey>(url);
  }
}
