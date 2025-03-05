import {createReducer, on, createSelector, createFeatureSelector} from '@ngrx/store';
import {Transaction} from '../../services/transaction-service/transaction.model';
import {TransactionActions} from './transaction.actions';

export const transactionFeatureKey = 'transaction';

export interface TransactionState {
  error: string | null,
  message: string | null,
  currentTransaction: Transaction | null,
  lastTransactions: Transaction[],
  pagination: {
    pageIndex: number;
    pageSize: number;
    totalItemsCount: number;
    totalPagesCount: number;
  };
}

export const initialState: TransactionState = {
  error: null,
  message: null,
  currentTransaction: null,
  lastTransactions: [],
  pagination: {
    pageIndex: 0,
    pageSize: 5,
    totalItemsCount: 0,
    totalPagesCount: 0,
  }
};

export const selectFeature = createFeatureSelector<TransactionState>(
  transactionFeatureKey);

export const selectCurrentTransaction = createSelector(
  selectFeature,
  (state: TransactionState) => state.currentTransaction
);

export const selectLastTransactions = createSelector(
  selectFeature,
  (state: TransactionState) => state.lastTransactions
);

export const selectPagination = createSelector(
  selectFeature,
  (state: TransactionState) => state.pagination
);

export const selectMessage = createSelector(
  selectFeature,
  (state: TransactionState) => state.message
);

export const selectErrorTransactionService = createSelector(
  selectFeature,
  (state: TransactionState) => state.error
);

export const transactionReducer = createReducer(
  initialState,
  on(TransactionActions.errorTransactionService, (state, {error}) => {
    return {...state, error: error};
  }),
  on(TransactionActions.showMessage, (state, { message }) => {
    return { ...state, message: message };
  }),
  on(TransactionActions.clearMessage, (state) => {
    return { ...state, message: null };
  }),
  on(TransactionActions.setCurrentTransaction, (state, {response}) => {
    return {...state, currentTransaction: response, error: null};
  }),
  on(TransactionActions.setLastTransactions, (state, {response}) => {
    return {
      ...state,
      lastTransactions: response.items,
      pagination: {
        pageIndex: response.pageIndex,
        pageSize: response.pageSize,
        totalItemsCount: response.totalItemsCount,
        totalPagesCount: response.totalPagesCount,
      },
    };
  }),
  on(TransactionActions.clearLastTransactions, (state) => {
    return {
      ...state,
      lastTransactions: [],
      pagination: {
        pageIndex: 0,
        pageSize: 10,
        totalItemsCount: 0,
        totalPagesCount: 0,
      },
      error: null
    };
  }),
);
