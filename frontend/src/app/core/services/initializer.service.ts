import { Injectable, WritableSignal, signal } from '@angular/core';
import { EMPTY, catchError, finalize, tap } from 'rxjs';

import { LoadingOverlayService } from './loading-overlay.service';
import { TokenService } from './token.service';
import { UserService } from './user.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class InitializerService {
  isInitializing: WritableSignal<boolean> = signal(true);
  isBackendOffline: WritableSignal<boolean> = signal(false);

  constructor(
    private loadingOverlayService: LoadingOverlayService,
    private tokenService: TokenService,
    private userService: UserService
  ) { }

  initialize() {
    if (this.tokenService.isEmpty())
      return;

    this.userService.getUser()
      .pipe(
        tap(user => {
          this.userService.setUser(user);

          if (localStorage.getItem('isBackendOffline')) {
            localStorage.removeItem('isBackendOffline');
            location.reload();
          }
        }),
        catchError(err => {
          if (err instanceof HttpErrorResponse && err.status == 0) {
            this.isBackendOffline.set(true);
            localStorage.setItem('isBackendOffline', '1');
          }
          ;
          return EMPTY;
        }),
        finalize(() => {
          this.isInitializing.set(false);
        })
      ).subscribe();
  }
}
