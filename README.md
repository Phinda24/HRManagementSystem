# HR Management System

A full-stack HR Management System built with ASP.NET Core MVC 
(.NET 8), Entity Framework Core, SQL Server, Cookie Authentication 
and xUnit unit tests.

---

## Features

- Multi-role authentication — Admin, Manager, Employee
- Cookie-based secure login with 8 hour session
- Employee management — create, edit, view, soft delete
- Department management with manager assignment
- Payroll — generate payslips with live net salary calculator
- Role-based dashboards — each role sees relevant data only
- Manager sees only their department employees and payslips
- Employee sees only their own payslips
- 19 unit tests using xUnit and FluentAssertions

---

## Tech Stack

| Layer | Technology |
|---|---|
| Language | C# |
| Framework | ASP.NET Core MVC (.NET 8) |
| Database | SQL Server |
| ORM | Entity Framework Core 8 |
| Authentication | Cookie Authentication |
| Frontend | Razor Views, Bootstrap 5, Bootstrap Icons |
| Testing | xUnit, FluentAssertions, EF Core InMemory |
| Architecture | Repository Pattern, Service Layer |

---

## Project Structure
```
HR Management System/
├── Controllers/         # MVC Controllers
├── Models/
│   ├── Entities/        # Database models
│   └── ViewModels/      # Form and view models
├── Views/               # Razor views
├── Services/            # Business logic layer
├── Repositories/        # Data access layer
├── Data/                # AppDbContext
└── HRManagementSystem.Tests/
    ├── Helpers/         # InMemory DB helper
    └── Services/        # Unit tests
```

---

## Role Based Access

| Feature | Admin | Manager | Employee |
|---|---|---|---|
| View All Employees | ✅ | ❌ | ❌ |
| View Dept Employees | ✅ | ✅ | ❌ |
| Create Employee | ✅ | ❌ | ❌ |
| Edit Employee | ✅ | ✅ | ❌ |
| Delete Employee | ✅ | ❌ | ❌ |
| Manage Departments | ✅ | ❌ | ❌ |
| Register Users | ✅ | ❌ | ❌ |
| Generate Payslip | ✅ | ❌ | ❌ |
| View All Payslips | ✅ | ✅ | ❌ |
| View Own Payslips | ✅ | ✅ | ✅ |

---

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server or LocalDB
- Visual Studio 2022

### Setup

1. Clone the repository
```bash
git clone https://github.com/Phinda24/HRManagementSystem.git
```

2. Update connection string in appsettings.json
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;
   Database=HRManagementDB;Trusted_Connection=True;"
}
```

3. Run migrations
```bash
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

5. Register first Admin account at `/Auth/Register`
katleho@gmail.com
1. password :
---

## Running Tests
```bash
dotnet test
```

**19 tests passing:**
- AuthServiceTests — 8 tests
- DepartmentServiceTests — 5 tests
- EmployeeServiceTests — 6 tests

---
## Screenshots
- <img width="1358" height="684" alt="Screenshot 2026-03-23 173135" src="https://github.com/user-attachments/assets/c991693d-82f2-45e7-afe6-0a46ea5830b2" />


## Author

**Phinda Mpho Moloko**
South Africa
[LinkedIn](https://www.linkedin.com/in/phinda-mpho-35844a309/ |
[GitHub](https://github.com/Phinda24)
```
