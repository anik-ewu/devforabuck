import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  get isLoggedIn(): boolean {
    return !!localStorage.getItem('access_token');
  }

  async login(): Promise<void> {
    const nonce = crypto.randomUUID();
    const state = crypto.randomUUID();

    const { codeVerifier, codeChallenge } = await this.generatePKCECodes();
    sessionStorage.setItem('pkce_code_verifier', codeVerifier);

    const url =
      `${environment.authority}/${environment.tenantDomain}/oauth2/v2.0/authorize` +
      `?client_id=${environment.clientId}` +
      `&response_type=code` +
      `&redirect_uri=${encodeURIComponent(environment.redirectUri)}` +
      `&scope=${encodeURIComponent(environment.scope)}` +
      `&code_challenge=${codeChallenge}` +
      `&code_challenge_method=S256` +
      `&nonce=${nonce}` +
      `&state=${state}`;

    window.location.href = url;
  }

  logout(): void {
    sessionStorage.clear();
    localStorage.clear();

    const logoutUrl =
      `${environment.authority}/${environment.tenantDomain}/oauth2/v2.0/logout` +
      `?post_logout_redirect_uri=${encodeURIComponent(environment.logoutRedirectUri)}`;

    window.location.href = logoutUrl;
  }

  private async generatePKCECodes(): Promise<{ codeVerifier: string; codeChallenge: string }> {
    const codeVerifier = this.base64URLEncode(crypto.getRandomValues(new Uint8Array(32)));
    const encoder = new TextEncoder();
    const data = encoder.encode(codeVerifier);
    const digest = await crypto.subtle.digest('SHA-256', data);
    const hash = new Uint8Array(digest);
    const codeChallenge = this.base64URLEncode(hash);
    return { codeVerifier, codeChallenge };
  }

  private base64URLEncode(input: Uint8Array): string {
    return btoa(String.fromCharCode(...input))
      .replace(/\+/g, '-')
      .replace(/\//g, '_')
      .replace(/=+$/, '');
  }
}
