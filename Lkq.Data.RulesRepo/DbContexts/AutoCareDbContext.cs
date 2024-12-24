using Microsoft.EntityFrameworkCore;
using Lkq.Domain.RulesRepo;

namespace Lkq.Data.RulesRepo.DbContexts
{
    public class AutoCareDbContext : DbContext
    {
        public DbSet<AttributeValues> AttributeValues { get; set; }

        public AutoCareDbContext(DbContextOptions<AutoCareDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}