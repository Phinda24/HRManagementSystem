using HR_Management_System.Models.Entities;
using HR_Management_System.Models.ViewModels.Auth;

namespace HR_Management_System.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(RegisterViewModel model);
        Task<bool> EmailExistsAsync(string email);
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
