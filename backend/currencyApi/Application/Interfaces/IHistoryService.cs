using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyAPI.Domain.Entities;

namespace CurrencyAPI.Application.Interfaces
{
    public interface IHistoryService
    {
        Task<IEnumerable<History>> GetByCurrencyIdAsync(Guid currencyId);
        Task<IEnumerable<History>> GetByDateRangeAsync(Guid currencyId, DateTime from, DateTime to);
        Task AddAsync(History history);
        Task DeleteAsync(Guid id);
    }
}
