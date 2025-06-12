using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyAPI.Application.Interfaces;
using CurrencyAPI.Domain.Entities;
using CurrencyAPI.Domain.Interfaces;
using CurrencyAPI.API.DTOs;

namespace CurrencyAPI.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _repository;

        public CurrencyService(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _repository.GetAllAsync();

        }

        public async Task<Currency?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Currency?> GetBySymbolAsync(string symbol)
        {
            return await _repository.GetBySymbolAsync(symbol);
        }

        public async Task AddAsync(Currency currency)
        {
            await _repository.AddAsync(currency);
        }

        public async Task UpdateAsync(Currency currency)
        {
            await _repository.UpdateAsync(currency);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<History>> GetHistoryAsync(Guid currencyId, DateTime? start, DateTime? end)
        {
            return await _repository.GetHistoryAsync(currencyId, start, end);
        }

        public async Task<CurrencyWithLastPriceDto?> GetLastPriceBySymbolAsync(string symbol)
        {
            return await _repository.GetLastPriceBySymbolAsync(symbol);
        }

        public async Task<IEnumerable<CurrencySummaryDto>> GetCurrencySummariesAsync()
        {
            return await _repository.GetCurrencySummariesAsync();
        }

        public async Task<IEnumerable<ChartPointDto>> GetChartDataAsync(Guid currencyId, int quantity)
        {
            return await _repository.GetChartDataAsync(currencyId, quantity);
        }



    }
}
