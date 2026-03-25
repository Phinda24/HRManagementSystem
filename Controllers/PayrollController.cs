using HR_Management_System.Models.Entities;
using HR_Management_System.Models.ViewModels.Payroll;
using HR_Management_System.Services;
using HR_Management_System.Services.Interfaces;
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
    public class PayrollController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPayrollService _payrollService;
        private readonly IDepartmentService _departmentService;

        public PayrollController(
            IEmployeeService employeeService,
            IPayrollService payrollService,
            IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _payrollService = payrollService;
            _departmentService = departmentService;
        }

        // GET: /Payroll
        // Admin and Manager see ALL payslips
        // Employee sees ONLY their own
        public async Task<IActionResult> Index()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            try
            {
                if (userRole == "Admin")
                {
                    var allPayslips = await _payrollService.GetAllAsync();
                    return View(allPayslips ?? new List<Payslip>());
                }

                if (userRole == "Manager")
                {
                    var myEmployees = await _employeeService
                        .GetByManagerEmailAsync(userEmail);

                    if (myEmployees == null || !myEmployees.Any())
                        return View(new List<Payslip>());

                    var myEmployeeIds = myEmployees
                        .Select(e => e.Id).ToList();

                    var allPayslips = await _payrollService.GetAllAsync();

                    var deptPayslips = allPayslips
                        .Where(p => myEmployeeIds.Contains(p.EmployeeId))
                        .ToList();

                    return View(deptPayslips);
                }

                // Employee sees only their own
                var employee = await _employeeService
                    .GetByEmailAsync(userEmail);

                if (employee == null)
                {
                    TempData["Error"] =
                        "No employee record found for your account. " +
                        "Please contact your administrator.";
                    return View(new List<Payslip>());
                }

                var myPayslips = await _payrollService
                    .GetByEmployeeIdAsync(employee.Id);

                return View(myPayslips ?? new List<Payslip>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading payroll: {ex.Message}";
                return View(new List<Payslip>());
            }
        } 
        // GET: /Payroll/Details/5
        // Only Admin, Manager, or the employee themselves can view
        public async Task<IActionResult> Details(int id)
        {
            var payslip = await _payrollService.GetByIdAsync(id);
            if (payslip == null)
            {
                TempData["Error"] = "Payslip not found.";
                return RedirectToAction(nameof(Index));
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // Admin and Manager can view any payslip
            if (userRole == "Admin" || userRole == "Manager")
                return View(payslip);

            // Employee can only view their own
            var employee = await _employeeService
                .GetByEmailAsync(userEmail);

            if (employee == null || payslip.EmployeeId != employee.Id)
            {
                TempData["Error"] =
                    "You do not have permission to view this payslip.";
                return RedirectToAction(nameof(Index));
            }

            return View(payslip);
        }

        // GET: /Payroll/Employee/5
        // Admin and Manager only
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Employee(int id)
        {
            var payslips = await _payrollService
                .GetByEmployeeIdAsync(id);
            return View(payslips);
        }

        // GET: /Payroll/Generate
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Generate()
        {
            var model = new GeneratePayslipViewModel
            {
                Employees = await GetEmployeeSelectList(),
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year
            };
            return View(model);
        }

        // POST: /Payroll/Generate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Generate(
            GeneratePayslipViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(" | ",
                        ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .Select(x => $"{x.Key}: " +
                                x.Value.Errors.First().ErrorMessage));

                    TempData["Error"] = $"Validation: {errors}";
                    model.Employees = await GetEmployeeSelectList();
                    return View(model);
                }

                bool exists = await _payrollService.PayslipExistsAsync(
                    model.EmployeeId, model.Month, model.Year);

                if (exists)
                {
                    ModelState.AddModelError("",
                        "A payslip for this employee already " +
                        "exists for the selected month and year.");
                    model.Employees = await GetEmployeeSelectList();
                    return View(model);
                }

                await _payrollService.GenerateAsync(model);
                TempData["Success"] = "Payslip generated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    $"Error: {ex.Message} | " +
                    $"Inner: {ex.InnerException?.Message}";
                model.Employees = await GetEmployeeSelectList();
                return View(model);
            }
        }

        // POST: /Payroll/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _payrollService.DeleteAsync(id);
            TempData["Success"] = "Payslip deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // Helper — builds employee dropdown
        private async Task<IEnumerable<SelectListItem>>
            GetEmployeeSelectList()
        {
            var employees = await _employeeService.GetAllAsync();
            return employees.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.FullName
            });
        }
    }
}