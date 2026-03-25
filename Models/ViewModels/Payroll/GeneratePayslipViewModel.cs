using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HR_Management_System.Models.ViewModels.Payroll
{
    public class GeneratePayslipViewModel
    {
        [Required(ErrorMessage = "Employee is required")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Month is required")]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        [Display(Name = "Month")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(2000, 2100, ErrorMessage = "Please enter a valid year")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Basic salary is required")]
        [Range(0, double.MaxValue,
            ErrorMessage = "Salary must be a positive number")]
        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }

        [Required(ErrorMessage = "Deductions are required")]
        [Range(0, double.MaxValue,
            ErrorMessage = "Deductions must be a positive number")]
        [Display(Name = "Deductions")]
        public decimal Deductions { get; set; }

        [ValidateNever] // ✅ prevents dropdown from failing validation
        public IEnumerable<SelectListItem>? Employees { get; set; }
    }
}
