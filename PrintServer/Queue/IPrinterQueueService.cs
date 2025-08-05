using PrintServer.Models;

namespace PrintServer.Queue;

public interface IPrinterQueueService
{
    Task EnqueueAsync(PrintJob job);
    Task<List<PrintJob>> GetPendingAsync();
    Task MarkAsPrintedAsync(int id);
}