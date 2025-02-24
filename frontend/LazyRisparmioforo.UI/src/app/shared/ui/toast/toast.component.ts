import {Component, inject} from '@angular/core';
import {NgClass, NgForOf} from '@angular/common';
import {ToastService} from './toast.service';

@Component({
  selector: 'app-toast',
  imports: [
    NgClass,
    NgForOf
  ],
  templateUrl: './toast.component.html'
})
export class ToastComponent {
  public toastService = inject(ToastService);
}
