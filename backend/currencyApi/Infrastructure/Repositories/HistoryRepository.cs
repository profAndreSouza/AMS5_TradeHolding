using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CurrencyAPI.Domain.Entities;
using CurrencyAPI.Domain.Interfaces;
using CurrencyAPI.Infrastructure.Data;

namespace CurrencyAPI.Infrastructure.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly AppDbContext _context;

        public HistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<History?> GetByIdAsync(Guid id)
        {
            return await _context.Histories.FindAsync(id);
        }

        public async Task<IEnumerable<History>> GetByCurrencyIdAsync(Guid currencyId)
        {
            return await _context.Histories
                .Where(h => h.CurrencyId == currencyId)
                .OrderByDescending(h => h.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<History>> GetByDateRangeAsync(Guid currencyId, DateTime from, DateTime to)
        {
            return await _context.Histories
                .Where(h => h.CurrencyId == currencyId && h.Date >= from && h.Date <= to)
                .OrderBy(h => h.Date)
                .ToListAsync();
        }

        public async Task AddAsync(History history)
        {
            await _context.Histories.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var history = await _context.Histories.FindAsync(id);
            if (history != null)
            {
                _context.Histories.Remove(history);
                await _context.SaveChangesAsync();
            }
        }
    }
}
