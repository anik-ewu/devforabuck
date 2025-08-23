import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const AdminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const token = localStorage.getItem('access_token');
  if (!token) {
    router.navigate(['/']);
    return false;
  }
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const roles = payload?.roles || [];
    if (roles.includes('Admin')) {
      return true;
    }
  } catch {
    // ignore invalid token
  }
  router.navigate(['/']);
  return false;
};
