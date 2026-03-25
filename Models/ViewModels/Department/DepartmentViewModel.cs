namespace HRManagementSystem.Models.ViewModels.Department
{
    public class DepartmentViewModel: CreateDepartmentViewModel
    {
        public int Id { get; set; }
       
        public int EmployeeCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
