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
    const code = new URLSearchParams(window.location.search).get('code');
    const token = localStorage.getItem('access_token');

    if (code || token) {
      this.auth.setLoggedIn(true);
      this.loggedIn = true;
    } else {
      this.loggedIn = false;
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
