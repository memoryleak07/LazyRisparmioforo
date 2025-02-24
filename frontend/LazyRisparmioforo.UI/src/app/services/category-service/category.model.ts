export interface Category {
  id: number;
  name: string;
}

export interface CategoryPredictRequest {
  input: string;
}
export interface CategoryPredictResponse {
  id: number;
  name: string;
  confidence: number;
}
