
using HR_Management_System.Models.Entities;
using HRManagementSystem.Data;
using HR_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HR_Management_System.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees
                    .Where(e => e.IsActive))
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees
                    .Where(e => e.IsActive))
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Department> GetByNameAsync(string name)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d =>
                    d.Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _context.Departments
                .FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Departments
                .AnyAsync(d => d.Id == id);
        }

        public async Task<bool> HasEmployeesAsync(int id)
        {
            return await _context.Employees
                .AnyAsync(e => e.DepartmentId == id && e.IsActive);
        }

        public async Task<Department> GetByManagerEmailAsync(
            string email)
        {
            return await _context.Departments
                .Include(d => d.Employees
                    .Where(e => e.IsActive))
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(d =>
                    d.Manager != null &&
                    d.Manager.Email == email);
        }
    }
}