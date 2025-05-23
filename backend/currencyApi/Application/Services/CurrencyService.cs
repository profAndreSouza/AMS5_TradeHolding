using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyAPI.Application.Interfaces;
using CurrencyAPI.Domain.Entities;
using CurrencyAPI.Domain.Interfaces;

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
    }
}
