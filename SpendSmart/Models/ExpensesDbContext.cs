using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SpendSmart.Models
{
    public class ExpensesDbContext : IdentityDbContext<IdentityUser>
    {
        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    // Explicitly configure ApplicationUser if needed
        //    builder.Entity<IdentityUser>(b =>
        //    {
        //        b.Property(u => u.FirstName).HasMaxLength(100);
        //    });
        //}
    }
}