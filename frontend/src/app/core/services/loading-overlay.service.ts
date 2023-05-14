import { Injectable, signal, WritableSignal } from '@angular/core';
import { finalize, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingOverlayService {
  show: WritableSignal<boolean> = signal(false)

  constructor() { }

  load<T>(observable: Observable<T>): Observable<T> {
    this.show.set(true);
    return observable.pipe(
      finalize(() => {
        this.show.set(false);
      })
    );
  }
}
