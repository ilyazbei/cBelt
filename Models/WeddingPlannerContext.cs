using Microsoft.EntityFrameworkCore;
 
namespace WeddingPlanner.Models
{
    public class WeddingPlannerContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public WeddingPlannerContext(DbContextOptions<WeddingPlannerContext> options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Weddings> Weddings { get; set; }
        public DbSet<RSVPs> RSVPs { get; set; }
    }
}