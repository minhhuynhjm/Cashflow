using AutoMapper;
using CashflowAI.Domain.DTOs;
using CashflowAI.Domain.Entities;
using CashflowAI.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CashflowAI.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<TransactionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionDto>> GetByUserIdAsync(int userId)
        {
            var transactions = await _unitOfWork.Transactions.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<TransactionDto> GetByIdAsync(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new Exception($"Transaction with ID {id} not found");
            }
            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task<IEnumerable<TransactionDto>> GetByCategoryAsync(int categoryId)
        {
            var transactions = await _unitOfWork.Transactions.GetByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
        {
            var transaction = _mapper.Map<Transaction>(dto);
            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task UpdateAsync(int id, UpdateTransactionDto dto)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new Exception($"Transaction with ID {id} not found");
            }

            _mapper.Map(dto, transaction);
            await _unitOfWork.Transactions.UpdateAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new Exception($"Transaction with ID {id} not found");
            }

            await _unitOfWork.Transactions.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalIncomeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var transactions = await _unitOfWork.Transactions.GetByUserIdAsync(userId);
            return transactions
                .Where(t => t.Type == "Income" && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .Sum(t => t.Amount);
        }

        public async Task<decimal> GetTotalExpensesAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var transactions = await _unitOfWork.Transactions.GetByUserIdAsync(userId);
            return transactions
                .Where(t => t.Type == "Expense" && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .Sum(t => t.Amount);
        }

        public async Task<IEnumerable<TransactionDto>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var transactions = await _unitOfWork.Transactions.GetByDateRangeAsync(userId, startDate, endDate);

            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(int userId)
        {
            var transactions = await _unitOfWork.Transactions.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<(IEnumerable<TransactionDto> Transactions, int TotalRecords)> GetPagedTransactionsAsync(int userId, DateTime? startDate, DateTime? endDate, int? categoryId, string type, string searchTerm, int page, int pageSize)
        {
            var (transactions, totalRecords) = await _unitOfWork.Transactions.GetPagedAsync(userId, startDate, endDate, categoryId, type, searchTerm, page, pageSize);
            return (_mapper.Map<IEnumerable<TransactionDto>>(transactions), totalRecords);
        }
    }
} 