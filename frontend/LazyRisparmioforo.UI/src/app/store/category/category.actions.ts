import { createAction, props } from '@ngrx/store';
import {Category} from '../../services/category-service/category.model';
import {CategoryConfiguration} from '../../constants/default-categories';

export class CategoryActions {

  static readonly allCategories = createAction(
    '[Category] All');

  static readonly setCategories = createAction(
    '[Category] SetCategories',
    props<{ response: CategoryConfiguration[] }>());

  static readonly errorCategoryService = createAction(
    '[Category] Error',
    props<{ error: string }>()
  );
}
