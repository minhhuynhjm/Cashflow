using CashflowAI.Domain.Entities;
using CashflowAI.Domain.Interfaces.Repositories;
using CashflowAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashflowAI.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.CategoryId == categoryId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Transaction> Transactions, int TotalRecords)> GetPagedAsync(int userId, DateTime? startDate, DateTime? endDate, int? categoryId, string type, string searchTerm, int page, int pageSize)
        {
            var query = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId);

            if (startDate.HasValue)
                query = query.Where(t => t.TransactionDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(t => t.TransactionDate <= endDate.Value);
            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);
            if (!string.IsNullOrEmpty(type))
                query = query.Where(t => t.Type == type);
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(t => t.Description.Contains(searchTerm) || (t.Notes != null && t.Notes.Contains(searchTerm)));

            var totalRecords = await query.CountAsync();
            var transactions = await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (transactions, totalRecords);
        }
    }
} 