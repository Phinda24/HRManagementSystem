namespace HR_Management_System.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        //Admi, Manager, Employee
        public string Role { get; set; } 
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        //Navigation
        public Employee Employee { get; set; }

    }
}
