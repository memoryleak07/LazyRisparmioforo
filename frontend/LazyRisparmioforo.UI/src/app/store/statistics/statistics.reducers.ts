import {createReducer, on, createSelector, createFeatureSelector} from '@ngrx/store';
import {
  StatMainResponse,
  CategoryAmountResponse,
  MonthlySummaryResponse
} from '../../services/statistics-service/statistics.models';
import {StatisticsActions} from './statistics.actions';

export const statisticsFeatureKey = 'statistics';

export interface StatisticsState {
  error: string | null,
  mainStat: StatMainResponse | null,
  spentPerCategory: CategoryAmountResponse[],
  monthlySummary: MonthlySummaryResponse[],
}

export const initialState: StatisticsState = {
  error: null,
  mainStat: null,
  spentPerCategory: [],
  monthlySummary: [],
};

export const selectFeature = createFeatureSelector<StatisticsState>(
  statisticsFeatureKey);

export const selectMainStat = createSelector(
  selectFeature,
  (state: StatisticsState) => state.mainStat
);

export const selectSpentPerCategory = createSelector(
  selectFeature,
  (state: StatisticsState) => state.spentPerCategory
);

export const selectMonthlySummary = createSelector(
  selectFeature,
  (state: StatisticsState) => state.monthlySummary
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
  on(StatisticsActions.setMainStatistics, (state, {response}) => {
    return {...state, mainStat: response, error: null};
  }),
  on(StatisticsActions.setSpentPerCategory, (state, {response}) => {
    return {...state, spentPerCategory: response, error: null};
  }),
  on(StatisticsActions.setMonthlySummary, (state, {response}) => {
    return {...state, monthlySummary: response, error: null};
  }),
);
