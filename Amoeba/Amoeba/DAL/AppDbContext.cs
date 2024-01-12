using Amoeba.Models;
using Microsoft.EntityFrameworkCore;

namespace Amoeba.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Settings> Settings { get; set; }
    }
}
