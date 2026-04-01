# EduTrail

**EduTrail** is a modern, scalable e-commerce application built using **Angular** (frontend), **.NET Core** (backend), and **GraphQL**. The project follows **Clean Architecture principles** to ensure maintainability, testability, and scalability.

---
dotnet ef migrations add initial-migration -p src/EduTrail.Backend/EduTrail.Infrastructure -s src/EduTrail.Backend/EduTrail.API

## **Folder Structure (Clean Architecture)**


EduTrail/
├── src/
│ ├── EduTrail.Backend/
│ │ ├── EduTrail.API/ # REST API project  # Presentation layer
│ │ ├── EduTrail.Application/ # Application services, DTOs, use cases # Application layer
│ │ ├── EduTrail.Domain/ # Entities, Value Objects, Enums  # Core layer
│ │ └── EduTrail.Infrastructure/ # EF Core, Repositories, external services # Infrastructure layer
│ │
│ └── Web-Spa/ # Angular frontend project
├── tests/ # Unit and integration tests
├── docker-compose.yml # Docker Compose for backend + frontend
└── Edutrail.sln # .NET Solution file

---

## **Backend Project Setup (Commands)**

### **Create Projects**

```bashcd
dotnet new sln -n EduTrail

# API
dotnet new webapi -n EduTrail.API -o src/EduTrail.Backend/EduTrail.API
REST added to Api

# Application
dotnet new classlib -n EduTrail.Application -o src/EduTrail.Backend/EduTrail.Application
dotnet Add mediator and mapper
command and query to Application

# Domain
dotnet new classlib -n EduTrail.Domain -o src/EduTrail.Backend/EduTrail.Domain
IAuditEntry + All the entities

# Infrastructure
dotnet new classlib -n EduTrail.Infrastructure -o src/EduTrail.Backend/EduTrail.Infrastructure
dotnet add reference ../EduTrail.Application/EduTrail.Application.csproj
Repository + AuditEntry 

# API → Application &
cd src/EduTrail.Backend/EduTrail.API
dotnet add reference ../EduTrail.Application/EduTrail.Application.csproj
dotnet add reference ../EduTrail.Infrastructure/EduTrail.Infrastructure.csproj



# Application → Domain
cd ../Edutrail.Application
dotnet add reference ../Edutrail.Domain/Edutrail.Domain.csproj



# Infrastructure → Domain & Applicationv
cd ../Edutrail.Infrastructure
dotnet add reference ../Edutrail.Domain/Edutrail.Domain.csproj

Configure Application to Run on Debug mode
.vscode/launch.json
.vscode/tasks.json

DataMigration Configuration SetUP

Middleware
Exception Handle //done
Middleware order
app.UseGlobalExceptionHandler();   // 1️⃣ Catch everything
app.UseHttpsRedirection();         // 2️⃣ Security
app.UseAuthentication();           // 3️⃣ Auth
app.UseAuthorization();            // 4️⃣ Permissions
app.MapControllers();              // 5️⃣ Endpoints

Add Audit trail for entity.
---

## **Frontend Project Setup (Commands)**

### **Create Projects**

cd /src
ng new Edutrail.WebSpa --routing --style=scss

# Hot reloading

Package.Json

"start": "ng serve --host 0.0.0.0 --poll 2000",



# **Database and Entity Framework Edutrail.API**
dotnet add package Microsoft.EntityFrameworkCore           # Core ORM functionality
dotnet add package Microsoft.EntityFrameworkCore.SqlServer # SQL Server provider
dotnet add package Microsoft.EntityFrameworkCore.Tools     # Enables dotnet ef commands
dotnet add package Microsoft.EntityFrameworkCore.Design    # Provides design-time services for migrations generated class in migration folder
dotnet add package BCrypt.Net-Next                 


# ** Edutrail.Infrastructure**
dotnet add package Microsoft.EntityFrameworkCore   # Core ORM functionality
dotnet add package Microsoft.EntityFrameworkCore.SqlServer # SQL Server provider 
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Quartz
dotnet add package Quartz.Extensions.Hosting
dotnet add package Quartz.Serialization.Json

Add AppDbContext #Entity Framework Core (EF Core) DbContext class (AppDbContext inherits from DbContext, which is the main class in EF Core responsible for interacting with the database.)
Add DependencyInjection to the Edutrail.Infrastructure

# **Run migration command**
dotnet ef migrations add InitialCreate -s ../Edutrail.API/Edutrail.API.csproj -o Data/Migrations
Update database Add Code to Program.cs auto update the database 



## **Program.cs SetUp**
## **Service layer**
AddCors()

## **App layer**
UseCors()
MapGraphQL("/graphql");

# Add configuration Audit trail 
# Add mediatR Configuration Register in Applicationlayer


# Add Certificate 
dotnet dev-certs https --trust --export-path .devcontainer/local_cert.pfx -p Edutrail123

# Configuration for GraphQl
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();
app.MapGraphQL("/graphql");

# Angular app# https 
ng serve --ssl true --ssl-cert "D:\Edutrail\.devcontainer\server.crt" --ssl-key "D:\Edutrail\.devcontainer\server.key"

# Frontend Layout 

src/app/
├── app.config.ts                # replaces app.module.ts in standalone projects
├── app.routes.ts                # replaces app-routing.module.ts
├── shared/
│   ├── material.config.ts       # standalone import of Angular Material
│   └── models/
│       ├── user.model.ts
│       ├── category.model.ts
│       ├── brand.model.ts
│       ├── product.model.ts
│       └── order.model.ts
├── auth/
│   ├── auth.routes.ts
│   ├── layouts/
│   │   └── auth-layout/
│   │       ├── auth-layout.component.ts
│   │       └── auth-layout.component.html
│   ├── components/
│   │   ├── login/
│   │   │   ├── login.component.ts
│   │   │   └── login.component.html
│   │   ├── register/
│   │   │   ├── register.component.ts
│   │   │   └── register.component.html
│   │   └── forgot-password/
│   │       ├── forgot-password.component.ts
│   │       └── forgot-password.component.html
│   └── services/
│       └── auth.service.ts
├── admin/
│   ├── admin.routes.ts
│   ├── components/
│   │   ├── dashboard/
│   │   ├── users/
│   │   ├── categories/
│   │   ├── brands/
│   │   ├── products/
│   │   └── orders/
│   └── services/
│       ├── user.service.ts
│       ├── category.service.ts
│       ├── brand.service.ts
│       ├── product.service.ts
│       └── order.service.ts
└── ecommerce/
    ├── ecommerce.routes.ts
    ├── layouts/
    │   └── ecommerce-layout/
    │       ├── ecommerce-layout.component.ts
    │       └── ecommerce-layout.component.html
    ├── components/
    │   ├── home/
    │   │   ├── home.component.ts
    │   │   └── home.component.html
    │   ├── products/
    │   │   ├── product-list/
    │   │   │   ├── product-list.component.ts
    │   │   │   └── product-list.component.html
    │   │   └── product-detail/
    │   │       ├── product-detail.component.ts
    │   │       └── product-detail.component.html
    │   ├── cart/
    │   │   ├── cart.component.ts
    │   │   └── cart.component.html
    │   ├── checkout/
    │   │   ├── checkout.component.ts
    │   │   └── checkout.component.html
    │   └── orders/
    │       ├── my-orders.component.ts
    │       └── my-orders.component.html
    └── services/
        ├── ecommerce.service.ts
        ├── cart.service.ts
        └── order.service.ts


#Identity Implementation
Api layer
appsettings.json
Program.cs
---------------------------
Infrastructure Layer
Identity/JwtSettings.cs
Identity/JwtTokenService.cs
# ng generate module admin --routing --flat  give me both router an module too


# Problem: Could not resolve angular routing  Onload 404 Issue
# Solve https://medium.com/@dulanjayasandaruwan1998/solving-angulars-404-error-on-page-refresh-two-effective-approaches-619cbd544379

# Appllo set up

ng add apollo-angular
# Problem 
We couldn't find 'esnext.asynciterable' in the list of library files to be included in the compilation.
It's required by '@apollo/client/core' package so please add it to your tsconfig.


We couldn't enable 'allowSyntheticDefaultImports' flag.
It's required by '@apollo/client/core' package so please add it to your tsconfig.
# Solve: https://stackoverflow.com/questions/79270208/why-does-a-new-angular-19-app-error-when-executing-ng-add-apollo-angular





Frontend (Angular)
├── Student UI
├── Instructor UI
└── Auth Guards
↓ REST API
Backend (ASP.NET Core Web API)
├── Auth (JWT)
├── Question Engine
├── Auto Grading
├── Courses / Assignments
└── SQL Server / PostgreSQL


UniversityLabQueue.sln
│
├── UniversityLabQueue.API
│ ├── Controllers
│ │ ├── AuthController.cs
│ │ ├── StudentController.cs
│ │ ├── TeacherController.cs
│ │ ├── LabRequestController.cs
│ │ ├── LabSessionController.cs
│ │ ├── SubmissionController.cs
│ │ └── QueueController.cs
│ │
│ ├── Program.cs
│ └── appsettings.json
│
├── UniversityLabQueue.Application
│ ├── DTOs
│ ├── Interfaces
│ ├── Services
│ └── Validators
│
├── UniversityLabQueue.Domain
│ ├── Entities
│ ├── Enums
│ └── ValueObjects
│
├── UniversityLabQueue.Infrastructure
│ ├── Data
│ │ ├── UniversityLabDbContext.cs
│ │ └── Migrations
│ ├── Repositories
│ └── Configurations
│
└── UniversityLabQueue.Shared
├── Constants
└── Utilities

INSERT INTO [EduTrailDb].[dbo].[Roles]
(
    [Id],
    [Name],
    [Description],
    [CreatedDate],
    [CreatedById]
)
VALUES
('8F3B2A91-6E5C-4C7B-9E91-1A2D4F8C3B10', 'Instructor', 'Role for course instructors', GETDATE(), '00000000-0000-0000-0000-000000000000'),
('2C9D7F41-8A3E-4F2B-B6A5-9E1C3D4A7F82', 'Student',    'Role for students',         GETDATE(), '00000000-0000-0000-0000-000000000000'),
('5A1E4C7D-9B82-4F36-A3C1-6D9E2F8B0A55', 'TA',         'Role for teaching assistants', GETDATE(), '00000000-0000-0000-0000-000000000000');


INSERT INTO [EduTrailDb].[dbo].[TermTypes]
(
    [Id],
    [Name],
    [CreatedDate],
    [CreatedById],
    [UpdatedDate],
    [UpdatedById]
)
VALUES
('7E8F9F7E-75B3-4866-94A3-464F8711C544', 'Spring', GETDATE(), NULL, GETDATE(), NULL),
('F262DE21-7519-4468-B63A-653DAFC6B8F9', 'Fall',   GETDATE(), NULL, GETDATE(), NULL),
('855021E3-8D31-47B2-B787-65E1DDBB4FE0', 'Winter', GETDATE(), NULL, GETDATE(), NULL),
('f2231caa-ad7f-42f6-8283-043d54af790c', 'Summer', GETDATE(), NULL, GETDATE(), NULL);

INSERT INTO [EduTrailDb].[dbo].[StatusTypes]
(
    Id,
    Name,
    Description,
    CreatedDate,
    CreatedById,
    UpdatedDate,
    UpdatedById
)
VALUES
(
    'ce5e6303-3ac6-4af1-92b4-f708da026d20',
    'Help Request',
    'Status type for help request workflow',
    GETDATE(),
    NULL,
    NULL,
    NULL
);
INSERT INTO [EduTrailDb].[dbo].[Statuses]
(
    Id,
    Name,
    Description,
    StatusTypeId,
    CreatedDate,
    CreatedById,
    UpdatedDate,
    UpdatedById
)
VALUES
(
    '627407c8-700d-46fc-a3e5-02dc368fb75e',
    'Pending',
    'Help request is pending',
    'ce5e6303-3ac6-4af1-92b4-f708da026d20',
    GETDATE(),
    NULL,
    NULL,
    NULL
);

//Postgres
INSERT INTO "Roles"
(
    "Id",
    "Name",
    "Description",
    "CreatedDate",
    "CreatedById"
)
VALUES
('8f3b2a91-6e5c-4c7b-9e91-1a2d4f8c3b10', 'Instructor', 'Role for course instructors', NOW(), '00000000-0000-0000-0000-000000000000'),
('2c9d7f41-8a3e-4f2b-b6a5-9e1c3d4a7f82', 'Student',    'Role for students',           NOW(), '00000000-0000-0000-0000-000000000000'),
('5a1e4c7d-9b82-4f36-a3c1-6d9e2f8b0a55', 'TA',         'Role for teaching assistants', NOW(), '00000000-0000-0000-0000-000000000000');


INSERT INTO "TermTypes"
(
    "Id",
    "Name",
    "CreatedDate",
    "CreatedById",
    "UpdatedDate",
    "UpdatedById"
)
VALUES
('7e8f9f7e-75b3-4866-94a3-464f8711c544', 'Spring', NOW(), NULL, NOW(), NULL),
('f262de21-7519-4468-b63a-653dafc6b8f9', 'Fall',   NOW(), NULL, NOW(), NULL),
('855021e3-8d31-47b2-b787-65e1ddbb4fe0', 'Winter', NOW(), NULL, NOW(), NULL),
('f2231caa-ad7f-42f6-8283-043d54af790c', 'Summer', NOW(), NULL, NOW(), NULL);


INSERT INTO "StatusTypes"
(
    "Id",
    "Name",
    "Description",
    "CreatedDate",
    "CreatedById",
    "UpdatedDate",
    "UpdatedById"
)
VALUES
(
    'ce5e6303-3ac6-4af1-92b4-f708da026d20',
    'Help Request',
    'Status type for help request workflow',
    NOW(),
    NULL,
    NULL,
    NULL
);


INSERT INTO "Statuses"
(
    "Id",
    "Name",
    "Description",
    "StatusTypeId",
    "CreatedDate",
    "CreatedById",
    "UpdatedDate",
    "UpdatedById"
)
VALUES
(
    '627407c8-700d-46fc-a3e5-02dc368fb75e',
    'Pending',
    'Help request is pending',
    'ce5e6303-3ac6-4af1-92b4-f708da026d20',
    NOW(),
    NULL,
    NULL,
    NULL
);
INSERT INTO "Users" (
    "Id", 
    "FirstName", 
    "MiddleName", 
    "LastName", 
    "Email", 
    "ImageUrl", 
    "CanvasUserId", 
    "SISId", 
    "PasswordHash", 
    "PasswordSalt", 
    "IsActive", 
    "CreatedDate", 
    "CreatedById", 
    "UpdatedDate", 
    "UpdatedById"
)
VALUES (
    gen_random_uuid(),            -- Generates a new unique Id
    'Admin',                      -- FirstName
    NULL,                         -- MiddleName
    'Power',                      -- LastName
    'adminpower@yopmail.com',     -- Email
    NULL,                         -- ImageUrl
    NULL,                         -- CanvasUserId
    NULL,                         -- SISId
    'OIvraEJwj1gYf1uQu+8lYeyw8ripiCmFWWCenFC20h7aG7GOOgmhftzqN02nxt4b5h97rymt0a7rRVUG7afLUg==', -- PasswordHash
    'wLsTHb6azf2oj7JQ/vEA9cb/1g/axBQS4BSdoero47jX6XPkwEVFtllcKbr3t5e6hd7JDDEdhFjNUtSfmoUKtfa3wJY9F9rRLXCoLAdeRJGln8YsbJgKvfz5mDgsXao+7PuelGiPKcm/lTE0FIQ+F0H47dOmRbpgAE1qA8FkEqc=', -- PasswordSalt
    TRUE,                         -- IsActive
    NOW(),                        -- CreatedDate
    '8F3B2A91-6E5C-4C7B-9E91-1A2D4F8C3B10', -- CreatedById (RoleId of Admin)
    NOW(),                        -- UpdatedDate
    '8F3B2A91-6E5C-4C7B-9E91-1A2D4F8C3B10'  -- UpdatedById (RoleId of Admin)
);


Admin PASS Hash and Salt
-----------------------
Hash: p7fCwcYSX9RBZxY7A1GfoBb6wjSWpCe7iZgOSw2FmrVpGFeYjAZ5NtJ2VhGO+onJM15ICUlHGZJ0H2QuFuamLw==
Salt: 8p8/8SQodYAGCItjYQdhwHTsSspxKU1Al2WCLZdBcYiiT7hhE52w1JgCiKIi0tSHkf9Dix1XIIf7UP1Sqg0S2Wg3sllq9Y/XcYoKBQ9+6uceUUIjr3eYHaAZZt7ihkwReCvzAkdUGhPnAssxX74H9towc0tXbtFl2bu3u3uhsQI=


------------------------------
Winter January to March
Spring April to June
Summer July to September
Fall October to December

-------------------------------------
Consider TA Lab hours as well as Tutor hours, while following these rules
Wednesdays and Thursdays
No more than one tutor at any given time
No hours earlier than 9 AM
No hours later than 5 PM
Fridays
No more than two tutors at any given time
No hours earlier than 8 AM
No hours later than 8 PM
Saturdays and Sundays
No more than three tutors at any given time
No hours earlier than 8 AM
No hours later than 9:30 PM
Mondays and Tuesdays
No more than four tutors at any given time
No hours earlier than 8 AM
No hours later than 9:30 PM
Try to have coverage at all times throughout the day
General
No lab hours during scheduled Problem-Solving Sessions or Discussion Section
No more than 4 Lab Hours in a row
First day of Lab Hours is Wednesday of Week 1 (the oﬃcial release date of PA1)
