using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Configuration;
using Microsoft.Extensions.Options;

namespace DiscordBot.Services;

public class BotService
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly BotSettings _settings;
    private readonly IServiceProvider _services;

    public BotService(
        DiscordSocketClient client,
        InteractionService interactionService,
        IOptions<BotSettings> settings,
        IServiceProvider services)
    {
        _client = client;
        _interactionService = interactionService;
        _settings = settings.Value;
        _services = services;

        _client.Ready += OnReady;
        _client.InteractionCreated += HandleInteraction;
        _client.Log += Log;
        _interactionService.Log += Log;
    }

    public async Task RunAsync()
    {
        await _client.LoginAsync(TokenType.Bot, _settings.Token);
        await _client.StartAsync();
        await Task.Delay(Timeout.Infinite);
    }

    private async Task OnReady()
    {
        Console.WriteLine("Bot is connected and ready.");

        await _interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);

        if (ulong.TryParse(_settings.GuildId, out var guildId))
        {
            await _interactionService.RegisterCommandsToGuildAsync(guildId);
            Console.WriteLine($"Registered commands to guild {guildId}");
        }
        else
        {
            Console.WriteLine("Invalid GuildId in configuration.");
        }
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(context, _services);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] {ex.Message}");
            if (interaction.Type == InteractionType.ApplicationCommand)
            {
                await interaction.RespondAsync("Something went wrong while processing your command.");
            }
        }
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine($"[Log] {msg}");
        return Task.CompletedTask;
    }
}