using CashflowAI.Domain.Entities;

namespace CashflowAI.Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetByUserIdAsync(int userId);
    Task<Category> AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
    Task<IEnumerable<Category>> GetDefaultCategoriesAsync();
} 