import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'amount'
})
export class AmountPipe implements PipeTransform {

  transform(value: number, currency: string = '€'): string {
    if (isNaN(value) || value === null) {
      return `${currency}0.00`;
    }
    return `${currency} ${value.toLocaleString('it-IT', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
  }
}
