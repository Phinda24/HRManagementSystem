using HR_Management_System.Models.Entities;
using HRManagementSystem.Data;

using HRManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly AppDbContext _context;

        public PayrollRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payslip>> GetAllAsync()
        {
            return await _context.Payslips
                .Include(p => p.Employee)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payslip>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.Payslips
                .Include(p => p.Employee)
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();
        }

        public async Task<Payslip> GetByIdAsync(int id)
        {
            return await _context.Payslips
                .Include(p => p.Employee)
                .ThenInclude(e => e.Department)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payslip> GetByMonthYearAsync(int employeeId, int month, int year)
        {
            return await _context.Payslips
                .FirstOrDefaultAsync(p =>
                    p.EmployeeId == employeeId &&
                    p.Month == month &&
                    p.Year == year);
        }

        public async Task AddAsync(Payslip payslip)
        {
            await _context.Payslips.AddAsync(payslip);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payslip payslip)
        {
            _context.Payslips.Update(payslip);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payslip = await _context.Payslips.FindAsync(id);
            if (payslip != null)
            {
                _context.Payslips.Remove(payslip);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int employeeId, int month, int year)
        {
            return await _context.Payslips
                .AnyAsync(p =>
                    p.EmployeeId == employeeId &&
                    p.Month == month &&
                    p.Year == year);
        }
    }
}
