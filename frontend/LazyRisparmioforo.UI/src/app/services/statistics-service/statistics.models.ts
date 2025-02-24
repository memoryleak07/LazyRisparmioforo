export interface StatRequest {
  fromDate: string;
  toDate: string;
}

export interface StatTotalSpentResponse {
  amount: number;
}

export interface StatSpentPerCategoryResponse {
  categoryId: number;
  amount: number;
}
