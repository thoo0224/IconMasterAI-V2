import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, WritableSignal, signal } from '@angular/core';

import { ProblemDetails } from '../interfaces/responses';

@Injectable({
  providedIn: 'root'
})
export class ErrorModalService {
  message: WritableSignal<string> = signal('');
  show: WritableSignal<boolean> = signal(false);

  constructor() { }

  close() {
    this.show.set(false);
  }

  open(message: string) {
    this.message.set(message);
    this.show.set(true);
  }

  openHttpError(res: HttpErrorResponse) {
    if (res.status == 0) {
      this.open('Failed to connect to server.');
      return;
    }

    const error = res.error as ProblemDetails;
    this.open(`${error.detail}`)
  }
}
