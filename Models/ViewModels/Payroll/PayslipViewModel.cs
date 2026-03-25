namespace HRManagementSystem.Models.ViewModels.Payroll
{
    public class PayslipViewModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string DepartmentName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary => BasicSalary - Deductions;
        public DateTime PaidOn { get; set; }

        public string MonthName =>
            new DateTime(Year, Month, 1).ToString("MMMM");
    }
}