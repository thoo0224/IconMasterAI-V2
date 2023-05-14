import { Injectable } from '@angular/core';

const KEY: string = "accessToken"

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  constructor() { }

  get(): string {
    return localStorage.getItem(KEY);
  }

  set(token: string) {
    localStorage.setItem(KEY, token);
  }

  remove() {
    localStorage.removeItem(KEY);
  }

  isEmpty(): boolean {
    return this.get() == null;
  }
}
