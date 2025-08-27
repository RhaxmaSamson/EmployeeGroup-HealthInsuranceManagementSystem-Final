# Employee Group Health Insurance System — Architecture One-Pager

## Overview
- Purpose: Manage organizations, employees, policies, enrollments, and claims; provide premium calculation and reporting
- Style: ASP.NET Core 8 MVC with layered architecture (Controllers → Services → Data)
- AuthN/AuthZ: ASP.NET Core Identity with cookie auth and role-based authorization (Admin, HRManager, Employee)
- Persistence: EF Core (SQL Server) using code-first with migrations
- Reporting: QuestPDF (PDF), ClosedXML (Excel)
- UI: Razor Views + Bootstrap + jQuery

## High-Level Architecture
```mermaid
flowchart TD
    A[User Browser] --> B[Kestrel ASP.NET Core]
    B --> C[Middleware: HTTPS, Static Files, Routing, Auth]
    C --> D[Controllers]
    D --> E[Services (Business Logic)]
    E --> F[(EF Core / ApplicationDbContext)]
    F --> G[(SQL Server)]
    D --> H[Razor Views + Bootstrap]
    D --> I[Reporting: QuestPDF / ClosedXML]
```

## Layers and Responsibilities
- Controllers: Request handling, model validation, authorization, orchestration, returning Views/Files
- Services: Business logic and data access via EF Core; injected via DI
- Data: `ApplicationDbContext` with `DbSet<TEntity>`; migrations, entity configuration
- DTOs/ViewModels: Input/output shaping for forms and views
- Views: Razor pages rendered server-side; anti-forgery on POST

## Security Model
- Authentication: Cookie-based via ASP.NET Core Identity
- Authorization: Role-based (`[Authorize(Roles = "...")]`)
- Seed: Roles and default users seeded on startup via `DbInitializer`
- Anti-forgery: `[ValidateAntiForgeryToken]` on state-changing actions

## Domain Model
- Organization 1─* Employee
- Employee 1─* Enrollment
- Policy 1─* Enrollment
- Enrollment 1─* Claim, 1─* EnrollmentDependent
- Enums: `PolicyType` (int), `EnrollmentStatus` (string), `ClaimStatus` (string)

## Key Workflows
- On Startup: Apply migrations; seed roles and users; enable Swagger in Development
- Employee Lifecycle (HR): Register → Identity account creation → Assign role → CRUD profile
- Enrollment (Employee): Select policy → Create `Enrollment` (ACTIVE) → Cancel (CANCELLED)
- Claims: Employee submits (SUBMITTED) → HR updates status (APPROVED/REJECTED)
- Policies: Browse and view; Admin may delete
- Reporting: Download PDF/Excel for Employees, Policies, Enrollments, Claims, Organizations, Premium Matrix
- Premium Calculation: Compute based on policy type, age bands, dependents cap

## Configuration
- Connection string in `appsettings.json` → SQL Server
- Cookie paths: `/Auth/Index` (login), `/Auth/AccessDenied`
- Swagger enabled in Development only

## Non-Functional Notes
- DI with scoped services; `AddControllersWithViews`
- Eager loading via `Include/ThenInclude` to avoid N+1 queries
- Enum conversion for DB readability/consistency
- File streaming for reports with correct MIME types