
using Microsoft.EntityFrameworkCore;
using MonoGame.Randomchaos.EFCore.DbContextBase;
using Samples.MonoGame.Randomchaos.EFCore.Models;

namespace Samples.MonoGame.Randomchaos.EFCore.DbContext
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sample database context. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public  class SampleDbContext : SQLLightDbContextBase
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the test data class. </summary>
        ///
        /// <value> The test data class. </value>
        ///-------------------------------------------------------------------------------------------------

        public DbSet<TestDataClass> TestDataClass { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public SampleDbContext() : base() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        ///-------------------------------------------------------------------------------------------------

        public SampleDbContext(string connectionString) : base(connectionString) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="options">          Options for controlling the operation. </param>
        ///-------------------------------------------------------------------------------------------------

        public SampleDbContext(string connectionString, DbContextOptions<SampleDbContext> options) : base(connectionString, options) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from
        /// the entity types exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties
        /// on your derived context. The resulting model may be cached and re-used for subsequent
        /// instances of your derived context.
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        ///                 If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />
        ///                 )
        ///                 then this method will not be run. However, it will still run when creating a
        ///                 compiled model.
        ///             </para>
        /// <para>
        ///                 See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and
        ///                 relationships</see> for more information and examples.
        ///             </para>
        /// </remarks>
        ///
        /// <param name="modelBuilder">
        ///     The builder being used to construct the model for this context. Databases (and other
        ///     extensions) typically define extension methods on this object that allow you to configure
        ///     aspects of the model that are specific to a given database.
        /// </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestDataClass>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
        }
    }
}
