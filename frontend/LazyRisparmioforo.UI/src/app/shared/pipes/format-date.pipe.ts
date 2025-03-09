import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatDate'
})
export class FormatDatePipe implements PipeTransform {
  transform(value: string): string {
    if (!value) {
      return "";
    }

    const date = new Date(value);
    if (isNaN(date.getTime())) {
      return "Invalid Date";
    }

    return new Intl.DateTimeFormat('it-IT', {
      weekday: "short",
      day: "2-digit",
      month: "short",
      year: "numeric"
    }).format(date);
  }
}
