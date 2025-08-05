using Discord.Interactions;
using PrintServer.Models;
using PrintServer.Queue;

namespace DiscordBot.Modules;

public class PrintModule(IPrinterQueueService queueService) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("print", "Prints a todo note to the thermal printer")]
    public async Task Print(
        [Summary("message", "The content of the note")] string message,
        [Summary("project", "Optional project name")] string? project = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            await RespondAsync("Message can't be empty.", ephemeral: true);
            return;
        }

        var job = new PrintJob
        {
            Message = message,
            Sender = Context.User.Username,
            Project = project,
            CreatedAt = DateTime.UtcNow
        };

        await queueService.EnqueueAsync(job);

        await RespondAsync($"Queued: \"{message}\"", ephemeral: true);
    }
}