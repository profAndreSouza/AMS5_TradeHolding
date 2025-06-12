using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CurrencyAPI.Domain.Entities;
using CurrencyAPI.Domain.Interfaces;
using CurrencyAPI.Infrastructure.Data;
using CurrencyAPI.API.DTOs;

namespace CurrencyAPI.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly AppDbContext _context;

        public CurrencyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Currency?> GetByIdAsync(Guid id)
        {
            return await _context.Currencies
                .Include(c => c.Histories)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Currency?> GetBySymbolAsync(string symbol)
        {
            return await _context.Currencies
                .Include(c => c.Histories)
                .FirstOrDefaultAsync(c => c.Symbol == symbol.ToUpper());
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _context.Currencies
                .Include(c => c.Histories)
                .ToListAsync();
        }

        public async Task AddAsync(Currency currency)
        {
            await _context.Currencies.AddAsync(currency);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Currency currency)
        {
            _context.Currencies.Update(currency);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency != null)
            {
                _context.Currencies.Remove(currency);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Currencies.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<History>> GetHistoryAsync(Guid currencyId, DateTime? start, DateTime? end)
        {
            var query = _context.Histories
                .Where(h => h.CurrencyId == currencyId);

            if (start.HasValue)
                query = query.Where(h => h.Date >= start.Value);

            if (end.HasValue)
                query = query.Where(h => h.Date <= end.Value);

            return await query.OrderBy(h => h.Date).ToListAsync();
        }

        public async Task<CurrencyWithLastPriceDto?> GetLastPriceBySymbolAsync(string symbol)
        {
            var currency = await _context.Currencies
                .Where(c => c.Symbol == symbol.ToUpper())
                .Select(c => new CurrencyWithLastPriceDto
                {
                    Id = c.Id,
                    Symbol = c.Symbol,
                    Name = c.Name,
                    Backing = c.Backing,
                    Reverse = c.Reverse,
                    LastPrice = c.Histories
                        .OrderByDescending(h => h.Date)
                        .Select(h => h.Price)
                        .FirstOrDefault(),
                    LastPriceDate = c.Histories
                        .OrderByDescending(h => h.Date)
                        .Select(h => h.Date)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            return currency;
        }

        public async Task<IEnumerable<CurrencySummaryDto>> GetCurrencySummariesAsync()
        {
            var currencies = await _context.Currencies.ToListAsync();
            var summaries = new List<CurrencySummaryDto>();
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            foreach (var currency in currencies)
            {
                var lastPrice = await _context.Histories
                    .Where(h => h.CurrencyId == currency.Id)
                    .OrderByDescending(h => h.Date)
                    .Select(h => h.Price)
                    .FirstOrDefaultAsync();

                if (lastPrice == 0) continue;

                // Tenta pegar a cotação de 30 dias atrás
                var oldPrice = await _context.Histories
                    .Where(h => h.CurrencyId == currency.Id && h.Date <= thirtyDaysAgo)
                    .OrderByDescending(h => h.Date)
                    .Select(h => h.Price)
                    .FirstOrDefaultAsync();

                // Se não encontrou, pega a mais antiga disponível
                if (oldPrice == 0)
                {
                    oldPrice = await _context.Histories
                        .Where(h => h.CurrencyId == currency.Id)
                        .OrderBy(h => h.Date)
                        .Select(h => h.Price)
                        .FirstOrDefaultAsync();
                }

                if (oldPrice == 0) continue;

                decimal change = ((lastPrice - oldPrice) / oldPrice) * 100;

                summaries.Add(new CurrencySummaryDto
                {
                    Id = currency.Id,
                    Symbol = currency.Symbol,
                    Name = currency.Name,
                    Price = currency.Reverse ? Math.Round(1 / lastPrice, 4) : Math.Round(lastPrice, 4),
                    Change = Math.Round(change, 2)
                });
            }

            return summaries;
        }

        public async Task<IEnumerable<ChartPointDto>> GetChartDataAsync(Guid currencyId, int quantity)
        {
            return await _context.Histories
                .Where(h => h.CurrencyId == currencyId)
                .OrderByDescending(h => h.Date)
                .Take(quantity)
                .OrderBy(h => h.Date) // para exibir no gráfico da esquerda para direita
                .Select(h => new ChartPointDto
                {
                    Time = h.Date.ToUniversalTime().ToString("o"), // ajusta para UTC-3
                    Value = Math.Round(h.Price, 4)
                })
                .ToListAsync();
        }


    }
    
}
