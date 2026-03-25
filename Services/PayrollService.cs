using HR_Management_System.Models.Entities;
using HR_Management_System.Models.ViewModels.Payroll; 
using HRManagementSystem.Repositories.Interfaces;
using HRManagementSystem.Services.Interfaces;

namespace HRManagementSystem.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public PayrollService(
            IPayrollRepository payrollRepository,
            IEmployeeRepository employeeRepository)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Payslip>> GetAllAsync()
        {
            return await _payrollRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Payslip>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _payrollRepository.GetByEmployeeIdAsync(employeeId);
        }

        public async Task<Payslip> GetByIdAsync(int id)
        {
            return await _payrollRepository.GetByIdAsync(id);
        }

        public async Task GenerateAsync(GeneratePayslipViewModel model)
        {
            var employee = await _employeeRepository.GetByIdAsync(model.EmployeeId);
            if (employee == null) return;

            var payslip = new Payslip
            {
                EmployeeId = model.EmployeeId,
                Month = model.Month,
                Year = model.Year,
                BasicSalary = model.BasicSalary,
                Deductions = model.Deductions,
                PaidOn = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            await _payrollRepository.AddAsync(payslip);
        }

        public async Task DeleteAsync(int id)
        {
            await _payrollRepository.DeleteAsync(id);
        }

        public async Task<bool> PayslipExistsAsync(int employeeId, int month, int year)
        {
            return await _payrollRepository.ExistsAsync(employeeId, month, year);
        }
    }
}