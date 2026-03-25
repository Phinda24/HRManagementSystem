using HR_Management_System.Models.Entities;
using HRManagementSystem.Data;
using HRManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace HR_Management_System.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.User)
                .Where(e => e.IsActive)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.User)
                .Include(e => e.Payslips)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e =>
                    e.Email == email && e.IsActive);
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentAsync(
            int departmentId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Where(e =>
                    e.DepartmentId == departmentId && e.IsActive)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var employee = await _context.Employees
                .FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Employees
                .AnyAsync(e => e.Id == id && e.IsActive);
        }
    }
}