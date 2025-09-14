using RabbitMQ.Client;

namespace SharedKernel.Infrastructure;

public class RabbitMqHealthChecker
{
    private readonly string _connectionString;

    public RabbitMqHealthChecker(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> IsAvailableAsync()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_connectionString),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(2) // timeout nội bộ
            };

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)); // ngắt cứng
            await using var connection = await factory.CreateConnectionAsync("health-check", cts.Token);

            return connection.IsOpen;
        }
        catch
        {
            return false;
        }
    }
}
