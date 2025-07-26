import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { About } from './pages/about/about';
import { Contact } from './pages/contact/contact';
import { Bookings } from './pages/bookings/bookings';
import { Admin } from './pages/admin/admin';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },
  { path: 'bookings', component: Bookings },
  // TODO: add guard later
  { path: 'admin', component: Admin },
];
