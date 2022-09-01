using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Data
{
    public class CompensationContext : DbContext
    {
        public CompensationContext(DbContextOptions<CompensationContext> options) : base(options)
        {

        }

        public DbSet<Compensation> Compensation { get; set; }

        /// <summary>
        /// Required to defined the primary key for compensation. 
        /// Otherwise, error is thrown
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Compensation>().HasKey(e => e.EmployeeId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
