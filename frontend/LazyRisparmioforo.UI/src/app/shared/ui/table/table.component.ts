import {Component, Input} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {NgForOf, NgIf} from '@angular/common';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-table',
  imports: [
    FormsModule,
    NgForOf,
    NgIf,
    RouterLink
  ],
  templateUrl: './table.component.html'
})
export class TableComponent<T> {
  @Input() cardTitle: string = "Title";
  @Input() routerLink: string | undefined = undefined;
  @Input() items: T[] = [];
  @Input() columns: { key: keyof T, label: string }[] = [];
}
