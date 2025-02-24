import {Component, inject, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Store} from '@ngrx/store';
import {Subscription} from 'rxjs';
import {selectLastTransactions, selectPagination} from '../../store/transaction/transaction.reducers';
import {Transaction} from '../../services/transaction-service/transaction.model';
import {TransactionActions} from '../../store/transaction/transaction.actions';
import {NgForOf} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {FlowSelectorComponent} from '../../shared/components/flow-selector/flow-selector.component';
import {Flow} from '../../constants/enums';
import {CategoryBadgeComponent} from '../../shared/components/category-badge/category-badge.component';
import {RouterLink} from '@angular/router';
import {TransactionDialogComponent} from '../../shared/components/transaction-dialog/transaction-dialog.component';

@Component({
  selector: 'app-transactions',
  imports: [
    NgForOf,
    FormsModule,
    FlowSelectorComponent,
    CategoryBadgeComponent,
    RouterLink,
    TransactionDialogComponent
  ],
  templateUrl: './transactions.component.html'
})
export class TransactionsComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscriptions = new Subscription();
  public transactions : Transaction[] = [];
  public searchFlow: Flow | undefined = undefined;
  public searchQuery: string | undefined = undefined;
  public pagination = {
    pageIndex: 0,
    pageSize: 10,
    totalItemsCount: 0,
    totalPagesCount: 0,
  };

  @ViewChild('transactionDialog') transactionDialog!: TransactionDialogComponent;

  constructor() {
    this.subscriptions.add(this.store.select(selectLastTransactions).subscribe((lastTransactions) => {
      this.transactions = lastTransactions;
    }));

    this.subscriptions.add(this.store.select(selectPagination).subscribe((pagination) => {
      this.pagination = pagination;
    }));
  }

  ngOnInit() {
    this.searchPagedTransactions();
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }

  searchPagedTransactions() {
    this.store.dispatch(
      TransactionActions.searchTransactions({
        query: {
          flow: this.searchFlow,
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

    if (this.pagination.pageIndex > 0) {
      this.pagination = { ...this.pagination, pageIndex: 0 };
    }
    this.searchPagedTransactions();
  }

  openTransactionDialog(transactionId: number): void {
    console.log("openTransactionDialog", transactionId);
    this.transactionDialog.transactionId = transactionId;
    this.transactionDialog.open();
  }
}
