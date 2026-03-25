using HR_Management_System.Models.Entities;
using HRManagementSystem.Models.ViewModels.Department;

namespace HR_Management_System.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task CreateAsync(CreateDepartmentViewModel model);
        Task UpdateAsync(int id, DepartmentViewModel model);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> HasEmployeesAsync(int id);
        Task<Department> GetByManagerEmailAsync(string email);
    }
}
