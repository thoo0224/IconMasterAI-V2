import { Component, OnInit, WritableSignal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { EMPTY, Observable, catchError, tap } from 'rxjs';

import { LoadingOverlayService } from 'src/app/core/services/loading-overlay.service';
import { AuthService } from 'src/app/core/services/app/auth.service';
import { LoginResponse, ProblemDetails } from 'src/app/core/interfaces/responses';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  errorAlertMessage: string | undefined;
  loginForm: FormGroup;

  public get emailControl() {
    return this.loginForm.get('email');
  }

  public get passwordControl() {
    return this.loginForm.get('password');
  }

  constructor(
    private loadingOverlayService: LoadingOverlayService,
    private activatedRoute: ActivatedRoute,
    private formsBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.loginForm = this.formsBuilder.group({
      'email': new FormControl('', [Validators.required, Validators.email]),
      'password': new FormControl('', [Validators.required])
    });
  }

  ngOnInit() {
    const params = this.activatedRoute.snapshot.queryParamMap;
    const pathWithoutQueryParams = window.location.pathname;
    window.history.replaceState(null, null, pathWithoutQueryParams);

    const accessToken = params.get('accessToken');
    if (!accessToken)
      return;

    this.handleLoginResponse({ token: accessToken })
  }

  login() {
    const email = this.loginForm.controls['email'].value;
    const password = this.loginForm.controls['password'].value;

    this.loadingOverlayService.load(
      this.authService.login(email, password)
        .pipe(
          tap(response => {
            this.handleLoginResponse(response);
          }),
          catchError(response => {
            return this.handleErrorResponse(response);
          })
        )
    ).subscribe();
  }

  handleLoginResponse(response: LoginResponse) {
    console.log(response);
    this.authService.handleAccessToken(response.token);
    location.href = "/";
  }

  handleErrorResponse(response: HttpErrorResponse) {
    const problem = response.error as ProblemDetails;
    this.errorAlertMessage = problem.detail;
    return EMPTY;
  }
}
