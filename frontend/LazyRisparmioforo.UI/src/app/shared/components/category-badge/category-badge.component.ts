import {Component, Input} from '@angular/core';
import {CATEGORY_CONFIG} from '../../../constants/default-categories';
import {NgIf, NgOptimizedImage, NgStyle} from '@angular/common';

@Component({
  selector: 'app-category-badge',
  imports: [
    NgIf,
    NgOptimizedImage,
    NgStyle,
  ],
  template: `
    <div class="badge font-semibold p-3 text-white"
         [ngStyle]="{'background-color': category?.color}">
      <img *ngIf="showIcon"
           ngSrc="{{category?.icon}}"
           alt="{{category?.name }}"
           [width]="12"
           [height]="12" />
      <span>{{ category?.name ?? 'Unknown' }}</span>
    </div>
  `,
})
export class CategoryBadgeComponent {
  @Input() categoryId!: number;
  @Input() showIcon: boolean = true;

  get category() {
    return CATEGORY_CONFIG.find(c => c.id === this.categoryId)
      || CATEGORY_CONFIG.find(c => c.id === 99);
  }
}
