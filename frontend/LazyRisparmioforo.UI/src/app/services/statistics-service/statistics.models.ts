export interface StatRequest {
  fromDate: string;
  toDate: string;
}

export interface StatResponse {
  income: number;
  expense: number;
}

export interface StatMainResponse {
  weekly: StatResponse;
  monthly: StatResponse;
  yearly: StatResponse;
}

export interface CategoryAmountResponse {
  categoryId: number;
  amount: number;
}
