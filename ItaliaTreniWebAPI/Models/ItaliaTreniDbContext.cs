using ItaliaTreniSharedLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ItaliaTreniWebAPI.Models
{
    public class ItaliaTreniDbContext : DbContext
    {
        public ItaliaTreniDbContext(DbContextOptions<ItaliaTreniDbContext> options) : base(options) { }

        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Defect> Defects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Db Context Configuration
            base.OnModelCreating(modelBuilder);
        }
    }
}