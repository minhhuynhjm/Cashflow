using CashflowAI.Domain.Entities;

namespace CashflowAI.Domain.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(int id);
    Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(int categoryId);
    Task<Transaction> AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(int id);
    Task<(IEnumerable<Transaction> Transactions, int TotalRecords)> GetPagedAsync(int userId, DateTime? startDate, DateTime? endDate, int? categoryId, string type, string searchTerm, int page, int pageSize);
} 