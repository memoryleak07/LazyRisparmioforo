import {createReducer, on, createSelector, createFeatureSelector} from '@ngrx/store';
import {CategoryActions} from './category.actions';
import {Category} from '../../services/category-service/category.model';
import {CategoryConfiguration} from '../../constants/default-categories';

export const categoryFeatureKey = 'category';

export interface CategoryState {
  error: string | null,
  allCategories: CategoryConfiguration[],
}

export const initialState: CategoryState = {
  error: null,
  allCategories: [],
};

export const selectFeature = createFeatureSelector<CategoryState>(
  categoryFeatureKey);

export const selectAllCategories = createSelector(
  selectFeature,
  (state: CategoryState) => state.allCategories
);

export const errorCategoryService = createSelector(
  selectFeature,
  (state: CategoryState) => state.error
);


export const categoryReducer = createReducer(
  initialState,
  on(CategoryActions.errorCategoryService, (state, {error}) => {
    return {...state, error: error};
  }),
  on(CategoryActions.setCategories, (state, { response }) => {
    return {
      ...state,
      allCategories: response,
    };
  }),
);
