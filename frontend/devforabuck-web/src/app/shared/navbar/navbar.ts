import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar implements OnInit {
  loggedIn = false;

  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    const params = new URLSearchParams(window.location.search);
    const code = params.get('code');
    if (code) {
      this.loggedIn = true;
      console.log('Authorization Code:', code);
      // Call backend to exchange the code if needed
    } else {
      this.loggedIn = this.auth.isLoggedIn;
    }
  }

  login(): void {
    this.auth.login();
  }

  logout(): void {
    this.auth.logout();
  }

  isLoggedIn(): boolean {
    return this.loggedIn;
  }
}
