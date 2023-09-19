
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Samples.MonoGame.Randomchaos.EFCore.DbContext;
using System;

namespace Samples.MonoGame.Randomchaos.EFCore.Factories
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sample database context factory. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SampleDbContextFactory : IDesignTimeDbContextFactory<SampleDbContext>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates a new instance of a derived context. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="args"> Arguments provided by the design-time service. </param>
        ///
        /// <returns>   An instance of <typeparamref name="TContext" />. </returns>
        ///-------------------------------------------------------------------------------------------------

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
