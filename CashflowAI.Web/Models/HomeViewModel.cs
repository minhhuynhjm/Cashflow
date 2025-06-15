using System.Collections.Generic;
using CashflowAI.Domain.DTOs;
using CashflowAI.Domain.Entities;

namespace CashflowAI.Web.Models
{
    public class HomeViewModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance => TotalIncome - TotalExpenses;
        public List<TransactionDto> RecentTransactions { get; set; } = new List<TransactionDto>();
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }
} 