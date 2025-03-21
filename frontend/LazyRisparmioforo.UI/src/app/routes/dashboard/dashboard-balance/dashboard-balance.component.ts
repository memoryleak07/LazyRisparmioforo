import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {AmountPipe} from '../../../shared/pipes/amount.pipe';
import {Store} from '@ngrx/store';
import {Subscription} from 'rxjs';
import {selectMainStat} from '../../../store/statistics/statistics.reducers';
import {StatMainResponse} from '../../../services/statistics-service/statistics.models';

@Component({
  selector: 'app-dashboard-balance',
  imports: [
    AmountPipe,
  ],
  templateUrl: './dashboard-balance.component.html'
})
export class DashboardBalanceComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscriptions = new Subscription();
  public data: StatMainResponse = {
    weekly: { income: 0, expense: 0, balance: 0 },
    monthly: { income: 0, expense: 0, balance: 0 },
    yearly: { income: 0, expense: 0, balance: 0 }
  };

  constructor() {}

  ngOnInit() {
    this.subscriptions.add(this.store.select(selectMainStat).subscribe((data) => {
      if (data) this.data = data;
    }));
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
}
