using CashflowAI.Domain.DTOs;

namespace CashflowAI.Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetByUserIdAsync(int userId);
        Task<CategoryDto> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CategoryDto dto);
        Task UpdateAsync(int id, CategoryDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<CategoryDto>> GetListAsync();
    }
} 