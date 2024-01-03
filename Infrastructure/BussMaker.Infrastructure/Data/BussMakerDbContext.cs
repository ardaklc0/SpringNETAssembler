using BussMaker.Entities;
using Microsoft.EntityFrameworkCore;

namespace BussMaker.Infrastructure.Data
{
    public class BussMakerDbContext : DbContext
    {
        public DbSet<Entity> Entities { get; set; }
        public BussMakerDbContext(DbContextOptions<BussMakerDbContext> options) : base(options)
        {
        }
    }
}
