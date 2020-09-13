import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { JourneysService } from './services/journeys.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
  stations = ['Basingstoke', 'Reading', 'Farnborough', 'Clapham Junction', 'London Victoria', 'London Paddington'];
  title = 'HowLongItTakesToGetTo';
  journey$: Observable<string>;
  journeyForm:FormGroup;

  constructor(private readonly formBuilder: FormBuilder, private readonly journeyService: JourneysService) {}

  ngOnInit() {
    this.journeyForm = this.formBuilder.group({
      departFrom: [null],
      arriveAt: [null]
    });
  }

  onSubmit(): void {
    if (this.journeyForm.valid) {
      this.journey$ = this.journeyService.getJourneyTime(this.journeyForm.value.departFrom, this.journeyForm.value.arriveAt)
      .pipe(map(journey => `${journey.time} minutes if leave now, run!`));
    }
  }
}


