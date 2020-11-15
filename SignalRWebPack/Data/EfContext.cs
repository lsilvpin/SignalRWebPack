using Microsoft.EntityFrameworkCore;
using SignalRWebPack.Models;

namespace SignalRWebPack.Data
{
    public class EfContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public EfContext(DbContextOptions<EfContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
