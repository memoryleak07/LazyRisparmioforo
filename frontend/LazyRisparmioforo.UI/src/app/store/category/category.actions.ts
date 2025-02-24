import { createAction, props } from '@ngrx/store';
import {Category} from '../../services/category-service/category.model';

export class CategoryActions {

  static readonly allCategories = createAction(
    '[Category] All');

  static readonly setCategories = createAction(
    '[Category] SetCategories',
    props<{ response: Category[] }>());

  static readonly errorCategoryService = createAction(
    '[Category] Error',
    props<{ error: string }>()
  );
}
