using System;
using System.ComponentModel.DataAnnotations;

namespace CashflowAI.Domain.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string Color { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsDefault { get; set; }
        public int UserId { get; set; }
    }
} 