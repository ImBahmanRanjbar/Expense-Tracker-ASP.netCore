using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers;

public class TransactionController : Controller
{
    private readonly ExpenseDbContext _context;

    public TransactionController(ExpenseDbContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        var expenseDbContext = _context.Transactions.Include(t => t.Category);
        return View(await expenseDbContext.ToListAsync());
    }


    public IActionResult AddOrEdit(int id=0)
    {   PopulateCategories();
        if (id==0)
        {
            return View(new Transaction());
        }
        else
        {
            return View(_context.Transactions.Find(id));
        }
     
      
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
    {
        if (ModelState.IsValid)
        {
            if (transaction.TransactionId==0)
            {
                _context.Add(transaction);
            }
            else
            {
                _context.Update(transaction);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        PopulateCategories();
        return View(transaction);
    }


    // POST: Transacton/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Transactions == null) return Problem("Entity set 'ExpenseDbContext.Transactions'  is null.");
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null) _context.Transactions.Remove(transaction);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [NonAction]
    public void PopulateCategories()
    {
        var CategoryCollection = _context.Categories.ToList();
        Category defaultCategory = new Category()
        {
            CategoryId = 0, Title = "Choose a Category"
        };
            CategoryCollection.Insert(0,defaultCategory);
            ViewBag.Categories = CategoryCollection;
    }
}