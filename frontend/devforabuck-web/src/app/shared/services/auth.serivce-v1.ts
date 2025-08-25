import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthServiceV1 {
  private _loggedIn = false;

  get isLoggedIn(): boolean {
    return this._loggedIn;
  }

  setLoggedIn(value: boolean) {
    this._loggedIn = value;
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

  async exchangeCodeForTokens(code: string): Promise<boolean> {
    const codeVerifier = sessionStorage.getItem('pkce_code_verifier');
    const tokenEndpoint = `${environment.authority}/${environment.tenantDomain}/oauth2/v2.0/token`;

    const body = new URLSearchParams();
    body.set('grant_type', 'authorization_code');
    body.set('code', code);
    body.set('redirect_uri', environment.redirectUri);
    body.set('client_id', environment.clientId);
    body.set('code_verifier', codeVerifier || '');

    try {
      const response = await fetch(tokenEndpoint, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: body.toString(),
      });

      const data = await response.json();

      if (data.access_token) {
        localStorage.setItem('access_token', data.access_token);
        localStorage.setItem('id_token', data.id_token);
        this.setLoggedIn(true);
        window.history.replaceState({}, '', '/'); // clean up URL after login
        return true;
      } else {
        console.error('Token response missing access_token:', data);
        return false;
      }
    } catch (error) {
      console.error('Token exchange failed:', error);
      return false;
    }
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
