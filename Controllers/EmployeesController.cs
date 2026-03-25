using HR_Management_System.Models.Entities;
using HR_Management_System.Services.Interfaces;
using HRManagementSystem.Models.ViewModels.Employee;
using HRManagementSystem.Services.Interfaces;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HR_Management_System.Controllers
{
    [Authorize(AuthenticationSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme)]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public EmployeesController(
            IEmployeeService employeeService,
            IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        // GET: /Employees
        public async Task<IActionResult> Index()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            IEnumerable<Employee> employees;

            if (userRole == "Manager")
            {
                // Manager sees only their department employees
                employees = await _employeeService
                    .GetByManagerEmailAsync(userEmail);
            }
            else
            {
                // Admin sees all
                employees = await _employeeService.GetAllAsync();
            }

            return View(employees);
        }

        // GET: /Employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                TempData["Error"] = "Employee not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: /Employees/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateEmployeeViewModel
            {
                Departments = await GetDepartmentSelectList() // ✅ fixed
            };
            return View(model);
        }

        // POST: /Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model , int userId)
        {
            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DepartmentId = model.DepartmentId,
                Position = model.Position,
                Salary = model.Salary,
                StartDate = model.StartDate,
                IsActive = true,
                CreatedAt = DateTime.Now,

                UserId = userId // ✅ THIS IS THE FIX
            };
            if (!ModelState.IsValid)
            {
                model.Departments = await GetDepartmentSelectList();
                return View(model);
            }

            bool emailExists = await _employeeService
                .EmailExistsAsync(model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email already exists.");
                model.Departments = await GetDepartmentSelectList();
                return View(model);
            }

            var userIdClaim = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                TempData["Error"] = "Session expired. Please login again.";
                return RedirectToAction("Login", "Auth");
            }

          
            try
            {
                await _employeeService.CreateAsync(model, userId);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            TempData["Success"] = "Employee created successfully."; // ✅ fixed
            return RedirectToAction(nameof(Index));

        }

        // GET: /Employees/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                TempData["Error"] = "Employee not found."; // ✅ fixed
                return RedirectToAction(nameof(Index));
            }

            var model = new EditEmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DepartmentId = employee.DepartmentId,
                Position = employee.Position,
                Salary = employee.Salary,
                StartDate = employee.StartDate,
                Departments = await GetDepartmentSelectList()
            };
            return View(model);
        }

        // POST: /Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")] // ✅ fixed typo "Adim"
        public async Task<IActionResult> Edit(int id, EditEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = await GetDepartmentSelectList();
                return View(model);
            }

            bool exists = await _employeeService.ExistsAsync(id);
            if (!exists)
            {
                TempData["Error"] = "Employee not found."; // ✅ fixed
                return RedirectToAction(nameof(Index));
            }

            await _employeeService.UpdateAsync(id, model);
            TempData["Success"] = "Employee updated successfully."; // ✅ fixed
            return RedirectToAction(nameof(Index));
        }

        // POST: /Employees/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            bool exists = await _employeeService.ExistsAsync(id);
            if (!exists)
            {
                TempData["Error"] = "Employee not found."; // ✅ fixed
                return RedirectToAction(nameof(Index));
            }

            await _employeeService.SoftDeleteAsync(id);
            TempData["Success"] = "Employee deleted successfully."; // ✅ fixed
            return RedirectToAction(nameof(Index));
        }

        // Private helper
        private async Task<IEnumerable<SelectListItem>> GetDepartmentSelectList()
        {
            var departments = await _departmentService.GetAllAsync();
            return departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
        }
    }
}