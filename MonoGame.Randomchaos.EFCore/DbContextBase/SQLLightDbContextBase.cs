
using Microsoft.EntityFrameworkCore;

namespace MonoGame.Randomchaos.EFCore.DbContextBase
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A SQL light database context base. </summary>
    ///
    /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class SQLLightDbContextBase :  DbContext
    {
        /// <summary>   (Immutable) the connection string. </summary>
        public readonly string ConnectionString;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public SQLLightDbContextBase() : base() { ConnectionString = string.Empty; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        ///-------------------------------------------------------------------------------------------------

        public SQLLightDbContextBase(string connectionString) : base()
        {
            ConnectionString = connectionString;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 19/09/2023. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        /// <param name="options">          Options for controlling the operation. </param>
        ///-------------------------------------------------------------------------------------------------

        public SQLLightDbContextBase(string connectionString, DbContextOptions options) : base(options)
        {
            ConnectionString = connectionString;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Override this method to configure the database (and other options) to be used for this
        /// context. This method is called for each instance of the context that is created. The base
        /// implementation does nothing.
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        ///                 In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" />
        ///                 may or may not have been passed to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" />
        ///                 to determine if the options have already been set, and skip some or all of
        ///                 the logic in
        ///                 <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        ///                 
        ///             </para>
        /// <para>
        ///                 See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime,
        ///                 configuration, and initialization</see>
        ///                 for more information and examples.
        ///             </para>
        /// </remarks>
        ///
        /// <param name="optionsBuilder">
        ///     A builder used to create or modify options for this context. Databases (and other
        ///     extensions)
        ///     typically define extension methods on this object that allow you to configure the context.
        /// </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString, options => options.CommandTimeout(360));
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

    }
}
