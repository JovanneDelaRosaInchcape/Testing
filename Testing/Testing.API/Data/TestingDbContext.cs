using Microsoft.EntityFrameworkCore;
using Testing.API.Models.Domain;

namespace Testing.API.Data
{
    public class TestingDbContext : DbContext
    {
        public TestingDbContext(DbContextOptions<TestingDbContext> option): base(option)
        {

        }

        public DbSet<Region> Region { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
    }
}
