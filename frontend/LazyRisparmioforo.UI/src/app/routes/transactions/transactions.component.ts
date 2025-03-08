import {Component, inject, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Store} from '@ngrx/store';
import {Subscription} from 'rxjs';
import {selectLastTransactions, selectPagination} from '../../store/transaction/transaction.reducers';
import {Transaction} from '../../services/transaction-service/transaction.model';
import {TransactionActions} from '../../store/transaction/transaction.actions';
import {NgForOf, NgIf} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {FlowSelectorComponent} from '../../shared/components/flow-selector/flow-selector.component';
import {Flow} from '../../constants/enums';
import {CategoryBadgeComponent} from '../../shared/components/category-badge/category-badge.component';
import {TransactionDialogComponent} from '../../shared/components/transaction-dialog/transaction-dialog.component';
import {AmountPipe} from '../../shared/pipes/amount.pipe';
import {DatePickerComponent} from '../../shared/ui/date-picker/date-picker.component';
import {ChartComponent} from 'ng-apexcharts';

@Component({
  selector: 'app-transactions',
  imports: [
    NgForOf,
    FormsModule,
    FlowSelectorComponent,
    CategoryBadgeComponent,
    TransactionDialogComponent,
    NgIf,
    AmountPipe,
    DatePickerComponent,
    ChartComponent
  ],
  templateUrl: './transactions.component.html'
})
export class TransactionsComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscriptions = new Subscription();
  public transactions : Transaction[] = [];
  public searchFlow: Flow | undefined = undefined;
  public searchQuery: string | undefined = undefined;
  public searchFromDate: string | undefined = undefined;
  public searchToDate: string | undefined = undefined;
  public pagination = {
    pageIndex: 0,
    pageSize: 5,
    totalItemsCount: 0,
    totalPagesCount: 0,
  };

  @ViewChild('transactionDialog') transactionDialog!: TransactionDialogComponent;

  constructor() {}

  ngOnInit() {
    this.searchPagedTransactions();

    this.subscriptions.add(this.store.select(selectLastTransactions).subscribe((lastTransactions) => {
      this.transactions = lastTransactions;
    }));

    this.subscriptions.add(this.store.select(selectPagination).subscribe((pagination) => {
      this.pagination = pagination;
    }));
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }

  searchPagedTransactions() {
    this.store.dispatch(
      TransactionActions.searchTransactions({
        query: {
          flow: this.searchFlow,
          fromDate: this.searchFromDate,
          toDate: this.searchToDate,
          query: this.searchQuery,
          pageIndex: this.pagination.pageIndex,
          pageSize: this.pagination.pageSize,
        },
      })
    );
  }

  onSearchQuery(){
    if (this.searchQuery && this.searchQuery.length < 3) {
      return;
    }
    this.resetPagination();
    this.searchPagedTransactions();
  }

  onNextPage() {
    if (this.pagination.pageIndex < this.pagination.totalPagesCount - 1) {
      this.pagination = { ...this.pagination, pageIndex: this.pagination.pageIndex + 1 };
      this.searchPagedTransactions();
    }
  }

  onPreviousPage() {
    if (this.pagination.pageIndex > 0) {
      this.pagination = { ...this.pagination, pageIndex: this.pagination.pageIndex - 1 };
      this.searchPagedTransactions();
    }
  }

  onFlowChange(flow: Flow | undefined): void {
    this.searchFlow = flow;

    this.resetPagination();
    this.searchPagedTransactions();
  }

  onDatesSelected(selectedDates: { startDate: string | null; endDate: string | null }): void {
    this.searchFromDate = selectedDates.startDate ?? undefined;
    this.searchToDate = selectedDates.endDate ?? undefined;

    this.resetPagination();
    this.searchPagedTransactions();
  }

  openTransactionDialog(transactionId: number): void {
    this.transactionDialog.transactionId = transactionId;
    this.transactionDialog.open();
  }

  private resetPagination(): void {
    if (this.pagination.pageIndex > 0) {
      this.pagination = { ...this.pagination, pageIndex: 0 };
    }
  }
}
