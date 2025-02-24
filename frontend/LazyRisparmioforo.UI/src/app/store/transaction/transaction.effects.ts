import {Injectable, inject} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {mergeMap, of} from 'rxjs';
import {exhaustMap, catchError} from 'rxjs/operators';
import {TransactionService} from '../../services/transaction-service/transaction.service';
import {TransactionActions} from './transaction.actions';

@Injectable()
export class TransactionEffects {
  private actions$ = inject(Actions);
  private transactionService = inject(TransactionService);

  getTransaction = createEffect(() => {
    return this.actions$.pipe(
      ofType(TransactionActions.getTransaction),
      exhaustMap((data =>
            this.transactionService.get(data.query).pipe(
              mergeMap((response) => [
                TransactionActions.setCurrentTransaction({response})
              ]),
              catchError((error: { message: string }) =>
                of(TransactionActions.errorTransactionService({error: error.message}))
              )
            )
        )
      ));
  });

  searchTransactions = createEffect(() => {
    return this.actions$.pipe(
      ofType(TransactionActions.searchTransactions),
      exhaustMap((data =>
            this.transactionService.search(data.query).pipe(
              mergeMap((response) => [
                TransactionActions.setLastTransactions({response})
              ]),
              catchError((error: { message: string }) =>
                of(TransactionActions.errorTransactionService({error: error.message}))
              )
            )
        )
      ));
  });

  createTransaction = createEffect(() => {
    return this.actions$.pipe(
      ofType(TransactionActions.createTransaction),
      exhaustMap((data =>
            this.transactionService.create(data.request).pipe(
              mergeMap(() => [
                TransactionActions.searchTransactions({
                  query: { pageIndex: 0, pageSize: 10 }
                }),
                TransactionActions.showMessage({
                  message: "Transaction created successfully."
                }),
                TransactionActions.clearMessage(),
              ]),
              catchError((error: { message: string }) =>
                of(TransactionActions.errorTransactionService({error: error.message}))
              )
            )
        )
      ));
  });

  updateTransaction = createEffect(() => {
    return this.actions$.pipe(
      ofType(TransactionActions.updateTransaction),
      exhaustMap((data =>
            this.transactionService.update(data.request).pipe(
              mergeMap(() => [
                TransactionActions.searchTransactions({
                  query: { pageIndex: 0, pageSize: 10 }
                }),
                TransactionActions.showMessage({
                  message: "Transaction updated successfully."
                }),
                TransactionActions.clearMessage(),
              ]),
              catchError((error: { message: string }) =>
                of(TransactionActions.errorTransactionService({error: error.message}))
              )
            )
        )
      ));
  });

  deleteTransaction = createEffect(() => {
    return this.actions$.pipe(
      ofType(TransactionActions.deleteTransaction),
      exhaustMap((data =>
            this.transactionService.delete(data.query).pipe(
              mergeMap(() => [
                TransactionActions.searchTransactions({
                  query: { pageIndex: 0, pageSize: 10 }
                }),
                TransactionActions.showMessage({
                  message: "Transaction deleted successfully."
                }),
                TransactionActions.clearMessage()
              ]),
              catchError((error: { message: string }) =>
                of(TransactionActions.errorTransactionService({error: error.message}))
              )
            )
        )
      ));
  });
}
