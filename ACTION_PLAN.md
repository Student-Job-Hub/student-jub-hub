# Student Job Hub — Team Action Plan & Working Manual

**DCIT318 Group Project | Team Lead: Addo Michael Obiri | Version 1.0 | August 2026**

This is the manual every team member follows for the lifetime of this project. If you're unsure how to do something on this project — branching, committing, picking up a task, asking for help — the answer should be in this document. Read it fully before writing your first line of code.

---

## 1. Purpose of This Document

We are 12 developers team lead building one shared application. Without agreed rules, that many people on one codebase produces chaos: broken builds, merge conflicts, duplicated work, and inconsistent code. This manual is the single source of truth for **how we work together**, not what we're building (see `/docs/PRD.docx`) or how the system is designed (see `/docs/Technical_Architecture.docx`).

---

## 2. Team Structure

- **Team Lead:** Addo Michael Obiri — owns the GitHub Organization, final say on scope/architecture disputes, manages the task board, coordinates milestones.
- **11 Members (A–L):** each owns one primary feature end-to-end (API + data + Blazor UI for that feature) and reviews at least one other member's Pull Requests per week.
- **Sub-teams **
  - Foundation & Data — Member K
  - Identity & Users — Members A, B
  - Marketplace — Members C, D
  - Jobs — Members E, F
  - Real-Time — Members G, H
  - Reviews & Admin — Members I, J
  - DevOps & QA — Member L

Full assignment table with feature names is in `TEAM_ASSIGNMENTS.md`.

---

## 3. Communication

- **Primary channel:** [team to decide — WhatsApp group / Discord / Slack] — create this within 24 hours of role assignment.
- **Task board:** GitHub Projects (Kanban) on the org repo. Every piece of work must be a card/issue before it is worked on.
- **Standups:** Two short async check-ins per week (e.g. Sunday & Wednesday) — each member posts: what I did, what I'm doing next, any blocker.
- **Meetings:** One weekly video/in-person sync — Team Lead leads it, reviews board status, unblocks people, re-prioritizes if needed.

---

## 4. Timeline (mirrors the PRD milestones)

| Phase | Focus | Who leads |
|---|---|---|
| 1. Setup & Design | Org/repo created, DB schema finalized, wireframes agreed | K + Team Lead |
| 2. Core Backend | Auth, API endpoints, EF Core models functional | A, B, K |
| 3. Core Frontend | Blazor pages consuming API for auth, listings, jobs | C, D, E, F |
| 4. Real-Time & Reviews | SignalR messaging/notifications, ratings | G, H, I |
| 5. Integration & Testing | End-to-end testing, bug fixing, polish | L + everyone |
| 6. Final Demo & Submission | Deployed/working demo, docs, presentation | Everyone |

Exact dates to be set by the Team Lead once the semester calendar and submission deadline are confirmed.

---

## 5. Git & GitHub Workflow

### 5.1 Branches
- `main` — always working, demo-ready. **Protected.** No direct pushes.
- `dev` — integration branch. All feature branches merge here first. **Protected.**
- `feature/<branch_name-<firstname>` — one branch per task, branched from the latest `dev`.
  - Example: `feature/service-listing-michael`

### 5.2 Commits
Use [Conventional Commits](https://www.conventionalcommits.org/):
```
-Examples:
feat: add service listing creation endpoint
fix: correct null reference in job status update
docs: update API spec for reviews endpoint
test: add unit tests for JobApplication service
chore: update NuGet packages
```

### 5.3 Pull
ALWAYS pull the latest changes from the `dev` branch before working-**Very Important**

### 5.4 Pull Requests
1. Push your feature branch, open a PR **into `dev`** (NEVER directly into `main`).
2. Add PR description 
3. At least **one teammate review** is required before merge — you may not approve/merge your own PR.
4. Resolve all review comments before merging.
5. Squash-merge to keep `dev` history clean.
6. Delete the feature branch after merge.

### 5.4 Who merges `dev` → `main`
Only the Team Lead (or DevOps, by delegation) merges `dev` into `main`, and only when the build is green and the feature is demo-ready.

---

## 6. Coding Standards (summary — full detail in `CONTRIBUTING.md` once created)

- C# naming: PascalCase for classes/methods/public properties, camelCase for locals/private fields (`_camelCase` for private fields).
- One class per file; file name matches class name.
- Controllers are thin — business logic lives in a Service class, not the controller.
- All API inputs validated with Data Annotations or FluentValidation; never trust client input.
- No hard-coded connection strings or secrets in code — use `appsettings.Development.json` (gitignored) 
- Every new EF Core model change must come with a Migration, committed in the same PR.

---


## 7. Testing Expectations

- Every member writes at least basic unit tests for their service/business logic (in `/tests/StudentJobHub.Tests`).
- Before opening a PR, manually run the full app locally and click through your feature.


---

## 10. Escalation & Conflict Resolution

- **Technical blocker (>1 day stuck):** post in the team channel immediately — don't sit on it silently.
- **Merge conflict you can't resolve:** tag the other author + Member K (data layer owner) or the Team Lead.
- **Missed deadlines / non-participation:** Team Lead follows up privately first; repeated issues are documented for the group's academic integrity/contribution record.
- **Scope disagreements:** Team Lead has final call, informed by the PRD's explicit in-scope/out-of-scope list.

---

## 11. Academic Integrity

- Every member commits their own work under their own GitHub account — no shared logins, no ghost-writing commits for someone else.
- Commit history and GitHub Insights/contribution graphs will be part of how individual contribution is assessed — commit early, commit often, with meaningful messages.


---

## 12. Submission Checklist (fill in closer to deadline)

- [ ] `main` branch builds and runs cleanly from a fresh clone.
- [ ] PRD, Technical Architecture, User Stories, and this Action Plan are all in `/docs`.
- [ ] README has clear setup/run instructions.
- [ ] Demo data seeded for presentation.
- [ ] Presentation slides prepared and rehearsed.
- [ ] Each member can speak to the feature they built.
