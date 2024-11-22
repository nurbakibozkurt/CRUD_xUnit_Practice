using CRUD_Practice.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Practice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
    }
}
