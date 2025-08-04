namespace DiscordBot.Configuration;

/// <summary>
/// Configuration settings for the Discord bot.
/// Populated from user secrets or appsettings.json.
/// </summary>
public class BotSettings
{
    /// <summary>
    /// The bot token used to authenticate with Discord.
    /// Should be kept secret. Loaded from user secrets in dev.
    /// </summary>
    public string Token { get; set; } = "";
    
    /// <summary>
    /// The Discord Guild (server) ID where the bot should register slash commands.
    /// Used to scope commands during development.
    /// </summary>
    public string GuildId { get; set; } = "";
}