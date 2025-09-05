using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using CurrencyAPI.Application.Interfaces;
using CurrencyAPI.API.DTOs;
using CurrencyAPI.Application.Mappers;


namespace CurrencyAPI.Infrastructure.Services
{
    public class ExternalApiWorker : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _services;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);
        private readonly string _cryptoPricesUrl;

        public ExternalApiWorker(IHttpClientFactory httpClientFactory, IConfiguration configuration, IServiceProvider services)
        {
            _httpClientFactory = httpClientFactory;
            _services = services;
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
                    using var scope = _services.CreateScope();
                    var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
                    var historyService = scope.ServiceProvider.GetRequiredService<IHistoryService>();

                    var currencies = await currencyService.GetAllAsync();


                    foreach (var currency in currencies)
                    {
                        try
                        {
                            if (currency.Symbol != currency.Backing)
                            {
                                var currencyUrl = "";
                                if (currency.Reverse)
                                {
                                    currencyUrl = $"{_cryptoPricesUrl}?symbol={currency.Backing}{currency.Symbol}";
                                }
                                else
                                {
                                    currencyUrl = $"{_cryptoPricesUrl}?symbol={currency.Symbol}{currency.Backing}";
                                }
                                // Console.WriteLine($"Consultando moeda {currency.Symbol} - URL: {currencyUrl}");

                                var response = await client.GetAsync(currencyUrl, stoppingToken);

                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync(stoppingToken);
                                    var apiResponse = JsonSerializer.Deserialize<CryptoApiResponseDto>(content);

                                    var historyDto = new HistoryDto
                                    {
                                        CurrencyId = currency.Id,
                                        Price = decimal.Parse(apiResponse.Price),
                                        Date = DateTime.UtcNow
                                    };

                                    await historyService.AddAsync(historyDto.ToEntity());

                                    // Console.WriteLine($"[{DateTime.Now}] Preços de Cripto: {JsonSerializer.Serialize(historyDto)}");
                                }
                                else
                                {
                                    Console.WriteLine($"Erro ao consultar API: {response.StatusCode}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro durante a requisição: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                     Console.WriteLine($"Erro no worker de consulta à API externa {ex.Message}");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            Console.WriteLine("Serviço de consulta de criptomoedas finalizado.");
        }
    }
}
