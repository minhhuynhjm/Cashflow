using CashflowAI.Domain.Constants;
using CashflowAI.Domain.DTOs;
using CashflowAI.Domain.Entities;
using CashflowAI.Domain.Exceptions;
using CashflowAI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CashflowAI.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(
            ITransactionService transactionService,
            ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? categoryId,
            [FromQuery] string? type,
            [FromQuery] string? searchTerm,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = 1; // For demo purposes
                var (transactions, totalRecords) = await _transactionService.GetPagedTransactionsAsync(userId, startDate, endDate, categoryId, type, searchTerm, page, pageSize);
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                return Ok(new { success = true, data = transactions, totalPages, totalRecords });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transactions");
                return StatusCode(500, new { success = false, message = "Error getting transactions" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            try
            {
                var transaction = await _transactionService.GetByIdAsync(id);
                return Ok(new { success = true, data = transaction });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transaction {TransactionId}", id);
                return StatusCode(500, new { success = false, message = "Error getting transaction" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Type))
                {
                    model.Type = Constants.TransactionTypeIncome;
                }

                if (!model.CategoryId.HasValue)
                {
                    model.CategoryId = 1; // Salary
                }

                model.UserId = 1;
                var transaction = await _transactionService.CreateAsync(model);
                return Ok(new { success = true, data = transaction });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transaction");
                return StatusCode(500, new { success = false, message = "Error creating transaction" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionDto dto)
        {
            try
            {
                await _transactionService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "Transaction updated successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction {TransactionId}", id);
                return StatusCode(500, new { success = false, message = "Error updating transaction" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                await _transactionService.DeleteAsync(id);
                return Ok(new { success = true, message = "Transaction deleted successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transaction {TransactionId}", id);
                return StatusCode(500, new { success = false, message = "Error deleting transaction" });
            }
        }

        [HttpGet("stats/summary")]
        public async Task<IActionResult> GetSummary([FromQuery] int year, [FromQuery] int? month)
        {
            try
            {
                var userId = 1; // For demo
                var now = DateTime.UtcNow;
                var start = new DateTime(year, month ?? 1, 1);
                var end = month.HasValue ? start.AddMonths(1).AddDays(-1) : start.AddYears(1).AddDays(-1);
                var income = await _transactionService.GetTotalIncomeAsync(userId, start, end);
                var expenses = await _transactionService.GetTotalExpensesAsync(userId, start, end);
                return Ok(new { success = true, income, expenses });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting summary stats");
                return StatusCode(500, new { success = false, message = "Error getting summary stats" });
            }
        }

        [HttpGet("stats/monthly")]
        public async Task<IActionResult> GetMonthlyStats([FromQuery] int year)
        {
            try
            {
                var userId = 1;
                var income = new decimal[12];
                var expenses = new decimal[12];
                for (int m = 1; m <= 12; m++)
                {
                    var start = new DateTime(year, m, 1);
                    var end = start.AddMonths(1).AddDays(-1);
                    income[m-1] = await _transactionService.GetTotalIncomeAsync(userId, start, end);
                    expenses[m-1] = await _transactionService.GetTotalExpensesAsync(userId, start, end);
                }
                return Ok(new { success = true, income, expenses });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly stats");
                return StatusCode(500, new { success = false, message = "Error getting monthly stats" });
            }
        }

        [HttpGet("stats/daily")]
        public async Task<IActionResult> GetDailyStats([FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                var userId = 1;
                var daysInMonth = DateTime.DaysInMonth(year, month);
                var net = new decimal[daysInMonth];
                for (int d = 1; d <= daysInMonth; d++)
                {
                    var start = new DateTime(year, month, d);
                    var end = start;
                    var income = await _transactionService.GetTotalIncomeAsync(userId, start, end);
                    var expenses = await _transactionService.GetTotalExpensesAsync(userId, start, end);
                    net[d-1] = income + expenses; // expenses là số âm
                }
                return Ok(new { success = true, net });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting daily stats");
                return StatusCode(500, new { success = false, message = "Error getting daily stats" });
            }
        }

        [HttpGet("stats/category-pie")]
        public async Task<IActionResult> GetCategoryPie([FromQuery] int year, [FromQuery] int? month)
        {
            try
            {
                var userId = 1;
                var start = new DateTime(year, month ?? 1, 1);
                var end = month.HasValue ? start.AddMonths(1).AddDays(-1) : start.AddYears(1).AddDays(-1);
                var transactions = await _transactionService.GetTransactionsAsync(userId);
                var filtered = transactions.Where(t => t.TransactionDate >= start && t.TransactionDate <= end);
                var groups = filtered.GroupBy(t => t.Category.Name)
                    .Select(g => new { category = g.Key, total = g.Sum(x => Math.Abs(x.Amount)), type = g.First().Type })
                    .ToList();
                return Ok(new { success = true, data = groups });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category pie stats");
                return StatusCode(500, new { success = false, message = "Error getting category pie stats" });
            }
        }
    }
} 