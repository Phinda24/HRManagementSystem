using HR_Management_System.Models.ViewModels.Auth;
using HR_Management_System.Services.Interfaces;
using HRManagementSystem.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HR_Management_System.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IDepartmentService _departmentService;

        public AuthController(
            IAuthService authService,
            IDepartmentService departmentService)
        {
            _authService = authService;
            _departmentService = departmentService;
        }

        // GET: /Auth/Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Reports");
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _authService
                .LoginAsync(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("",
                    "Invalid email or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,
                    user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Reports");
        }

        // GET: /Auth/Register
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel
            {
                AvailableDepartments =
                    await GetAvailableDepartments()
            };
            return View(model);
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            // If Manager selected validate department is chosen
            if (model.Role == "Manager" && model.DepartmentId == null)
            {
                ModelState.AddModelError("DepartmentId",
                    "Please assign a department to this manager.");
            }

            if (!ModelState.IsValid)
            {
                model.AvailableDepartments =
                    await GetAvailableDepartments();
                return View(model);
            }

            // Check email not already registered
            bool emailExists = await _authService
                .EmailExistsAsync(model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email",
                    "Email is already registered.");
                model.AvailableDepartments =
                    await GetAvailableDepartments();
                return View(model);
            }

            // Check department does not already have a manager
            if (model.Role == "Manager" &&
                model.DepartmentId.HasValue)
            {
                var dept = await _departmentService
                    .GetByIdAsync(model.DepartmentId.Value);

                if (dept?.ManagerId != null)
                {
                    ModelState.AddModelError("DepartmentId",
                        "This department already has a manager " +
                        "assigned. Please choose another department.");
                    model.AvailableDepartments =
                        await GetAvailableDepartments();
                    return View(model);
                }
            }

            bool success = await _authService
                .RegisterAsync(model);
            if (!success)
            {
                ModelState.AddModelError("",
                    "Registration failed. Please try again.");
                model.AvailableDepartments =
                    await GetAvailableDepartments();
                return View(model);
            }

            TempData["Success"] =
                $"Account created successfully for {model.FullName}.";
            return RedirectToAction("Index", "Reports");
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        // GET: /Auth/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Helper — only departments without a manager
        private async Task<IEnumerable<SelectListItem>>
            GetAvailableDepartments()
        {
            var departments = await _departmentService.GetAllAsync();
            var available = departments
                .Where(d => d.ManagerId == null)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                });
            return available;
        }
    }
}