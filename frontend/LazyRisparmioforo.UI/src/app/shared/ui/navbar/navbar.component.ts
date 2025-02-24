import {Component, inject} from '@angular/core';
import {NAV_MENU_ITEMS, NavMenuItem} from '../../../constants/navigation-config';
import {NgForOf, NgIf} from '@angular/common';
import {Router, RouterLink} from '@angular/router';
import {
  TransactionCreateButtonComponent
} from '../../components/transaction-create-button/transaction-create-button.component';
import {ThemesComponent} from '../theme/themes.component';

@Component({
  selector: 'app-navbar',
  imports: [
    NgIf,
    NgForOf,
    RouterLink,
    TransactionCreateButtonComponent,
    ThemesComponent
  ],
  templateUrl: './navbar.component.html'
})
export class NavbarComponent {
  private readonly router = inject(Router);
  navMenuItems: NavMenuItem[] = NAV_MENU_ITEMS;

  navigateTo(route: string, queryParams?: Record<string, any>) {
    this.router.navigate([route], {queryParams})
      .then(() =>{
        // Close the drawer
        // this.drawerChecked = this.drawerChecked ? !this.drawerChecked : false;
    });
  }
}
