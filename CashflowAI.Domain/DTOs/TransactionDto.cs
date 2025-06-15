using System;
using System.ComponentModel.DataAnnotations;

namespace CashflowAI.Domain.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = null!;
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; } = null!;
        public string? Notes { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; } = null!;
    }

    public class CreateTransactionDto
    {
        [Required]
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int? CategoryId { get; set; }
    }

    public class UpdateTransactionDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = null!;

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
} 