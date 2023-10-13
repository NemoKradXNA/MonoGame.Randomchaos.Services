using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace MonoGame.Randomchaos.KestrelServer.Models
{
    public class KestrelServer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the web host server. </summary>
        ///
        /// <value> The web host server. </value>
        ///-------------------------------------------------------------------------------------------------

        protected IWebHost _webHostServer { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the server status. </summary>
        ///
        /// <value> The server status. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ServerStatus { get; protected set; } = "Initialized";

        /// <summary>   (Immutable) the logger. </summary>
        protected readonly ILogger<KestrelServer> _logger;
        /// <summary>   (Immutable) the configuration. </summary>
        protected readonly IConfiguration _configuration;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <param name="config">   The configuration. </param>
        /// <param name="logger">   The logger. </param>
        ///-------------------------------------------------------------------------------------------------

        public KestrelServer(IConfiguration config, ILogger<KestrelServer> logger)
        {
            _configuration = config; 
            _logger = logger;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Prepare web server. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void PrepareWebServer()
        {
            try
            {
                int extrenalPort = _configuration.GetValue<int>("KestrelServer:ExternalPort");
                int port = _configuration.GetValue<int>("KestrelServer:LocalPort");

                _webHostServer = WebHost.CreateDefaultBuilder()
                    .UseKestrel(opts =>
                    {
                        try
                        {
                            // Configure HTTPS protocols
                            if (_configuration.GetValue<bool>("KestrelServer:EnableHttps"))
                            {
                                int extrenalPortSSL = _configuration.GetValue<int>("KestrelServer:ExternalPortSSL");
                                int portSSL = _configuration.GetValue<int>("KestrelServer:LocalPortSSL");

                                string cert = _configuration["KestrelServer:SSLCertLocation"];
                                string secret = _configuration["KestrelServer:SSLCertPassword"];

                                if (string.IsNullOrEmpty(cert) || string.IsNullOrEmpty(secret))
                                {
                                    _logger.LogWarning("No certificate or password configured.");

                                    if (_configuration.GetValue<bool>("KestrelServer:AllowExternalIPs"))
                                    {
                                        opts.ListenAnyIP(extrenalPortSSL, opt => { opt.UseHttps(); });
                                    }

                                    opts.ListenLocalhost(portSSL, opt => { opt.UseHttps(); });
                                }
                                else
                                {
                                    if (_configuration.GetValue<bool>("KestrelServer:AllowExternalIPs"))
                                    {
                                        opts.ListenAnyIP(extrenalPortSSL, opt => { opt.UseHttps(cert, secret); });
                                    }

                                    opts.ListenLocalhost(portSSL, opt => { opt.UseHttps(cert, secret); });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "HTTPS configuration error...");
                        }

                        // Configure HTTP protocols.
                        if (_configuration.GetValue<bool>("KestrelServer:AllowExternalIPs"))
                            opts.ListenAnyIP(extrenalPort);

                        opts.ListenLocalhost(port);
                    })                    
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<KestrelStartup>().UseDefaultServiceProvider((b, o) => { }).UseConfiguration(_configuration)
                    .Build();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in PrepareWebService");

                throw new Exception("Exception in PrepareWebService", ex);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts an asynchronous. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="cancellationToken">    A token that allows processing to be cancelled. </param>
        ///
        /// <returns>   A Task. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            OnStarting();

            _logger.LogInformation("StartAsync has been called.");

            PrepareWebServer();

            Exception webEx = null;
            try
            {
                _ = Task.Run(() =>
                {
                    Task t = _webHostServer.RunAsync();

                    if (t.Status != TaskStatus.Faulted)
                        ServerStatus = "Running";
                    else
                    {
                        webEx = t.Exception;
                        ServerStatus = "Failed to start...";
                    }

                    if (webEx != null)
                    {
                        throw webEx;
                    }
                });

                OnStarted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in StartWebServer");

                throw new Exception("Exception in StartWebServer", ex);
            }
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stops an asynchronous. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///
        /// <param name="cancellationToken">    A token that allows processing to be cancelled. </param>
        ///
        /// <returns>   A Task. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            OnStopping();

            _logger.LogInformation("StopAsync has been called.");

            if (this._webHostServer != null)
            {
                OnShuttingDown();
                await _webHostServer.StopAsync();
            }

            Thread.Sleep(3000);

            OnStopped();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the 'shutting down' action. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnShuttingDown()
        {
            ServerStatus = "Shutting down";
            _logger.LogInformation("OnShuttingDown has been called.");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the 'starting' action. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnStarting()
        {
            ServerStatus = "Starting";
            _logger.LogInformation("OnStarting has been called.");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the 'started' action. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnStarted()
        {
            ServerStatus = "Started";
            _logger.LogInformation("OnStarted has been called.");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the 'stopping' action. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnStopping()
        {
            ServerStatus = "Stopping";
            _logger.LogInformation("OnStopping has been called.");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the 'stopped' action. </summary>
        ///
        /// <remarks>   Charles Humphrey, 13/10/2023. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void OnStopped()
        {
            ServerStatus = "Stopped";
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
