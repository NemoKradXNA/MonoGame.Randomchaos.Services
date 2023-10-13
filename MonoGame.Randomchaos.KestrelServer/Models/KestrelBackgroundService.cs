
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace MonoGame.Randomchaos.KestrelServer.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A service for accessing kestrel backgrounds information. </summary>
    ///
    /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class KestrelBackgroundService : BackgroundService
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the web host server. </summary>
        ///
        /// <value> The web host server. </value>
        ///-------------------------------------------------------------------------------------------------

        protected KestrelServer _webHostServer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the configuration. </summary>
        ///
        /// <value> The configuration. </value>
        ///-------------------------------------------------------------------------------------------------

        protected readonly IConfiguration _configuration;
        /// <summary>   (Immutable) the logger. </summary>
        protected readonly ILogger<KestrelBackgroundService> _logger;

        public static KestrelConfigurationOptions Options { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <param name="configuration">    The configuration. </param>
        /// <param name="logger">           The logger. </param>
        ///-------------------------------------------------------------------------------------------------

        public KestrelBackgroundService(IConfiguration configuration, ILogger<KestrelBackgroundService> logger, IOptions<KestrelConfigurationOptions> options)
        {
            _configuration = configuration;
            _logger = logger;
            Options = options.Value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Triggered when the application host is ready to start the service. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <param name="cancellationToken">    Indicates that the start process has been aborted. </param>
        ///
        /// <returns>   A Task. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Kestrel Background Service");
            await base.StartAsync(cancellationToken);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" />
        /// starts. The implementation should return a task that represents the lifetime of the long
        /// running operation(s) being performed.
        /// </summary>
        ///
        /// <remarks>
        /// See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in
        /// .NET</see> for implementation guidelines.
        /// </remarks>
        ///
        /// <param name="stoppingToken">
        ///     Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" />
        ///     is called.
        /// </param>
        ///
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Executing Kestrel Background Service");

            var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddConsole());

            var serverLogger = loggerFactory.CreateLogger<KestrelServer>();
            _webHostServer = new KestrelServer(_configuration, serverLogger);

            await _webHostServer.StartAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(100);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Triggered when the application host is performing a graceful shutdown. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <param name="cancellationToken">
        ///     Indicates that the shutdown process should no longer be graceful.
        /// </param>
        ///
        /// <returns>   A Task. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Kestrel Background Service");

            await _webHostServer.StopAsync(cancellationToken);

            await base.StopAsync(cancellationToken);
        }
    }
}
