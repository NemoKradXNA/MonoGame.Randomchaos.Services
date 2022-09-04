using Microsoft.EntityFrameworkCore;

namespace MonoGame.Randomchaos.EFCore.DbContextBase
{
    public abstract class SQLLightDbContextBase :  DbContext
    {
        public readonly string ConnectionString;

        public SQLLightDbContextBase() : base() { ConnectionString = string.Empty; }

        public SQLLightDbContextBase(string connectionString) : base()
        {
            ConnectionString = connectionString;
        }

        public SQLLightDbContextBase(string connectionString, DbContextOptions options) : base(options)
        {
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString, options => options.CommandTimeout(360));
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

    }
}
