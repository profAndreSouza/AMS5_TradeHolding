using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CurrencyAPI.Domain.Entities;
using CurrencyAPI.Domain.Interfaces;
using CurrencyAPI.Infrastructure.Data;

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
    }
}
