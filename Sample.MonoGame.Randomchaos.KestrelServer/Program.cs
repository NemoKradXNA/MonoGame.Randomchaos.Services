
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonoGame.Randomchaos.KestrelServer.Models;

using var game = new Sample.MonoGame.Randomchaos.KestrelServer.Game1();

_ = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<KestrelBackgroundService>();
        services.Configure <KestrelConfigurationOptions>(options => options.GameInstance = game );
    }).Build().RunAsync();


game.Run();



