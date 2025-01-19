using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Models;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
}