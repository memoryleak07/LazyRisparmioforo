import {Flow} from '../../constants/enums';
import {SearchCommand} from '../../shared/models/commands';


export interface Transaction {
  id: number,
  flow: Flow,
  amount: number,
  description: string,
  categoryId: number,
  registrationDate: string,
  valueDate: string,
  category?: string,
}

export interface TransactionSearchCommand extends SearchCommand  {
  flow?: Flow;
}

export interface TransactionGetCommand {
  id: number;
}

export interface TransactionCreateCommand {
  categoryId: number;
  date: string;
  description: string;
  amount: number;
}

export interface TransactionUpdateCommand {
  id: number;
  categoryId: number;
  date?: string;
  description: string;
  amount: number;
}

export interface TransactionRemoveCommand {
  id: number;
}
