import { Component, Signal, computed } from '@angular/core';
import { Router } from '@angular/router';

import { User } from 'src/app/core/interfaces/user';
import { AuthService } from 'src/app/core/services/app/auth.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {

  showProfileMenu: boolean = false
  showMobileMenu: boolean = false;

  routes = [
    {
      label: 'Home',
      route: '/'
    },
    {
      label: 'Generator',
      route: '/generator'
    },
    // {
    //   label: 'Pricing',
    //   route: '/pricing'
    // },
    // {
    //   label: 'Collection',
    //   route: '/collection'
    // },
    // {
    //   label: 'Community',
    //   route: '/community'
    // }
  ]

  user: Signal<User>;
  userProfilePicture: Signal<string> = computed(() => {
    const avatarUrl = this.user().avatarUrl;
    return avatarUrl ?? "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Default_pfp.svg/1200px-Default_pfp.svg.png"
  });

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) {
    this.user = this.userService.user;
  }

  toggleMobileMenu(): void {
    this.showMobileMenu = !this.showMobileMenu;
  }

  toggleProfileMenu(): void {
    this.showProfileMenu = !this.showProfileMenu;
  }

  logout() {
    this.authService.logout();
  }

  // TODO: fix and change deprecated method
  isRouteActive(route: string): boolean {
    return this.router.isActive(route, false);
  }
}
