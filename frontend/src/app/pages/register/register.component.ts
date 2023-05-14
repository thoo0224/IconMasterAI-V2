import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { EMPTY, Observable, catchError, tap } from 'rxjs';

import { PasswordMaxLength, PasswordMinLength, passwordValidator } from 'src/app/core/validators/password.validator';
import { LoadingOverlayService } from 'src/app/core/services/loading-overlay.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { environment } from 'src/environments/environment';
import { ErrorModalService } from 'src/app/core/services/error-modal.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  public googleCallbackUrl = `${environment.backendUrl}/api/auth/login/external/google`;
  public googleClientId = environment.googleClientId

  loginForm: FormGroup;

  public get userNameControl() {
    return this.loginForm.get('userName');
  }

  public get emailControl() {
    return this.loginForm.get('email');
  }

  public get passwordControl() {
    return this.loginForm.get('password');
  }

  constructor(
    private loadingOverlayService: LoadingOverlayService,
    private errorModalService: ErrorModalService,
    private formsBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.loginForm = this.formsBuilder.group({
      'userName': new FormControl('', [Validators.required]),
      'email': new FormControl('', [Validators.required, Validators.email]),
      'password': new FormControl('', Validators.compose([
        Validators.required,
        passwordValidator()
      ]))
    });
  }

  register() {
    const userName = this.userNameControl.value;
    const email = this.emailControl.value;
    const password = this.passwordControl.value;

    this.loadingOverlayService.load(
      this.authService.register(userName, email, password)
        .pipe(
          tap(_ => {
            console.log('success')
          }),
          catchError(res => {
            this.errorModalService.openHttpError(res);
            return EMPTY;
          })
        )
    ).subscribe();
  }

  getErrorNames(errors: ValidationErrors): string[] {
    return errors ? Object.keys(errors) : [];
  }

  getErrorMessage(errorName: string): string {
    const errorMessages = {
      required: 'Password is required',
      maxLength: `Password must be at most ${PasswordMaxLength} characters long`,
      minLength: `Password must be at least ${PasswordMinLength} characters long`,
      nonAlphanumeric: 'Password must contain at least one non-alphanumeric character',
      digit: 'Password must contain at least one digit',
      uppercase: 'Password must contain at least one uppercase letter',
      lowercase: 'Password must contain at least one lowercase letter'
    };

    return errorMessages[errorName];
  }
}
