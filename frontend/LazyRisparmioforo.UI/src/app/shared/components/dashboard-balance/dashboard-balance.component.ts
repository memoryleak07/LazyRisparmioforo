import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {NgIf} from '@angular/common';
import {AmountPipe} from '../../pipes/amount.pipe';
import {Store} from '@ngrx/store';
import {Subscription} from 'rxjs';
import {selectMainStat} from '../../../store/statistics/statistics.reducers';
import {selectAllCategories} from '../../../store/category/category.reducers';
import {StatMainResponse, StatResponse} from '../../../services/statistics-service/statistics.models';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-dashboard-balance',
  imports: [
    NgIf,
    AmountPipe,
  ],
  templateUrl: './dashboard-balance.component.html'
})
export class DashboardBalanceComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscriptions = new Subscription();
  public activeTab: 'weekly' | 'monthly' | 'yearly' = 'weekly';
  public data: StatMainResponse = {
    weekly: { income: 0, expense: 0 },
    monthly: { income: 0, expense: 0 },
    yearly: { income: 0, expense: 0 }
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
