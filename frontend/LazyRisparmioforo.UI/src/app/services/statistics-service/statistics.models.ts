import {Flow} from '../../constants/enums';

export interface StatRequest {
  fromDate: string;
  toDate: string;
  flow?: Flow;
}

export interface StatMainResponse {
  totalSpentWeek: number;
  totalSpentMonth: number;
  totalSpentYear: number;
  totalEarnedMonth: number;
  totalEarnedYear: number;
}

export interface StatTotalSpentResponse {
  amount: number;
}

export interface StatSpentPerCategoryResponse {
  categoryId: number;
  amount: number;
}
