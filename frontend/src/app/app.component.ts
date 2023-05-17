import { Component, Signal, computed } from '@angular/core';
import { InitializerService } from './core/services/initializer.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  isInitializing: Signal<boolean>;
  isBackendOffline: Signal<boolean>;

  shouldShowOfflinePage: Signal<boolean> = computed(() => this.isBackendOffline() || localStorage.getItem('isBackendOffline') == '1');

  constructor(
    private initializerService: InitializerService
  ) {
    this.isInitializing = this.initializerService.isInitializing;
    this.isBackendOffline = this.initializerService.isBackendOffline;
  }
}
