using System;
using System.ComponentModel.DataAnnotations;

namespace CashflowAI.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Icon { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Color { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDefault { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
} 