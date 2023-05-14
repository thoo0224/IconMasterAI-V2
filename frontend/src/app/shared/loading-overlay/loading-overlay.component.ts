import { Component, Signal, computed } from '@angular/core';

import { LoadingOverlayService } from 'src/app/core/services/loading-overlay.service';

@Component({
  selector: 'app-loading-overlay',
  templateUrl: './loading-overlay.component.html',
  styleUrls: ['./loading-overlay.component.scss']
})
export class LoadingOverlayComponent {
  show: Signal<boolean> = computed(() => this.loadingOverlayService.show())

  constructor(
    private loadingOverlayService: LoadingOverlayService
  ) { }
}
