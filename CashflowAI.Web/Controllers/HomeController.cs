using Microsoft.AspNetCore.Mvc;
using CashflowAI.Web.Models;
using CashflowAI.Domain.Interfaces;

namespace CashflowAI.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITransactionService _transactionService;
    private readonly ICategoryService _categoryService;

    public HomeController(
        ILogger<HomeController> logger,
        ITransactionService transactionService,
        ICategoryService categoryService)
    {
        _logger = logger;
        _transactionService = transactionService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = 1;
            var startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var transactions = await _transactionService.GetByUserIdAsync(userId);
            var monthlyTransactions = transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate);

            var viewModel = new HomeViewModel
            {
                TotalIncome = await _transactionService.GetTotalIncomeAsync(userId, startDate, endDate),
                TotalExpenses = await _transactionService.GetTotalExpensesAsync(userId, startDate, endDate),
                RecentTransactions = transactions.OrderByDescending(t => t.TransactionDate).Take(5).ToList(),
                Categories = (await _categoryService.GetByUserIdAsync(userId)).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading dashboard data");
            return View("Error");
        }
    }

    public async Task<IActionResult> Transactions()
    {
        try
        {
            var userId = 1;
            var transactions = await _transactionService.GetByUserIdAsync(userId);
            var categories = await _categoryService.GetByUserIdAsync(userId);

            ViewBag.Categories = categories;
            return View(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading transactions");
            return View("Error");
        }
    }

    public async Task<IActionResult> Categories()
    {
        try
        {
            var userId = 1;
            var categories = await _categoryService.GetByUserIdAsync(userId);
            return View(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading categories");
            return View("Error");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}
