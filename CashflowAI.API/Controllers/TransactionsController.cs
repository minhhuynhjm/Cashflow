using CashflowAI.Domain.DTOs;
using CashflowAI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CashflowAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetById(int id)
        {
            var transaction = await _transactionService.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            return Ok(transaction);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetByUserId(int userId)
        {
            var transactions = await _transactionService.GetByUserIdAsync(userId);
            return Ok(transactions);
        }

        [HttpGet("user/{userId}/date-range")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetByDateRange(
            int userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var transactions = await _transactionService.GetByDateRangeAsync(userId, startDate, endDate);
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Create(CreateTransactionDto createDto)
        {
            var result = await _transactionService.CreateAsync(createDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTransactionDto transactionDto)
        {
            await _transactionService.UpdateAsync(id, transactionDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionService.DeleteAsync(id);
            return NoContent();
        }
    }
} 