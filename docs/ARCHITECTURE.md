# System Architecture — Student Job Hub

**Stack:** ASP.NET Core Web API (.NET 10) | Blazor WebAssembly (.NET 10) | Entity Framework Core 10 | SQL Server | SignalR

---

## 🏗️ 1. High-Level Architecture Overview

Student Job Hub follows a decoupled client-server architecture:

```text
┌─────────────────────────────────────────────────────────────┐
│                   Blazor WebAssembly Client                 │
│  (Pages: Jobs, MyJobs, Applications, Services, Profile)      │
│  (Services: AuthService, JobApiService, ApplicationService) │
│  (Auth: JwtAuthorizationHandler -> LocalStorage Bearer Token)│
└──────────────────────────────┬──────────────────────────────┘
                               │ HTTP / REST (JSON) + WebSockets
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                    ASP.NET Core Web API                     │
│  (Controllers: Auth, Jobs, Applications, Services, Users)   │
│  (Middleware: CorsPolicy, JwtBearerAuth, ExceptionHandling) │
│  (Services: JobService, JobApplicationService, AuthService) │
│  (Real-Time: NotificationHub -> SignalR WebSocket)          │
└──────────────────────────────┬──────────────────────────────┘
                               │ EF Core 10
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                    SQL Server Database                      │
│  (Tables: AspNetUsers, Jobs, JobApplications, Services...)  │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔑 2. Authentication & Authorization Architecture

1. **Authentication:**
   - User submits credentials to `POST /api/auth/login`.
   - `AuthService` verifies password via `UserManager<ApplicationUser>`.
   - On success, `AuthService` signs a JWT containing `Sub`, `Email`, `NameIdentifier`, `Name`, and `Role` claims using `HMAC-SHA256`.
   - Client stores token in browser `localStorage`.

2. **Authorization Pipeline:**
   - Client requests are intercepted by `JwtAuthorizationHandler`, attaching `Authorization: Bearer <token>`.
   - Server validates issuer, audience, lifetime, and signature.
   - Resource-level ownership checks are enforced inside application services (`JobService`, `JobApplicationService`, `ServiceService`, `ReviewService`).

---

## ⚡ 3. Real-Time Notification Pipeline

- SignalR hub configured at `/hubs/notifications`.
- Upon event creation (e.g., job application submitted or status updated), `JobApplicationService` persists a `Notification` entity in EF Core and dispatches real-time events to `user-{userId}` SignalR group.

---

## 📂 4. Project Structure & Dependency Graph

- `StudentJobHub.Api`: Thin controllers delegating all business logic to scoped application services.
- `StudentJobHub.Client`: Componentized Razor pages consuming typed HttpClient services.
- `StudentJobHub.Tests`: Unit & integration tests utilizing EF Core InMemory provider and mock SignalR hub context.
