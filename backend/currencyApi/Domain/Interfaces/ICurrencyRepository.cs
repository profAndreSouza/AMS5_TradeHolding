using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyAPI.Domain.Entities;
using CurrencyAPI.API.DTOs;

namespace CurrencyAPI.Domain.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<Currency?> GetByIdAsync(Guid id);
        Task<Currency?> GetBySymbolAsync(string symbol);
        Task<IEnumerable<Currency>> GetAllAsync();
        Task AddAsync(Currency currency);
        Task UpdateAsync(Currency currency);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<History>> GetHistoryAsync(Guid currencyId, DateTime? start, DateTime? end);
        Task<CurrencyWithLastPriceDto?> GetLastPriceBySymbolAsync(string symbol);


    }
}
