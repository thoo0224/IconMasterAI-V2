import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './pages/home/home.component';
import { GeneratorComponent } from './pages/generator/generator.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

const routes: Routes = [
  {
    title: createTitle('Home'),
    path: '',
    component: HomeComponent
  },
  {
    title: createTitle('Generator'),
    path: 'generator',
    component: GeneratorComponent
  },
  {
    title: createTitle('Login'),
    path: 'login',
    component: LoginComponent
  },
  {
    title: createTitle('Register'),
    path: 'register',
    component: RegisterComponent
  }
];

function createTitle(title: string): string {
  return `IconMasterAI - ${title}`
}

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
