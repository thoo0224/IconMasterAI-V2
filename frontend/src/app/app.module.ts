import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HomeComponent } from './pages/home/home.component';
import { GeneratorComponent } from './pages/generator/generator.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

import { InitializerService } from './core/services/initializer.service';

import { NavigationComponent } from './shared/navigation/navigation.component';
import { GoogleLoginButtonComponent } from './shared/google-login-button/google-login-button.component';
import { LoadingOverlayComponent } from './shared/loading-overlay/loading-overlay.component';
import { ErrorModalComponent } from './shared/error-modal/error-modal.component';

import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AlertComponent } from './shared/alert/alert.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    HomeComponent,
    GeneratorComponent,
    LoginComponent,
    RegisterComponent,
    GoogleLoginButtonComponent,
    LoadingOverlayComponent,
    ErrorModalComponent,
    AlertComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: (initializerService: InitializerService) =>
        () => initializerService.initialize(),
      deps: [InitializerService],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
