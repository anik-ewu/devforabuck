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

  async ngOnInit(): Promise<void> {
    const code = new URLSearchParams(window.location.search).get('code');
    const token = localStorage.getItem('access_token');

    if (token) {
      this.auth.setLoggedIn(true);
      this.loggedIn = true;
    } else if (code) {
      const success = await this.auth.exchangeCodeForTokens(code);
      this.loggedIn = success;
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
