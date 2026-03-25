using HR_Management_System.Models.Entities;


namespace HRManagementSystem.Repositories.Interfaces
{
    public interface IPayrollRepository
    {
        Task<IEnumerable<Payslip>> GetAllAsync();
        Task<IEnumerable<Payslip>> GetByEmployeeIdAsync(int employeeId);
        Task<Payslip> GetByIdAsync(int id);
        Task<Payslip> GetByMonthYearAsync(int employeeId, int month, int year);
        Task AddAsync(Payslip payslip);
        Task UpdateAsync(Payslip payslip);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int employeeId, int month, int year);
    }
}
