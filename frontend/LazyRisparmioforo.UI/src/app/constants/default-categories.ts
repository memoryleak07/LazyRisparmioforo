import {Category} from '../services/category-service/category.model';

const path = 'icons'

export interface CategoryConfiguration extends Category {
  icon: string;
  color: string;
}

export const CATEGORY_CONFIG: CategoryConfiguration[] = [
  {
    id: 0,
    name: "BILLS_SUBSCRIPTIONS",
    icon: `${path}/shopping_cart.svg`,
    color: "bg-blue-500",
  },
  {
    id: 1,
    name: "GROCERIES",
    icon: `${path}/shopping_cart.svg`,
    color: "text-green-500"
  },
  {
    id: 2,
    name: "EATING_OUT",
    icon: `${path}/shopping_cart.svg`,
    color: "text-orange-500"},
  {
    id: 3,
    name: "TRANSPORTATION",
    icon: `${path}/shopping_cart.svg`,
    color: "text-yellow-500"},
  {
    id: 4,
    name: "CLOTHING",
    icon: `${path}/shopping_cart.svg`,
    color: "bg-purple-500"
  },
  {
    id: 5,
    name: "HEALTH_WELLNESS",
    icon: `${path}/shopping_cart.svg`,
    color: "text-red-500"},
  {

    id: 6,
    name: "SHOPPING",
    icon: `${path}/shopping_cart.svg`,
    color: "text-pink-500"},
  {

    id: 7,
    name: "LEISURE_ENTERTAINMENT",
    icon: `${path}/shopping_cart.svg`,
    color: "text-indigo-500"},
  {
    id: 8, name: "HOME_MAINTENANCE",
    icon: `${path}/shopping_cart.svg`,
    color: "text-gray-500"},
  {
    id: 9, name: "PERSONAL_CARE",
    icon: `${path}/shopping_cart.svg`,
    color: "text-teal-500"
  },
  {
    id: 10, name: "TRAVEL_DAILY",
    icon: `${path}/shopping_cart.svg`,
    color: "text-blue-700"
  },
  {
    id: 11, name: "WAGES_INCOME",
    icon: `${path}/shopping_cart.svg`,
    color: "text-green-700"
  },
  {
    id: 12,
    name: "TRANSFERS",
    icon: `${path}/shopping_cart.svg`,
    color: "text-gray-700"
  },
  {
    id: 99,
    name: "OTHER",
    icon: `${path}/shopping_cart.svg`,
    color: "text-gray-400"
  }
];
