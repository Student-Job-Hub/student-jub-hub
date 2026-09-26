# Product Requirements Document (PRD) — Student Job Hub

**Version:** 1.0  
**Project:** Student Job Hub  
**Target Platform:** ASP.NET Core Web API + Blazor WebAssembly (.NET 10)  

---

## 🎯 1. Executive Summary

Student Job Hub is a campus-centric marketplace designed to bridge the gap between students seeking part-time employment or freelance work and employers (businesses, campus departments, and lecturers) offering job opportunities and tasks. Additionally, it enables students to offer skill-based services (tutoring, design, software development, event support) to peers across campus.

---

## 👥 2. Target Roles & User Stories

### A. Student Role
- **Goal:** Find flexible work, apply to open postings, track application status, offer freelance services, and receive real-time updates.
- **User Stories:**
  - *As a Student*, I want to browse open job postings on campus with filter options so that I can quickly find opportunities fitting my schedule.
  - *As a Student*, I want to apply to a job with a custom pitch message so that employers understand my qualifications.
  - *As a Student*, I want to track my application status (`Pending`, `Accepted`, `Rejected`) in `/my-applications` and withdraw pending applications if needed.
  - *As a Student*, I want to post my freelance services in `/services` so campus members can hire me.

### B. Business / Lecturer Role
- **Goal:** Post job vacancies, review incoming candidate applications, select candidates, and provide reviews.
- **User Stories:**
  - *As an Employer*, I want to post new job openings specifying budget, deadline, and requirements so students can apply.
  - *As an Employer*, I want to view all applications submitted to my jobs in `/my-jobs/{id}/applications` and accept or reject candidates.
  - *As an Employer*, I want to close job postings once filled so no new applications are accepted.

### C. Admin Role
- **Goal:** Oversee campus operations, enforce safety, and maintain system integrity.

---

## 🛠️ 3. Functional Requirements

### 1. Authentication & Security
- User registration (`Student`, `Business`, `Lecturer`) with validation.
- Secure login generating JWT Bearer tokens stored in browser `localStorage`.
- Role-based and ownership-based authorization on all API endpoints.

### 2. Job Marketplace
- Full CRUD for jobs with real-time status filtering (`Open`, `Closed`).
- Contextual job details interface supporting owner management (`Edit`, `Close`, `Delete`, `Applications`) and applicant entry.

### 3. Application Workflow
- One application per student per job guard.
- Poster self-application prevention.
- Status transition validation (`Pending` → `Accepted` / `Rejected`).
- Real-time SignalR notifications for posters and applicants.

### 4. Services Marketplace
- Browse skill offerings tagged by category and price in GH₵.
- Owner controls for service editing and deletion.

### 5. Notifications & Reviews
- Notification feed with unread indicators and deletion.
- Peer rating system (1 to 5 stars) with self-review and duplicate review guards.

### 6. User Profile
- View and update profile fields (`FullName`, `Phone`, `University`, `Bio`, `ProfilePictureUrl`).

---

## 🔒 4. Non-Functional Requirements

- **Performance:** Sub-second API response times; efficient EF Core queries with projection.
- **Security:** Zero hardcoded sensitive production credentials; all endpoints enforced via `[Authorize]`.
- **Usability:** Responsive CSS layout tailored for mobile and desktop screens.
