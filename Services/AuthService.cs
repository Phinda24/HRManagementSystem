using HR_Management_System.Services.Interfaces;
using HR_Management_System.Models.Entities;
using HR_Management_System.Models.ViewModels.Auth;

using HRManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HR_Management_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> LoginAsync(
            string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == email && u.IsActive);

            if (user == null) return null;

            bool isValid = VerifyPassword(
                password, user.PasswordHash);
            return isValid ? user : null;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            if (await EmailExistsAsync(model.Email))
                return false;

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                Role = model.Role,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // If Manager assign department ManagerId
            if (model.Role == "Manager" &&
                model.DepartmentId.HasValue)
            {
                var department = await _context.Departments
                    .FindAsync(model.DepartmentId.Value);

                if (department != null)
                {
                    // Create employee record for manager
                    var managerEmployee = new Employee
                    {
                        FirstName = model.FullName.Split(' ')[0],
                        LastName = model.FullName.Contains(' ')
                            ? model.FullName.Substring(
                                model.FullName.IndexOf(' ') + 1)
                            : model.FullName,
                        Email = model.Email,
                        DepartmentId = model.DepartmentId.Value,
                        Position = "Manager",
                        UserId = null,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        StartDate = DateTime.Now
                    };

                    await _context.Employees.AddAsync(managerEmployee);
                    await _context.SaveChangesAsync();

                    // Assign manager to department
                    department.ManagerId = managerEmployee.Id;
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 11);
        }

        public bool VerifyPassword(
            string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}