import {Component, CUSTOM_ELEMENTS_SCHEMA, inject, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {combineLatest, Subscription} from 'rxjs';
import {selectLastTransactions} from '../../store/transaction/transaction.reducers';
import {Transaction} from '../../services/transaction-service/transaction.model';
import {TransactionActions} from '../../store/transaction/transaction.actions';
import {TableComponent} from '../../shared/ui/table/table.component';
import {selectAllCategories} from '../../store/category/category.reducers';
import {StatisticsActions} from '../../store/statistics/statistics.actions';
import {DashboardBalanceComponent} from '../../shared/components/dashboard-balance/dashboard-balance.component';
import {DashboardCategoryAmountComponent} from '../../shared/components/dashboard-category-amount/dashboard-category-amount.component';

@Component({
  selector: 'app-dashboard',
  imports: [
    TableComponent,
    DashboardBalanceComponent,
    DashboardCategoryAmountComponent
  ],
  templateUrl: './dashboard.component.html',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class DashboardComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscriptions = new Subscription();
  public tableColumns: { key: keyof Transaction, label: string }[] = [
    { key: 'registrationDate', label: 'Date' },
    { key: 'category', label: 'Category' },
    { key: 'amount', label: 'Amount' }
  ];
  public tableItems: Transaction[] = [];

  constructor() {}

  ngOnInit() {
    this.store.dispatch(StatisticsActions.getMainStatistics());
    this.store.dispatch(TransactionActions.searchTransactions({
      query: {
        pageIndex: 0,
        pageSize: 5,
      }
    }));

    this.subscriptions.add(
      combineLatest([
        this.store.select(selectLastTransactions),
        this.store.select(selectAllCategories)
      ]).subscribe(([lastTransactions, categories]) => {
        this.tableItems = lastTransactions.slice(0, 5).map(transaction => {
          const category = categories.find(cat => cat.id === transaction.categoryId);
          return {
            ...transaction,
            category: category ? category.name : 'Unknown',
          };
        });
      })
    );
  }

  ngOnDestroy() {
    // this.store.dispatch(TransactionActions.clearLastTransactions());
    this.subscriptions.unsubscribe();
  }
}
