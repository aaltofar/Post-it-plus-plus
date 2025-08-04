using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Configuration;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>();
    })
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<BotSettings>(ctx.Configuration);
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<InteractionService>();
        services.AddSingleton<BotService>();
        // services.AddSingleton<PrinterService>(); 

    })
    .Build()
    .Services
    .GetRequiredService<BotService>()
    .RunAsync()
    .GetAwaiter()
    .GetResult();