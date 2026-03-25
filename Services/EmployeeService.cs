using HR_Management_System.Models.Entities;
using HRManagementSystem.Models.ViewModels.Employee;
using HR_Management_System.Services.Interfaces;

using HRManagementSystem.Repositories.Interfaces;
using HRManagementSystem.Services.Interfaces;

namespace HRManagementSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
        {
            return await _employeeRepository.GetByDepartmentAsync(departmentId);
        }

        public async Task CreateAsync(CreateEmployeeViewModel model, int userId)
        {
            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DepartmentId = model.DepartmentId,
                Position = model.Position,
                Salary = model.Salary,
                StartDate = model.StartDate,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _employeeRepository.AddAsync(employee);
        }

        public async Task UpdateAsync(int id, EditEmployeeViewModel model)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return;

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.PhoneNumber = model.PhoneNumber;
            employee.DepartmentId = model.DepartmentId;
            employee.Position = model.Position;
            employee.Salary = model.Salary;
            employee.StartDate = model.StartDate;

            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task SoftDeleteAsync(int id)
        {
            await _employeeRepository.SoftDeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _employeeRepository.ExistsAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailAsync(email);
            return employee != null;
        }
        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _employeeRepository.GetByEmailAsync(email);
        }
        public async Task<IEnumerable<Employee>> GetByManagerEmailAsync(
    string email)
        {
            // Find the manager employee record
            var manager = await _employeeRepository
                .GetByEmailAsync(email);
            if (manager == null)
                return Enumerable.Empty<Employee>();

            // Get their department
            return await _employeeRepository
                .GetByDepartmentAsync(manager.DepartmentId);
        }

        public async Task<Department> GetManagerDepartmentAsync(string email)
        {
            var manager = await _employeeRepository
                .GetByEmailAsync(email);
            if (manager == null) return null;

            return manager.Department;
        }
       

        // Add explicit implementation for the interface method with the correct name
        public async Task<IEnumerable<Employee>> GetByDepartmentAsyc(int departmentId)
        {
            return await GetByDepartmentAsync(departmentId);
        }
    }
}
