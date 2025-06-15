using System;
using System.Threading.Tasks;
using CashflowAI.Domain.Interfaces.Repositories;

namespace CashflowAI.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        ITransactionRepository Transactions { get; }
        Task<int> SaveChangesAsync();
    }
} 