import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {Subscription} from 'rxjs';
import {selectSpentPerCategory} from '../../../store/statistics/statistics.reducers';
import {CategoryAmountResponse} from '../../../services/statistics-service/statistics.models';
import {NgForOf} from '@angular/common';
import {CategoryBadgeComponent} from '../../../shared/components/category-badge/category-badge.component';
import {AmountPipe} from '../../../shared/pipes/amount.pipe';

@Component({
  selector: 'app-dashboard-category-amount',
  imports: [
    NgForOf,
    CategoryBadgeComponent,
    AmountPipe
  ],
  templateUrl: './dashboard-category-amount.component.html'
})
export class DashboardCategoryAmountComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscription: Subscription = new Subscription();
  public data: CategoryAmountResponse[] = [];

  constructor() {}

  ngOnInit() {
    this.subscription.add(this.store.select(selectSpentPerCategory).subscribe(data => {
      this.data = data.slice(0, 5);
    }));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
