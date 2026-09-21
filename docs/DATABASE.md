# Database Schema & Entity Documentation — Student Job Hub

**Database Engine:** Microsoft SQL Server / LocalDB  
**ORM:** Entity Framework Core 10 (`ApplicationDbContext`)  

---

## 📊 1. Entity Relationship Diagram (ERD)

```text
  ┌──────────────────┐               ┌──────────────────┐
  │  AspNetUsers     │ 1           * │  Jobs            │
  ├──────────────────┤───────────────┼──────────────────┤
  │ Id (PK)          │               │ Id (PK)          │
  │ FullName         │               │ Title            │
  │ Email            │               │ Description      │
  │ University       │               │ Requirements     │
  │ Bio              │               │ Budget (18,2)    │
  │ ProfilePictureUrl│               │ Deadline         │
  └────────┬─────────┘               │ PostedById (FK)  │
           │ 1                       │ IsOpen           │
           │                         └────────┬─────────┘
           │                                  │ 1
           │ *                                │ *
  ┌────────┴─────────┐               ┌────────┴─────────┐
  │  Services        │               │ JobApplications  │
  ├──────────────────┤               ├──────────────────┤
  │ Id (PK)          │               │ Id (PK)          │
  │ Title            │               │ JobId (FK)       │
  │ Category         │               │ ApplicantId (FK) │
  │ Price (18,2)     │               │ Message          │
  │ ProviderId (FK)  │               │ Status           │
  └──────────────────┘               │ AppliedAt        │
                                     └──────────────────┘

  ┌──────────────────┐               ┌──────────────────┐
  │  Notifications   │               │  Reviews         │
  ├──────────────────┤               ├──────────────────┤
  │ Id (PK)          │               │ Id (PK)          │
  │ UserId (FK)      │               │ ReviewerId (FK)  │
  │ Message          │               │ RevieweeId (FK)  │
  │ IsRead           │               │ Rating           │
  │ CreatedAt        │               │ Comment          │
  └──────────────────┘               └──────────────────┘
```

---

## 📋 2. Detailed Table Schemas

### `AspNetUsers` (ApplicationUser)
- Extends `IdentityUser`.
- **Properties:**
  - `FullName` (`nvarchar(max)`, Required)
  - `University` (`nvarchar(max)`, Nullable)
  - `Bio` (`nvarchar(max)`, Nullable)
  - `ProfilePictureUrl` (`nvarchar(max)`, Nullable)
  - `CreatedAt` (`datetime2`, UtcNow)

### `Jobs`
- Represents job openings created by employers/lecturers.
- **Properties:**
  - `Id` (`int`, Primary Key, Identity)
  - `Title` (`nvarchar(max)`)
  - `Description` (`nvarchar(max)`)
  - `Requirements` (`nvarchar(max)`)
  - `Budget` (`decimal(18,2)`)
  - `Deadline` (`datetime2`)
  - `PostedById` (`nvarchar(450)`, FK → `AspNetUsers.Id`, `OnDelete: Restrict`)
  - `IsOpen` (`bit`, Default: `1`)
  - `CreatedAt` (`datetime2`)

### `JobApplications`
- Links applicants to job postings.
- **Properties:**
  - `Id` (`int`, Primary Key, Identity)
  - `JobId` (`int`, FK → `Jobs.Id`, `OnDelete: Restrict`)
  - `ApplicantId` (`nvarchar(450)`, FK → `AspNetUsers.Id`, `OnDelete: Restrict`)
  - `Message` (`nvarchar(max)`)
  - `Status` (`nvarchar(max)`, Options: `Pending`, `Accepted`, `Rejected`)
  - `AppliedAt` (`datetime2`)

### `Services`
- Represents student freelance offerings.
- **Properties:**
  - `Id` (`int`, Primary Key, Identity)
  - `Title` (`nvarchar(max)`)
  - `Description` (`nvarchar(max)`)
  - `Category` (`nvarchar(max)`)
  - `Price` (`decimal(18,2)`)
  - `ProviderId` (`nvarchar(450)`, FK → `AspNetUsers.Id`)
  - `CreatedAt` (`datetime2`)

### `Notifications`
- User alert queue.
- **Properties:**
  - `Id` (`int`, Primary Key)
  - `UserId` (`nvarchar(450)`, FK → `AspNetUsers.Id`)
  - `Message` (`nvarchar(max)`)
  - `IsRead` (`bit`)
  - `CreatedAt` (`datetime2`)

### `Reviews`
- Peer ratings and comments.
- **Properties:**
  - `Id` (`int`, Primary Key)
  - `ReviewerId` (`nvarchar(450)`, FK → `AspNetUsers.Id`, `OnDelete: Restrict`)
  - `RevieweeId` (`nvarchar(450)`, FK → `AspNetUsers.Id`, `OnDelete: Restrict`)
  - `Rating` (`int`, 1 to 5)
  - `Comment` (`nvarchar(max)`)
  - `CreatedAt` (`datetime2`)

---

## 🔒 3. Integrity Rules & Foreign Key Behaviors

- `OnDelete(DeleteBehavior.Restrict)` is configured on `Job.PostedById`, `JobApplication.JobId`, `JobApplication.ApplicantId`, `Review.ReviewerId`, and `Review.RevieweeId` to prevent multi-cascade delete path conflicts in SQL Server.
- Precision `(18,2)` explicitly declared for monetary properties `Job.Budget` and `Service.Price`.
