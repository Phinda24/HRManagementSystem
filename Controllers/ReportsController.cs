using HR_Management_System.Services.Interfaces;
using HRManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HR_Management_System.Controllers
{
    [Authorize(AuthenticationSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ReportsController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IPayrollService _payrollService;

        public ReportsController(
            IEmployeeService employeeService,
            IDepartmentService departmentService,
            IPayrollService payrollService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _payrollService = payrollService;
        }

        public async Task<IActionResult> Index()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userRole == "Admin")
            {
                var employees = await _employeeService.GetAllAsync();
                var departments = await _departmentService.GetAllAsync();
                var payslips = await _payrollService.GetAllAsync();

                ViewBag.TotalEmployees = employees.Count();
                ViewBag.TotalDepartments = departments.Count();
                ViewBag.TotalPayslips = payslips.Count();
                ViewBag.TotalPayroll = payslips.Any()
                    ? payslips.Sum(p => p.NetSalary) : 0m;
                ViewBag.RecentEmployees = employees
                    .OrderByDescending(e => e.CreatedAt)
                    .Take(5).ToList();
            }
            else if (userRole == "Manager")
            {
                var myEmployees = await _employeeService
                    .GetByManagerEmailAsync(userEmail);

                var myDepartment = await _departmentService
                    .GetByManagerEmailAsync(userEmail);

                var allPayslips = await _payrollService.GetAllAsync();

                var myEmployeeIds = myEmployees
                    .Select(e => e.Id).ToList();

                var myPayslips = allPayslips
                    .Where(p => myEmployeeIds.Contains(p.EmployeeId))
                    .ToList();

                ViewBag.TotalEmployees = myEmployees.Count();
                ViewBag.TotalDepartments = 1;
                ViewBag.TotalPayslips = myPayslips.Count();
                ViewBag.TotalPayroll = myPayslips.Any()
                    ? myPayslips.Sum(p => p.NetSalary) : 0m;
                ViewBag.DepartmentName = myDepartment?.Name
                    ?? "Not Assigned";
                ViewBag.RecentEmployees = myEmployees
                    .OrderByDescending(e => e.CreatedAt)
                    .Take(5).ToList();
            }
            else
            {
                // Employee
                var employee = await _employeeService
                    .GetByEmailAsync(userEmail);

                if (employee != null)
                {
                    var myPayslips = await _payrollService
                        .GetByEmployeeIdAsync(employee.Id);

                    var recentPayslips = myPayslips
                        .OrderByDescending(p => p.Year)
                        .ThenByDescending(p => p.Month)
                        .Take(5)
                        .ToList();

                    ViewBag.MyPayslipCount = myPayslips.Count();
                    ViewBag.MyDepartment = employee.Department?.Name
                        ?? "Not Assigned";
                    ViewBag.MyPosition = employee.Position
                        ?? "Not Assigned";
                    ViewBag.MyRecentPayslips = recentPayslips;
                    ViewBag.LatestNetSalary = recentPayslips.Any()
                        ? recentPayslips.First().NetSalary : 0m;
                }
                else
                {
                    ViewBag.MyPayslipCount = 0;
                    ViewBag.MyDepartment = "Not Assigned";
                    ViewBag.MyPosition = "Not Assigned";
                    ViewBag.MyRecentPayslips = null;
                    ViewBag.LatestNetSalary = 0m;
                }
            }

            return View();
        }
    }
}