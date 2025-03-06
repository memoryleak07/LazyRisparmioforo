import {Component, CUSTOM_ELEMENTS_SCHEMA, inject, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {combineLatest, Subscription} from 'rxjs';
import {selectLastTransactions} from '../../store/transaction/transaction.reducers';
import {Transaction} from '../../services/transaction-service/transaction.model';
import {TransactionActions} from '../../store/transaction/transaction.actions';
import {TableComponent} from '../../shared/ui/table/table.component';
import {selectAllCategories} from '../../store/category/category.reducers';
import {StatisticsActions} from '../../store/statistics/statistics.actions';
import {DashboardBalanceComponent} from './dashboard-balance/dashboard-balance.component';
import {DashboardCategoryAmountComponent} from './dashboard-category-amount/dashboard-category-amount.component';
import {RouterLink} from '@angular/router';
import {DashboardBarChartComponent} from './dashboard-bar-chart/dashboard-bar-chart.component';
import {DashboardDonutChartComponent} from './dashboard-donut-chart/dashboard-donut-chart.component';
import {formatAmount} from '../../shared/pipes/amount.pipe';

interface FormattedTransaction extends Transaction {
  formattedAmount: string;
  category: string;
}

@Component({
  selector: 'app-dashboard',
  imports: [
    TableComponent,
    DashboardBalanceComponent,
    DashboardCategoryAmountComponent,
    RouterLink,
    DashboardBarChartComponent,
    DashboardDonutChartComponent,
  ],
  templateUrl: './dashboard.component.html',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class DashboardComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscriptions = new Subscription();
  public tableColumns: { key: keyof FormattedTransaction, label: string }[] = [
    { key: 'registrationDate', label: 'Date' },
    { key: 'category', label: 'Category' },
    { key: 'formattedAmount', label: 'Amount' }
  ];
  public tableItems: FormattedTransaction[] = [];
  public tableTitle: string = 'Last transactions';

  constructor() { }

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
            formattedAmount: formatAmount(transaction.amount).toString(),
          };
        });
        this.tableTitle = `Last ${this.tableItems.length} transaction`;
      })
    );
  }

  ngOnDestroy() {
    // this.store.dispatch(TransactionActions.clearLastTransactions());
    this.subscriptions.unsubscribe();
  }
}
