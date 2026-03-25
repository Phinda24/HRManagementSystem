using HR_Management_System.Services.Interfaces;
using HRManagementSystem.Models.ViewModels.Department;
using HRManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace HR_Management_System.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        //Get: /Departments

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAllAsync();
            return View(departments);
        }

        //Get: /Departments/Details/5
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult create()
        {
            return View();
        }

        //POST: /Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _departmentService.CreateAsync(model);
            TempData["SuccessMessage"] = "Department created successfully.";
            return RedirectToAction(nameof(Index));
        }
        //Get: /Departments/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                TempData["ErrorMessage"] = "Department not found.";
                return RedirectToAction(nameof(Index));
            }
            var model = new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
                CreatedAt = department.CreatedAt
            };
            return View(model);
        }
        //POST: /Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool exists = await _departmentService.ExistsAsync(id);
            if (!exists)
            {
                TempData["ErrorMessage"] = "Department not found.";
                return RedirectToAction(nameof(Index));
            }
            await _departmentService.UpdateAsync(id, model);
            TempData["SuccessMessage"] = "Department updated successfully.";
            return RedirectToAction(nameof(Index));

        }
        //Post: /Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            bool HasEmployees = await _departmentService.HasEmployeesAsync(id);
            if (HasEmployees)
            {
                TempData["ErrorMessage"] = "Department not found.";
                return RedirectToAction(nameof(Index));
            }
            await _departmentService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Department deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
       
    }
}
