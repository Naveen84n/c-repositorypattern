using Microsoft.EntityFrameworkCore;
using Lkq.Domain.RulesRepo;

namespace Lkq.Data.RulesRepo.DbContexts
{
    public class PartsDbContext : DbContext
    {
        //Parts
        public DbSet<PartTypes> PartTypes { get; set; }
        public DbSet<Attributes> RulesAttributeTables { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<PartRules> PartRules { get; set; }

        public PartsDbContext(DbContextOptions<PartsDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartTypes>().ToTable("tbl_Part_Codes", "Parts").HasKey(x => x.Part_Codes_ID);
            modelBuilder.Entity<Attributes>().ToTable("tbl_ACES_Attributes", "Parts").HasKey(x => x.ACES_Attributes_ID);
            modelBuilder.Entity<PartRules>().ToTable("tbl_Part_Rules", "Parts").HasKey(x => x.Part_Rules_ID);
            modelBuilder.Entity<DataSource>().ToTable("tbl_Vindecoder_Source", "Parts").HasKey(x => x.Vindecoder_Source_ID);
               
        }
    }
}
