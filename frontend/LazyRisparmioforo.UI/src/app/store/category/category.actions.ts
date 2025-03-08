import { createAction, props } from '@ngrx/store';
import {Category} from '../../services/category-service/category.model';
import {CategoryDto} from '../../constants/default-categories';

export class CategoryActions {

  static readonly allCategories = createAction(
    '[Category] All');

  static readonly setCategories = createAction(
    '[Category] SetCategories',
    props<{ response: CategoryDto[] }>());

  static readonly errorCategoryService = createAction(
    '[Category] Error',
    props<{ error: string }>()
  );
}
