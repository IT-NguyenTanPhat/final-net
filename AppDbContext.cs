using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using timviec.Models;

namespace timviec
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) { }

        public DbSet<Category> categories { get; set; }

        public DbSet<Job> jobs { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Apply> applies { get; set; }
    }
}
