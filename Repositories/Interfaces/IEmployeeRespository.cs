using HR_Management_System.Models.Entities;
using HRManagementSystem.Repositories.Interfaces;


namespace HRManagementSystem.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<Employee> GetByEmailAsync(string email);
        Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task SoftDeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
    
