import { HttpClient } from '@angular/common/http';
import { Injectable, WritableSignal, signal } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { User } from '../interfaces/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  user: WritableSignal<User | undefined> = signal(null);

  constructor(
    private httpClient: HttpClient
  ) { }

  getUser(): Observable<User> {
    return this.httpClient.get<User>(`${environment.backendUrl}/api/user`);
  }

  setUser(user: User) {
    this.user.set(user);
  }
}
