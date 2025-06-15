using CashflowAI.Domain.DTOs;

namespace CashflowAI.Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetByUserIdAsync(int userId);
        Task<TransactionDto> GetByIdAsync(int id);
        Task<IEnumerable<TransactionDto>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<TransactionDto>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
        Task UpdateAsync(int id, UpdateTransactionDto dto);
        Task DeleteAsync(int id);
        Task<decimal> GetTotalIncomeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalExpensesAsync(int userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<TransactionDto>> GetTransactionsAsync(int userId);
        Task<(IEnumerable<TransactionDto> Transactions, int TotalRecords)> GetPagedTransactionsAsync(int userId, DateTime? startDate, DateTime? endDate, int? categoryId, string type, string searchTerm, int page, int pageSize);
    }
} 