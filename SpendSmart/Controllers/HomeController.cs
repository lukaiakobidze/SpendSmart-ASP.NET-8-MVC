using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpendSmart.Models;

namespace SpendSmart.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ExpensesDbContext _context;
    public HomeController(ILogger<HomeController> logger, ExpensesDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Expenses()
    {
        var allExpenses = _context.Expenses.ToList();

        var totalExpenses = allExpenses.Sum(x => x.Value);

        ViewBag.Expenses = totalExpenses;

        return View(allExpenses);
    }
    [HttpGet("Home/ExpensesMonthly")]
    public IActionResult ExpensesMonthly(int? month)
    {
        
        var allExpenses = _context.Expenses.ToList();
        List<Expense> monthExpenses = new List<Expense>();
        foreach (var expense in allExpenses)
        {
            if (month > 0)
            {
                if (expense.Date.Month == month)
                {
                    monthExpenses.Add(expense);
                }
            }
            else
            {
                if (expense.Date.Month == DateTime.Now.Month)
                {
                    monthExpenses.Add(expense);
                }
            }
        }
        var monthlySum = monthExpenses.Sum(x => x.Value);
        ViewBag.Expenses = monthlySum;
        return View(monthExpenses);
    }
    public IActionResult ExpensesCategory(int? category)
    {
        var allExpenses = _context.Expenses.ToList();
        List<Expense> categoryExpenses = new List<Expense>();

        foreach (var expense in allExpenses)
        {
            if (category > 0)
            {
                if (category == expense.CategoryId)
                {
                    categoryExpenses.Add(expense);
                }
            }
            else
            {
                categoryExpenses.Add(expense);
            }
        }
        var categorySum = categoryExpenses.Sum(x => x.Value);
        ViewBag.Expenses = categorySum;
        return View(categoryExpenses);
    }
    public IActionResult CreateEditExpense(int? id)
    {
        if (id != null)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            return View(expenseInDb);
        }

        return View();
    }
    public IActionResult DeleteExpense(int id)
    {
        var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
        _context.Expenses.Remove(expenseInDb);
        _context.SaveChanges();
        return RedirectToAction("Expenses");
    }
    public IActionResult CreateEditExpenseForm(Expense model)
    {
        if(model.Id == 0)
        {
            _context.Expenses.Add(model);
        }
        else
        {
            _context.Expenses.Update(model);
        }

        _context.SaveChanges();

        return RedirectToAction("Expenses");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
