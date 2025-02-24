import { createAction, props } from '@ngrx/store';
import {
  Transaction,
  TransactionSearchCommand,
  TransactionCreateCommand,
  TransactionRemoveCommand, TransactionGetCommand, TransactionUpdateCommand,
} from '../../services/transaction-service/transaction.model';
import {Pagination} from '../../shared/models/pagination';

export class TransactionActions {

  static readonly getTransaction = createAction(
    '[Transaction] Get',
    props<{ query: TransactionGetCommand }>()
  );

  static readonly setCurrentTransaction = createAction(
    '[Transaction] SetCurrentTransaction',
    props<{ response: Transaction }>()
  );

  static readonly searchTransactions = createAction(
    '[Transaction] Search',
    props<{ query: TransactionSearchCommand }>()
  );

  static readonly setLastTransactions = createAction(
    '[Transaction] SetLastTransactions',
    props<{ response: Pagination<Transaction> }>()
  );

  static readonly clearLastTransactions = createAction(
    '[Transaction] ClearLastTransactions'
  );

  static readonly createTransaction = createAction(
    '[Transaction] Create',
    props<{ request: TransactionCreateCommand }>()
  );

  static readonly updateTransaction = createAction(
    '[Transaction] Update',
    props<{ request: TransactionUpdateCommand }>()
  );

  static readonly deleteTransaction = createAction(
    '[Transaction] Delete',
    props<{ query: TransactionRemoveCommand }>()
  );

  static readonly showMessage = createAction(
    '[Transaction] ShowMessage',
    props<{ message: string }>()
  );

  static readonly clearMessage = createAction(
    '[Transaction] ClearMessage'
  );

  static readonly errorTransactionService = createAction(
    '[Transaction] Error',
    props<{ error: string }>()
  );
}
