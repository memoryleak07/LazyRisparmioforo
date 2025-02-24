import {createReducer, on, createSelector, createFeatureSelector} from '@ngrx/store';
import {StatSpentPerCategoryResponse} from '../../services/statistics-service/statistics.models';
import {StatisticsActions} from './statistics.actions';

export const statisticsFeatureKey = 'statistics';

export interface StatisticsState {
  error: string | null,
  totalSpent: number,
  spentPerCategory: StatSpentPerCategoryResponse[],
}

export const initialState: StatisticsState = {
  error: null,
  totalSpent: 0,
  spentPerCategory: []
};

export const selectFeature = createFeatureSelector<StatisticsState>(
  statisticsFeatureKey);

export const selectTotalSpent = createSelector(
  selectFeature,
  (state: StatisticsState) => state.totalSpent
);

export const selectSpentPerCategory = createSelector(
  selectFeature,
  (state: StatisticsState) => state.spentPerCategory
);

export const selectErrorStatService = createSelector(
  selectFeature,
  (state: StatisticsState) => state.error
);

export const statisticsReducer = createReducer(
  initialState,
  on(StatisticsActions.errorStatService, (state, {error}) => {
    return {...state, error: error};
  }),
  on(StatisticsActions.setTotalSpent, (state, {response}) => {
    return {...state, totalSpent: response, error: null};
  }),
  on(StatisticsActions.setSpentPerCategory, (state, {response}) => {
    return {...state, spentPerCategory: response, error: null};
  }),
);
