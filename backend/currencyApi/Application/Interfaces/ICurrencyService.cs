using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyAPI.Domain.Entities;

namespace CurrencyAPI.Application.Interfaces
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetAllAsync();
        Task<Currency?> GetByIdAsync(Guid id);
        Task<Currency?> GetBySymbolAsync(string symbol);
        Task AddAsync(Currency currency);
        Task UpdateAsync(Currency currency);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<History>> GetHistoryAsync(Guid currencyId, DateTime? start, DateTime? end);

    }
}
