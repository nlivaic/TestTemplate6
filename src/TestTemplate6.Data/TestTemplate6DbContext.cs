using MassTransit;
using Microsoft.EntityFrameworkCore;
using TestTemplate6.Core.Entities;

namespace TestTemplate6.Data
{
    public class TestTemplate6DbContext : DbContext
    {
        public TestTemplate6DbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Foo> Foos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
