namespace HR_Management_System.Models.Entities
{
    public class Payslip
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary => BasicSalary - Deductions;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime PaidOn { get; set; }

        //Navigation
        public Employee Employee { get; set; }

    }
}
