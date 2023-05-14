import { Component, Signal } from '@angular/core';
import { ErrorModalService } from 'src/app/core/services/error-modal.service';

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.scss']
})
export class ErrorModalComponent {
  message: Signal<string>;
  show: Signal<boolean>;

  constructor(
    private errorModalService: ErrorModalService
  ) {
    this.show = errorModalService.show;
    this.message = errorModalService.message;
  }

  close() {
    this.errorModalService.close();
  }
}
