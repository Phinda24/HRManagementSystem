using HR_Management_System.Models.Entities;

namespace HR_Management_System.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task<Department> GetByNameAsync(string name);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> HasEmployeesAsync(int id);
        Task<Department> GetByManagerEmailAsync(string email);
    }
}