namespace PrintServer.Models;

public class PrintJob
{
    public int Id { get; set; }
    public string Message { get; set; } = "";
    public string Sender { get; set; } = "";
    public string? Project { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PrintedAt { get; set; }
}