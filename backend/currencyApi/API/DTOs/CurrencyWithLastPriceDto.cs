using System;

namespace CurrencyAPI.API.DTOs
{
    public class CurrencyWithLastPriceDto
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Backing { get; set; }
        public bool Reverse { get; set; }
        public decimal? LastPrice { get; set; }
        public DateTime? LastPriceDate { get; set; }
    }
    
}