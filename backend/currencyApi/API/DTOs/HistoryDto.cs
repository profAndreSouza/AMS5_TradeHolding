using System;

namespace CurrencyAPI.API.DTOs
{
    public class HistoryDto
    {
        public Guid Id { get; set; }
        public Guid CurrencyId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
