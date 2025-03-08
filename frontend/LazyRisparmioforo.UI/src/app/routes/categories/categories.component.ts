import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {Subscription} from 'rxjs';
import {selectAllCategories} from '../../store/category/category.reducers';
import {CategoryDto} from '../../constants/default-categories';
import {FormsModule} from '@angular/forms';
import {NgForOf, NgIf, NgOptimizedImage} from '@angular/common';

@Component({
  selector: 'app-categories',
  imports: [
    FormsModule,
    NgForOf,
    NgIf,
    NgOptimizedImage,
  ],
  templateUrl: './categories.component.html'
})
export class CategoriesComponent implements OnInit, OnDestroy {
  private store = inject(Store);
  private subscriptions = new Subscription();
  public categories : CategoryDto[] = [];

  constructor() {}

  ngOnInit() {
    this.subscriptions.add(this.store.select(selectAllCategories).subscribe(categories => {
      this.categories = categories;
    }))
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
}
