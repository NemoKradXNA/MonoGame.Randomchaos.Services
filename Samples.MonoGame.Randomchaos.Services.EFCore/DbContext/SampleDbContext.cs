using Microsoft.EntityFrameworkCore;
using MonoGame.Randomchaos.EFCore.DbContextBase;
using Samples.MonoGame.Randomchaos.EFCore.Models;

namespace Samples.MonoGame.Randomchaos.EFCore.DbContext
{
    public  class SampleDbContext : SQLLightDbContextBase
    {
        public DbSet<TestDataClass> TestDataClass { get; set; }

        public SampleDbContext() : base() { }

        public SampleDbContext(string connectionString) : base(connectionString) { }

        public SampleDbContext(string connectionString, DbContextOptions<SampleDbContext> options) : base(connectionString, options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestDataClass>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
        }
    }
}
