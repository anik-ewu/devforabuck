import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { About } from './pages/about/about';
import { Contact } from './pages/contact/contact';
import { Bookings } from './pages/bookings/bookings';
import { Admin } from './pages/admin/admin';
import { AdminGuard } from './shared/guard/admin-guard';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },
  { path: 'bookings', component: Bookings },
  { path: 'admin', component: Admin, canActivate: [AdminGuard] },
];
