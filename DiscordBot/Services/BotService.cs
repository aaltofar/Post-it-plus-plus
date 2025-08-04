using DiscordBot.Configuration;
using Microsoft.Extensions.Options;

namespace DiscordBot.Services;

public class BotService(IOptions<BotSettings> options)
{
    private readonly BotSettings _settings = options.Value;

    public Task RunAsync()
    {
        var token = _settings.Token;
        var guildId = ulong.Parse(_settings.GuildId);
        return Task.CompletedTask;

        // Login, start, etc.
    }
}