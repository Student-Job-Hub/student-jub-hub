# 🎓 Student Job Hub

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor WASM](https://img.shields.io/badge/Frontend-Blazor%20WebAssembly-512BD4?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![ASP.NET Core API](https://img.shields.io/badge/Backend-ASP.NET%20Core%20Web%20API-512BD4?logo=dotnet)](https://asp.net/)
[![EF Core](https://img.shields.io/badge/ORM-Entity%20Framework%20Core-512BD4)](https://docs.microsoft.com/ef/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

> A modern, campus-focused marketplace connecting **Students**, **Lecturers**, and **Businesses**. Find student jobs, offer freelance campus services, manage applications, track real-time notifications, and leave peer reviews.

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Technology Stack](#-technology-stack)
- [System Architecture & Structure](#-system-architecture--structure)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Database Configuration](#database-configuration)
  - [Running the Application](#running-the-application)
- [API Reference](#-api-reference)
- [Testing](#-testing)
- [Contributing](#-contributing)

---

## 🚀 Overview

**Student Job Hub** provides a centralized digital campus ecosystem for work and freelance opportunities:
- **Students** can discover flexible part-time jobs, apply with personalized messages, track application progress, and offer skill-based services.
- **Businesses & Lecturers** can post job openings, manage incoming applications, accept or reject candidates, and review talent.
- **Security & Authorization** are powered by JWT tokens, ASP.NET Core Identity, and role-based permissions.

---

## ✨ Key Features

### 🔐 1. Authentication & Security
- **JWT Bearer Auth**: Secure token-based authentication with `localStorage` client persistence.
- **Role-Based Authorization**: Tailored capabilities for `Student`, `Business`, `Lecturer`, and `Admin`.
- **Identity Security**: Password hashing, claim-based authorization, and protected endpoints.

### 💼 2. Job Marketplace
- **Job Listings (`/jobs`)**: Filter open/closed opportunities, view budgets in GH₵, and track deadlines.
- **Detailed Job View (`/jobs/{id}`)**: Dynamic interface presenting owner management actions (`Edit`, `Close`, `Delete`, `Applications`) or student `Apply` controls.
- **Job Posting & Management**: Form validation, deadline controls, owner job management (`/my-jobs`), and status toggles.

### 📄 3. Application System
- **Job Application Flow**: Students submit applications with custom pitch messages. Self-application and duplicate application guards enforced.
- **Application Tracking (`/my-applications`)**: Real-time status updates (`Pending`, `Accepted`, `Rejected`) with pending withdrawal option.
- **Owner Review Panel (`/my-jobs/{id}/applications`)**: Job posters inspect applicants and update status with instant feedback.

### 🛠️ 4. Services Marketplace
- **Service Feed & Categories (`/services`)**: Browse campus services (e.g., tutoring, graphic design, tech support) with category tags and prices.
- **Provider Tools**: Service creation (`/create-service`), editing, and provider profile linking.

### 🔔 5. Notifications & Reviews
- **Real-Time SignalR Alerts**: Instant notifications for application events.
- **Notification Inbox (`/notifications`)**: Read/unread status toggles and deletion.
- **Peer Reviews (`/reviews/{userId}`)**: 1–5 star rating system with comment feedback (self-review and duplicate review guards).

### 👤 6. User Profile Management
- **Profile Center (`/profile`)**: Manage Full Name, University affiliation, Bio, and Avatar URLs with role indicators.

---

## 🛠️ Technology Stack

| Layer | Technologies Used |
|---|---|
| **Frontend** | Blazor WebAssembly (.NET 10), HttpClient, `localStorage` JWT integration, Vanilla CSS |
| **Backend API** | ASP.NET Core Web API (.NET 10), ASP.NET Core Identity, JWT Bearer Authentication, SignalR |
| **Database & ORM** | Microsoft SQL Server / LocalDB, Entity Framework Core 10 |
| **Testing** | xUnit, Entity Framework Core InMemory Database |

---

## 📂 System Architecture & Structure

```text
StudentJobHub/
├── src/
│   ├── StudentJobHub.Api/           # ASP.NET Core Web API (.NET 10)
│   │   ├── Controllers/             # Auth, Jobs, Applications, Services, Notifications, Reviews, Users
│   │   ├── Data/                    # ApplicationDbContext & Migrations
│   │   ├── DTOs/                    # Request & Response DTOs
│   │   ├── Hubs/                    # SignalR Real-Time Notification Hub
│   │   ├── Models/                  # ApplicationUser, Job, JobApplication, Service, Notification, Review
│   │   └── Services/                # Core Business Logic Services
│   │
│   └── StudentJobHub.Client/        # Blazor WebAssembly Frontend (.NET 10)
│       ├── Layout/                  # MainLayout & NavMenu navigation
│       ├── Models/                  # Client-side View Models
│       ├── Pages/                   # Razor Components (Jobs, Applications, Services, Profile, Auth)
│       └── Services/                # HttpClient API Services & JwtAuthorizationHandler
│
└── tests/
    └── StudentJobHub.Tests/         # xUnit Unit & Integration Test Suite
```

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- SQL Server LocalDB or full SQL Server instance

---

### Database Configuration

Update the connection string in `src/StudentJobHub.Api/appsettings.json` if necessary:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudentJobHubDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Migrations will automatically apply or create the database upon application launch.

---

### Running the Application

1. **Clone the Repository**:
   ```powershell
   git clone https://github.com/your-org/StudentJobHub.git
   cd StudentJobHub
   ```

2. **Start the API Server**:
   ```powershell
   dotnet run --project src/StudentJobHub.Api
   ```
   *The API will start listening at `http://localhost:5205`.*

3. **Start the Blazor WebAssembly Client**:
   ```powershell
   dotnet run --project src/StudentJobHub.Client
   ```
   *Open your browser and navigate to the local client URL displayed in your terminal.*

---

## 📡 API Reference

| Endpoint | Method | Auth | Description |
|---|---|---|---|
| `POST /api/auth/register` | `POST` | Public | Register a new user (`Student`, `Business`, `Lecturer`) |
| `POST /api/auth/login` | `POST` | Public | Authenticate user and receive JWT token |
| `GET /api/jobs` | `GET` | Public | List all posted jobs |
| `GET /api/jobs/my` | `GET` | Bearer | Retrieve jobs posted by current user |
| `POST /api/jobs` | `POST` | Bearer | Post a new job opportunity |
| `PATCH /api/jobs/{id}/close` | `PATCH` | Bearer | Close job posting |
| `POST /api/applications/{jobId}` | `POST` | Bearer | Submit application for a job |
| `GET /api/applications/my` | `GET` | Bearer | View current student's applications |
| `GET /api/applications/job/{jobId}` | `GET` | Bearer | View applications submitted to owned job |
| `PATCH /api/applications/{id}/status` | `PATCH` | Bearer | Accept or Reject applicant status |
| `GET /api/services` | `GET` | Public | Browse campus service marketplace |
| `GET /api/notifications` | `GET` | Bearer | Fetch user notifications |
| `GET /api/users/me` | `GET` | Bearer | Retrieve authenticated profile details |

---

## 🧪 Testing

Run the automated xUnit test suite covering authentication, job ownership enforcement, application workflow, and review rules:

```powershell
dotnet test
```

Expected output:
```text
Passed!  - Failed: 0, Passed: 9, Skipped: 0, Total: 9
```

---

## 👥 Contributing & Team

Built for the **DCIT318 Group Project** at University of Ghana under the leadership of Addo Michael Obiri and team contributors. Refer to [ACTION_PLAN.md](ACTION_PLAN.md) for team development guidelines.
