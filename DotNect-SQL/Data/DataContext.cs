
using DotNect_SQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNect_SQL.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<People> People { get; set; }
        public DbSet<BorrowHistory> BorrowHistories { get; set; }
    }
}
