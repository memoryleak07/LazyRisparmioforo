import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {ToastService} from '../../shared/ui/toast/toast.service';
import {combineLatest, Subscription} from 'rxjs';
import {
  selectErrorTransactionService,
  selectLastTransactions,
} from '../../store/transaction/transaction.reducers';
import {Transaction} from '../../services/transaction-service/transaction.model';
import {TransactionActions} from '../../store/transaction/transaction.actions';
import {TableComponent} from '../../shared/ui/table/table.component';
import {Category} from '../../services/category-service/category.model';
import {selectAllCategories} from '../../store/category/category.reducers';
import {StatisticsActions} from '../../store/statistics/statistics.actions';
import {StatSpentPerCategoryResponse} from '../../services/statistics-service/statistics.models';
import {AmountPipe} from '../../shared/pipes/amount.pipe';
// import {selectSpentPerCategory, selectTotalSpent} from '../../store/statistics/statistics.reducers';

@Component({
  selector: 'app-dashboard',
  imports: [
    TableComponent,
    AmountPipe
  ],
  templateUrl: './dashboard.component.html'
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
  public categories: Category[] = [];
  public totalSpent: number = 0;
  public spentPerCategory: StatSpentPerCategoryResponse[] = [];

  constructor() {
    // this.subscriptions.add(this.store.select(selectTotalSpent).subscribe((totalSpent) => {
    //   this.totalSpent = totalSpent;
    // }));
    // this.subscriptions.add(this.store.select(selectSpentPerCategory).subscribe((spentPerCategory) => {
    //   this.spentPerCategory = spentPerCategory;
    // }));
    this.subscriptions.add(
      combineLatest([
        this.store.select(selectLastTransactions),
        this.store.select(selectAllCategories)
      ]).subscribe(([lastTransactions, categories]) => {
        this.tableItems = lastTransactions.map(transaction => {
          const category = categories.find(cat => cat.id === transaction.categoryId);
          return {
            ...transaction,
            category: category ? category.name : 'Unknown'
          };
        });
      })
    );
  }

  ngOnInit() {
    this.store.dispatch(TransactionActions.searchTransactions({
      query: {
        pageIndex: 0,
        pageSize: 5,
      }
    }));

    this.store.dispatch(StatisticsActions.getMainStatistics());
  }

  ngOnDestroy() {
    this.store.dispatch(TransactionActions.clearLastTransactions());
    this.subscriptions.unsubscribe();
  }
}
