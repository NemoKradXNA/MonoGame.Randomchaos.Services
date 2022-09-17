using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Samples.MonoGame.Randomchaos.EFCore.DbContext;
using System;

namespace Samples.MonoGame.Randomchaos.EFCore.Factories
{
    public class SampleDbContextFactory : IDesignTimeDbContextFactory<SampleDbContext>
    {
        public SampleDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("Params:-");

            for (int x = 0; x < args.Length; x++)
                Console.WriteLine($"[{x}] - {args[x]}");

            string connectionString = args[0];

            var options = new DbContextOptionsBuilder<SampleDbContext>();

            return new SampleDbContext(connectionString, options.Options);
        }
    }
}
