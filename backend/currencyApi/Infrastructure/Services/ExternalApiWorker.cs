namespace CurrencyAPI.Infrastructure.Services
{
    public class ExternalApiWorker : BackgroundService
    {
        private const int IntervalSeconds = 5;
        private readonly ILogger<ExternalApiWorker> _logger;

        public ExternalApiWorker(ILogger<ExternalApiWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço de impressão iniciado.");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Oi");
                _logger.LogInformation("Impressão 'Oi' executada.");
                
                await Task.Delay(TimeSpan.FromSeconds(IntervalSeconds), stoppingToken);
            }
            
            _logger.LogInformation("Serviço de impressão finalizado.");
        }
    }
}