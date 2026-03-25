using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Models.ViewModels.Department
{
    public class CreateDepartmentViewModel
    {
        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Department Name")]
        public string Name { get; set; }
    }
}
