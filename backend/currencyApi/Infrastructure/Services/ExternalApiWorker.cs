using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace CurrencyAPI.Infrastructure.Services
{
    public class ExternalApiWorker : BackgroundService
    {
        private const int IntervalSeconds = 10;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _cryptoPricesUrl;

        public ExternalApiWorker(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cryptoPricesUrl = configuration["ExternalApi:CryptoPricesUrl"];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Serviço de consulta de criptomoedas iniciado.");

            var client = _httpClientFactory.CreateClient();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var currencyUrl = _cryptoPricesUrl + "?symbol=ETHBTC";
                    var response = await client.GetAsync(currencyUrl, stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync(stoppingToken);
                        Console.WriteLine($"[{DateTime.Now}] Preços de Cripto: {content}");
                    }
                    else
                    {
                        Console.WriteLine($"Erro ao consultar API: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro durante a requisição: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(IntervalSeconds), stoppingToken);
            }

            Console.WriteLine("Serviço de consulta de criptomoedas finalizado.");
        }
    }
}
