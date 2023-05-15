import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { LoginResponse } from '../../interfaces/responses';
import { Param, httpParamsOf } from '../../interfaces/params';
import { TokenService } from '../token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private tokenService: TokenService,
    private httpClient: HttpClient
  ) { }

  login(email: string, password: string): Observable<LoginResponse> {
    return this.loginInternal({
      email: email,
      password: password
    });
  }

  loginInternal(...params: Param[]): Observable<LoginResponse> {
    const body = httpParamsOf(...params);
    const headers = {
      'Content-Type': 'application/x-www-form-urlencoded'
    };

    return this.httpClient.post<LoginResponse>(`${environment.backendUrl}/api/auth/login`, body, {
      headers: headers
    });
  }

  logout() {
    this.tokenService.remove();
    location.href = "/";
  }

  register(userName: string, email: string, password: string): Observable<any> {
    const body = httpParamsOf({
      userName: userName,
      email: email,
      password: password
    });

    const headers = {
      'Content-Type': 'application/x-www-form-urlencoded'
    };

    return this.httpClient.post<LoginResponse>(`${environment.backendUrl}/api/auth/register`, body, {
      headers: headers
    });
  }

  handleAccessToken(accessToken: string) {
    this.tokenService.set(accessToken);
  }
}
