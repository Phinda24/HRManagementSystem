using HR_Management_System.Models.Entities;
using HRManagementSystem.Models.ViewModels.Department;
using HR_Management_System.Services.Interfaces;
using HR_Management_System.Repositories.Interfaces;

namespace HR_Management_System.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(
            IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(CreateDepartmentViewModel model)
        {
            var department = new Department
            {
                Name = model.Name,
                CreatedAt = DateTime.Now
            };
            await _departmentRepository.AddAsync(department);
        }

        public async Task UpdateAsync(int id, DepartmentViewModel model)
        {
            var department = await _departmentRepository
                .GetByIdAsync(id);
            if (department == null) return;

            department.Name = model.Name;
            await _departmentRepository.UpdateAsync(department);
        }

        public async Task DeleteAsync(int id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _departmentRepository.ExistsAsync(id);
        }

        public async Task<bool> HasEmployeesAsync(int id)
        {
            return await _departmentRepository.HasEmployeesAsync(id);
        }

        public async Task<Department> GetByManagerEmailAsync(
            string email)
        {
            return await _departmentRepository
                .GetByManagerEmailAsync(email);
        }
    }
}