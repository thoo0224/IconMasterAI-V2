import { AfterViewInit, Component } from '@angular/core';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-google-login-button',
  templateUrl: './google-login-button.component.html',
  styleUrls: ['./google-login-button.component.scss']
})
export class GoogleLoginButtonComponent implements AfterViewInit {
  public googleCallbackUrl = `${environment.backendUrl}/api/auth/login/external/google`;
  public googleClientId = environment.googleClientId

  ngAfterViewInit(): void {
    this.rerenderGoogleLoginButton();
  }

  rerenderGoogleLoginButton() {
    const script = document.createElement('script');
    script.src = 'https://accounts.google.com/gsi/client';
    script.async = true;
    script.defer = true;

    document.body.appendChild(script);
    document.body.removeChild(script);
  }
}
