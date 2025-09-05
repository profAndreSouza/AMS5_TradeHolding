using System;
using System.Text.Json.Serialization;

namespace CurrencyAPI.API.DTOs
{
    public class CryptoApiResponseDto
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("price")]
    public string Price { get; set; }
}
}
