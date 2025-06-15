using CashflowAI.Domain.Interfaces;
using CashflowAI.Domain.Interfaces.Repositories;
using CashflowAI.Infrastructure.Repositories;

namespace CashflowAI.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository Users { get; }
        public ICategoryRepository Categories { get; }
        public ITransactionRepository Transactions { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Categories = new CategoryRepository(_context);
            Transactions = new TransactionRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
} 