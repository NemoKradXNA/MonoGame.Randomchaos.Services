
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.MonoGame.Randomchaos.KestrelServer;
using MonoGame.Randomchaos.KestrelServer.Models;

using var game = new Sample.MonoGame.Randomchaos.KestrelServer.Game1();

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<KestrelBackgroundService>();
        services.Configure <KestrelConfigurationOptions>(options => options.GameInstance = game );
    }).Build().RunAsync();


game.Run();



