export const environment = {
  production: false,
  apiUrl: process.env['NG_APP_API_URL'] ?? '',
  clientId: process.env['NG_APP_CLIENT_ID'] ?? '',
  tenantDomain: process.env['NG_APP_TENANT_DOMAIN'] ?? '',
  authority: process.env['NG_APP_AUTHORITY'] ?? '',
  redirectUri: process.env['NG_APP_REDIRECT_URI'] ?? '',
  logoutRedirectUri: process.env['NG_APP_LOGOUT_REDIRECT_URI'] ?? '',
  scope: process.env['NG_APP_SCOPE'] ?? 'openid profile'
};