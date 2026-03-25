using System.ComponentModel.DataAnnotations;

namespace HR_Management_System.Models.Entities
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Manager assigned to this department
        public int? ManagerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Employee Manager { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
