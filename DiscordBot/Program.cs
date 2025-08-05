using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Configuration;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintServer.Queue;
SQLitePCL.Batteries_V2.Init();

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddUserSecrets<Program>(); // Can add .AddJsonFile if you're using appsettings too, or replaced
    })
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<BotSettings>(ctx.Configuration.GetSection("Bot"));

        // Discord core services
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<InteractionService>();
        services.AddSingleton<BotService>();
        services.AddSingleton<IPrinterQueueService, PrinterQueueService>();
    })
    .Build()
    .Services
    .GetRequiredService<BotService>()
    .RunAsync()
    .GetAwaiter()
    .GetResult();