using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CurrencyAPI.Application.Interfaces;
using CurrencyAPI.Application.Services;
using CurrencyAPI.Domain.Interfaces;
using CurrencyAPI.Infrastructure.Repositories;
using CurrencyAPI.Infrastructure.Data;

namespace CurrencyAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application Layer
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IHistoryService, HistoryService>();

            // Infrastructure Layer
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();

            // EF Core DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=Infrastructure/Data/currencydb.sqlite"));

            return services;
        }
    }
}
