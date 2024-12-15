using Microsoft.EntityFrameworkCore;

namespace test.Models
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

        public DbSet<ComCustomer> ComCustomers { get; set; } = null!;
        public DbSet<SoOrder> SoOrders { get; set; } = null!;
        public DbSet<SoItem> SoItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComCustomer>().ToTable("COM_CUSTOMER");
            modelBuilder.Entity<SoOrder>().ToTable("SO_ORDER");
            modelBuilder.Entity<SoItem>().ToTable("SO_ITEM");
        }
    }
}
