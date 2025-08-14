<!-- Hero Banner -->
<h1 align="center">🚀 Dev for a Buck</h1>
<p align="center">
A full-stack booking and consultation platform built with <b>ASP.NET Core (.NET 8)</b> and <b>Angular</b>, integrated with <b>Microsoft Entra External ID</b> for secure authentication.
</p>

<p align="center">
  <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-8.0-purple?style=for-the-badge&logo=dotnet" /></a>
  <a href="https://angular.io/"><img src="https://img.shields.io/badge/Angular-17-DD0031?style=for-the-badge&logo=angular" /></a>
  <a href="https://azure.microsoft.com/en-us/products/active-directory/external-identities/"><img src="https://img.shields.io/badge/Microsoft%20Entra%20ID-Auth-blue?style=for-the-badge&logo=microsoftazure" /></a>
  <a href="https://learn.microsoft.com/en-us/azure/cosmos-db/"><img src="https://img.shields.io/badge/Cosmos%20DB-NoSQL-green?style=for-the-badge&logo=azurecosmosdb" /></a>
</p>

---

## 📌 Overview
**Dev for a Buck** is a booking platform where:
- **Users** can browse and book consultation slots (CV reviews, career guidance).
- **Admins** can create, manage, and monitor bookings.
- Authentication is powered by **Microsoft Entra External ID** with **Google & Microsoft SSO**.

---

## ✨ Features

| Feature | Users | Admins |
|---------|-------|--------|
| 🔑 Secure login with Microsoft Entra | ✅ | ✅ |
| 📅 Book consultation slots | ✅ | ❌ |
| 📋 Manage own bookings (update/cancel) | ✅ | ❌ |
| 🗂 View all bookings | ❌ | ✅ |
| 🛠 Create/manage available slots | ❌ | ✅ |
| 📊 Approve/reject/cancel bookings | ❌ | ✅ |

---

## 🛠 Tech Stack
**Frontend**
- Angular 17 (Standalone Components, Reactive Forms)
- TypeScript, HTML5, CSS3, Bootstrap

**Backend**
- ASP.NET Core (.NET 8) REST API
- CQRS + MediatR
- Azure Cosmos DB
- Azure Blob Storage

**Authentication**
- Microsoft Entra External ID (B2C)
- OAuth 2.0 PKCE Flow
- Google & Microsoft SSO

---

📂 Project Structure
/frontend         # Angular application (UI)
/backend          # ASP.NET Core API
  /API            # Controllers
  /Application    # Commands, Queries, Services
  /Domain         # Entities, Aggregates
  /Infrastructure # Cosmos DB, Blob Storage, Auth setup

## 🔐 Authentication Flow
```mermaid
sequenceDiagram
  participant User
  participant Frontend
  participant EntraID
  participant Backend

  User->>Frontend: Clicks Login
  Frontend->>Frontend: Generate PKCE verifier & challenge
  Frontend->>EntraID: Redirect with PKCE challenge
  EntraID->>User: Authenticate (Google/Microsoft/Email)
  EntraID->>Frontend: Return auth code
  Frontend->>Backend: Exchange code for tokens
  Backend->>Frontend: Access token + ID token
  Frontend->>Backend: Make secure API calls


