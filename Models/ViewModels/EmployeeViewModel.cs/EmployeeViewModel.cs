namespace HRManagementSystem.Models.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public string DepartmentName { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
    }
}
