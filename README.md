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
Lefty@gmail.com
1. password :Lefty12
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
### Login
<img width="1366" height="720" alt="Screenshot 2026-03-24 181211" src="https://github.com/user-attachments/assets/b62a65c4-1bb6-4f42-905a-c6ed170ac78e" />

### Admin Dashboard
<img width="1366" height="720" alt="Screenshot 2026-03-24 181211" src="https://github.com/user-attachments/assets/eb55d621-3715-42db-ac82-fe520985b9de" />
<img width="1366" height="768" alt="Screenshot 2026-03-24 181411" src="https://github.com/user-attachments/assets/25f66749-5ece-46ba-98e4-04cdb85e67b7" />
<img width="1366" height="768" alt="Screenshot 2026-03-24 181546" src="https://github.com/user-attachments/assets/21fb7a12-69e3-4f3e-82ce-b1a73889c7de" />

### Employees
<img width="1366" height="768" alt="Screenshot 2026-03-24 181426" src="https://github.com/user-attachments/assets/ba4567e2-714c-4e3f-bc68-e93498e852cf" />
<img width="1366" height="768" alt="Screenshot 2026-03-24 181922" src="https://github.com/user-attachments/assets/2c396116-e773-4efb-a6a8-397e440842c8" />

### Departments
<img width="1366" height="768" alt="Screenshot 2026-03-24 181440" src="https://github.com/user-attachments/assets/8f68bd71-e49f-46af-92da-8af955b9ac72" />
<img width="1366" height="768" alt="Screenshot 2026-03-24 181947" src="https://github.com/user-attachments/assets/ad0891b7-41ce-4281-85a2-60dc968daaa2" />


### Payroll
<img width="1366" height="768" alt="Screenshot 2026-03-24 181455" src="https://github.com/user-attachments/assets/811da12b-e4a7-4bd2-8f65-d00026aaaacc" />
<img width="1366" height="768" alt="Screenshot 2026-03-24 181850" src="https://github.com/user-attachments/assets/9cf7c3f8-15f3-4c47-b8a1-1415a3d8a2c2" />

### add user
<img width="1366" height="768" alt="Screenshot 2026-03-24 181509" src="https://github.com/user-attachments/assets/3175f749-2e46-40f3-8d7c-b53c125f7dc1" />
<img width="1366" height="768" alt="Screenshot 2026-03-24 181523" src="https://github.com/user-attachments/assets/49dc5b9b-abf4-4bf8-ac1a-6ae7e15f691e" />


## Author

**Phinda Mpho Moloko**
South Africa
[LinkedIn](https://www.linkedin.com/in/phinda-mpho-35844a309/ |
[GitHub](https://github.com/Phinda24)
```
