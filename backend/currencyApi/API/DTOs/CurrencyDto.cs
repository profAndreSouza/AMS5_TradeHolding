using System;

namespace CurrencyAPI.API.DTOs
{
    public class CurrencyDto
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Backing { get; set; } = string.Empty;
        public bool Reverse { get; set; } = false;
        public List<HistoryDto> Histories { get; set; } = new();
    }
}
