namespace CurrencyAPI.API.DTOs.External
{
    public class ExternalPriceDto
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
