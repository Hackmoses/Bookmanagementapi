using Bookmanagementapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookmanagementapi.data 
{
    public class AppDbContext : IdentityDbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}

    public DbSet<BookList> BookLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
      modelBuilder.Entity<BookList>()
      .HasIndex(u => u.Book)
      .IsUnique(true);

      base.OnModelCreating(modelBuilder);
    }

   
  }

}