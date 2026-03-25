using System.ComponentModel.DataAnnotations;

namespace HR_Management_System.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        [Phone]
        [StringLength(10)]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Department is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a department")]
        public int DepartmentId { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //NAvigation
        public User User { get; set; }

        public Department Department { get; set; }
        public ICollection<Payslip> Payslips { get; set; }
    }
}
