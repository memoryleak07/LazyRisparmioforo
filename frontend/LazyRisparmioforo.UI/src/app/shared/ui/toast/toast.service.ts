import { Injectable } from '@angular/core';

export interface Toast {
  id: number;
  message: string;
  type: 'success' | 'error' | 'info' | 'warning';
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toasts: Toast[] = [];
  private counter = 0;

  getToasts() {
    return this.toasts;
  }

  addToast(message: string,
           type: 'success' | 'error' | 'info' | 'warning',
           removeAfter = 5000) {
    const id = this.counter++;
    this.toasts.push({ id, message, type });

    // Auto-remove toast after 5 seconds
    setTimeout(() => this.removeToast(id), removeAfter);
  }

  removeToast(id: number) {
    this.toasts = this.toasts.filter(toast => toast.id !== id);
  }
}
