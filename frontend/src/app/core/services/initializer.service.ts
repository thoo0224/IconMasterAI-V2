import { Injectable } from '@angular/core';
import { EMPTY, catchError, tap } from 'rxjs';

import { LoadingOverlayService } from './loading-overlay.service';
import { TokenService } from './token.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class InitializerService {

  constructor(
    private loadingOverlayService: LoadingOverlayService,
    private tokenService: TokenService,
    private userService: UserService
  ) { }

  // TODO: Other layout for loading when server is offline
  initialize() {
    if (this.tokenService.isEmpty())
      return;

    this.userService.getUser()
      .pipe(
        tap(user => {
          this.userService.setUser(user);
        }),
        catchError(err => {
          console.error(err); // TODO: Handle
          return EMPTY;
        })
      ).subscribe()
  }
}
