using HR_Management_System.Models.Entities;
// Fix: Update the namespace to match the correct one if 'ViewModels' is not under 'HR_Management_System.Models'
using HR_Management_System.Models.ViewModels.Payroll;

namespace HRManagementSystem.Services.Interfaces
{
    public interface IPayrollService
    {
        Task<IEnumerable<Payslip>> GetAllAsync();
        Task<IEnumerable<Payslip>> GetByEmployeeIdAsync(int employeeId);
        Task<Payslip> GetByIdAsync(int id);
        Task GenerateAsync(GeneratePayslipViewModel model);
        Task DeleteAsync(int id);
        Task<bool> PayslipExistsAsync(int employeeId, int month, int year);
    }
}
