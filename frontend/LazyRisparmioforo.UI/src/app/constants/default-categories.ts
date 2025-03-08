import {Category} from '../services/category-service/category.model';

const path = 'icons'

export interface CategoryDto extends Category {
  icon: string;
  color: string;
}

export const CATEGORY_CONFIG: CategoryDto[] = [
  {
    id: 0,
    name: "BILLS_SUBSCRIPTIONS",
    icon: `${path}/bell.svg`,
    color: "#541121",
  },
  {
    id: 1,
    name: "GROCERIES",
    icon: `${path}/basket-shopping.svg`,
    color: "#541121"
  },
  {
    id: 2,
    name: "EATING_OUT",
    icon: `${path}/utensils.svg`,
    color: "#FFB200"},
  {
    id: 3,
    name: "TRANSPORTATION",
    icon: `${path}/route.svg`,
    color: "#E53888"},
  {
    id: 4,
    name: "CLOTHING",
    icon: `${path}/shirt.svg`,
    color: "#F37199"
  },
  {
    id: 5,
    name: "HEALTH_WELLNESS",
    icon: `${path}/kit-medical.svg`,
    color: "#A7E6FF"},
  {

    id: 6,
    name: "SHOPPING",
    icon: `${path}/shopping_cart.svg`,
    color: "#3572EF"},
  {

    id: 7,
    name: "LEISURE_ENTERTAINMENT",
    icon: `${path}/swim.svg`,
    color: "#005B41"},
  {
    id: 8, name: "HOME_MAINTENANCE",
    icon: `${path}/house.svg`,
    color: "#FFD23F"},
  {
    id: 9, name: "PERSONAL_CARE",
    icon: `${path}/bath.svg`,
    color: "#40dff4"
  },
  {
    id: 10, name: "TRAVEL_DAILY",
    icon: `${path}/route.svg`,
    color: "#FC6736"
  },
  {
    id: 11, name: "WAGES_INCOME",
    icon: `${path}/hand-holding-dollar.svg`,
    color: "#005B41"
  },
  {
    id: 12,
    name: "TRANSFERS",
    icon: `${path}/right-left.svg`,
    color: "#4E6E81"
  },
  {
    id: 99,
    name: "OTHER",
    icon: `${path}/ghost.svg`,
    color: "#635985"
  }
];
