import {Component, inject, Input, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {Category} from '../../../services/category-service/category.model';
import {Subscription} from 'rxjs';
import {selectAllCategories} from '../../../store/category/category.reducers';

@Component({
  selector: 'app-category-badge',
  imports: [],
  template: `
    <span class="badge">{{ categoryName }}</span>
  `,
})
export class CategoryBadgeComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscriptions = new Subscription();
  public categories: Category[] = [];
  public categoryName: string = '';

  @Input() categoryId: number = 39; // Category.OTHER_OTHER

  ngOnInit() {
    this.subscriptions.add(this.store.select(selectAllCategories).subscribe((categories) => {
      this.categories = categories;
      const category = this.categories.find((cat) => cat.id === this.categoryId);
      this.categoryName = category ? category.name : 'UnknownCategory';
    }));
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
}
