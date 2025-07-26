import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(): boolean {
    const token = localStorage.getItem('access_token');
    if (!token) {
      this.router.navigate(['/']);
      return false;
    }

    const payload = JSON.parse(atob(token.split('.')[1]));
    const roles = payload?.roles || [];

    if (roles.includes('Admin')) {
      return true;
    }

    this.router.navigate(['/']);
    return false;
  }
}
