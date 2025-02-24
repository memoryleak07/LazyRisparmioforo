export interface NavMenuItem {
  id: string,
  label: string;
  link?: string; // For external or non-Angular links
  routerLink?: string; // For Angular router navigation
  queryParams?: Record<string, any>; // For Angular ActivatedRoute navigation
  icon?: string,
  children?: NavMenuItem[];
}

export const NAV_MENU_ITEMS: NavMenuItem[] = [
  {
    id: 'dashboard',
    label: 'Dashboard',
    routerLink: '/dashboard',
  },
  {
    id: 'transactions',
    label: 'Transactions',
    routerLink: '/transactions',
  },
  {
    id: 'import-transactions',
    label: 'Import',
    routerLink: '/import-transactions',
  },
];
