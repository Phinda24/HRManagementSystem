using HR_Management_System.Models.Entities;
using HRManagementSystem.Models.ViewModels.Employee;


namespace HR_Management_System.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetByDepartmentAsyc(int departmentId);
        Task CreateAsync(CreateEmployeeViewModel model, int userId);
        Task UpdateAsync(int id, EditEmployeeViewModel model);
        Task SoftDeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<Employee> GetByEmailAsync(string email);
        Task<IEnumerable<Employee>> GetByManagerEmailAsync(string email);
        Task<Department> GetManagerDepartmentAsync(string email);
    }
}
