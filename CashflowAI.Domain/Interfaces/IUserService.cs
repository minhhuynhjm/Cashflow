using System.Threading.Tasks;
using CashflowAI.Domain.DTOs;

namespace CashflowAI.Domain.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> GetByUsernameAsync(string username);
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(UserCreateDto dto);
        Task UpdateAsync(int id, UserUpdateDto dto);
        Task DeleteAsync(int id);
        Task<bool> ValidateUserAsync(string username, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
} 