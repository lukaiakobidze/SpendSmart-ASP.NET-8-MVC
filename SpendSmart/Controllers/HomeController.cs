using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpendSmart.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace SpendSmart.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ExpensesDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ExpensesDbContext context, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        //var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Spender";
        //ViewBag.UserName = userName;

        return View();
    }
    public IActionResult Expenses()
    {
        var userId = _userManager.GetUserId(User); 
        var userExpenses = _context.Expenses.Where(e => e.UserId == userId).ToList(); 

        var totalExpenses = userExpenses.Sum(x => x.Value);

        ViewBag.Expenses = totalExpenses;

        return View(userExpenses);
    }
    [HttpGet("Home/ExpensesMonthly")]

    public IActionResult ExpensesMonthly(int? month)
    {
        var userId = _userManager.GetUserId(User); 
        var allExpenses = _context.Expenses.Where(e => e.UserId == userId).ToList(); 

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
        if (month != null)
        {
            ViewBag.CurrentMonth = month;
        }
        else
        {
            ViewBag.CurrentMonth = DateTime.Now.Month;
        }

            return View(monthExpenses);
    }

    public IActionResult ExpensesCategory(int category = 0)
    {
        var userId = _userManager.GetUserId(User); 
        var allExpenses = _context.Expenses.Where(e => e.UserId == userId).ToList(); 

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
        ViewBag.CurrentCategory = category;

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
        if (model.Id == 0)
        {
            
            model.UserId = _userManager.GetUserId(User); 
            _context.Expenses.Add(model);
        }
        else
        {
            
            var existingExpense = _context.Expenses.Find(model.Id);

            if (existingExpense == null)
            {
                return NotFound();
            }

            existingExpense.Value = model.Value;
            existingExpense.Description = model.Description;
            existingExpense.CategoryId = model.CategoryId;
            existingExpense.Date = model.Date;
            
        }

        _context.SaveChanges();

        return RedirectToAction("Expenses");
    }

    public IActionResult ExportToPDF()
    {
        var userId = _userManager.GetUserId(User);
        var data = _context.Expenses.Where(e => e.UserId == userId).ToList();
        var doc = new Document();
        var stream = new MemoryStream();
        PdfWriter.GetInstance(doc, stream);

        doc.Open();
        doc.Add(new Paragraph($"    Expense Report for:     {_userManager.GetUserName(User)}"));
        doc.Add(new Paragraph(" "));
        PdfPTable table = new PdfPTable(4);
        
        table.AddCell("Category");
        table.AddCell("Amount");
        table.AddCell("Date");
        table.AddCell("Description");

        foreach (var item in data)
        {
            table.AddCell(item.CategoryId.ToString());
            table.AddCell(item.Value.ToString());
            table.AddCell(item.Date.ToString());
            table.AddCell(item.Description);
        }

        doc.Add(table);
        doc.Close();

        return File(stream.ToArray(), "application/pdf", "Expenses.pdf");
    }

    public IActionResult ExportToPDFCategory(int category)
    {
        var userId = _userManager.GetUserId(User);

        var expensesQuery = _context.Expenses.Where(e => e.UserId == userId);
        
        if (category > 0) 
        {
            expensesQuery = expensesQuery.Where(e => e.CategoryId == category);
        }

        var categorydata = expensesQuery.ToList();

        
        var doc = new Document();
        var stream = new MemoryStream();
        PdfWriter.GetInstance(doc, stream);

        doc.Open();
        doc.Add(new Paragraph($"Expense Report for: {_userManager.GetUserName(User)}"));
        doc.Add(new Paragraph(" "));

        PdfPTable table = new PdfPTable(4);
        table.AddCell("Category");
        table.AddCell("Amount");
        table.AddCell("Date");
        table.AddCell("Description");
        
        foreach (var item in categorydata)
        {
            table.AddCell(item.CategoryId.ToString());
            table.AddCell(item.Value.ToString("C"));
            table.AddCell(item.Date.ToString("yyyy-MM-dd"));
            table.AddCell(item.Description);
        }

        doc.Add(table);
        doc.Close();

        return File(stream.ToArray(), "application/pdf", $"Expenses_Category_{category}.pdf");
    }

    public IActionResult ExportToPDFMonth(int month)
    {
        var userId = _userManager.GetUserId(User);
        var expensesQuery = _context.Expenses.Where(e => e.UserId == userId);

        if (month > 0)
        {
            expensesQuery = expensesQuery.Where(e => e.Date.Month == month);
        }
        var categorydata = expensesQuery.ToList();

        
        var doc = new Document();
        var stream = new MemoryStream();
        PdfWriter.GetInstance(doc, stream);

        doc.Open();
        doc.Add(new Paragraph($"Expense Report for: {_userManager.GetUserName(User)}"));
        doc.Add(new Paragraph(" "));

        PdfPTable table = new PdfPTable(4);
        table.AddCell("Category");
        table.AddCell("Amount");
        table.AddCell("Date");
        table.AddCell("Description");

        foreach (var item in categorydata)
        {
            table.AddCell(item.CategoryId.ToString());
            table.AddCell(item.Value.ToString());
            table.AddCell(item.Date.ToString());
            table.AddCell(item.Description);
        }

        doc.Add(table);
        doc.Close();

        return File(stream.ToArray(), "application/pdf", $"Expenses_Month_{month}.pdf");
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
