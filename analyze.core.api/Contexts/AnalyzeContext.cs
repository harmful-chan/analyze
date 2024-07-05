
using analyze.core.Models.Daily;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace analyze.core.api.Contexts
{
    public class AnalyzeContext : DbContext
    {
        public AnalyzeContext(DbContextOptions<AnalyzeContext> options) : base(options)
        {
        }

        public DbSet<DailyDetail> DailyDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyDetail>().ToTable("DailyDetail");
        }
    }
}
