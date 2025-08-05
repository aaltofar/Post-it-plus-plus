using Microsoft.Data.Sqlite;
using PrintServer.Models;

namespace PrintServer.Queue;

public class PrinterQueueService : IPrinterQueueService
{
    private readonly string _dbPath = "printjobs.db";

    public PrinterQueueService()
    {
        EnsureDatabase();
    }

    private void EnsureDatabase()
    {
        using var conn = new SqliteConnection($"Data Source={_dbPath}");
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS PrintJobs (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Message TEXT NOT NULL,
                Sender TEXT NOT NULL,
                Project TEXT,
                CreatedAt TEXT NOT NULL,
                PrintedAt TEXT
            );
        """;
        cmd.ExecuteNonQuery();
    }

    public async Task EnqueueAsync(PrintJob job)
    {
        using var conn = new SqliteConnection($"Data Source={_dbPath}");
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO PrintJobs (Message, Sender, Project, CreatedAt)
            VALUES ($msg, $sender, $project, $createdAt);
        """;
        cmd.Parameters.AddWithValue("$msg", job.Message);
        cmd.Parameters.AddWithValue("$sender", job.Sender);
        cmd.Parameters.AddWithValue("$project", (object?)job.Project ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$createdAt", job.CreatedAt.ToString("o"));

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<PrintJob>> GetPendingAsync()
    {
        var jobs = new List<PrintJob>();

        await using var conn = new SqliteConnection($"Data Source={_dbPath}");
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM PrintJobs WHERE PrintedAt IS NULL ORDER BY CreatedAt ASC";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            jobs.Add(new PrintJob
            {
                Id = reader.GetInt32(0),
                Message = reader.GetString(1),
                Sender = reader.GetString(2),
                Project = reader.IsDBNull(3) ? null : reader.GetString(3),
                CreatedAt = DateTime.Parse(reader.GetString(4)),
                PrintedAt = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5))
            });
        }

        return jobs;
    }

    public async Task MarkAsPrintedAsync(int id)
    {
        await using var conn = new SqliteConnection($"Data Source={_dbPath}");
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = """
            UPDATE PrintJobs
            SET PrintedAt = $now
            WHERE Id = $id;
        """;
        cmd.Parameters.AddWithValue("$now", DateTime.UtcNow.ToString("o"));
        cmd.Parameters.AddWithValue("$id", id);

        await cmd.ExecuteNonQueryAsync();
    }
}